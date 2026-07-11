using Smart.ValueTypes.Types.IPs;
using Smart.ValueTypes.UnitTests.TestData;
using Xunit;

namespace Smart.ValueTypes.UnitTests.IPs
{
    public class IPTest
    {
        #region test data

        private static readonly string[] ValidIPv4 = IPv4Generator.CreateAddresses(1000);
        private static readonly string[] ValidIPv6 = IPv4Generator.CreateAddresses(1000);

        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // act
            var result = new IP();
            
            // assert
            Assert.Equal(IP.Empty, result);
        }

        [Fact]
        public void Constructor_ValidIPv4Input_ShouldReturnObject()
        {
            foreach (var value in ValidIPv4)
            {
                // act
                var result = new IP(value);
                
                // assert
                Assert.Equal(value, result);
            }
        }

        [Fact]
        public void Constructor_ValidIPv6Input_ShouldReturnObject()
        {
            foreach (var value in ValidIPv6)
            {
                // act
                var result = new IP(value);
                
                // assert
                Assert.Equal(value, result);
            }
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(IPv4TestData.TooShortValue)]
        [InlineData(IPv4TestData.TooLongValue)]
        [InlineData(IPv4TestData.StartsWithDotValue)]
        [InlineData(IPv4TestData.EndsWithDotValue)]
        [InlineData(IPv4TestData.ContainsIllegalCharacterValue)]
        [InlineData(IPv4TestData.InvalidSegmentNumberValue)]
        [InlineData(IPv4TestData.InvalidSegmentCountValue)]
        [InlineData(IPv6TestData.TooShortValue)]
        [InlineData(IPv6TestData.TooLongValue)]
        [InlineData(IPv6TestData.EndsWithColon)]
        [InlineData(IPv6TestData.MultipleColons)]
        [InlineData(IPv6TestData.SegmentNotHex)]
        [InlineData(IPv6TestData.SegmentTooLong)]
        public void Constructor_WrongInput_ShouldThrowException(string input)
        {
            // assert
            Assert.Throws<InvalidIPException>(() => new IP(input));
        }

        #endregion
        
        #region From

        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in ValidIPv4)
            {
                // act
                var result = IP.From(value);
                
                // assert
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(IPv4TestData.TooShortValue)]
        [InlineData(IPv4TestData.TooLongValue)]
        [InlineData(IPv4TestData.StartsWithDotValue)]
        [InlineData(IPv4TestData.EndsWithDotValue)]
        [InlineData(IPv4TestData.ContainsIllegalCharacterValue)]
        [InlineData(IPv4TestData.InvalidSegmentNumberValue)]
        [InlineData(IPv4TestData.InvalidSegmentCountValue)]
        [InlineData(IPv6TestData.TooShortValue)]
        [InlineData(IPv6TestData.TooLongValue)]
        [InlineData(IPv6TestData.EndsWithColon)]
        [InlineData(IPv6TestData.MultipleColons)]
        [InlineData(IPv6TestData.SegmentNotHex)]
        [InlineData(IPv6TestData.SegmentTooLong)]
        public void From_WrongInput_ShouldThrowException(string input)
        {
            // assert
            Assert.Throws<InvalidIPException>(() => new IP(input));
        }
        
        #endregion

        #region TryFrom

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnTrue()
        {
            foreach (var value in ValidIPv4)
            {
                // act
                var result = IP.TryFrom(value, out _);
                
                // assert
                Assert.True(result);
            }
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(IPv4TestData.TooShortValue)]
        [InlineData(IPv4TestData.TooLongValue)]
        [InlineData(IPv4TestData.StartsWithDotValue)]
        [InlineData(IPv4TestData.EndsWithDotValue)]
        [InlineData(IPv4TestData.ContainsIllegalCharacterValue)]
        [InlineData(IPv4TestData.InvalidSegmentNumberValue)]
        [InlineData(IPv4TestData.InvalidSegmentCountValue)]
        [InlineData(IPv6TestData.TooShortValue)]
        [InlineData(IPv6TestData.TooLongValue)]
        [InlineData(IPv6TestData.EndsWithColon)]
        [InlineData(IPv6TestData.MultipleColons)]
        [InlineData(IPv6TestData.SegmentNotHex)]
        [InlineData(IPv6TestData.SegmentTooLong)]
        public void TryFrom_WrongInput_ShouldReturnFalse(string input)
        {
            // act
            var result = IP.TryFrom(input, out _);
            
            // assert
            Assert.False(result);
        }

        #endregion
    }
}
