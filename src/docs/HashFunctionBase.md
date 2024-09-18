# HashFunctionBase.cs Documentation

The `HashFunctionBase` class serves as an abstract base class for hash function implementations in the Rebel.Alliance.Blake3.Hasher library. It provides a common structure and functionality that can be shared across different hash algorithms.

## Class Definition

```csharp
public unsafe abstract class HashFunctionBase
```

## Properties

- `InputBlockSize` (int): Gets the size of the input block in bits.
- `HashSize` (int): Gets the size of the hash output in bits.

## Protected Fields

- `dataBufferWithCallback` (BlockBufferWithUnsafeCallback): A buffer that holds input data and provides a callback mechanism for processing.
- `hashFinalCalled` (bool): A flag indicating whether the HashFinal method has been called.
- `CurrentMessageLengthWithoutPadding` (long): The total length of the message processed so far, excluding any padding.

## Constructor

```csharp
protected HashFunctionBase(int inputBlockSize, int hashSize)
```

Initializes a new instance of the HashFunctionBase class with the specified input block size and hash size.

## Methods

### HashBytes (byte[] overload)

```csharp
public virtual long HashBytes(byte[] buffer)
```

Processes a byte array of data through the hash function.

### HashBytes (Stream overload)

```csharp
public virtual long HashBytes(Stream stream)
```

Processes data from a stream through the hash function.

### HashBytes (byte[], long, long overload)

```csharp
public virtual long HashBytes(byte[] buffer, long offset, long length)
```

Processes a portion of a byte array through the hash function.

### HashFinal

```csharp
public virtual byte[] HashFinal()
```

Finalizes the hash computation and returns the hash value. This method can only be called once after initialization or reset.

### ResetState

```csharp
public virtual void ResetState()
```

Resets the state of the hash function, preparing it for a new hashing operation.

## Protected Abstract Methods

### GetPadding

```csharp
protected abstract byte[] GetPadding();
```

Should be implemented to return the padding required for the specific hash algorithm.

### ExecuteHashing

```csharp
protected abstract void ExecuteHashing(byte* buffer, long length);
```

Should be implemented to perform the core hashing operation on a block of data.

### GetCurrentHash

```csharp
protected abstract byte[] GetCurrentHash();
```

Should be implemented to return the current hash value based on the processed data.

## Usage

This base class is designed to be inherited by specific hash function implementations. The derived classes should:

1. Implement the abstract methods (GetPadding, ExecuteHashing, GetCurrentHash).
2. Override virtual methods if necessary (e.g., HashFinal for algorithms with special finalization steps).
3. Use the provided fields and methods to manage the hashing state and process data.

The HashFunctionBase class handles common tasks such as:
- Managing the input buffer and invoking the hashing callback.
- Tracking the hash computation state (e.g., whether HashFinal has been called).
- Providing a common interface for processing data from various sources (byte arrays, streams).

This design allows for a consistent interface across different hash implementations while allowing for algorithm-specific optimizations and behaviors.
