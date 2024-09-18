# MemMap.cs Documentation

The `MemMap` class provides static methods for mapping between different data representations, particularly for converting between byte arrays and integer arrays. These operations are crucial for the efficient implementation of the BLAKE3 algorithm.

## Class Definition

```csharp
public static unsafe class MemMap
```

## Methods

### ToBytes4UIntLE

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static void ToBytes4UIntLE(uint* array, byte* output)
```

Converts an array of 4 unsigned integers to a byte array in little-endian format.

### ToByteArrayBE

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static byte[] ToByteArrayBE(uint* uintPtr, int length)
```

Converts an array of unsigned integers to a byte array in big-endian format.

### ToByteArrayLE

```csharp
public static byte[] ToByteArrayLE(uint* input, int length)
```

Converts an array of unsigned integers to a byte array in little-endian format.

### ToUInt16BytesLE

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static void ToUInt16BytesLE(byte* input, uint* output)
```

Converts 16 bytes to 4 uint values where input bytes are interpreted as little-endian 4-byte integers.

### ToUInt32BytesLE

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static void ToUInt32BytesLE(byte* input, uint* output)
```

Converts 32 bytes to 8 uint values where input bytes are interpreted as little-endian 4-byte integers.

### ToUInt64BytesLE

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static void ToUInt64BytesLE(byte* input, uint* output)
```

Converts 64 bytes to 16 uint values where bytes are interpreted as little-endian integers.

### ToULong128BytesLE

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static void ToULong128BytesLE(byte* src, ulong* dst)
```

Converts 128 bytes to 16 ulong values where bytes are interpreted as little-endian integers.

### ToByteArrayLE (ulong[] overload)

```csharp
public static byte[] ToByteArrayLE(ulong[] src)
```

Converts an array of ulong values to a byte array in little-endian format.

### ToNewByteArrayLE

```csharp
public static byte[] ToNewByteArrayLE(uint[] input, int length)
```

Creates a new byte array from an array of uint values in little-endian format.

## Usage

The MemMap class is used extensively in the BLAKE3 implementation for converting between different data representations. These conversions are often necessary when processing input data or preparing output data.

The methods in this class are optimized for performance, using unsafe code and pointer arithmetic to achieve fast conversions. Many methods are marked with `[MethodImpl(MethodImplOptions.AggressiveInlining)]` to suggest to the compiler that these methods should be inlined for better performance.

Example usage:

```csharp
uint* input = stackalloc uint[4];
byte* output = stackalloc byte[16];
MemMap.ToBytes4UIntLE(input, output);

byte* input128 = stackalloc byte[128];
ulong* output128 = stackalloc ulong[16];
MemMap.ToULong128BytesLE(input128, output128);
```

These methods are particularly useful in cryptographic algorithms where data often needs to be processed in specific formats (e.g., as 32-bit or 64-bit words) but may be received or need to be output as byte arrays.

Note that these methods use unsafe code and should be used with caution, ensuring proper bounds checking and memory management to prevent security vulnerabilities or memory corruption.
