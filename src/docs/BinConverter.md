# BinConverter.cs Documentation

The `BinConverter` class provides static methods for various binary conversions, particularly for converting between different integer representations and byte arrays. These methods are essential for the low-level data manipulation required in cryptographic algorithms like BLAKE3.

## Class Definition

```csharp
public unsafe static class BinConverter
```

## Methods

### ToUIntLE

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static uint ToUIntLE(byte[] buffer, long offset)
```

Converts 4 bytes from a byte array to an unsigned integer using little-endian byte order.

### ToUShortBE

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static ushort ToUShortBE(byte[] buffer, long offset)
```

Converts 2 bytes from a byte array to an unsigned short using big-endian byte order.

### ToUIntBE

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static uint ToUIntBE(byte[] buffer, long offset)
```

Converts 4 bytes from a byte array to an unsigned integer using big-endian byte order.

### ToULongBE

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static ulong ToULongBE(byte[] buffer, long offset)
```

Converts 8 bytes from a byte array to an unsigned long using big-endian byte order.

### ToULongLE

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static ulong ToULongLE(byte[] buffer, long offset)
```

Converts 8 bytes from a byte array to an unsigned long using little-endian byte order.

### ToULongLE (with length)

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static ulong ToULongLE(byte[] buffer, long offset, long length)
```

Converts a variable number of bytes (up to 8) from a byte array to an unsigned long using little-endian byte order.

### ToULongBE (with length)

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static ulong ToULongBE(byte[] buffer, long offset, int length)
```

Converts a variable number of bytes (up to 8) from a byte array to an unsigned long using big-endian byte order.

### ToBooleanArray

```csharp
public static bool[] ToBooleanArray(byte flagsArray)
```

Converts a byte representing flags to an array of boolean values.

### ToBytesBE (ulong overload)

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static void ToBytesBE(byte[] buffer, long offset, ulong value)
```

Converts an unsigned long to 8 bytes in big-endian order and writes them to a byte array.

### ToBytesBE (uint overload)

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static void ToBytesBE(byte[] buffer, long offset, uint value)
```

Converts an unsigned integer to 4 bytes in big-endian order and writes them to a byte array.

### ToBytesBE (ulong to new array)

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static byte[] ToBytesBE(ulong value)
```

Converts an unsigned long to a new byte array in big-endian order.

### GetBytesTrimToEmptyLE

```csharp
public static byte[] GetBytesTrimToEmptyLE(ulong value)
```

Converts an unsigned long to a byte array in little-endian order, trimming leading zero bytes.

### GetBytesTrimToLastLE

```csharp
public static byte[] GetBytesTrimToLastLE(ulong value)
```

Converts an unsigned long to a byte array in little-endian order, trimming leading zero bytes but always keeping at least one byte.

### FromString

```csharp
public static byte[] FromString(string hexString)
```

Converts a hexadecimal string to a byte array.

## Usage

The BinConverter class is used throughout the BLAKE3 implementation for various binary conversion tasks. These methods are particularly useful when dealing with data that needs to be processed in specific integer formats but may be stored or transmitted as byte arrays.

Example usage:

```csharp
byte[] data = new byte[8];
ulong value = BinConverter.ToULongLE(data, 0);

ulong number = 12345678UL;
byte[] bytes = BinConverter.ToBytesBE(number);

string hexString = "0A0B0C0D";
byte[] hexBytes = BinConverter.FromString(hexString);
```

The methods in this class are often marked with `[MethodImpl(MethodImplOptions.AggressiveInlining)]` to suggest to the compiler that these methods should be inlined for better performance, which is crucial in cryptographic operations where these conversions may be called frequently.
