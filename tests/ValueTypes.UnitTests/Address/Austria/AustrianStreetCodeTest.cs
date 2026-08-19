using System;
using Smart.ValueTypes.Unfinished.Location;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Address.Austria
{
    public class AustrianStreetCodeTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(AustrianStreetCode);
            
            // act
            var result = new AustrianStreetCode();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("000000")]
        [InlineData("999999")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new AustrianStreetCode(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1", typeof(InvalidAustrianStreetCodeException))]
        [InlineData("12345", typeof(InvalidAustrianStreetCodeException))]
        [InlineData("1234567", typeof(InvalidAustrianStreetCodeException))]
        [InlineData("12345a", typeof(InvalidAustrianStreetCodeException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new AustrianStreetCode(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData("000000")]
        [InlineData("999999")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = AustrianStreetCode.From(input);
                
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1", typeof(InvalidAustrianStreetCodeException))]
        [InlineData("12345", typeof(InvalidAustrianStreetCodeException))]
        [InlineData("1234567", typeof(InvalidAustrianStreetCodeException))]
        [InlineData("12345a", typeof(InvalidAustrianStreetCodeException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => AustrianStreetCode.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData("000000")]
        [InlineData("999999")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = AustrianStreetCode.TryFrom(input, out _);
                
            // assert
            Assert.Equal(AustrianStreetCode.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, AustrianStreetCode.Validation.Null)]
        [InlineData("", AustrianStreetCode.Validation.Empty)]
        [InlineData("1", AustrianStreetCode.Validation.InvalidLength)]
        [InlineData("12345", AustrianStreetCode.Validation.InvalidLength)]
        [InlineData("1234567", AustrianStreetCode.Validation.InvalidLength)]
        [InlineData("12345a", AustrianStreetCode.Validation.ContainsIllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, AustrianStreetCode.Validation expected)
        {
            // act
            var result = AustrianStreetCode.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
    }
}
