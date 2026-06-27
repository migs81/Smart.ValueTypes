using Migs.ValueTypes.Types.GeoCoordinate;
using Migs.ValueTypes.UnitTests.TestData;
using System;
using Xunit;

namespace Migs.ValueTypes.UnitTests.GeoCoordinate
{
    public class LongitudeTest
    {
        #region test data

        private static readonly double[] _validValues = NumberGenerator.NextDouble(1000, Longitude.MinValue, Longitude.MaxValue);
        private const double _tooLowValue = Longitude.MinValue - 1;
        private const double _tooHighValue = Longitude.MaxValue + 1;

        #endregion

        #region tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new Longitude();
            Assert.Equal(Longitude.Empty, result);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = new Longitude(value);
                Assert.Equal(value, result, 0);
            }
        }

        [Theory]
        [InlineData(_tooLowValue, typeof(InvalidLongitudeException))]
        [InlineData(_tooHighValue, typeof(InvalidLongitudeException))]
        public void Constructor_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            var result = Record.Exception(() => new Longitude(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = Longitude.From(value);
                Assert.Equal(value, result, 0);
            }
        }

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in _validValues)
            {
                var result = Longitude.TryFrom(value, out _);
                Assert.Equal(Longitude.Validation.Ok, result);
            }
        }

        [Theory]
        [InlineData(_tooLowValue, typeof(InvalidLongitudeException))]
        [InlineData(_tooHighValue, typeof(InvalidLongitudeException))]
        public void From_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            var result = Record.Exception(() => new Longitude(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(_tooLowValue, Longitude.Validation.TooLow)]
        [InlineData(_tooHighValue, Longitude.Validation.TooHigh)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Longitude.Validation expected)
        {
            var result = Longitude.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
