using Migs.ValueTypes.Types;
using System;
using Xunit;

namespace Migs.ValueTypes.UnitTests.BankAccount
{
    public class BICTest
    {
        #region test data

        private const string LENGTH_7 = "ABCDEFG"; // wrong length (must be 8 or 11!)
        private const string LENGTH_9 = "ABCDEFGHI"; // wrong length (must be 8 or 11!)
        private const string LENGTH_10 = "ABCDEFGHIJ"; // wrong length (must be 8 or 11!)
        private const string LENGTH_12 = "ABCDEFGHIJKL"; // wrong length (must be 8 or 11!)
        private const string WRONG_BANK_CODE = "ABc%efgh"; // wrong bank code (must be alphanumeric
        private const string WRONG_COUNTRY_CODE_1 = "abcdA1gh"; // wrong country code (only letters)
        private const string WRONG_COUNTRY_CODE_2 = "abcd%Bgh"; // wrong country code (only letters)
        private const string WRONG_CITY_CODE_1 = "abcdef0h"; // wrong city code (first letter can not be 0)
        private const string WRONG_CITY_CODE_2 = "abcdef1h"; // wrong city code (first letter can not be 0)
        private const string WRONG_CITY_CODE_3 = "abcdefgo"; // wrong city code (first letter can not be 0)
        private const string WRONG_BRANCH_CODE_1 = "abcdefghx23"; // wrong branch code (can not start with x unless it is XXX)
        private const string WRONG_BRANCH_CODE_2 = "abcdefghxx3"; // wrong branch code (can not start with x unless it is XXX)

        private static readonly string[] _validValues =
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

        #region tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var bic = new BIC();
            Assert.Equal(BIC.Default, bic);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var bic = new BIC(value);
                Assert.Equal(value.ToUpper(), bic);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(LENGTH_7, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(LENGTH_9, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(LENGTH_10, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(LENGTH_12, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(WRONG_BANK_CODE, typeof(InvalidBicException))] // wrong bank code (must be alphanumeric
        [InlineData(WRONG_COUNTRY_CODE_1, typeof(InvalidBicException))] // wrong country code (only letters)
        [InlineData(WRONG_COUNTRY_CODE_2, typeof(InvalidBicException))] // wrong country code (only letters)
        [InlineData(WRONG_CITY_CODE_1, typeof(InvalidBicException))] // wrong city code (first letter can not be 0)
        [InlineData(WRONG_CITY_CODE_2, typeof(InvalidBicException))] // wrong city code (first letter can not be 1)
        [InlineData(WRONG_CITY_CODE_3, typeof(InvalidBicException))] // wrong city code (second letter can not be the letter o)
        [InlineData(WRONG_BRANCH_CODE_1, typeof(InvalidBicException))] // wrong branch code (can not start with x unless it is XXX)
        [InlineData(WRONG_BRANCH_CODE_2, typeof(InvalidBicException))] // wrong branch code (can not start with x unless it is XXX)
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new BIC(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var bic = BIC.From(value);
                Assert.Equal(value.ToUpper(), bic);
            }
        }

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in _validValues)
            {
                var result = BIC.TryFrom(value, out _);
                Assert.Equal(BIC.Validation.Ok, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(LENGTH_7, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(LENGTH_9, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(LENGTH_10, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(LENGTH_12, typeof(InvalidBicException))] // wrong length (must be 8 or 11!)
        [InlineData(WRONG_BANK_CODE, typeof(InvalidBicException))] // wrong bank code (must be alphanumeric
        [InlineData(WRONG_COUNTRY_CODE_1, typeof(InvalidBicException))] // wrong country code (only letters)
        [InlineData(WRONG_COUNTRY_CODE_2, typeof(InvalidBicException))] // wrong country code (only letters)
        [InlineData(WRONG_CITY_CODE_1, typeof(InvalidBicException))] // wrong city code (first letter can not be 0)
        [InlineData(WRONG_CITY_CODE_2, typeof(InvalidBicException))] // wrong city code (first letter can not be 1)
        [InlineData(WRONG_CITY_CODE_3, typeof(InvalidBicException))] // wrong city code (second letter can not be the letter o)
        [InlineData(WRONG_BRANCH_CODE_1, typeof(InvalidBicException))] // wrong branch code (can not start with x unless it is XXX)
        [InlineData(WRONG_BRANCH_CODE_2, typeof(InvalidBicException))] // wrong branch code (can not start with x unless it is XXX)
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new BIC(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(null, BIC.Validation.Null)]
        [InlineData("", BIC.Validation.Empty)]
        [InlineData(LENGTH_7, BIC.Validation.WrongLength)]
        [InlineData(LENGTH_9, BIC.Validation.WrongLength)]
        [InlineData(LENGTH_10, BIC.Validation.WrongLength)]
        [InlineData(LENGTH_12, BIC.Validation.WrongLength)]
        [InlineData(WRONG_BANK_CODE, BIC.Validation.InvalidBankCode)]
        [InlineData(WRONG_COUNTRY_CODE_1, BIC.Validation.InvalidCountryCode)]
        [InlineData(WRONG_COUNTRY_CODE_2, BIC.Validation.InvalidCountryCode)]
        [InlineData(WRONG_CITY_CODE_1, BIC.Validation.InvalidCityCode)]
        [InlineData(WRONG_CITY_CODE_2, BIC.Validation.InvalidCityCode)]
        [InlineData(WRONG_CITY_CODE_3, BIC.Validation.InvalidCityCode)]
        [InlineData(WRONG_BRANCH_CODE_1, BIC.Validation.InvalidBranchCode)]
        [InlineData(WRONG_BRANCH_CODE_2, BIC.Validation.InvalidBranchCode)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, BIC.Validation expected)
        {
            var result = BIC.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
