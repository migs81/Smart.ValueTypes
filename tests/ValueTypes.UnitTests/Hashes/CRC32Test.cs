using Smart.ValueTypes.Types.Hashes;
using System;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Hashes
{
    public class CRC32Test
    {
        #region test data

        private const string WrongCharacter = "gggggggg";

        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(CRC32);
            
            // act
            var result = new CRC32();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("00000000")]
        [InlineData("40DF0B66")]
        [InlineData("68DDB3F8")]
        [InlineData("026D930A")]
        [InlineData("5268E236")]
        [InlineData("54DE5729")]
        [InlineData("9ABFB3B6")]
        [InlineData("71B18589")]
        [InlineData("DBBBC9D6")]
        [InlineData("EDB88320")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new CRC32(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidCrc32Exception))]
        [InlineData("wronginput", typeof(InvalidCrc32Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new CRC32(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData("00000000")]
        [InlineData("40DF0B66")]
        [InlineData("68DDB3F8")]
        [InlineData("026D930A")]
        [InlineData("5268E236")]
        [InlineData("54DE5729")]
        [InlineData("9ABFB3B6")]
        [InlineData("71B18589")]
        [InlineData("DBBBC9D6")]
        [InlineData("EDB88320")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = CRC32.From(input);
                
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidCrc32Exception))]
        [InlineData(WrongCharacter, typeof(InvalidCrc32Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new CRC32(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region TryFrom

        [Theory]
        [InlineData("00000000")]
        [InlineData("40DF0B66")]
        [InlineData("68DDB3F8")]
        [InlineData("026D930A")]
        [InlineData("5268E236")]
        [InlineData("54DE5729")]
        [InlineData("9ABFB3B6")]
        [InlineData("71B18589")]
        [InlineData("DBBBC9D6")]
        [InlineData("EDB88320")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = CRC32.TryFrom(input, out _);
                
            // assert
            Assert.Equal(CRC32.Validation.Ok, result);
        }

        [Theory]
        [InlineData(null, CRC32.Validation.Null)]
        [InlineData("", CRC32.Validation.WrongLength)]
        [InlineData(WrongCharacter, CRC32.Validation.IllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, CRC32.Validation expected)
        {
            // act
            var result = CRC32.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
