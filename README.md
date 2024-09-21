[nuget package](https://www.nuget.org/packages/Rebel.Alliance.Blake3.Hasher)
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

This library is a re-packaging of the Blake3 Hash Alogrithm implementation that I stripped out of the suite of algos from the Arctium project found here: https://github.com/NeuroXiq/Arctium; an implementation based on the official BLAKE3 specification and reference implementation.

  Aside from reorganizing and isolating the BLAKE3 implementation, the changes I made were specifically to the namespace and project name. I also added a NuGet package configuration and unit tests to enhance the usability and reliability of the library.

  Therefore, the original Author still holds the copyright to the original implementation, and I am grateful for their work in making this available.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
