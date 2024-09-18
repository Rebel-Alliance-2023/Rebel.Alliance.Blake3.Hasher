# MemCpy.cs Documentation

The `MemCpy` class provides a set of static methods for copying memory between different types of arrays and pointers. It's designed to offer efficient memory operations for the BLAKE3 implementation.

## Class Definition

```csharp
public static unsafe partial class MemCpy
```

## Methods

### Copy (byte[] to byte[])

```csharp
public static long Copy(byte[] inputBuffer, long offset, byte[] outputBuffer, long outputOffset, long length)
```

Copies bytes from one buffer to another.

- Parameters:
  - `inputBuffer` (byte[]): Source buffer
  - `offset` (long): Starting offset in the source buffer
  - `outputBuffer` (byte[]): Destination buffer
  - `outputOffset` (long): Starting offset in the destination buffer
  - `length` (long): Number of bytes to copy
- Returns: Number of bytes copied

### Copy (byte[] to byte[])

```csharp
public static long Copy(byte[] source, byte[] destination)
```

Copies all bytes from one array to another of the same length.

### CopyRange

```csharp
public static byte[] CopyRange(byte[] buffer, long offset, long length)
```

Creates a new byte array containing a range of bytes from the input buffer.

### Copy (ulong* to ulong[])

```csharp
public static void Copy(ulong* src, ulong[] dst)
```

Copies data from an unmanaged ulong pointer to a managed ulong array.

### Copy8Uint

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static void Copy8Uint(uint* src, uint* dst)
```

Copies 8 uint values from one unmanaged location to another.

### Copy (ulong[] to ulong*)

```csharp
public static void Copy(ulong[] source, ulong* dst)
```

Copies data from a managed ulong array to an unmanaged ulong pointer.

### Copy8ULong

```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
public static void Copy8ULong(ulong* source, ulong* dst)
```

Copies 8 ulong values from one unmanaged location to another.

### Copy (byte* to byte[])

```csharp
public static void Copy(byte* src, byte[] dst, long count)
```

Copies data from an unmanaged byte pointer to a managed byte array.

### ToArray

```csharp
public static uint[] ToArray(uint* input, int length)
```

Creates a managed uint array from an unmanaged uint pointer.

### Copy (byte* to byte*)

```csharp
public static void Copy(byte* src, long srcOffset, byte* output, long outOffset, long length)
```

Copies bytes between unmanaged memory locations.

### Copy (uint* to uint*)

```csharp
public static void Copy(uint* src, uint* dst, long length)
```

Copies uint values between unmanaged memory locations.

### Copy (byte[] to byte*)

```csharp
public static void Copy(byte[] input, long inputOffset, byte* output, int outputOffset, long length)
```

Copies bytes from a managed byte array to unmanaged memory.

### Copy (byte[] to byte*)

```csharp
public static void Copy(byte[] input, byte* output)
```

Copies all bytes from a managed byte array to unmanaged memory.

### Copy (byte[], uint* to uint[])

```csharp
public static void Copy(byte[] key2, uint* input, uint[] output, long outputOffset, long length)
```

Copies uint values from unmanaged memory to a managed uint array.

### Copy (byte* to byte*)

```csharp
public static void Copy(byte* src, byte* dst, long length)
```

Copies bytes between unmanaged memory locations.

### Copy (uint[] to uint*)

```csharp
public static void Copy(uint[] src, uint* dst)
```

Copies uint values from a managed array to unmanaged memory.

## Usage

The MemCpy class is used extensively in the BLAKE3 implementation for efficient memory operations. It provides methods for copying between different types of memory (managed and unmanaged) and different data types (byte, uint, ulong).

These methods are particularly useful in cryptographic algorithms where performance is critical and direct memory manipulation is often required. The use of unsafe code and pointers allows for more efficient memory operations compared to managed code in certain scenarios.

Example usage:

```csharp
byte[] source = new byte[1024];
byte[] destination = new byte[1024];
MemCpy.Copy(source, destination);

uint* unmanagedBuffer = stackalloc uint[256];
uint[] managedArray = new uint[256];
MemCpy.Copy(unmanagedBuffer, managedArray);
```

Note that many of these methods use unsafe code and should be used with caution, ensuring proper bounds checking and memory management to prevent security vulnerabilities or memory corruption.
