using System;
using Smart.ValueTypes.Unfinished.Location;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Address.Austria
{
    public class AustrianLocalityCodeTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(AustrianLocalityCode);
            
            // act
            var result = new AustrianLocalityCode();
            
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
            var result = new AustrianLocalityCode(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1", typeof(InvalidAustrianLocalityCodeException))]
        [InlineData("1234", typeof(InvalidAustrianLocalityCodeException))]
        [InlineData("123456", typeof(InvalidAustrianLocalityCodeException))]
        [InlineData("1234a", typeof(InvalidAustrianLocalityCodeException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new AustrianLocalityCode(input));
            
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
            var result = AustrianLocalityCode.From(input);
                
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1", typeof(InvalidAustrianLocalityCodeException))]
        [InlineData("1234", typeof(InvalidAustrianLocalityCodeException))]
        [InlineData("123456", typeof(InvalidAustrianLocalityCodeException))]
        [InlineData("1234a", typeof(InvalidAustrianLocalityCodeException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => AustrianLocalityCode.From(input));
            
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
            var result = AustrianLocalityCode.TryFrom(input, out _);
                
            // assert
            Assert.Equal(AustrianLocalityCode.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, AustrianLocalityCode.Validation.Null)]
        [InlineData("", AustrianLocalityCode.Validation.Empty)]
        [InlineData("1", AustrianLocalityCode.Validation.InvalidLength)]
        [InlineData("1234", AustrianLocalityCode.Validation.InvalidLength)]
        [InlineData("123456", AustrianLocalityCode.Validation.InvalidLength)]
        [InlineData("1234a", AustrianLocalityCode.Validation.ContainsIllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, AustrianLocalityCode.Validation expected)
        {
            // act
            var result = AustrianLocalityCode.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
    }
}
