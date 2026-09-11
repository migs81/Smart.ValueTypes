using System;
using Smart.ValueTypes.Types.Identifiers.ISBNs;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Identifiers.ISBNs
{
    public class Isbn10Test
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(ISBN10);

	        // act
            var result = new ISBN10();
            
            // assert
            Assert.Equal(expected, result);
            Assert.Equal(-1, result.CheckDigit);
            Assert.Equal("", result.Normalized);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("0306406152")]
        [InlineData("0-19853453-1")]
        [InlineData("0-131-10362-8")]
        [InlineData("0 201 61622 x")]
        [InlineData("080442957X")]
        [InlineData("012648550X")]
        [InlineData("0471117099")]
        [InlineData("026110330x")]
        [InlineData("0596007124")]
        [InlineData("0321125215")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // arrange
            var checkDigit = (input[^1] is 'x' or 'X') ? 10 : int.Parse(input[^1].ToString());
            
            // act
            var result = new ISBN10(input);
            
            // assert
            Assert.Equal(input, result);
            Assert.Equal(checkDigit, result.CheckDigit);
        }

        [Theory]
        // general
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        // too short
        [InlineData("0", typeof(InvalidIsbn10Exception))]
        [InlineData("030640615", typeof(InvalidIsbn10Exception))]
        // too long
        [InlineData("03064061520", typeof(InvalidIsbn10Exception))]
        // invalid characters
        [InlineData("030640615A", typeof(InvalidIsbn10Exception))]
        [InlineData("03064061F52", typeof(InvalidIsbn10Exception))]
        // invalid separators
        [InlineData("0306406152-", typeof(InvalidIsbn10Exception))]
        [InlineData("0306406152 ", typeof(InvalidIsbn10Exception))]
        [InlineData("-0306406152", typeof(InvalidIsbn10Exception))]
        [InlineData(" 0306406152", typeof(InvalidIsbn10Exception))]
        [InlineData("0--306406152", typeof(InvalidIsbn10Exception))]      
        [InlineData("0  306406152", typeof(InvalidIsbn10Exception))] 
        // invalid checksum
        [InlineData("0306406153", typeof(InvalidIsbn10Exception))]
        [InlineData("0198534532", typeof(InvalidIsbn10Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => new ISBN10(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData("0306406152")]
        [InlineData("0198534531")]
        [InlineData("0131103628")]
        [InlineData("020161622x")]
        [InlineData("080442957X")]
        [InlineData("012648550X")]
        [InlineData("0471117099")]
        [InlineData("026110330x")]
        [InlineData("0596007124")]
        [InlineData("0321125215")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = ISBN10.From(input);
            
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        // general
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        // too short
        [InlineData("0", typeof(InvalidIsbn10Exception))]
        [InlineData("030640615", typeof(InvalidIsbn10Exception))]
        // too long
        [InlineData("03064061520", typeof(InvalidIsbn10Exception))]
        // invalid characters
        [InlineData("030640615A", typeof(InvalidIsbn10Exception))]
        [InlineData("03064061F52", typeof(InvalidIsbn10Exception))]
        // invalid separators
        [InlineData("0306406152-", typeof(InvalidIsbn10Exception))]
        [InlineData("0306406152 ", typeof(InvalidIsbn10Exception))]
        [InlineData("-0306406152", typeof(InvalidIsbn10Exception))]
        [InlineData(" 0306406152", typeof(InvalidIsbn10Exception))]
        [InlineData("0--306406152", typeof(InvalidIsbn10Exception))]      
        [InlineData("0  306406152", typeof(InvalidIsbn10Exception))] 
        // invalid checksum
        [InlineData("0306406153", typeof(InvalidIsbn10Exception))]
        [InlineData("0198534532", typeof(InvalidIsbn10Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => ISBN10.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData("0306406152")]
        [InlineData("0198534531")]
        [InlineData("0131103628")]
        [InlineData("020161622x")]
        [InlineData("080442957X")]
        [InlineData("012648550X")]
        [InlineData("0471117099")]
        [InlineData("026110330x")]
        [InlineData("0596007124")]
        [InlineData("0321125215")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = ISBN10.TryFrom(input, out _);
                
            // assert
            Assert.Equal(ISBN10.Validation.Ok, result);
        }
        
        [Theory]
        // general
        [InlineData(null, ISBN10.Validation.Null)]
        [InlineData("", ISBN10.Validation.Empty)]
        // too short
        [InlineData("0", ISBN10.Validation.WrongLength)]
        [InlineData("030640615", ISBN10.Validation.WrongLength)]
        // too long
        [InlineData("03064061520", ISBN10.Validation.WrongLength)]
        // invalid characters
        [InlineData("030640615A", ISBN10.Validation.ContainsIllegalCharacter)]
        [InlineData("0306406F52", ISBN10.Validation.ContainsIllegalCharacter)]
        // invalid separators
        [InlineData("0306406152-", ISBN10.Validation.TrailingSeparator)]
        [InlineData("0306406152 ", ISBN10.Validation.TrailingSeparator)]
        [InlineData("-0306406152", ISBN10.Validation.LeadingSeparator)]
        [InlineData(" 0306406152", ISBN10.Validation.LeadingSeparator)]
        [InlineData("0--306406152", ISBN10.Validation.ConsecutiveSeparators)]      
        [InlineData("0  306406152", ISBN10.Validation.ConsecutiveSeparators)] 
        // invalid checksum
        [InlineData("0306406153", ISBN10.Validation.InvalidChecksum)]
        [InlineData("0198534532", ISBN10.Validation.InvalidChecksum)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, ISBN10.Validation expected)
        {
	        // act
            var result = ISBN10.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
        
        #region Parse
        
        [Theory]
        [InlineData("0-30640-615-2")]
        [InlineData("0 19853 453 1")]
        public void Parse_ValidInput_ShouldReturnObject(string input)
        {
            // arrange
            var expected = input.Replace(" ", "").Replace("-", "");
            
            // act
            var result = ISBN10.Parse(input);
                
            // assert
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData("0.30640.615.2", typeof(InvalidIsbn10Exception))]
        [InlineData("0/19853/453/1", typeof(InvalidIsbn10Exception))]
        public void Parse_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => ISBN10.Parse(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryParse
        
        [Theory]
        [InlineData("0-30640-615-2")]
        [InlineData("0 19853 453 1")]
        public void TryParse_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = ISBN10.TryParse(input, out _);
                
            // assert
            Assert.True(result);
        }
        
        [Theory]
        [InlineData("0.30640.615.2")]
        [InlineData("0/19853/453/1")]
        public void TryParse_WrongInput_ShouldThrowException(string input)
        {
            // act
            var result = ISBN10.TryParse(input, out _);
            
            // assert
            Assert.False(result);
        }
        
        #endregion
    }
}
