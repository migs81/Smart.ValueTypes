using System;
using Smart.ValueTypes.Types.Identifiers;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Identifiers
{
    public class IsinTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(ISIN);

	        // act
            var result = new ISIN();
            
            // assert
            Assert.Equal(expected, result);
            Assert.Equal(-1, result.CheckDigit);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("AT0000A00007")]
        [InlineData("AT0F0TA00009")]
        [InlineData("DE000A0F6K18")]
        [InlineData("DE123A0F6K10")]
        [InlineData("US0004026250")]
        [InlineData("US0XF4026259")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // arrange
            var checkDigit = int.Parse(input[^1].ToString());
            var countryCode = input[..2];
            var basicNumber = input[2..11];
            
            // act
            var result = new ISIN(input);
            
            // assert
            Assert.Equal(input, result);
            Assert.Equal(countryCode, result.CountryCode);
            Assert.Equal(basicNumber, result.BasicNumber);
            Assert.Equal(checkDigit, result.CheckDigit);
        }

        [Theory]
        // general
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        // wrong length
        [InlineData("DE000A0F6K1", typeof(InvalidIsinException))]
        [InlineData("DE000A0F6K181", typeof(InvalidIsinException))]
        // invalid country code
        [InlineData("1E000A0F6K18", typeof(InvalidIsinException))]
        [InlineData("D1000A0F6K18", typeof(InvalidIsinException))]
        // invalid check digit
        [InlineData("DE000A0F6K1X", typeof(InvalidIsinException))]
        // contains illegal character
        [InlineData("DE-00A0F6K18", typeof(InvalidIsinException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => new ISIN(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData("AT0000A00007")]
        [InlineData("AT0F0TA00009")]
        [InlineData("DE000A0F6K18")]
        [InlineData("DE123A0F6K10")]
        [InlineData("US0004026250")]
        [InlineData("US0XF4026259")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = ISIN.From(input);
            
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        // general
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        // wrong length
        [InlineData("DE000A0F6K1", typeof(InvalidIsinException))]
        [InlineData("DE000A0F6K181", typeof(InvalidIsinException))]
        // invalid country code
        [InlineData("1E000A0F6K18", typeof(InvalidIsinException))]
        [InlineData("D1000A0F6K18", typeof(InvalidIsinException))]
        // invalid check digit
        [InlineData("DE000A0F6K1X", typeof(InvalidIsinException))]
        // contains illegal character
        [InlineData("DE-00A0F6K18", typeof(InvalidIsinException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => ISIN.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData("AT0000A00007")]
        [InlineData("AT0F0TA00009")]
        [InlineData("DE000A0F6K18")]
        [InlineData("DE123A0F6K10")]
        [InlineData("US0004026250")]
        [InlineData("US0XF4026259")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = ISIN.TryFrom(input, out _);
                
            // assert
            Assert.Equal(ISIN.Validation.Ok, result);
        }
        
        [Theory]
        // general
        [InlineData(null, ISIN.Validation.Null)]
        [InlineData("", ISIN.Validation.Empty)]
        // wrong length
        [InlineData("DE000A0F6K1", ISIN.Validation.WrongLength)]
        [InlineData("DE000A0F6K181", ISIN.Validation.WrongLength)]
        // invalid country code
        [InlineData("1E000A0F6K18", ISIN.Validation.InvalidCountryCode)]
        [InlineData("D1000A0F6K18", ISIN.Validation.InvalidCountryCode)]
        // invalid check digit
        [InlineData("DE000A0F6K1X", ISIN.Validation.InvalidCheckDigit)]
        // contains illegal character
        [InlineData("DE-00A0F6K18", ISIN.Validation.ContainsIllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, ISIN.Validation expected)
        {
	        // act
            var result = ISIN.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
    }
}
