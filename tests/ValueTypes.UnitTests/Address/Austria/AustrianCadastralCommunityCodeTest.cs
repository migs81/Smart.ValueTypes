using System;
using Smart.ValueTypes.Types.Address.Austria;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Address.Austria
{
    public class AustrianCadastralCommunityCodeTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(AustrianCadastralCommunityCode);
            
            // act
            var result = new AustrianCadastralCommunityCode();
            
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
            var result = new AustrianCadastralCommunityCode(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1", typeof(InvalidAustrianCadastralCommunityCodeException))]
        [InlineData("1234", typeof(InvalidAustrianCadastralCommunityCodeException))]
        [InlineData("123456", typeof(InvalidAustrianCadastralCommunityCodeException))]
        [InlineData("1234a", typeof(InvalidAustrianCadastralCommunityCodeException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new AustrianCadastralCommunityCode(input));
            
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
            var result = AustrianCadastralCommunityCode.From(input);
                
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1", typeof(InvalidAustrianCadastralCommunityCodeException))]
        [InlineData("1234", typeof(InvalidAustrianCadastralCommunityCodeException))]
        [InlineData("123456", typeof(InvalidAustrianCadastralCommunityCodeException))]
        [InlineData("1234a", typeof(InvalidAustrianCadastralCommunityCodeException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => AustrianCadastralCommunityCode.From(input));
            
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
            var result = AustrianCadastralCommunityCode.TryFrom(input, out _);
                
            // assert
            Assert.Equal(AustrianCadastralCommunityCode.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, AustrianCadastralCommunityCode.Validation.Null)]
        [InlineData("", AustrianCadastralCommunityCode.Validation.Empty)]
        [InlineData("1", AustrianCadastralCommunityCode.Validation.InvalidLength)]
        [InlineData("1234", AustrianCadastralCommunityCode.Validation.InvalidLength)]
        [InlineData("123456", AustrianCadastralCommunityCode.Validation.InvalidLength)]
        [InlineData("1234a", AustrianCadastralCommunityCode.Validation.ContainsIllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, AustrianCadastralCommunityCode.Validation expected)
        {
            // act
            var result = AustrianCadastralCommunityCode.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
    }
}
