using System;
using Smart.ValueTypes.Types.Graphics;
using Smart.ValueTypes.UnitTests.TestData;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Graphics
{
    public class OpacityTest
    {
        #region test data

        private static readonly double[] ValidValues = NumberGenerator.NextDouble(1000, Opacity.MinValue, Opacity.MaxValue);
        private const double TooLowValue = Opacity.MinValue - .1d;
        private const double TooHighValue = Opacity.MaxValue + .1d;

        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(Opacity);
            
            // act
            var result = new Opacity();
            
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
                var result = new Opacity(value);
                
                // assert
                Assert.Equal(value, result, 0);
            }
        }

        [Theory]
        [InlineData(TooLowValue, typeof(InvalidOpacityException))]
        [InlineData(TooHighValue, typeof(InvalidOpacityException))]
        public void Constructor_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new Opacity(input));
            
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
                var result = Opacity.From(value);
                
                // assert
                Assert.Equal(value, result, 0);
            }
        }
        
        [Theory]
        [InlineData(TooLowValue, typeof(InvalidOpacityException))]
        [InlineData(TooHighValue, typeof(InvalidOpacityException))]
        public void From_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new Opacity(input));
            
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
                var result = Opacity.TryFrom(value, out _);
                
                // assert
                Assert.Equal(Opacity.Validation.Ok, result);
            }
        }
        
        [Theory]
        [InlineData(TooLowValue, Opacity.Validation.TooLow)]
        [InlineData(TooHighValue, Opacity.Validation.TooHigh)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Opacity.Validation expected)
        {
            // act
            var result = Opacity.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
