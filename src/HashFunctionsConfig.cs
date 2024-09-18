namespace Rebel.Alliance.Blake3.Hasher
{
    public static class HashFunctionsConfig
    {
        /// <summary>
        /// Size of the internal buffer as a multiple of input block size of the specific hash function.
        /// Example: if Hash function takes 64-bytes input block, BufferSizeInBlocks == 16 means
        /// that there will be allocated 64 * 16 bytes and hash procedure runs after reaching this limit.
        /// </summary>
        public static int BufferSizeInBlocks = 0x20;
    }
}
