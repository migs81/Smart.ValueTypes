using System;
using Smart.ValueTypes.Types.Identifiers.ISBNs;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Identifiers.ISBNs
{
    public class Isbn13Test
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(ISBN13);

	        // act
            var result = new ISBN13();
            
            // assert
            Assert.Equal(expected, result);
            Assert.Equal("", result.Prefix);
            Assert.Equal(-1, result.CheckDigit);
            Assert.Equal("", result.Normalized);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("9780306406157")]
        [InlineData("9780126485509")]
        [InlineData("9780471117094")]
        [InlineData("9780261103306")]
        [InlineData("9780596007126")]
        [InlineData("9780321125217")]
        [InlineData("978-019853453-2")]
        [InlineData("978-0-13110362-7")]
        [InlineData("978-0-201-61622-4")]
        [InlineData("978 0 804 42957 3")]
        [InlineData("978-3-16-148410-0")]
        [InlineData("978-1-4028-9462-6")]
        [InlineData("978-0-306-40615-7")]
        [InlineData("978-0-13-468599-1")]
        [InlineData("978-1-56619-909-4")]
        [InlineData("979-8-886-45174-0")]
        [InlineData("979-0-201-12345-5")]
        [InlineData("979-1-23-456789-6")]
        [InlineData("979-1-09-061234-1")]
        [InlineData("979-8-765-43210-5")]
        [InlineData("978-3-598-21577-3")]
        [InlineData("979-1-567-89012-8")]
        [InlineData("978-0-123-45678-6")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // arrange
            var checkDigit = (input[^1] is 'x' or 'X') ? 10 : int.Parse(input[^1].ToString());
            var prefix = input[..3];
            
            // act
            var result = new ISBN13(input);
            
            // assert
            Assert.Equal(input, result);
            Assert.Equal(prefix, result.Prefix);
            Assert.Equal(checkDigit, result.CheckDigit);
        }

        [Theory]
        // general
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        // too short
        [InlineData("0", typeof(InvalidIsbn13Exception))]
        [InlineData("978030640615", typeof(InvalidIsbn13Exception))]
        // too long
        [InlineData("97803064061570", typeof(InvalidIsbn13Exception))]
        // invalid prefix
        [InlineData("9770306406157", typeof(InvalidIsbn13Exception))]
        [InlineData("9760306406157", typeof(InvalidIsbn13Exception))]
        [InlineData("9800306406157", typeof(InvalidIsbn13Exception))]
        [InlineData("1230306406157", typeof(InvalidIsbn13Exception))]
        // invalid characters
        [InlineData("97803064061A7", typeof(InvalidIsbn13Exception))]
        [InlineData("97803064061.57", typeof(InvalidIsbn13Exception))]
        [InlineData("97803064061/57", typeof(InvalidIsbn13Exception))]
        // invalid separators
        [InlineData("978-0-20161622-4-", typeof(InvalidIsbn13Exception))]
        [InlineData("978-0-20161622-4 ", typeof(InvalidIsbn13Exception))]
        [InlineData("978-0--20161622-4", typeof(InvalidIsbn13Exception))]
        [InlineData("978-0  20161622-4", typeof(InvalidIsbn13Exception))]
        // invalid checksum
        [InlineData("9780306406158", typeof(InvalidIsbn13Exception))]
        [InlineData("9780198534537", typeof(InvalidIsbn13Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => new ISBN13(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData("9780306406157")]
        [InlineData("978-019853453-2")]
        [InlineData("978-0-13110362-7")]
        [InlineData("978-0-201-61622-4")]
        [InlineData("978 0 804 42957 3")]
        [InlineData("979-8-886-45174-0")]
        [InlineData("9780126485509")]
        [InlineData("9780471117094")]
        [InlineData("9780261103306")]
        [InlineData("9780596007126")]
        [InlineData("9780321125217")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = ISBN13.From(input);
            
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        // general
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        // too short
        [InlineData("0", typeof(InvalidIsbn13Exception))]
        [InlineData("978030640615", typeof(InvalidIsbn13Exception))]
        // too long
        [InlineData("97803064061570", typeof(InvalidIsbn13Exception))]
        // invalid prefix
        [InlineData("9770306406157", typeof(InvalidIsbn13Exception))]
        [InlineData("9760306406157", typeof(InvalidIsbn13Exception))]
        [InlineData("9800306406157", typeof(InvalidIsbn13Exception))]
        [InlineData("1230306406157", typeof(InvalidIsbn13Exception))]
        // invalid characters
        [InlineData("97803064061A7", typeof(InvalidIsbn13Exception))]
        [InlineData("97803064061.57", typeof(InvalidIsbn13Exception))]
        [InlineData("97803064061/57", typeof(InvalidIsbn13Exception))]
        // invalid separators
        [InlineData("978-0-20161622-4-", typeof(InvalidIsbn13Exception))]
        [InlineData("978-0-20161622-4 ", typeof(InvalidIsbn13Exception))]
        [InlineData("978-0--20161622-4", typeof(InvalidIsbn13Exception))]
        [InlineData("978-0  20161622-4", typeof(InvalidIsbn13Exception))]
        // invalid checksum
        [InlineData("9780306406158", typeof(InvalidIsbn13Exception))]
        [InlineData("9780198534537", typeof(InvalidIsbn13Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => ISBN13.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData("9780306406157")]
        [InlineData("978-019853453-2")]
        [InlineData("978-0-13110362-7")]
        [InlineData("978-0-201-61622-4")]
        [InlineData("978 0 804 42957 3")]
        [InlineData("979-8-886-45174-0")]
        [InlineData("9780126485509")]
        [InlineData("9780471117094")]
        [InlineData("9780261103306")]
        [InlineData("9780596007126")]
        [InlineData("9780321125217")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = ISBN13.TryFrom(input, out _);
                
            // assert
            Assert.Equal(ISBN13.Validation.Ok, result);
        }
        
        [Theory]
        // general
        [InlineData(null, ISBN13.Validation.Null)]
        [InlineData("", ISBN13.Validation.Empty)]
        // too short
        [InlineData("0", ISBN13.Validation.WrongLength)]
        [InlineData("978030640615", ISBN13.Validation.WrongLength)]
        // too long
        [InlineData("97803064061570", ISBN13.Validation.WrongLength)]
        // invalid prefix
        [InlineData("9770306406157", ISBN13.Validation.InvalidPrefix)]
        [InlineData("9760306406157", ISBN13.Validation.InvalidPrefix)]
        [InlineData("9800306406157", ISBN13.Validation.InvalidPrefix)]
        [InlineData("1230306406157", ISBN13.Validation.InvalidPrefix)]
        // invalid characters

        [InlineData("97803064061A7", ISBN13.Validation.ContainsIllegalCharacter)]
        [InlineData("9780306406.57", ISBN13.Validation.ContainsIllegalCharacter)]
        [InlineData("9780306406/57", ISBN13.Validation.ContainsIllegalCharacter)]
        // invalid separators
        [InlineData("978-0-20161622-4-", ISBN13.Validation.TrailingSeparator)]
        [InlineData("978-0-20161622-4 ", ISBN13.Validation.TrailingSeparator)]
        [InlineData("978-0--20161622-4", ISBN13.Validation.ConsecutiveSeparators)]
        [InlineData("978-0  20161622-4", ISBN13.Validation.ConsecutiveSeparators)]
        // invalid checksum
        [InlineData("9780306406158", ISBN13.Validation.InvalidChecksum)]
        [InlineData("9780198534537", ISBN13.Validation.InvalidChecksum)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, ISBN13.Validation expected)
        {
	        // act
            var result = ISBN13.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
        
        #region Parse
        
        [Theory]
        [InlineData("9780306406157")]
        [InlineData("978-0-30640-615-7")]
        [InlineData("978 0 19853 453 2")]
        public void Parse_ValidInput_ShouldReturnObject(string input)
        {
            // arrange
            var expected = input.Replace(" ", "").Replace("-", "");
            
            // act
            var result = ISBN13.Parse(input);
                
            // assert
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData("978.030640.615.7", typeof(InvalidIsbn13Exception))]
        [InlineData("978.019853.453.2", typeof(InvalidIsbn13Exception))]
        public void Parse_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => ISBN13.Parse(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryParse
        
        [Theory]
        [InlineData("9780306406157")]
        [InlineData("978-0-30640-615-7")]
        [InlineData("978 0 19853 453 2")]
        public void TryParse_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = ISBN13.TryParse(input, out _);
                
            // assert
            Assert.True(result);
        }
        
        [Theory]
        [InlineData("978.030640.615.7")]
        [InlineData("978.019853.453.2")]
        public void TryParse_WrongInput_ShouldThrowException(string input)
        {
            // act
            var result = ISBN13.TryParse(input, out _);
            
            // assert
            Assert.False(result);
        }
        
        #endregion
    }
}
