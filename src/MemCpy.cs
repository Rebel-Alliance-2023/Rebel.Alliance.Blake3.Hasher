using System;
using System.Runtime.CompilerServices;

namespace Rebel.Alliance.Blake3.Hasher
{
    public static unsafe partial class MemCpy
    {
        public static long Copy(byte[] inputBuffer, long offset, byte[] outputBuffer, long outputOffset, long length)
        {
            long copyEnd = offset + length;
            for (long i = offset, j = outputOffset; i < copyEnd; i++, outputOffset++)
            {
                outputBuffer[outputOffset] = inputBuffer[i];
            }

            return length;
        }

        public static long Copy(byte[] source, byte[] destination)
        {
            if (source.Length != destination.Length)
                throw new ArgumentException("Length of the source array does not match length of the destination");
            return Copy(source, 0, destination, 0, destination.Length);
        }

        public static byte[] CopyRange(byte[] buffer, long offset, long length)
        {
            byte[] range = new byte[length];
            Copy(buffer, offset, range, 0, length);
            return range;
        }

        public static void Copy(ulong* src, ulong[] dst)
        {
            for (int i = 0; i < dst.Length; i++)
            {
                dst[i] = src[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy8Uint(uint* src, uint* dst)
        {
            for (int i = 0; i < 8; i++)
            {
                dst[i] = src[i];
            }
        }

        public static void Copy(ulong[] source, ulong* dst)
        {
            for (int i = 0; i < source.Length; i++)
            {
                dst[i] = source[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy8ULong(ulong* source, ulong* dst)
        {
            for (int i = 0; i < 8; i++)
            {
                dst[i] = source[i];
            }
        }

        public static void Copy(byte* src, byte[] dst, long count)
        {
            for (int i = 0; i < count; i++)
            {
                dst[i] = src[i];
            }
        }

        public static uint[] ToArray(uint* input, int length)
        {
            uint[] result = new uint[length];
            for (int i = 0; i < length; i++) result[i] = input[i];
            return result;
        }

        public static void Copy(byte* src, long srcOffset, byte* output, long outOffset, long length)
        {
            for (int i = 0; i < length; i++)
            {
                output[i + outOffset] = src[srcOffset + i];
            }
        }

        public static void Copy(uint* src, uint* dst, long length)
        {
            for (long i = 0; i < length; i++)
            {
                dst[i] = src[i];
            }
        }

        public static void Copy(byte[] input, long inputOffset, byte* output, int outputOffset, long length)
        {
            for (long i = 0; i < length; i++)
            {
                output[i + outputOffset] = input[i + inputOffset];
            }
        }

        public static void Copy(byte[] input, byte* output)
        {
            Copy(input, 0, output, 0, input.Length);
        }

        public static void Copy(byte[] key2, uint* input, uint[] output, long outputOffset, long length)
        {
            for (int i = 0; i < length; i++)
            {
                output[i + outputOffset] = input[i];
            }
        }

        public static void Copy(byte* src, byte* dst, long length)
        {
            Copy(src, 0, dst, 0, length);
        }

        public static void Copy(uint[] src, uint* dst)
        {
            for (int i = 0; i < src.Length; i++)
            {
                dst[i] = src[i];
            }
        }
    }
}
