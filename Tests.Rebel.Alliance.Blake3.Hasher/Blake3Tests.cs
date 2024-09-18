using System;
using System.Text;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using Serilog;
using Rebel.Alliance.Blake3.Hasher;

namespace Blake3.Tests
{
    public class Blake3Tests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly ILogger _logger;
        private static readonly Dictionary<int, string> KnownHashes = new Dictionary<int, string>();

        public Blake3Tests(ITestOutputHelper output)
        {
            _output = output;
            _logger = new LoggerConfiguration()
                .WriteTo.TestOutput(_output)
                .CreateLogger();
        }

        public void Dispose()
        {
            Log.CloseAndFlush();
        }

        private string ComputeHash(byte[] input)
        {
            var blake3 = new BLAKE3();
            blake3.HashBytes(input);
            var result = blake3.HashFinal();
            return Convert.ToBase64String(result);
        }

        [Fact]
        public void Hash_EmptyInput_ReturnsCorrectHash()
        {
            var blake3 = new BLAKE3();
            var result = blake3.HashFinal();
            var expectedHash = ComputeHash(new byte[0]);

            Assert.Equal(Convert.FromBase64String(expectedHash), result);
            _logger.Information("Empty input hash test passed");
        }

        [Fact]
        public void Hash_SimpleString_ReturnsCorrectHash()
        {
            var input = Encoding.ASCII.GetBytes("Hello BLAKE3");
            var expectedHash = ComputeHash(input);

            var blake3 = new BLAKE3();
            blake3.HashBytes(input);
            var result = blake3.HashFinal();

            Assert.Equal(Convert.FromBase64String(expectedHash), result);
            _logger.Information("Simple string hash test passed");
        }

        [Fact]
        public void Hash_LargeInput_ReturnsCorrectHash()
        {
            var input = new byte[1024 * 1024]; // 1 MB of data
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = (byte)(i % 256);
            }
            var expectedHash = ComputeHash(input);

            var blake3 = new BLAKE3();
            blake3.HashBytes(input);
            var result = blake3.HashFinal();

            Assert.Equal(Convert.FromBase64String(expectedHash), result);
            _logger.Information("Large input hash test passed");
        }

        [Fact]
        public void Hash_MultipleInputs_ReturnsCorrectHash()
        {
            var input1 = new byte[] { 1, 1, 1 };
            var input2 = new byte[] { 2, 2, 2 };
            var input3 = new byte[] { 3, 3, 3 };
            var combinedInput = input1.Concat(input2).Concat(input3).ToArray();
            var expectedHash = ComputeHash(combinedInput);

            var blake3 = new BLAKE3();
            blake3.HashBytes(input1);
            blake3.HashBytes(input2);
            blake3.HashBytes(input3);
            var result = blake3.HashFinal();

            Assert.Equal(Convert.FromBase64String(expectedHash), result);
            _logger.Information("Multiple inputs hash test passed");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(1023)]
        [InlineData(1024)]
        [InlineData(1025)]
        [InlineData(2048)]
        [InlineData(2049)]
        [InlineData(3072)]
        [InlineData(3073)]
        [InlineData(4096)]
        [InlineData(4097)]
        [InlineData(5120)]
        [InlineData(5121)]
        [InlineData(6144)]
        [InlineData(6145)]
        [InlineData(7168)]
        [InlineData(7169)]
        [InlineData(8192)]
        [InlineData(8193)]
        [InlineData(16384)]
        [InlineData(31744)]
        [InlineData(102400)]
        public void Hash_VariousInputSizes_ReturnsCorrectHash(int inputSize)
        {
            var input = new byte[inputSize];
            for (int i = 0; i < inputSize; i++)
            {
                input[i] = (byte)(i % 251);
            }

            string expectedHash;
            if (!KnownHashes.TryGetValue(inputSize, out expectedHash))
            {
                expectedHash = ComputeHash(input);
                KnownHashes[inputSize] = expectedHash;
                _logger.Information($"Stored new hash for input size {inputSize}");
            }

            var blake3 = new BLAKE3();
            blake3.HashBytes(input);
            var result = blake3.HashFinal();

            Assert.Equal(Convert.FromBase64String(expectedHash), result);
            _logger.Information($"Input size {inputSize} hash test passed");
        }

        [Fact]
        public void ResetState_AfterHashing_AllowsNewHash()
        {
            var blake3 = new BLAKE3();
            var input1 = Encoding.ASCII.GetBytes("First hash");
            blake3.HashBytes(input1);
            var result1 = blake3.HashFinal();

            blake3.ResetState();

            var input2 = Encoding.ASCII.GetBytes("Second hash");
            blake3.HashBytes(input2);
            var result2 = blake3.HashFinal();

            Assert.NotEqual(result1, result2);
            Assert.Equal(ComputeHash(input1), Convert.ToBase64String(result1));
            Assert.Equal(ComputeHash(input2), Convert.ToBase64String(result2));
            _logger.Information("Reset state test passed");
        }

        [Fact]
        public void HashFinal_CalledTwice_ThrowsException()
        {
            var blake3 = new BLAKE3();
            var input = Encoding.ASCII.GetBytes("Test input");
            blake3.HashBytes(input);
            blake3.HashFinal();

            Assert.Throws<InvalidOperationException>(() => blake3.HashFinal());
            _logger.Information("HashFinal called twice test passed");
        }

        [Fact]
        public void Hash_LongUnicodeString_ReturnsCorrectHash()
        {
            var input = @"l4y3H5dxvBSETSNP/NOzeRvaUWU38gNiW5EbnbHHQF/GglV2c00Wa/
                          sgXfq7zQi6tNqxiIB7spQVyz3YGrPWbSZgr/ZR9cAcK7l6o7/5TdCUr
                          /oZTjNYxbcicuW7+p5B8sUPJ8Pg2T1VGfdjQdWEEDfaBun80N2wXswWj
                          XO4N/iNxqwrMdgLwtaVsRjJdgzjMLpeGVjzm6nbqFDOvnXUA6RXek0Q1
                          ESVc3j7T+uERvhIqMMxeT08MZRRu39HFlclYvu6O+6xvokSu9lSYnHq4n
                          ZYqVNzBa24OPLv3nAwvyBAQ4QHQJDK70fuBTrlgdCZyyjgJ6y8tZAdQt8
                          hcWuwxQ==";
            var byteData = Encoding.Unicode.GetBytes(input);
            var expectedHash = ComputeHash(byteData);

            var blake3 = new BLAKE3();
            blake3.HashBytes(byteData);
            var result = blake3.HashFinal();

            Assert.Equal(Convert.FromBase64String(expectedHash), result);
            _logger.Information("Long Unicode string hash test passed");
        }

        [Fact]
        public void Hash_StreamInput_ReturnsCorrectHash()
        {
            var input = new byte[1024 * 1024]; // 1 MB of data
            new Random(42).NextBytes(input);
            var expectedHash = ComputeHash(input);

            var blake3 = new BLAKE3();
            using (var stream = new MemoryStream(input))
            {
                blake3.HashBytes(stream);
            }
            var result = blake3.HashFinal();

            Assert.Equal(Convert.FromBase64String(expectedHash), result);
            _logger.Information("Stream input hash test passed");
        }
    }
}