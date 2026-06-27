using Migs.ValueTypes.Types.Hashes;
using System;
using Xunit;

namespace Migs.ValueTypes.UnitTests.Hashes
{
    public class SHA256Test
    {
        #region test data

        private static readonly string[] _validValues =
        [
            "ca978112ca1bbdcafac231b39a23dc4da786eff8147c4e72b9807785afee48bb",
            "3e23e8160039594a33894f6564e1b1348bbd7a0088d42c4acb73eeaed59c009d",
            "2e7d2c03a9507ae265ecf5b5356885a53393a2029d241394997265a1a25aefc6",
            "18ac3e7343f016890c510e93f935261169d9e3f565436429830faf0934f4f8e4",
            "3f79bb7b435b05321651daefd374cdc681dc06faa65e374e38337b88ca046dea",
            "252f10c83610ebca1a059c0bae8255eba2f95be4d1d7bcfa89d7248a82d9f111",
            "cd0aa9856147b6c5b4ff2b7dfee5da20aa38253099ef1b4a64aced233c9afe29",
            "aaa9402664f1a41f40ebbc52c9993eb66aeb366602958fdfaa283b71e64db123",
            "de7d1b721a1e0632b7cf04edf5032c8ecffa9f9a08492152b926f1a5a7e765d7",
            "189f40034be7a199f1fa9891668ee3ab6049f82d38c68be70f596eab2e1857b7",
        ];
        private static readonly ValueTuple<string, string> _validValue = new("test",
            "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08");

        private const string _wrongCharacter = 
            "gggggggggg" +
            "gggggggggg" +
            "gggggggggg" +
            "gggggggggg" +
            "gggggggggg" +
            "gggggggggg" +
            "gggg";

        #endregion

        #region tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new SHA256();
            Assert.Equal(SHA256.Empty, result);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = new SHA256(value);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidSHA256Exception))]
        [InlineData(_wrongCharacter, typeof(InvalidSHA256Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new SHA256(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = SHA256.From(value);
                Assert.Equal(value, result);
            }
        }

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in _validValues)
            {
                var result = SHA256.TryFrom(value, out _);
                Assert.Equal(SHA256.Validation.Ok, result);
            }
        }

        [Fact]
        public void Create_ValidInput_ShouldReturnObject()
        {
            var result = SHA256.Create(_validValue.Item1);
            Assert.Equal(_validValue.Item2.ToUpper(), result);
        }

        [Fact]
        public void TryCreate_ValidInput_ShouldReturnTrue()
        {
            var result = SHA256.TryCreate(_validValue.Item1, out _);
            Assert.True(result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidSHA256Exception))]
        [InlineData(_wrongCharacter, typeof(InvalidSHA256Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new SHA256(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(null, SHA256.Validation.Null)]
        [InlineData("", SHA256.Validation.WrongLength)]
        [InlineData(_wrongCharacter, SHA256.Validation.IllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, SHA256.Validation expected)
        {
            var result = SHA256.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
