using System;
using System.Runtime.CompilerServices;

namespace Rebel.Alliance.Blake3.Hasher
{
    public unsafe static class BinConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUIntLE(byte[] buffer, long offset)
        {
            return (uint)(
                (buffer[offset + 0] << 0) |
                (buffer[offset + 1] << 8) |
                (buffer[offset + 2] << 16) |
                (buffer[offset + 3] << 24));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ToUShortBE(byte[] buffer, long offset)
        {
            return (ushort)((buffer[offset] << 8) | buffer[offset + 1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUIntBE(byte[] buffer, long offset)
        {
            return (uint)
               (((uint)buffer[offset + 0] << 24) |
                ((uint)buffer[offset + 1] << 16) |
                ((uint)buffer[offset + 2] << 8) |
                ((uint)buffer[offset + 3] << 0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToULongBE(byte[] buffer, long offset)
        {
            return (ulong)
                (
                    ((ulong)buffer[offset + 7] << 0) |
                    ((ulong)buffer[offset + 6] << 8) |
                    ((ulong)buffer[offset + 5] << 16) |
                    ((ulong)buffer[offset + 4] << 24) |
                    ((ulong)buffer[offset + 3] << 32) |
                    ((ulong)buffer[offset + 2] << 40) |
                    ((ulong)buffer[offset + 1] << 48) |
                    ((ulong)buffer[offset + 0] << 56)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToULongLE(byte[] buffer, long offset)
        {
            return (ulong)
                (
                    ((ulong)buffer[offset + 0] << 0) |
                    ((ulong)buffer[offset + 1] << 8) |
                    ((ulong)buffer[offset + 2] << 16) |
                    ((ulong)buffer[offset + 3] << 24) |
                    ((ulong)buffer[offset + 4] << 32) |
                    ((ulong)buffer[offset + 5] << 40) |
                    ((ulong)buffer[offset + 6] << 48) |
                    ((ulong)buffer[offset + 7] << 56)
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToULongLE(byte[] buffer, long offset, long length)
        {
            if (length < 0 || length > 8) throw new ArgumentException("length 0-8");

            ulong result = 0;
            int shift = 0;
            for (long i = offset + length - 1; i >= offset; i--)
            {
                result |= ((ulong)buffer[i] << (shift));
                shift += 8;
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToULongBE(byte[] buffer, long offset, int length)
        {
            if (length > 8 || length < 1)
                throw new ArgumentException("length in bytes of the ulong value must be in range 1-8");

            ulong value = 0;
            for (int i = 0; i < length; i++)
            {
                value |= (ulong)(buffer[offset + i]) << ((length - 1 - i) * 8);
            }

            return value;
        }

        public static bool[] ToBooleanArray(byte flagsArray)
        {
            bool[] flags = new bool[8];
            for (int j = 0; j < 8; j++)
            {
                flags[j] = (flagsArray & (1 << j)) > 0;
            }
            return flags;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToBytesBE(byte[] buffer, long offset, ulong value)
        {
            buffer[offset + 0] = (byte)((value >> 56) & 0xFF);
            buffer[offset + 1] = (byte)((value >> 48) & 0xFF);
            buffer[offset + 2] = (byte)((value >> 40) & 0xFF);
            buffer[offset + 3] = (byte)((value >> 32) & 0xFF);
            buffer[offset + 4] = (byte)((value >> 24) & 0xFF);
            buffer[offset + 5] = (byte)((value >> 16) & 0xFF);
            buffer[offset + 6] = (byte)((value >> 8) & 0xFF);
            buffer[offset + 7] = (byte)((value >> 0) & 0xFF);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToBytesBE(byte[] buffer, long offset, uint value)
        {
            buffer[offset + 0] = (byte)((value >> 24) & 0xff);
            buffer[offset + 1] = (byte)((value >> 16) & 0xff);
            buffer[offset + 2] = (byte)((value >> 8) & 0xff);
            buffer[offset + 3] = (byte)((value >> 0) & 0xff);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytesBE(ulong value)
        {
            byte[] result = new byte[8];
            ToBytesBE(result, 0, value);
            return result;
        }

        public static byte[] GetBytesTrimToEmptyLE(ulong value)
        {
            int length = 0;
            for (int i = 0; i < 8; i++)
            {
                if (((value >> (i * 8)) & 0xFF) != 0) length++;
                else break;
            }

            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[length - i - 1] = (byte)(value >> (i * 8));
            }

            return result;
        }

        public static byte[] GetBytesTrimToLastLE(ulong value)
        {
            int length = 1;
            for (int i = 1; i < 8; i++)
            {
                if (((value >> (i * 8)) & 0xFF) != 0) length++;
                else break;
            }

            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[length - i - 1] = (byte)(value >> (i * 8));
            }

            return result;
        }

        public static byte[] FromString(string hexString)
        {
            if (hexString.Length % 2 != 0)
                throw new ArgumentException("String value must be valid hex string (multiply of 2)");

            int length = hexString.Length / 2;
            byte[] parsed = new byte[length];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                parsed[i / 2] = byte.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return parsed;
        }
    }
}
