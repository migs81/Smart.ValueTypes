using System;
using Smart.ValueTypes.Types.Address.Austria;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Address.Austria
{
    public class AustrianAddressCodeTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(AustrianAddressCode);
            
            // act
            var result = new AustrianAddressCode();
            
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
            var result = new AustrianAddressCode(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1", typeof(InvalidAustrianAddressCodeException))]
        [InlineData("1234", typeof(InvalidAustrianAddressCodeException))]
        [InlineData("123456", typeof(InvalidAustrianAddressCodeException))]
        [InlineData("1234a", typeof(InvalidAustrianAddressCodeException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new AustrianAddressCode(input));
            
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
            var result = AustrianAddressCode.From(input);
                
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1", typeof(InvalidAustrianAddressCodeException))]
        [InlineData("1234", typeof(InvalidAustrianAddressCodeException))]
        [InlineData("123456", typeof(InvalidAustrianAddressCodeException))]
        [InlineData("1234a", typeof(InvalidAustrianAddressCodeException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => AustrianAddressCode.From(input));
            
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
            var result = AustrianAddressCode.TryFrom(input, out _);
                
            // assert
            Assert.Equal(AustrianAddressCode.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, AustrianAddressCode.Validation.Null)]
        [InlineData("", AustrianAddressCode.Validation.Empty)]
        [InlineData("1", AustrianAddressCode.Validation.InvalidLength)]
        [InlineData("1234", AustrianAddressCode.Validation.InvalidLength)]
        [InlineData("123456", AustrianAddressCode.Validation.InvalidLength)]
        [InlineData("1234a", AustrianAddressCode.Validation.ContainsIllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, AustrianAddressCode.Validation expected)
        {
            // act
            var result = AustrianAddressCode.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
    }
}
