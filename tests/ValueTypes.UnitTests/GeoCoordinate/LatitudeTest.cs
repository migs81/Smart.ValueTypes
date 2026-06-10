using Migs.ValueTypes.Types.GeoCoordinate;
using Migs.ValueTypes.UnitTests.TestData;
using System;
using Xunit;

namespace Migs.ValueTypes.UnitTests.GeoCoordinate
{
    public class LatitudeTest
    {
        #region test data

        private static readonly double[] _validValues = NumberGenerator.NextDouble(1000, Latitude.MinValue, Latitude.MaxValue);
        private const double _tooLowValue = Latitude.MinValue - 1;
        private const double _tooHighValue = Latitude.MaxValue + 1;

        #endregion

        #region tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new Latitude();
            Assert.Equal(Latitude.Default, result);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = new Latitude(value);
                Assert.Equal(value, result, 0);
            }
        }

        [Theory]
        [InlineData(_tooLowValue, typeof(InvalidLatitudeException))]
        [InlineData(_tooHighValue, typeof(InvalidLatitudeException))]
        public void Constructor_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            var result = Record.Exception(() => new Latitude(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = Latitude.From(value);
                Assert.Equal(value, result, 0);
            }
        }

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in _validValues)
            {
                var result = Latitude.TryFrom(value, out _);
                Assert.Equal(Latitude.Validation.OK, result);
            }
        }

        [Theory]
        [InlineData(_tooLowValue, typeof(InvalidLatitudeException))]
        [InlineData(_tooHighValue, typeof(InvalidLatitudeException))]
        public void From_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            var result = Record.Exception(() => new Latitude(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(_tooLowValue, Latitude.Validation.TooLow)]
        [InlineData(_tooHighValue, Latitude.Validation.TooHigh)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Latitude.Validation expected)
        {
            var result = Latitude.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
