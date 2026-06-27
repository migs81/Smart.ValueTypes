using Migs.ValueTypes.Types.IPs;
using Migs.ValueTypes.UnitTests.TestData;
using System;
using Xunit;

namespace Migs.ValueTypes.UnitTests.IPs
{
    public class IPv6Test
    {
        #region test data

        private static readonly string[] _validValues = IPv6Generator.CreateAddresses(10000);

        private const string _tooShortValue = "0";
        private const string _tooLongValue = "0000:0000:0000:0000:0000:0000:0000:0000:0000";
        private const string _MultipleColons = "0:::0:0:0:0:0:0";
        private const string _SegmentTooLong = "0:00000000:0:0:0:0:0:0.";
        private const string _SegmentNotHex = "0:G:0:0:0:0:0:0";
        private const string _EndsWithColon = "0:0:0:0:0:0:0:0:";

        #endregion

        #region Tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new IPv6();
            Assert.Equal(IPv6.Empty, result);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = new IPv6(value);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(_tooShortValue, typeof(InvalidIPv6Exception))]
        [InlineData(_tooLongValue, typeof(InvalidIPv6Exception))]
        [InlineData(_MultipleColons, typeof(InvalidIPv6Exception))]
        [InlineData(_SegmentTooLong, typeof(InvalidIPv6Exception))]
        [InlineData(_SegmentNotHex, typeof(InvalidIPv6Exception))]
        [InlineData(_EndsWithColon, typeof(InvalidIPv6Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new IPv6(input));
            Assert.Equal(expectedException, result.GetType());
        }


        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = IPv6.From(value);
                Assert.Equal(value, result);
            }
        }

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in _validValues)
            {
                var result = IPv6.TryFrom(value, out _);
                Assert.Equal(IPv6.Validation.Ok, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(_tooShortValue, typeof(InvalidIPv6Exception))]
        [InlineData(_tooLongValue, typeof(InvalidIPv6Exception))]
        [InlineData(_MultipleColons, typeof(InvalidIPv6Exception))]
        [InlineData(_SegmentTooLong, typeof(InvalidIPv6Exception))]
        [InlineData(_SegmentNotHex, typeof(InvalidIPv6Exception))]
        [InlineData(_EndsWithColon, typeof(InvalidIPv6Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new IPv6(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(null, IPv6.Validation.Null)]
        [InlineData("", IPv6.Validation.Empty)]
        [InlineData(_tooShortValue, IPv6.Validation.TooShort)]
        [InlineData(_tooLongValue, IPv6.Validation.TooLong)]
        [InlineData(_MultipleColons, IPv6.Validation.MultipleColons)]
        [InlineData(_SegmentTooLong, IPv6.Validation.SegmentTooLong)]
        [InlineData(_SegmentNotHex, IPv6.Validation.SegmentNotHex)]
        [InlineData(_EndsWithColon, IPv6.Validation.EndsWithColon)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, IPv6.Validation expected)
        {
            var result = IPv6.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
