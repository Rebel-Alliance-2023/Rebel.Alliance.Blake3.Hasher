using System;

namespace Rebel.Alliance.Blake3.Hasher
{
    public class BLAKE3 : HashFunctionBase
    {
        const int InputBlockLength = 8192;
        const int HashSizeBlake3 = 256;

        BLAKE3Algorithm.State state;
        byte[] cachedChunk = new byte[1024];
        bool cachedChunkHaveData = false;

        public BLAKE3() : base(InputBlockLength, HashSizeBlake3)
        {
            state = new BLAKE3Algorithm.State();
            this.ResetState();
        }

        public unsafe override byte[] HashFinal()
        {
            if (hashFinalCalled) throw new InvalidOperationException("Has final was called. Must reset state");
            hashFinalCalled = true;

            long dataLength = dataBufferWithCallback.Count;

            if (dataLength == 0 && !cachedChunkHaveData)
            {
                byte* emptyBuffer = stackalloc byte[64];
                return BLAKE3Algorithm.HashLastChunk(emptyBuffer, 1, 0, state);
            }
            else if (dataLength == 0 && cachedChunkHaveData)
            {
                fixed (byte* input = &cachedChunk[0])
                {
                    return BLAKE3Algorithm.HashLastChunk(input, 16, 64, state);
                }
            }

            if (cachedChunkHaveData)
            {
                fixed (byte* input = &cachedChunk[0])
                {
                    BLAKE3Algorithm.HashFullChunksWhichAreNotTheLast(input, 1, state);
                }
            }

            // compute parameters of the last chunk
            long notLastChunksCount = (dataLength - 1) / 1024;
            long lastChunkOffset = notLastChunksCount * 1024;
            long lastChunkLength = dataLength - (notLastChunksCount * 1024);
            long lastChunkLengthWithPadding = RoundUpTo64(lastChunkLength);
            uint lastBlockLength = (uint)lastChunkLength % 64;
            lastBlockLength = lastBlockLength == 0 ? 64 : lastBlockLength;
            byte[] lastChunk = new byte[lastChunkLengthWithPadding];

            MemCpy.Copy(dataBufferWithCallback.Buffer, lastChunkOffset, lastChunk, 0, lastChunkLength);

            if (notLastChunksCount > 0)
            {
                fixed (byte* input = &dataBufferWithCallback.Buffer[0])
                {
                    BLAKE3Algorithm.HashFullChunksWhichAreNotTheLast(input, notLastChunksCount, state);
                }
            }

            fixed (byte* input = &lastChunk[0])
            {
                return BLAKE3Algorithm.HashLastChunk(input, lastChunkLengthWithPadding / 64, lastBlockLength, state);
            }
        }

        protected override unsafe void ExecuteHashing(byte* buffer, long length)
        {
            if (cachedChunkHaveData)
            {
                fixed (byte* input = &cachedChunk[0])
                {
                    BLAKE3Algorithm.HashFullChunksWhichAreNotTheLast(input, 1, state);
                }
            }

            long lengthWithoutLastChunk = length - 1024;

            // always store last chunk 
            MemCpy.Copy(buffer + lengthWithoutLastChunk, cachedChunk, 1024);
            cachedChunkHaveData = true;

            if (lengthWithoutLastChunk > 0)
            {
                BLAKE3Algorithm.HashFullChunksWhichAreNotTheLast(buffer, lengthWithoutLastChunk / 1024, state);
            }
        }

        protected override byte[] GetCurrentHash()
        {
            throw new InvalidOperationException("Overloaded in hash final");
        }

        protected override byte[] GetPadding()
        {
            throw new InvalidOperationException("Padding created in hash final");
        }

        public override void ResetState()
        {
            BLAKE3Algorithm.ResetState(state);
            base.ResetState();
        }

        private long RoundUpTo64(long value)
        {
            if (value > 0)
            {
                return (((value - 1) / 64) + 1) * 64;
            }
            else return 64;
        }
    }
}
