using System;
using Smart.ValueTypes.Types.Coordinates;
using Smart.ValueTypes.UnitTests.TestData;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Coordinates
{
    public class LongitudeTest
    {
        #region test data

        private static readonly double[] ValidValues = NumberGenerator.NextDouble(1000, Longitude.MinValue, Longitude.MaxValue);
        private const double TooLowValue = Longitude.MinValue - 1;
        private const double TooHighValue = Longitude.MaxValue + 1;

        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // act
            var result = new Longitude();
            
            // assert
            Assert.Equal(Longitude.Empty, result);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in ValidValues)
            {
                // act
                var result = new Longitude(value);
                
                // assert
                Assert.Equal(value, result, 0);
            }
        }

        [Theory]
        [InlineData(TooLowValue, typeof(InvalidLongitudeException))]
        [InlineData(TooHighValue, typeof(InvalidLongitudeException))]
        public void Constructor_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new Longitude(input));
            
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
                var result = Longitude.From(value);
                
                // assert
                Assert.Equal(value, result, 0);
            }
        }
        
        [Theory]
        [InlineData(TooLowValue, typeof(InvalidLongitudeException))]
        [InlineData(TooHighValue, typeof(InvalidLongitudeException))]
        public void From_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new Longitude(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in ValidValues)
            {
                // act
                var result = Longitude.TryFrom(value, out _);
                
                // assert
                Assert.Equal(Longitude.Validation.Ok, result);
            }
        }
        
        [Theory]
        [InlineData(TooLowValue, Longitude.Validation.TooLow)]
        [InlineData(TooHighValue, Longitude.Validation.TooHigh)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Longitude.Validation expected)
        {
            // act
            var result = Longitude.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
