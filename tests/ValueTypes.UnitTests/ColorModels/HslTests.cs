using Smart.ValueTypes.Types.ColorModels;
using Xunit;

namespace Smart.ValueTypes.UnitTests.ColorModels
{
    public class HSLTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(HSL);
            
            // act
            var result = new HSL();
            
            // assert
            Assert.Equal(expected, result);
            Assert.Equal(0f, result.Hue);
            Assert.Equal(0f, result.Saturation);
            Assert.Equal(0f, result.Lightness);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData(0f,0f,0f)]
        [InlineData(360f,1f,1f)]
        public void Constructor_ValidInput_ShouldReturnObject(float hue, float saturation, float  lightness)
        {
            // act
            var result = new HSL(hue, saturation, lightness);
            
            // assert
            Assert.Equal(hue, result.Hue);
            Assert.Equal(saturation, result.Saturation);
            Assert.Equal(lightness, result.Lightness);
        }

        [Theory]
        [InlineData(-1f, 0f,0f)]
        [InlineData(0f, -1f,0f)]
        [InlineData(0f, 0f,-1f)]
        [InlineData(361f, 0f,0f)]
        [InlineData(0f, 1.1f,0f)]
        [InlineData(0f, 0f,1.1f)]
        public void Constructor_WrongInput_ShouldThrowException(float hue, float saturation, float lightness)
        {
            // assert
            Assert.Throws<InvalidHslException>(() => new HSL(hue, saturation, lightness));
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData(0f,0f,0f)]
        [InlineData(360f,1f,1f)]
        public void From_ValidInput_ShouldReturnObject(float hue, float saturation, float  lightness)
        {
            // act
            var result = HSL.From(hue, saturation, lightness);
            
            // assert
            Assert.Equal(hue, result.Hue);
            Assert.Equal(saturation, result.Saturation);
            Assert.Equal(lightness, result.Lightness);
        }
        
        [Theory]
        [InlineData(-1f, 0f,0f)]
        [InlineData(0f, -1f,0f)]
        [InlineData(0f, 0f,-1f)]
        [InlineData(361f, 0f,0f)]
        [InlineData(0f, 1.1f,0f)]
        [InlineData(0f, 0f,1.1f)]
        public void From_WrongInput_ShouldThrowException(float hue, float saturation, float lightness)
        {
            // assert
            Assert.Throws<InvalidHslException>(() => HSL.From(hue, saturation, lightness));
        }
        
        #endregion
        
        #region TryFrom

        [Theory]
        [InlineData(0f,0f,0f)]
        [InlineData(360f,1f,1f)]
        public void TryFrom_ValidInput_ShouldReturnOK(float hue, float saturation, float lightness)
        {
            // act
            var result = HSL.TryFrom(hue, saturation, lightness, out var hsl);
            
            // assert
            Assert.Equal(HSL.Validation.Ok, result);
            Assert.Equal(hue, hsl.Hue);
            Assert.Equal(saturation, hsl.Saturation);
            Assert.Equal(lightness, hsl.Lightness);
        }
        
        [Theory]
        [InlineData(-1f, 0f,0f, HSL.Validation.HueTooLow)]
        [InlineData(0f, -1f,0f, HSL.Validation.SaturationTooLow)]
        [InlineData(0f, 0f,-1f, HSL.Validation.LightnessTooLow)]
        [InlineData(361f, 0f,0f, HSL.Validation.HueTooHigh)]
        [InlineData(0f, 1.1f,0f, HSL.Validation.SaturationToHigh)]
        [InlineData(0f, 0f,1.1f, HSL.Validation.LightnessTooHigh)]
        public void TryFrom_WrongInput_ShouldReturnError(float hue, float saturation, float lightness, HSL.Validation expected)
        {
            // act
            var result = HSL.TryFrom(hue, saturation, lightness, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
