# BLAKE3.cs Documentation

The `BLAKE3` class is the main entry point for using the BLAKE3 hashing algorithm in the Rebel.Alliance.Blake3.Hasher library. It inherits from `HashFunctionBase` and implements the core functionality of the BLAKE3 algorithm.

## Class Definition

```csharp
public class BLAKE3 : HashFunctionBase
```

## Constants

- `InputBlockLength` (int): 8192 - The length of the input block in bytes.
- `HashSizeBlake3` (int): 256 - The size of the BLAKE3 hash output in bits.

## Fields

- `state` (BLAKE3Algorithm.State): Holds the current state of the BLAKE3 algorithm.
- `cachedChunk` (byte[]): A 1024-byte array used to cache the last chunk of data.
- `cachedChunkHaveData` (bool): Indicates whether the cached chunk contains data.

## Constructor

```csharp
public BLAKE3() : base(InputBlockLength, HashSizeBlake3)
```

Initializes a new instance of the BLAKE3 class. It sets up the initial state and prepares the hasher for use.

## Methods

### HashFinal

```csharp
public unsafe override byte[] HashFinal()
```

Finalizes the hash computation and returns the hash value. This method handles various scenarios:
1. No data hashed and no cached chunk
2. No data hashed but cached chunk exists
3. Data hashed and possibly cached chunk exists

It processes any remaining data, including the cached chunk, and computes the final hash value.

### ExecuteHashing

```csharp
protected override unsafe void ExecuteHashing(byte* buffer, long length)
```

Processes a block of data during the hashing operation. It handles the cached chunk and processes full chunks of data.

### GetCurrentHash

```csharp
protected override byte[] GetCurrentHash()
```

This method is not implemented in BLAKE3 and throws an `InvalidOperationException`. The hash is computed in `HashFinal`.

### GetPadding

```csharp
protected override byte[] GetPadding()
```

This method is not implemented in BLAKE3 and throws an `InvalidOperationException`. Padding is handled internally in the algorithm.

### ResetState

```csharp
public override void ResetState()
```

Resets the state of the BLAKE3 hasher, preparing it for a new hashing operation.

### RoundUpTo64

```csharp
private long RoundUpTo64(long value)
```

A utility method that rounds up the given value to the nearest multiple of 64. This is used for padding calculations.

## Usage

The BLAKE3 class is designed to be used as follows:

1. Create an instance of the BLAKE3 class.
2. Call `HashBytes` method (inherited from `HashFunctionBase`) to process data.
3. Call `HashFinal` to get the final hash value.

Example:
```csharp
var hasher = new BLAKE3();
hasher.HashBytes(data);
byte[] hash = hasher.HashFinal();
```

This class encapsulates the BLAKE3 algorithm, providing a simple interface for hashing data while handling the complexities of the algorithm internally.
