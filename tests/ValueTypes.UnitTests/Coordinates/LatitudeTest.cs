using System;
using Smart.ValueTypes.Types.Coordinates;
using Smart.ValueTypes.UnitTests.TestData;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Coordinates
{
    public class LatitudeTest
    {
        #region test data

        private static readonly double[] ValidValues = NumberGenerator.NextDouble(1000, Latitude.MinValue, Latitude.MaxValue);
        private const double TooLowValue = Latitude.MinValue - 1;
        private const double TooHighValue = Latitude.MaxValue + 1;

        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(Latitude);
            
            // act
            var result = new Latitude();
            
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
                var result = new Latitude(value);
                
                // assert
                Assert.Equal(value, result, 0);
            }
        }

        [Theory]
        [InlineData(TooLowValue, typeof(InvalidLatitudeException))]
        [InlineData(TooHighValue, typeof(InvalidLatitudeException))]
        public void Constructor_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new Latitude(input));
            
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
                var result = Latitude.From(value);
                
                // assert
                Assert.Equal(value, result, 0);
            }
        }
        
        [Theory]
        [InlineData(TooLowValue, typeof(InvalidLatitudeException))]
        [InlineData(TooHighValue, typeof(InvalidLatitudeException))]
        public void From_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new Latitude(input));
            
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
                var result = Latitude.TryFrom(value, out _);
                
                // assert
                Assert.Equal(Latitude.Validation.Ok, result);
            }
        }
        
        [Theory]
        [InlineData(TooLowValue, Latitude.Validation.TooLow)]
        [InlineData(TooHighValue, Latitude.Validation.TooHigh)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Latitude.Validation expected)
        {
            // act
            var result = Latitude.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
