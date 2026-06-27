using Migs.ValueTypes.Types.Hashes;
using System;
using Xunit;

namespace Migs.ValueTypes.UnitTests.Hashes
{
    public class SHA1Test
    {
        #region test data

        private static readonly string[] _validValues =
        [
            "86f7e437faa5a7fce15d1ddcb9eaeaea377667b8",
            "e9d71f5ee7c92d6dc9e92ffdad17b8bd49418f98",
            "84a516841ba77a5b4648de2cd0dfcb30ea46dbb4",
            "3c363836cf4e16666669a25da280a1865c2d2874",
            "58e6b3a414a1e090dfc6029add0f3555ccba127f",
            "4a0a19218e082a343a1b17e5333409af9d98f0f5",
            "54fd1711209fb1c0781092374132c66e79e2241b",
            "27d5482eebd075de44389774fce28c69f45c8a75",
            "042dc4512fa3d391c5170cf3aa61e6a638f84342",
            "5c2dd944dde9e08881bef0894fe7b22a5c9c4b06",
        ];
        private static readonly ValueTuple<string, string> _validValue = new("test",
            "a94a8fe5ccb19ba61c4c0873d391e987982fbbd3");

        private const string _wrongCharacter = 
            "gggggggggg" +
            "gggggggggg" +
            "gggggggggg" +
            "gggggggggg";
        
        #endregion

        #region tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new SHA1();
            Assert.Equal(SHA1.Empty, result);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = new SHA1(value);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidSHA1Exception))]
        [InlineData(_wrongCharacter, typeof(InvalidSHA1Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new SHA1(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = SHA1.From(value);
                Assert.Equal(value, result);
            }
        }

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in _validValues)
            {
                var result = SHA1.TryFrom(value, out _);
                Assert.Equal(SHA1.Validation.Ok, result);
            }
        }

        [Fact]
        public void Create_ValidInput_ShouldReturnObject()
        {
            var result = SHA1.Create(_validValue.Item1);
            Assert.Equal(_validValue.Item2.ToUpper(), result);
        }

        [Fact]
        public void TryCreate_ValidInput_ShouldReturnTrue()
        {
            var result = SHA1.TryCreate(_validValue.Item1, out _);
            Assert.True(result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidSHA1Exception))]
        [InlineData(_wrongCharacter, typeof(InvalidSHA1Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new SHA1(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(null, SHA1.Validation.Null)]
        [InlineData("", SHA1.Validation.WrongLength)]
        [InlineData(_wrongCharacter, SHA1.Validation.IllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, SHA1.Validation expected)
        {
            var result = SHA1.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
