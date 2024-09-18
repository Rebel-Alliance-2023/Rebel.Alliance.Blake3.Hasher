# BlockBufferWithUnsafeCallback.cs Documentation

The `BlockBufferWithUnsafeCallback` class manages a buffer for block-based data processing, providing an efficient way to handle data input for hash functions like BLAKE3.

## Class Definition

```csharp
public unsafe class BlockBufferWithUnsafeCallback
```

## Properties and Fields

- `BlockSize` (int): The size of a single block in bytes.
- `BufferSize` (int): The total size of the buffer in bytes.
- `Count` (long): The number of bytes currently in the buffer.
- `Buffer` (byte[]): The underlying byte array used for buffering data.

## Delegate

```csharp
public delegate void Callback(byte* buffer, long bytesCount);
```

This delegate defines the signature for the callback function that processes blocks of data.

## Constructor

```csharp
public BlockBufferWithUnsafeCallback(int bufferSize, int blockSize, Callback callback)
```

Initializes a new instance of the BlockBufferWithUnsafeCallback class.

## Methods

### Load (byte[] overload)

```csharp
public long Load(byte[] input, long offset, long length)
```

Loads data from a byte array into the buffer, processing full blocks as they become available.

### Load (Stream overload)

```csharp
public long Load(Stream stream)
```

Loads data from a stream into the buffer, processing full blocks as they become available.

### FlushBuffer

```csharp
public void FlushBuffer()
```

Processes any remaining data in the buffer using the callback function.

### Clear

```csharp
public void Clear()
```

Clears the buffer, resetting the byte count to zero.

## Private Methods

### LoadMaxPossibleDataToBuffer

```csharp
private long LoadMaxPossibleDataToBuffer(byte[] input, long offset, long length)
```

Loads as much data as possible into the buffer without exceeding its capacity.

## Usage

This class is used to efficiently manage data input for hash functions. It buffers incoming data and calls a provided callback function to process full blocks of data. This approach allows for efficient memory usage and can improve performance by reducing the number of method calls for processing data.

Example usage:

```csharp
void ProcessBlock(byte* buffer, long bytesCount)
{
    // Process the block of data
}

var blockBuffer = new BlockBufferWithUnsafeCallback(4096, 64, ProcessBlock);
blockBuffer.Load(inputData, 0, inputData.Length);
blockBuffer.FlushBuffer(); // Process any remaining data
```

The use of unsafe code and callbacks allows for high-performance data processing, which is crucial for cryptographic hash functions like BLAKE3.
