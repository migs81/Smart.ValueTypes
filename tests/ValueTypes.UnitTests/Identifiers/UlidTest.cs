using System;
using Smart.ValueTypes.Types.Identifiers;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Identifiers
{
    public class UlidTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(ULID);

	        // act
            var result = new ULID();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("01J8R5T4W8M3F2X9QK7HCVN6PA", 1727388259208L)]
        [InlineData("07HY8N3FQ5R7W2XMK9CVTJ4DBG", 8313199771365L)]
        public void Constructor_ValidInput_ShouldReturnObject(string input, long expectedTimeStamp)
        {
            // act
            var result = new ULID(input);
            
            // assert
            Assert.Equal(input, result);
            Assert.Equal(expectedTimeStamp, result.TimeStamp.ToUnixTimeMilliseconds());
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("0000000000000000000000000", typeof(InvalidUlidException))]
        [InlineData("000000000000000000000000000", typeof(InvalidUlidException))]
        [InlineData("80000000000000000000000000", typeof(InvalidUlidException))]
        [InlineData("A0000000000000000000000000", typeof(InvalidUlidException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => new ULID(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData("01J8R5T4W8M3F2X9QK7HCVN6PA", 1727388259208L)]
        [InlineData("07HY8N3FQ5R7W2XMK9CVTJ4DBG", 8313199771365L)]
        public void From_ValidInput_ShouldReturnObject(string input, long expectedTimeStamp)
        {
            // act
            var result = ULID.From(input);
            
            // assert
            Assert.Equal(input, result);
            Assert.Equal(expectedTimeStamp, result.TimeStamp.ToUnixTimeMilliseconds());
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("0000000000000000000000000", typeof(InvalidUlidException))]
        [InlineData("000000000000000000000000000", typeof(InvalidUlidException))]
        [InlineData("80000000000000000000000000", typeof(InvalidUlidException))]
        [InlineData("A0000000000000000000000000", typeof(InvalidUlidException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => ULID.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData("01J8R5T4W8M3F2X9QK7HCVN6PA")]
        [InlineData("07HY8N3FQ5R7W2XMK9CVTJ4DBG")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = ULID.TryFrom(input, out _);
                
            // assert
            Assert.Equal(ULID.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, ULID.Validation.Null)]
        [InlineData("", ULID.Validation.Empty)]
        [InlineData("0000000000000000000000000", ULID.Validation.WrongLength)]
        [InlineData("000000000000000000000000000", ULID.Validation.WrongLength)]
        [InlineData("80000000000000000000000000", ULID.Validation.InvalidFirstCharacter)]
        [InlineData("A0000000000000000000000000", ULID.Validation.InvalidFirstCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, ULID.Validation expected)
        {
	        // act
            var result = ULID.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
