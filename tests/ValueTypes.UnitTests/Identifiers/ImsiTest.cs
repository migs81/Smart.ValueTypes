using System;
using Smart.ValueTypes.Types.Identifiers;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Identifiers
{
    public class ImsiTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(IMSI);

            // act
            var result = new IMSI();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("123456789012345")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new IMSI(input);
            
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("12345", typeof(InvalidImsiException))]
        [InlineData("1234567890123456", typeof(InvalidImsiException))]
        [InlineData("a23456", typeof(InvalidImsiException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new IMSI(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData("123456")]
        [InlineData("123456789012345")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = IMSI.From(input);
            
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("12345", typeof(InvalidImsiException))]
        [InlineData("1234567890123456", typeof(InvalidImsiException))]
        [InlineData("a23456", typeof(InvalidImsiException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new IMSI(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region TryFrom

        [Theory]
        [InlineData("123456")]
        [InlineData("123456789012345")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = IMSI.TryFrom(input, out _);
            
            // assert
            Assert.Equal(IMSI.Validation.Ok, result);
        }

        [Theory]
        [InlineData(null, IMSI.Validation.Null)]
        [InlineData("", IMSI.Validation.Empty)]
        [InlineData("12345", IMSI.Validation.TooShort)]
        [InlineData("1234567890123456", IMSI.Validation.TooLong)]
        [InlineData("a23456", IMSI.Validation.IllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, IMSI.Validation expected)
        {
            // act
            var result = IMSI.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
