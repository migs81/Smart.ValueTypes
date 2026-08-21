using System;
using Smart.ValueTypes.Types.Bank;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Bank
{
    public class IbanTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(IBAN);
            
            // act
            var result = new IBAN();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("AL35202111090000000001234567")]
        [InlineData("CR23015108410026012345")]
        [InlineData("FR7630006000011234567890189")]
        [InlineData("SI56192001234567892")]
        [InlineData("TN5904018104004942712345")]
        [InlineData("RU0204452560040702810412345678901")]
        [InlineData("SC74MCBL01031234567890123456USD")]
        [InlineData("IQ20CBIQ861800101010500")]
        [InlineData("SA4420000001234567891234")]
        [InlineData("SV43ACAT00000000000000123123")]
        [InlineData("DJ2110002010010409943020008")]
        [InlineData("VG07ABVI0000000123456789")]
        [InlineData("TR320010009999901234567890")]
        [InlineData("DE75512108001245126199")]
        [InlineData("EG800002000156789012345180002")]
        [InlineData("MR1300020001010000123456753")]
        [InlineData("HR1723600001101234565")]
        [InlineData("BR1500000000000010932840814P2")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // arrange
            var countryCode = input[0..2];
            var checksum = int.Parse(input[2..4]);
            var accountIdentifier = input[4..];
            
            // act
            var result = new IBAN(input);
                
            // assert
            Assert.Equal(input, result);
            Assert.Equal(countryCode, result.CountryCode);
            Assert.Equal(checksum, result.Checksum);
            Assert.Equal(accountIdentifier, result.AccountIdentifier);
            Assert.False(result.IsDefault);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new IBAN(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData("AL35202111090000000001234567")]
        [InlineData("CR23015108410026012345")]
        [InlineData("FR7630006000011234567890189")]
        [InlineData("SI56192001234567892")]
        [InlineData("TN5904018104004942712345")]
        [InlineData("RU0204452560040702810412345678901")]
        [InlineData("SC74MCBL01031234567890123456USD")]
        [InlineData("IQ20CBIQ861800101010500")]
        [InlineData("SA4420000001234567891234")]
        [InlineData("SV43ACAT00000000000000123123")]
        [InlineData("DJ2110002010010409943020008")]
        [InlineData("VG07ABVI0000000123456789")]
        [InlineData("TR320010009999901234567890")]
        [InlineData("DE75512108001245126199")]
        [InlineData("EG800002000156789012345180002")]
        [InlineData("MR1300020001010000123456753")]
        [InlineData("HR1723600001101234565")]
        [InlineData("BR1500000000000010932840814P2")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = IBAN.From(input);
                
            // assert
            Assert.Equal(input.ToUpper(), result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new IBAN(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData("AL35202111090000000001234567")]
        [InlineData("CR23015108410026012345")]
        [InlineData("FR7630006000011234567890189")]
        [InlineData("SI56192001234567892")]
        [InlineData("TN5904018104004942712345")]
        [InlineData("RU0204452560040702810412345678901")]
        [InlineData("SC74MCBL01031234567890123456USD")]
        [InlineData("IQ20CBIQ861800101010500")]
        [InlineData("SA4420000001234567891234")]
        [InlineData("SV43ACAT00000000000000123123")]
        [InlineData("DJ2110002010010409943020008")]
        [InlineData("VG07ABVI0000000123456789")]
        [InlineData("TR320010009999901234567890")]
        [InlineData("DE75512108001245126199")]
        [InlineData("EG800002000156789012345180002")]
        [InlineData("MR1300020001010000123456753")]
        [InlineData("HR1723600001101234565")]
        [InlineData("BR1500000000000010932840814P2")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = IBAN.TryFrom(input, out _);
                
            // assert
            Assert.Equal(IBAN.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, IBAN.Validation.Null)]
        [InlineData("", IBAN.Validation.Empty)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, IBAN.Validation expected)
        {
            // act
            var result = IBAN.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
