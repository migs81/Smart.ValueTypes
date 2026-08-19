using System;
using Smart.ValueTypes.Unfinished.Location;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Address.Austria
{
    public class AustrianMunicipalityCodeTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(AustrianMunicipalityCode);
            
            // act
            var result = new AustrianMunicipalityCode();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("00000")]
        [InlineData("99999")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new AustrianMunicipalityCode(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1", typeof(InvalidAustrianMunicipalityCodeException))]
        [InlineData("1234", typeof(InvalidAustrianMunicipalityCodeException))]
        [InlineData("123456", typeof(InvalidAustrianMunicipalityCodeException))]
        [InlineData("1234a", typeof(InvalidAustrianMunicipalityCodeException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new AustrianMunicipalityCode(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData("00000")]
        [InlineData("99999")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = AustrianMunicipalityCode.From(input);
                
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1", typeof(InvalidAustrianMunicipalityCodeException))]
        [InlineData("1234", typeof(InvalidAustrianMunicipalityCodeException))]
        [InlineData("123456", typeof(InvalidAustrianMunicipalityCodeException))]
        [InlineData("1234a", typeof(InvalidAustrianMunicipalityCodeException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => AustrianMunicipalityCode.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData("00000")]
        [InlineData("99999")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = AustrianMunicipalityCode.TryFrom(input, out _);
                
            // assert
            Assert.Equal(AustrianMunicipalityCode.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, AustrianMunicipalityCode.Validation.Null)]
        [InlineData("", AustrianMunicipalityCode.Validation.Empty)]
        [InlineData("1", AustrianMunicipalityCode.Validation.InvalidLength)]
        [InlineData("1234", AustrianMunicipalityCode.Validation.InvalidLength)]
        [InlineData("123456", AustrianMunicipalityCode.Validation.InvalidLength)]
        [InlineData("1234a", AustrianMunicipalityCode.Validation.ContainsIllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, AustrianMunicipalityCode.Validation expected)
        {
            // act
            var result = AustrianMunicipalityCode.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
    }
}
