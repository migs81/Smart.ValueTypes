using System;
using Smart.ValueTypes.Types.Identifiers;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Identifiers
{
    public class WknTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(WKN);

	        // act
            var result = new WKN();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("ABCDEF")]
        [InlineData("1A2B3C")]
        [InlineData("abcdef")]
        [InlineData("a1b2c3")]
        [InlineData("X1y2Z3")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new WKN(input);
            
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        // general
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        // wrong length
        [InlineData("12345", typeof(InvalidWknException))]
        [InlineData("1234567", typeof(InvalidWknException))]
        // contains illegal character
        [InlineData("12345+", typeof(InvalidWknException))]
        [InlineData("12-456", typeof(InvalidWknException))]
        [InlineData("123 56", typeof(InvalidWknException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => new WKN(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData("123456")]
        [InlineData("ABCDEF")]
        [InlineData("1A2B3C")]
        [InlineData("abcdef")]
        [InlineData("a1b2c3")]
        [InlineData("X1y2Z3")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = WKN.From(input);
            
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        // general
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        // wrong length
        [InlineData("12345", typeof(InvalidWknException))]
        [InlineData("1234567", typeof(InvalidWknException))]
        // contains illegal character
        [InlineData("12345+", typeof(InvalidWknException))]
        [InlineData("12-456", typeof(InvalidWknException))]
        [InlineData("123 56", typeof(InvalidWknException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => WKN.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData("123456")]
        [InlineData("ABCDEF")]
        [InlineData("1A2B3C")]
        [InlineData("abcdef")]
        [InlineData("a1b2c3")]
        [InlineData("X1y2Z3")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = WKN.TryFrom(input, out _);
                
            // assert
            Assert.Equal(WKN.Validation.Ok, result);
        }
        
        [Theory]
        // general
        [InlineData(null, WKN.Validation.Null)]
        [InlineData("", WKN.Validation.Empty)]
        // wrong length
        [InlineData("12345", WKN.Validation.WrongLength)]
        [InlineData("1234567", WKN.Validation.WrongLength)]
        // contains illegal character
        [InlineData("12345+", WKN.Validation.ContainsIllegalCharacter)]
        [InlineData("12-456", WKN.Validation.ContainsIllegalCharacter)]
        [InlineData("123 56", WKN.Validation.ContainsIllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, WKN.Validation expected)
        {
	        // act
            var result = WKN.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
    }
}
