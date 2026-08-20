using System;
using Smart.ValueTypes.Types.Address.Austria;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Address.Austria
{
    public class AustrianPostalCodeTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(AustrianPostalCode);
            
            // act
            var result = new AustrianPostalCode();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
            Assert.Equal(0, result.Zone);
            Assert.Equal(0, result.District);
            Assert.Equal(0, result.Route);
        }

        [Theory]
        [InlineData("0000")]
        [InlineData("9999")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // arrange
            var zone = int.Parse(input[0].ToString());
            var district = int.Parse(input[0..1]);
            var route = int.Parse(input[0..2]);
            
            // act
            var result = new AustrianPostalCode(input);
                
            // assert
            Assert.Equal(input, result);
            Assert.False(result.IsDefault);
            Assert.Equal(zone, result.Zone);
            Assert.Equal(district, result.District);
            Assert.Equal(route, result.Route);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1", typeof(InvalidAustrianPostalCodeException))]
        [InlineData("123", typeof(InvalidAustrianPostalCodeException))]
        [InlineData("12345", typeof(InvalidAustrianPostalCodeException))]
        [InlineData("123a", typeof(InvalidAustrianPostalCodeException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new AustrianPostalCode(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData("0000")]
        [InlineData("9999")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = AustrianPostalCode.From(input);
                
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1", typeof(InvalidAustrianPostalCodeException))]
        [InlineData("123", typeof(InvalidAustrianPostalCodeException))]
        [InlineData("12345", typeof(InvalidAustrianPostalCodeException))]
        [InlineData("123a", typeof(InvalidAustrianPostalCodeException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => AustrianPostalCode.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData("0000")]
        [InlineData("9999")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = AustrianPostalCode.TryFrom(input, out _);
                
            // assert
            Assert.Equal(AustrianPostalCode.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, AustrianPostalCode.Validation.Null)]
        [InlineData("", AustrianPostalCode.Validation.Empty)]
        [InlineData("1", AustrianPostalCode.Validation.InvalidLength)]
        [InlineData("123", AustrianPostalCode.Validation.InvalidLength)]
        [InlineData("12345", AustrianPostalCode.Validation.InvalidLength)]
        [InlineData("123a", AustrianPostalCode.Validation.ContainsIllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, AustrianPostalCode.Validation expected)
        {
            // act
            var result = AustrianPostalCode.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
    }
}
