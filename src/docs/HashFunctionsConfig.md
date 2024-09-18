# HashFunctionsConfig.cs Documentation

The `HashFunctionsConfig` class provides configuration settings for hash functions in the Rebel.Alliance.Blake3.Hasher library. It contains static properties that can be used to adjust the behavior of hash functions, particularly regarding buffer sizes.

## Class Definition

```csharp
public static class HashFunctionsConfig
```

## Properties

### BufferSizeInBlocks

```csharp
public static int BufferSizeInBlocks = 0x20;
```

This static property defines the size of the internal buffer as a multiple of the input block size for hash functions.

- Default Value: 0x20 (32 in decimal)
- Purpose: Determines how much data is buffered before processing in hash functions.

## Usage

The `BufferSizeInBlocks` property is used to configure the size of the internal buffer used in hash functions. This can affect performance and memory usage:

- A larger buffer size may improve performance for large inputs by reducing the number of method calls, but it will use more memory.
- A smaller buffer size will use less memory but may result in more method calls for large inputs.

The actual buffer size in bytes is calculated by multiplying this value by the input block size of the specific hash function. For example, if a hash function has an input block size of 64 bytes:

```csharp
int actualBufferSizeInBytes = HashFunctionsConfig.BufferSizeInBlocks * 64;
// With the default value, this would be 32 * 64 = 2048 bytes
```

This configuration can be particularly important for the BLAKE3 implementation, as it affects how the algorithm buffers and processes input data.

Example of how this might be used in the hash function implementation:

```csharp
int bufferSize = HashFunctionsConfig.BufferSizeInBlocks * (inputBlockSize / 8);
dataBufferWithCallback = new BlockBufferWithUnsafeCallback(bufferSize, blockSizeInBytes, HashDataBufferCallback);
```

By adjusting this value, users of the library can fine-tune the performance characteristics of the hash functions to suit their specific use cases and hardware environments.
