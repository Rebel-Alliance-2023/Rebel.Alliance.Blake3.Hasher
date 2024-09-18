using System;
using System.IO;

namespace Rebel.Alliance.Blake3.Hasher
{
    public unsafe abstract class HashFunctionBase
    {
        public int InputBlockSize { get; private set; }
        public int HashSize { get; private set; }

        protected BlockBufferWithUnsafeCallback dataBufferWithCallback;
        protected bool hashFinalCalled;
        protected long CurrentMessageLengthWithoutPadding { get; set; }

        protected HashFunctionBase(int inputBlockSize, int hashSize)
        {
            InputBlockSize = inputBlockSize;
            HashSize = hashSize;

            int blockSizeInBytes = inputBlockSize / 8;
            int bufferSize = HashFunctionsConfig.BufferSizeInBlocks * blockSizeInBytes;
            dataBufferWithCallback = new BlockBufferWithUnsafeCallback(bufferSize, blockSizeInBytes, HashDataBufferCallback);

            hashFinalCalled = false;
        }

        public virtual long HashBytes(byte[] buffer)
        {
            long loaded = dataBufferWithCallback.Load(buffer, 0, buffer.Length);
            CurrentMessageLengthWithoutPadding += loaded;
            return loaded;
        }

        public virtual long HashBytes(Stream stream)
        {
            long loaded = dataBufferWithCallback.Load(stream);
            CurrentMessageLengthWithoutPadding += loaded;
            return loaded;
        }

        public virtual long HashBytes(byte[] buffer, long offset, long length)
        {
            long loaded = dataBufferWithCallback.Load(buffer, offset, length);
            CurrentMessageLengthWithoutPadding += loaded;
            return loaded;
        }

        public virtual byte[] HashFinal()
        {
            if (hashFinalCalled) throw new InvalidOperationException("HashFinal can be called only once after ResetState or instance creation.");
            hashFinalCalled = true;

            byte[] padding = GetPadding();
            if (padding != null) { dataBufferWithCallback.Load(padding, 0, padding.Length); }

            if (dataBufferWithCallback.Count > 0)
                dataBufferWithCallback.FlushBuffer();

            return GetCurrentHash();
        }

        private void HashDataBufferCallback(byte* buffer, long length)
        {
            ExecuteHashing(buffer, length);
        }

        protected abstract byte[] GetPadding();
        protected abstract void ExecuteHashing(byte* buffer, long length);
        protected abstract byte[] GetCurrentHash();

        public virtual void ResetState()
        {
            hashFinalCalled = false;
            CurrentMessageLengthWithoutPadding = 0;
            dataBufferWithCallback.Clear();
        }
    }
}
