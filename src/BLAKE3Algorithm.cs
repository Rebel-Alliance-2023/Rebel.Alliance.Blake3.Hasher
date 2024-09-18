using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static Rebel.Alliance.Blake3.Hasher.BinOps;

namespace Rebel.Alliance.Blake3.Hasher
{
    static unsafe class BLAKE3Algorithm
    {
        const uint FlagChunkStart = 1 << 0;
        const uint FlagChunkEnd = 1 << 1;
        const uint FlagParent = 1 << 2;
        const uint FlagRoot = 1 << 3;
        const uint FlagKeyedHash = 1 << 4;
        const uint FlagDeriveKeyContext = 1 << 5;
        const uint FlagDeriveKeyMaterial = 1 << 6;

        static readonly uint[] IVConstants = new uint[] {
            0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a,
            0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19
        };

        public struct TreeNode
        {
            public uint[] ChainingValue;
            public int Level;

            public TreeNode(uint[] chainingValues, int level)
            {
                ChainingValue = chainingValues;
                Level = level;
            }
        }

        public class State
        {
            public long ProcessedChunks;
            public Stack<TreeNode> Stack;
        }

        public static void ResetState(State state)
        {
            state.ProcessedChunks = 0;
            state.Stack = new Stack<TreeNode>();
        }

        public static void HashFullChunksWhichAreNotTheLast(byte* input, long chunksCount, State state)
        {
            uint* inputBuffer = stackalloc uint[16];
            uint* outputBuffer = stackalloc uint[16];
            uint* chainingValues = stackalloc uint[8];
            uint flags = 0;

            while (chunksCount > 0)
            {
                SetInitialChainingValuesForHash(chainingValues);

                MemMap.ToUInt64BytesLE(input, inputBuffer);
                flags = FlagChunkStart;
                CompressionFunction(inputBuffer, chainingValues, state.ProcessedChunks, 64, flags, outputBuffer);
                MemCpy.Copy(outputBuffer, chainingValues, 8);
                input += 64;

                flags = 0;
                for (int i = 1; i < 15; i++)
                {
                    MemMap.ToUInt64BytesLE(input, inputBuffer);
                    CompressionFunction(inputBuffer, chainingValues, state.ProcessedChunks, 64, flags, outputBuffer);
                    MemCpy.Copy(outputBuffer, chainingValues, 8);
                    input += 64;
                }

                MemMap.ToUInt64BytesLE(input, inputBuffer);
                flags = FlagChunkEnd;
                CompressionFunction(inputBuffer, chainingValues, state.ProcessedChunks, 64, flags, outputBuffer);
                input += 64;

                ++state.ProcessedChunks;
                --chunksCount;

                state.Stack.Push(new TreeNode(MemCpy.ToArray(outputBuffer, 8), 0));
                MergeTreeNodesWithSameLevel(state);
            }
        }

        public static byte[] HashLastChunk(byte* input, long blocksCount, uint lastBlockLength, State state)
        {
            uint* inputBuffer = stackalloc uint[16];
            uint* outputBuffer = stackalloc uint[16];
            uint* chainingValues = stackalloc uint[8];
            uint flags = 0;
            uint bytesInChunk = 64;
            long counter = state.ProcessedChunks;

            SetInitialChainingValuesForHash(chainingValues);

            for (int i = 0; i < blocksCount; i++)
            {
                MemMap.ToUInt64BytesLE(input, inputBuffer);
                flags = 0;

                if (i == 0)
                {
                    flags |= FlagChunkStart;
                }
                if (i == blocksCount - 1)
                {
                    flags |= FlagChunkEnd;
                    if (state.ProcessedChunks == 0) flags |= FlagRoot;

                    bytesInChunk = lastBlockLength;
                }

                CompressionFunction(inputBuffer, chainingValues, counter, bytesInChunk, flags, outputBuffer);
                MemCpy.Copy(outputBuffer, chainingValues, 8);
                input += 64;
            }

            state.Stack.Push(new TreeNode(MemCpy.ToArray(outputBuffer, 8), 0));

            return ComputeHashFromCurrentState(state);
        }

