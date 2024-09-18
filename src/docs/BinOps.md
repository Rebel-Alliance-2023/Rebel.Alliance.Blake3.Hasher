# BinOps.cs Documentation

The `BinOps` class provides a set of static methods for common binary operations used in cryptographic algorithms, particularly in the BLAKE3 implementation.

## Class Definition

```csharp
public static class BinOps
```

## Methods

### ROR (uint overload)

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static uint ROR(uint v, int r)
```

Performs a bitwise right rotation (circular right shift) on a 32-bit unsigned integer.

- Parameters:
  - `v` (uint): The value to rotate.
  - `r` (int): The number of positions to rotate by.
- Returns: The rotated value.

### ROL

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static uint ROL(uint value, int r)
```

Performs a bitwise left rotation (circular left shift) on a 32-bit unsigned integer.

- Parameters:
  - `value` (uint): The value to rotate.
  - `r` (int): The number of positions to rotate by.
- Returns: The rotated value.

### ROR (ulong overload)

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static ulong ROR(ulong value, int r)
```

Performs a bitwise right rotation (circular right shift) on a 64-bit unsigned integer.

- Parameters:
  - `value` (ulong): The value to rotate.
  - `r` (int): The number of positions to rotate by.
- Returns: The rotated value.

## Usage

The BinOps class is typically used in the inner loops of cryptographic algorithms where performance is critical. The methods are marked with `[MethodImpl(MethodImplOptions.AggressiveInlining)]` to suggest to the compiler that these methods should be inlined for better performance.

Example usage in a cryptographic function:

```csharp
uint a = 0x12345678;
int rotation = 5;
uint rotated = BinOps.ROR(a, rotation);
```

These operations are fundamental to many cryptographic algorithms, including BLAKE3. They are used to create diffusion in the data, which is essential for the security properties of the hash function.

The inclusion of both 32-bit and 64-bit versions of ROR allows the library to efficiently support different variants of algorithms or to be optimized for different processor architectures.

Note that while these operations are simple, their efficient implementation can significantly impact the overall performance of the cryptographic algorithm, especially when processing large amounts of data.
