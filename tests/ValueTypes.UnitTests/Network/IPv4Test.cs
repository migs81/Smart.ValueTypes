using System;
using Smart.ValueTypes.Types.Network;
using Smart.ValueTypes.UnitTests.TestData;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Network
{
    public class IPv4Test
    {
        #region test data

        private static readonly string[] ValidValues = IPv4Generator.CreateAddresses(1000);

        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(IPv4);
            
            // act
            var result = new IPv4();
            
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
                var result = new IPv4(value);
                
                // assert
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(IPv4TestData.TooShortValue, typeof(InvalidIPv4Exception))]
        [InlineData(IPv4TestData.TooLongValue, typeof(InvalidIPv4Exception))]
        [InlineData(IPv4TestData.StartsWithDotValue, typeof(InvalidIPv4Exception))]
        [InlineData(IPv4TestData.EndsWithDotValue, typeof(InvalidIPv4Exception))]
        [InlineData(IPv4TestData.ContainsIllegalCharacterValue, typeof(InvalidIPv4Exception))]
        [InlineData(IPv4TestData.InvalidSegmentNumberValue, typeof(InvalidIPv4Exception))]
        [InlineData(IPv4TestData.InvalidSegmentCountValue, typeof(InvalidIPv4Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new IPv4(input));
            
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
                var result = IPv4.From(value);
                
                // assert
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(IPv4TestData.TooShortValue, typeof(InvalidIPv4Exception))]
        [InlineData(IPv4TestData.TooLongValue, typeof(InvalidIPv4Exception))]
        [InlineData(IPv4TestData.StartsWithDotValue, typeof(InvalidIPv4Exception))]
        [InlineData(IPv4TestData.EndsWithDotValue, typeof(InvalidIPv4Exception))]
        [InlineData(IPv4TestData.ContainsIllegalCharacterValue, typeof(InvalidIPv4Exception))]
        [InlineData(IPv4TestData.InvalidSegmentNumberValue, typeof(InvalidIPv4Exception))]
        [InlineData(IPv4TestData.InvalidSegmentCountValue, typeof(InvalidIPv4Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => IPv4.From(input));
            
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
                var result = IPv4.TryFrom(value, out _);
                Assert.Equal(IPv4.Validation.Ok, result);
            }
        }
        
        [Theory]
        [InlineData(null, IPv4.Validation.Null)]
        [InlineData("", IPv4.Validation.Empty)]
        [InlineData(IPv4TestData.TooShortValue, IPv4.Validation.TooShort)]
        [InlineData(IPv4TestData.TooLongValue, IPv4.Validation.TooLong)]
        [InlineData(IPv4TestData.StartsWithDotValue, IPv4.Validation.StartsWithDot)]
        [InlineData(IPv4TestData.EndsWithDotValue, IPv4.Validation.EndsWithDot)]
        [InlineData(IPv4TestData.ContainsIllegalCharacterValue, IPv4.Validation.ContainsIllegalCharacter)]
        [InlineData(IPv4TestData.InvalidSegmentNumberValue, IPv4.Validation.InvalidSegmentNumber)]
        [InlineData(IPv4TestData.InvalidSegmentCountValue, IPv4.Validation.InvalidSegmentCount)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, IPv4.Validation expected)
        {
            var result = IPv4.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
