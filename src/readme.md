# Rebel.Alliance.Blake3.Hasher

This project is an implementation of the BLAKE3 cryptographic hash function for .NET, targeting .NET Standard 2.0.

## Features

- Full implementation of the BLAKE3 hash algorithm
- Optimized for performance with unsafe code and inlining
- Easy to use API

## Usage

Here's a simple example of how to use the BLAKE3 hasher:

```csharp
using Rebel.Alliance.Blake3.Hasher;

// Create a new BLAKE3 hasher
var hasher = new BLAKE3();

// Hash some data
byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello, BLAKE3!");
hasher.HashBytes(data);

// Finalize and get the hash
byte[] hash = hasher.HashFinal();

// Convert hash to hexadecimal string
string hashString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
Console.WriteLine($"BLAKE3 Hash: {hashString}");
```

## Installation

You can install the Rebel.Alliance.Blake3.Hasher via NuGet package manager:

```
Install-Package Rebel.Alliance.Blake3.Hasher
```

## License

This project is licensed under the MIT License.

## Acknowledgements

This library is a re-packaging of the Blake3 Hash Alogrithm implementation that I stripped out of the suite of algos from the Arctium project found here: https://github.com/NeuroXiq/Arctium; and implementation based on the official BLAKE3 specification and reference implementation.

  Apart from the reorganization and isolation of the BLAKE3 implementation, the only changes I made were to the namespace and project name, and the addition of a NuGet package configuration and unit tests.

  Therefore the original Author still holds copyright to the original implementation and I am grateful for the work they did to make this available.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
