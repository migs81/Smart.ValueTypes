using System;
using Smart.ValueTypes.Types.Network;
using Smart.ValueTypes.UnitTests.TestData;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Network
{
    public class IPv6Test
    {
        #region test data

        private static readonly string[] ValidValues = IPv6Generator.CreateAddresses(10000);

        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // act
            var result = new IPv6();
            
            // assert
            Assert.Equal(IPv6.Empty, result);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in ValidValues)
            {
                // act
                var result = new IPv6(value);
                
                // assert
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(IPv6TestData.TooShortValue, typeof(InvalidIPv6Exception))]
        [InlineData(IPv6TestData.TooLongValue, typeof(InvalidIPv6Exception))]
        [InlineData(IPv6TestData.MultipleColons, typeof(InvalidIPv6Exception))]
        [InlineData(IPv6TestData.SegmentTooLong, typeof(InvalidIPv6Exception))]
        [InlineData(IPv6TestData.SegmentNotHex, typeof(InvalidIPv6Exception))]
        [InlineData(IPv6TestData.EndsWithColon, typeof(InvalidIPv6Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new IPv6(input));
            
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
                var result = IPv6.From(value);

                // assert
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(IPv6TestData.TooShortValue, typeof(InvalidIPv6Exception))]
        [InlineData(IPv6TestData.TooLongValue, typeof(InvalidIPv6Exception))]
        [InlineData(IPv6TestData.MultipleColons, typeof(InvalidIPv6Exception))]
        [InlineData(IPv6TestData.SegmentTooLong, typeof(InvalidIPv6Exception))]
        [InlineData(IPv6TestData.SegmentNotHex, typeof(InvalidIPv6Exception))]
        [InlineData(IPv6TestData.EndsWithColon, typeof(InvalidIPv6Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new IPv6(input));
            
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
                var result = IPv6.TryFrom(value, out _);
                
                // assert
                Assert.Equal(IPv6.Validation.Ok, result);
            }
        }
        
        [Theory]
        [InlineData(null, IPv6.Validation.Null)]
        [InlineData("", IPv6.Validation.Empty)]
        [InlineData(IPv6TestData.TooShortValue, IPv6.Validation.TooShort)]
        [InlineData(IPv6TestData.TooLongValue, IPv6.Validation.TooLong)]
        [InlineData(IPv6TestData.MultipleColons, IPv6.Validation.MultipleColons)]
        [InlineData(IPv6TestData.SegmentTooLong, IPv6.Validation.SegmentTooLong)]
        [InlineData(IPv6TestData.SegmentNotHex, IPv6.Validation.SegmentNotHex)]
        [InlineData(IPv6TestData.EndsWithColon, IPv6.Validation.EndsWithColon)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, IPv6.Validation expected)
        {
            // act
            var result = IPv6.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
