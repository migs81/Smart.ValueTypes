using Migs.ValueTypes.Types.IPs;
using Migs.ValueTypes.UnitTests.TestData;
using System;
using Xunit;

namespace Migs.ValueTypes.UnitTests.IPs
{
    public class IPv4Test
    {
        #region test data

        private static readonly string[] _validValues = IPv4Generator.CreateAddresses(1000);

        private const string _tooShortValue = "255";
        private const string _tooLongValue = "255.255.255.255.0";
        private const string _startsWithDotValue = ".127.0.0.1";
        private const string _endsWithDotValue = "127.0.0.1.";
        private const string _containsIllegalCharacterValue = "127.a.0.1";
        private const string _invalidSegmentNumberValue = "1.0.0.256";
        private const string _invalidSegmentCountValue = "0.0.0.0.0";

        #endregion

        #region Tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new IPv4();
            Assert.Equal(IPv4.Empty, result);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = new IPv4(value);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(_tooShortValue, typeof(InvalidIPv4Exception))]
        [InlineData(_tooLongValue, typeof(InvalidIPv4Exception))]
        [InlineData(_startsWithDotValue, typeof(InvalidIPv4Exception))]
        [InlineData(_endsWithDotValue, typeof(InvalidIPv4Exception))]
        [InlineData(_containsIllegalCharacterValue, typeof(InvalidIPv4Exception))]
        [InlineData(_invalidSegmentNumberValue, typeof(InvalidIPv4Exception))]
        [InlineData(_invalidSegmentCountValue, typeof(InvalidIPv4Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new IPv4(input));
            Assert.Equal(expectedException, result.GetType());
        }


        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = IPv4.From(value);
                Assert.Equal(value, result);
            }
        }

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in _validValues)
            {
                var result = IPv4.TryFrom(value, out _);
                Assert.Equal(IPv4.Validation.Ok, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(_tooShortValue, typeof(InvalidIPv4Exception))]
        [InlineData(_tooLongValue, typeof(InvalidIPv4Exception))]
        [InlineData(_startsWithDotValue, typeof(InvalidIPv4Exception))]
        [InlineData(_endsWithDotValue, typeof(InvalidIPv4Exception))]
        [InlineData(_containsIllegalCharacterValue, typeof(InvalidIPv4Exception))]
        [InlineData(_invalidSegmentNumberValue, typeof(InvalidIPv4Exception))]
        [InlineData(_invalidSegmentCountValue, typeof(InvalidIPv4Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new IPv4(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(null, IPv4.Validation.Null)]
        [InlineData("", IPv4.Validation.Empty)]
        [InlineData(_tooShortValue, IPv4.Validation.TooShort)]
        [InlineData(_tooLongValue, IPv4.Validation.TooLong)]
        [InlineData(_startsWithDotValue, IPv4.Validation.StartsWithDot)]
        [InlineData(_endsWithDotValue, IPv4.Validation.EndsWithDot)]
        [InlineData(_containsIllegalCharacterValue, IPv4.Validation.ContainsIllegalCharacter)]
        [InlineData(_invalidSegmentNumberValue, IPv4.Validation.InvalidSegmentNumber)]
        [InlineData(_invalidSegmentCountValue, IPv4.Validation.InvalidSegmentCount)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, IPv4.Validation expected)
        {
            var result = IPv4.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
