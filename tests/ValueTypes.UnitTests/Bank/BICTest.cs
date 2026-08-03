using System;
using Smart.ValueTypes.Types.Bank;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Bank
{
    public class BICTest
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

        private static readonly string[] ValidValues =
        {
            "BYLADEM1001", "INGDDEFF", "BELADEBE", "CMCIDEDD", "HASPDEHH", "PBNKDEFF", "DAAEDEDD", "SOLADEST600",
            "HYVEDEMM", "COKSDE33", "BEVODEBB", "SSKMDEMM", "OPSKATWW", "EASYATW1", "RLNWATWW", "BKAUATWW", "GIBAATWW",
            "STSPAT2G", "SBGSAT2S", "RZOOAT2L", "ASPKAT2L", "RVSAAT2S", "VKBLAT2L", "RZTIAT22", "BFKKAT2K", "SPIHAT22",
            "RLNWATWWGTD", "POFICHBE", "CRESCHZZ80A", "ZKBKCHZZ80A", "MIGRCHZZ", "BCVLCH2L", "UBSWCHZH12A", "KBBECH22",
            "KBSGCH22", "UBSWCHZH80A", "BLKBCH22", "KBTGCH22", "VABECH22", "KBAGCH22", "LUKBCH2260A", "AHHBCH22",
            "LILALI2X", "VPBVLI2X", "BLFLLI2X", "HYIBLI22", "VOAGLI22", "CBKVLI2X", "BFRILI22", "RAIBLI22", "NBANLI22",
            "BALPLI22", "balpli22"
        };

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

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in ValidValues)
            {
                // act
                var bic = new BIC(value);
                
                // assert
                Assert.Equal(value.ToUpper(), bic);
            }
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
        
        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in ValidValues)
            {
                // act
                var bic = BIC.From(value);
                
                // assert
                Assert.Equal(value.ToUpper(), bic);
            }
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
            var result = Record.Exception(() => new BIC(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region TryFrom
        
        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in ValidValues)
            {
                // act
                var result = BIC.TryFrom(value, out _);
                
                // assert
                Assert.Equal(BIC.Validation.Ok, result);
            }
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
