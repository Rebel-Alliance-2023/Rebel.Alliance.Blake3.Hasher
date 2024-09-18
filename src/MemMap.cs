using System;
using System.Runtime.CompilerServices;

namespace Rebel.Alliance.Blake3.Hasher
{
    public static unsafe class MemMap
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToBytes4UIntLE(uint* array, byte* output)
        {
            for (int i = 0; i < 4; i++)
            {
                uint value = array[i];
                output[i * 4 + 0] = (byte)(value >> 0);
                output[i * 4 + 1] = (byte)(value >> 8);
                output[i * 4 + 2] = (byte)(value >> 16);
                output[i * 4 + 3] = (byte)(value >> 24);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ToByteArrayBE(uint* uintPtr, int length)
        {
            byte[] buf = new byte[length * 4];
            for (int i = 0, j = 0; i < length; i++, j += 4)
            {
                buf[j] = (byte)(uintPtr[i] >> 24);
                buf[j + 1] = (byte)(uintPtr[i] >> 16);
                buf[j + 2] = (byte)(uintPtr[i] >> 8);
                buf[j + 3] = (byte)(uintPtr[i] >> 0);
            }
            return buf;
        }

        public static byte[] ToByteArrayLE(uint* input, int length)
        {
            byte[] result = new byte[length * 4];
            for (int i = 0, j = 0; i < length; i++, j += 4)
            {
                result[j + 0] = (byte)(input[i] >> 0);
                result[j + 1] = (byte)(input[i] >> 8);
                result[j + 2] = (byte)(input[i] >> 16);
                result[j + 3] = (byte)(input[i] >> 24);
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToUInt16BytesLE(byte* input, uint* output)
        {
            for (int i = 0; i < 4; i++)
            {
                output[i] = (uint)input[i * 4] |
                            ((uint)input[i * 4 + 1] << 8) |
                            ((uint)input[i * 4 + 2] << 16) |
                            ((uint)input[i * 4 + 3] << 24);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToUInt32BytesLE(byte* input, uint* output)
        {
            for (int i = 0; i < 8; i++)
            {
                output[i] = (uint)input[i * 4] |
                            ((uint)input[i * 4 + 1] << 8) |
                            ((uint)input[i * 4 + 2] << 16) |
                            ((uint)input[i * 4 + 3] << 24);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToUInt64BytesLE(byte* input, uint* output)
        {
            for (int i = 0; i < 16; i++)
            {
                output[i] = (uint)input[i * 4] |
                            ((uint)input[i * 4 + 1] << 8) |
                            ((uint)input[i * 4 + 2] << 16) |
                            ((uint)input[i * 4 + 3] << 24);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToULong128BytesLE(byte* src, ulong* dst)
        {
            for (int i = 0; i < 16; i++)
            {
                dst[i] = (ulong)src[i * 8] |
                         ((ulong)src[i * 8 + 1] << 8) |
                         ((ulong)src[i * 8 + 2] << 16) |
                         ((ulong)src[i * 8 + 3] << 24) |
                         ((ulong)src[i * 8 + 4] << 32) |
                         ((ulong)src[i * 8 + 5] << 40) |
                         ((ulong)src[i * 8 + 6] << 48) |
                         ((ulong)src[i * 8 + 7] << 56);
            }
        }

        public static byte[] ToByteArrayLE(ulong[] src)
        {
            byte[] dst = new byte[src.Length * 8];
            for (int i = 0; i < src.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    dst[(i * 8) + j] = (byte)(src[i] >> (8 * j));
                }
            }
            return dst;
        }

        public static byte[] ToNewByteArrayLE(uint[] input, int length)
        {
            byte[] result = new byte[length * 4];
            for (int i = 0, j = 0; i < length; i++, j += 4)
            {
                result[j + 0] = (byte)(input[i] >> 0);
                result[j + 1] = (byte)(input[i] >> 8);
                result[j + 2] = (byte)(input[i] >> 16);
                result[j + 3] = (byte)(input[i] >> 24);
            }
            return result;
        }
    }
}
