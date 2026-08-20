using System;
using Smart.ValueTypes.Types.Security.Hashes;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Security.Hashes
{
    public class SHA1Test
    {
        #region test data

        private static readonly ValueTuple<string, string> ValidValue = new("test",
            "a94a8fe5ccb19ba61c4c0873d391e987982fbbd3");

        private const string WrongCharacter = "gggggggggggggggggggggggggggggggggggggggg";
        
        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(SHA1);

            // act
            var result = new SHA1();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("86f7e437faa5a7fce15d1ddcb9eaeaea377667b8")]
        [InlineData("e9d71f5ee7c92d6dc9e92ffdad17b8bd49418f98")]
        [InlineData("84a516841ba77a5b4648de2cd0dfcb30ea46dbb4")]
        [InlineData("3c363836cf4e16666669a25da280a1865c2d2874")]
        [InlineData("58e6b3a414a1e090dfc6029add0f3555ccba127f")]
        [InlineData("4a0a19218e082a343a1b17e5333409af9d98f0f5")]
        [InlineData("54fd1711209fb1c0781092374132c66e79e2241b")]
        [InlineData("27d5482eebd075de44389774fce28c69f45c8a75")]
        [InlineData("042dc4512fa3d391c5170cf3aa61e6a638f84342")]
        [InlineData("5c2dd944dde9e08881bef0894fe7b22a5c9c4b06")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new SHA1(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidSha1Exception))]
        [InlineData(WrongCharacter, typeof(InvalidSha1Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new SHA1(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData("86f7e437faa5a7fce15d1ddcb9eaeaea377667b8")]
        [InlineData("e9d71f5ee7c92d6dc9e92ffdad17b8bd49418f98")]
        [InlineData("84a516841ba77a5b4648de2cd0dfcb30ea46dbb4")]
        [InlineData("3c363836cf4e16666669a25da280a1865c2d2874")]
        [InlineData("58e6b3a414a1e090dfc6029add0f3555ccba127f")]
        [InlineData("4a0a19218e082a343a1b17e5333409af9d98f0f5")]
        [InlineData("54fd1711209fb1c0781092374132c66e79e2241b")]
        [InlineData("27d5482eebd075de44389774fce28c69f45c8a75")]
        [InlineData("042dc4512fa3d391c5170cf3aa61e6a638f84342")]
        [InlineData("5c2dd944dde9e08881bef0894fe7b22a5c9c4b06")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = SHA1.From(input);
                
            // assert
            Assert.Equal(input, result);
        }
                
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidSha1Exception))]
        [InlineData(WrongCharacter, typeof(InvalidSha1Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new SHA1(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom

        [Theory]
        [InlineData("86f7e437faa5a7fce15d1ddcb9eaeaea377667b8")]
        [InlineData("e9d71f5ee7c92d6dc9e92ffdad17b8bd49418f98")]
        [InlineData("84a516841ba77a5b4648de2cd0dfcb30ea46dbb4")]
        [InlineData("3c363836cf4e16666669a25da280a1865c2d2874")]
        [InlineData("58e6b3a414a1e090dfc6029add0f3555ccba127f")]
        [InlineData("4a0a19218e082a343a1b17e5333409af9d98f0f5")]
        [InlineData("54fd1711209fb1c0781092374132c66e79e2241b")]
        [InlineData("27d5482eebd075de44389774fce28c69f45c8a75")]
        [InlineData("042dc4512fa3d391c5170cf3aa61e6a638f84342")]
        [InlineData("5c2dd944dde9e08881bef0894fe7b22a5c9c4b06")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = SHA1.TryFrom(input, out _);
                
            // assert
            Assert.Equal(SHA1.Validation.Ok, result);
        }

        [Theory]
        [InlineData(null, SHA1.Validation.Null)]
        [InlineData("", SHA1.Validation.WrongLength)]
        [InlineData(WrongCharacter, SHA1.Validation.IllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, SHA1.Validation expected)
        {
            // act
            var result = SHA1.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
        
        #region Create

        [Fact]
        public void Create_ValidInput_ShouldReturnObject()
        {
            // act
            var result = SHA1.Create(ValidValue.Item1);
            
            // assert
            Assert.Equal(ValidValue.Item2.ToUpper(), result);
        }

        #endregion
        
        #region TryCreate

        [Fact]
        public void TryCreate_ValidInput_ShouldReturnTrue()
        {
            // act
            var result = SHA1.TryCreate(ValidValue.Item1, out _);
            
            // assert
            Assert.True(result);
        }

        #endregion
    }
}