        static void MergeTreeNodesWithSameLevel(State state)
        {
            Stack<TreeNode> stack = state.Stack;

            if (stack.Count < 2) return;

            TreeNode t1, t2;
            uint* input = stackalloc uint[16];
            uint* output = stackalloc uint[16];
            uint* chainingValue = stackalloc uint[8];

            do
            {
                SetInitialChainingValuesForHash(chainingValue);
                t2 = stack.Pop(); // right leaf
                t1 = stack.Pop(); // left leaf

                if (t1.Level == t2.Level)
                {
                    uint flags = FlagParent;

                    // start computing chaining value for parent of this two leafs
                    MemCpy.Copy(t1.ChainingValue, input);
                    MemCpy.Copy(t2.ChainingValue, input + 8);
                    CompressionFunction(input, chainingValue, 0, 64, flags, output);

                    // push newly created parent to stack with next level (more near to to tree root)
                    int levelUp = t1.Level + 1;
                    uint[] parentChaining = MemCpy.ToArray(output, 8);
                    stack.Push(new TreeNode(parentChaining, levelUp));
                }
                else
                {
                    // not on the same level, revert pop and exit
                    stack.Push(t1);
                    stack.Push(t2);
                }

            } while (t1.Level == t2.Level && stack.Count > 1);
        }

        private static byte[] ComputeHashFromCurrentState(State state)
        {
            Stack<TreeNode> stack = state.Stack;

            if (stack.Count == 0) throw new System.Exception($"{nameof(BLAKE3Algorithm)}");

            TreeNode t1, t2;
            uint* input = stackalloc uint[16];
            uint* output = stackalloc uint[16];
            uint* chainingValue = stackalloc uint[8];

            while (stack.Count > 1)
            {
                SetInitialChainingValuesForHash(chainingValue);
                t2 = stack.Pop();
                t1 = stack.Pop();

                uint flags = FlagParent;

                if (stack.Count == 0) flags |= FlagRoot;

                MemCpy.Copy(t1.ChainingValue, input);
                MemCpy.Copy(t2.ChainingValue, input + 8);

                CompressionFunction(input, chainingValue, 0, 64, flags, output);

                // level can be ignored because all nodes are merged and level is ignored
                stack.Push(new TreeNode(MemCpy.ToArray(output, 8), -1));
            }

            byte[] hash = MemMap.ToNewByteArrayLE(stack.Peek().ChainingValue, 8);
            return hash;
        }

        static void CompressionFunction(uint* input, uint* h, long counter, uint blockLength, uint flags, uint* output)
        {
            // initialize state
            uint* v = stackalloc uint[16];

            for (int i = 0; i < 8; i++) v[i] = h[i];

            v[8] = 0x6a09e667;
            v[9] = 0xbb67ae85;
            v[10] = 0x3c6ef372;
            v[11] = 0xa54ff53a;
            v[12] = (uint)counter;
            v[13] = (uint)(counter >> 32);
            v[14] = blockLength;
            v[15] = flags;

            uint* permute = stackalloc uint[16];

            for (int i = 0; i < 7; i++)
            {
                G(&v[0], &v[4], &v[8], &v[12], input[0], input[1]);
                G(&v[1], &v[5], &v[9], &v[13], input[2], input[3]);
                G(&v[2], &v[6], &v[10], &v[14], input[4], input[5]);
                G(&v[3], &v[7], &v[11], &v[15], input[6], input[7]);

                G(&v[0], &v[5], &v[10], &v[15], input[8], input[9]);
                G(&v[1], &v[6], &v[11], &v[12], input[10], input[11]);
                G(&v[2], &v[7], &v[8], &v[13], input[12], input[13]);
                G(&v[3], &v[4], &v[9], &v[14], input[14], input[15]);

                for (int j = 0; j < 16; j++)
                {
                    permute[j] = input[permuteInOut[j]];
                }

                uint* swapCpy = input;
                input = permute;
                permute = swapCpy;
            }

            // output
            for (int i = 0; i < 8; i++)
            {
                output[i] = v[i] ^ v[i + 8];
                output[i + 8] = v[i + 8] ^ h[i];
            }
        }

        static uint[] permuteInOut = new uint[]
        {
            2, 6, 3, 10, 7, 0, 4, 13, 1, 11, 12, 5, 9, 14, 15, 8
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void G(uint* inOuta, uint* inOutb, uint* inOutc, uint* inOutd, uint m0, uint m1)
        {
            uint a = *inOuta;
            uint b = *inOutb;
            uint c = *inOutc;
            uint d = *inOutd;

            a = a + b + m0;
            d = ROR(d ^ a, 16);
            c = c + d;
            b = ROR(b ^ c, 12);
            a = a + b + m1;
            d = ROR(d ^ a, 8);
            c = c + d;
            b = ROR(b ^ c, 7);

            *inOuta = a;
            *inOutb = b;
            *inOutc = c;
            *inOutd = d;
        }

        static void SetInitialChainingValuesForHash(uint* output) 
        { 
            for (int i = 0; i < 8; i++) output[i] = IVConstants[i]; 
        }
    }
}
