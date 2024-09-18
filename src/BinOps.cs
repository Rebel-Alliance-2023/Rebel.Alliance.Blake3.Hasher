using System.Runtime.CompilerServices;

namespace Rebel.Alliance.Blake3.Hasher
{
    public static class BinOps
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ROR(uint v, int r)
        {
            return (v >> r) | (v << (32 - r));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ROL(uint value, int r)
        {
            return (value << r) | (value >> (32 - r));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ROR(ulong value, int r)
        {
            return (value >> r) | (value << (64 - r));
        }
    }
}
