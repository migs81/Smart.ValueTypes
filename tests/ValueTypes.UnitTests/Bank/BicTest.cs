using System;
using Smart.ValueTypes.Types.Bank;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Bank
{
    public class BicTest
    {
        #region test data

        private const string Length7 = "ABCDEFG"; // wrong length (must be 8 or 11!)
        private const string Length9 = "ABCDEFGHI"; // wrong length (must be 8 or 11!)
        private const string Length10 = "ABCDEFGHIJ"; // wrong length (must be 8 or 11!)
        private const string Length12 = "ABCDEFGHIJKL"; // wrong length (must be 8 or 11!)
        private const string WrongBankCode = "ABc%efgh"; // wrong bank code (must be alphanumeric
        private const string WrongCountryCode1 = "abcdA1gh"; // wrong country code (only letters)
        private const string WrongCountryCode2 = "abcd%Bgh"; // wrong country code (only letters)
        private const string WrongCityCode1 = "abcdef0h"; // wrong city code (first letter can not be 0)
        private const string WrongCityCode2 = "abcdef1h"; // wrong city code (first letter can not be 0)
        private const string WrongCityCode3 = "abcdefgo"; // wrong city code (first letter can not be 0)
        private const string WrongBranchCode1 = "abcdefghx23"; // wrong branch code (can not start with x unless it is XXX)
        private const string WrongBranchCode2 = "abcdefghxx3"; // wrong branch code (can not start with x unless it is XXX)

        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(BIC);
            
            // act
            var result = new BIC();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("BYLADEM1001")]
        [InlineData("HYVEDEMM")]
        [InlineData("RLNWATWWGTD")]
        [InlineData("KBSGCH22")]
        [InlineData("balpli22")]
        [InlineData("ZKBKCHZZ80A")]
        [InlineData("UBSWCHZH12A")]
        [InlineData("GIBAATWW")]
        [InlineData("RAIBLI22")]
        [InlineData("BLFLLI2X")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var bic = new BIC(input);
                
            // assert
            Assert.Equal(input.ToUpper(), bic);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(Length7, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(Length9, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(Length10, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(Length12, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(WrongBankCode, typeof(InvalidBicException))] // wrong bank code (must be alphanumeric
        [InlineData(WrongCountryCode1, typeof(InvalidBicException))] // wrong country code (only letters)
        [InlineData(WrongCountryCode2, typeof(InvalidBicException))] // wrong country code (only letters)
        [InlineData(WrongCityCode1, typeof(InvalidBicException))] // wrong city code (first letter can not be 0)
        [InlineData(WrongCityCode2, typeof(InvalidBicException))] // wrong city code (first letter can not be 1)
        [InlineData(WrongCityCode3, typeof(InvalidBicException))] // wrong city code (second letter can not be the letter o)
        [InlineData(WrongBranchCode1, typeof(InvalidBicException))] // wrong branch code (can not start with x unless it is XXX)
        [InlineData(WrongBranchCode2, typeof(InvalidBicException))] // wrong branch code (can not start with x unless it is XXX)
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new BIC(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData("BYLADEM1001")]
        [InlineData("HYVEDEMM")]
        [InlineData("RLNWATWWGTD")]
        [InlineData("KBSGCH22")]
        [InlineData("balpli22")]
        [InlineData("ZKBKCHZZ80A")]
        [InlineData("UBSWCHZH12A")]
        [InlineData("GIBAATWW")]
        [InlineData("RAIBLI22")]
        [InlineData("BLFLLI2X")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var bic = BIC.From(input);
                
            // assert
            Assert.Equal(input.ToUpper(), bic);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(Length7, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(Length9, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(Length10, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(Length12, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(WrongBankCode, typeof(InvalidBicException))] // wrong bank code (must be alphanumeric
        [InlineData(WrongCountryCode1, typeof(InvalidBicException))] // wrong country code (only letters)
        [InlineData(WrongCountryCode2, typeof(InvalidBicException))] // wrong country code (only letters)
        [InlineData(WrongCityCode1, typeof(InvalidBicException))] // wrong city code (first letter can not be 0)
        [InlineData(WrongCityCode2, typeof(InvalidBicException))] // wrong city code (first letter can not be 1)
        [InlineData(WrongCityCode3, typeof(InvalidBicException))] // wrong city code (second letter can not be the letter o)
        [InlineData(WrongBranchCode1, typeof(InvalidBicException))] // wrong branch code (can not start with x unless it is XXX)
        [InlineData(WrongBranchCode2, typeof(InvalidBicException))] // wrong branch code (can not start with x unless it is XXX)
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => BIC.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData("BYLADEM1001")]
        [InlineData("HYVEDEMM")]
        [InlineData("RLNWATWWGTD")]
        [InlineData("KBSGCH22")]
        [InlineData("balpli22")]
        [InlineData("ZKBKCHZZ80A")]
        [InlineData("UBSWCHZH12A")]
        [InlineData("GIBAATWW")]
        [InlineData("RAIBLI22")]
        [InlineData("BLFLLI2X")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = BIC.TryFrom(input, out _);
                
            // assert
            Assert.Equal(BIC.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, BIC.Validation.Null)]
        [InlineData("", BIC.Validation.Empty)]
        [InlineData(Length7, BIC.Validation.WrongLength)]
        [InlineData(Length9, BIC.Validation.WrongLength)]
        [InlineData(Length10, BIC.Validation.WrongLength)]
        [InlineData(Length12, BIC.Validation.WrongLength)]
        [InlineData(WrongBankCode, BIC.Validation.InvalidBankCode)]
        [InlineData(WrongCountryCode1, BIC.Validation.InvalidCountryCode)]
        [InlineData(WrongCountryCode2, BIC.Validation.InvalidCountryCode)]
        [InlineData(WrongCityCode1, BIC.Validation.InvalidCityCode)]
        [InlineData(WrongCityCode2, BIC.Validation.InvalidCityCode)]
        [InlineData(WrongCityCode3, BIC.Validation.InvalidCityCode)]
        [InlineData(WrongBranchCode1, BIC.Validation.InvalidBranchCode)]
        [InlineData(WrongBranchCode2, BIC.Validation.InvalidBranchCode)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, BIC.Validation expected)
        {
            // act
            var result = BIC.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
