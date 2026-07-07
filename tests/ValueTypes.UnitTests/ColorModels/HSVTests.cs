using Smart.ValueTypes.Types.ColorModels;
using Xunit;

namespace Smart.ValueTypes.UnitTests.ColorModels
{
    public class HSVTest
    {
        #region tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // act
            var result = new HSV();
            
            // assert
            Assert.Equal(HSV.Empty, result);
        }

        [Theory]
        [InlineData(0f,0f,0f)]
        [InlineData(360f,1f,1f)]
        public void Constructor_ValidInput_ShouldReturnObject(float hue, float saturation, float  value)
        {
            // act
            var result = new HSV(hue, saturation, value);
            
            // assert
            Assert.Equal(hue, result.Hue);
            Assert.Equal(saturation, result.Saturation);
            Assert.Equal(value, result.Value);
        }

        [Theory]
        [InlineData(-1f, 0f,0f)]
        [InlineData(0f, -1f,0f)]
        [InlineData(0f, 0f,-1f)]
        [InlineData(361f, 0f,0f)]
        [InlineData(0f, 1.1f,0f)]
        [InlineData(0f, 0f,1.1f)]
        public void Constructor_WrongInput_ShouldThrowException(float hue, float saturation, float value)
        {
            // assert
            Assert.Throws<InvalidHsvException>(() => new HSV(hue, saturation, value));
        }

        [Theory]
        [InlineData(0f,0f,0f)]
        [InlineData(360f,1f,1f)]
        public void From_ValidInput_ShouldReturnObject(float hue, float saturation, float  value)
        {
            // act
            var result = HSV.From(hue, saturation, value);
            
            // assert
            Assert.Equal(hue, result.Hue);
            Assert.Equal(saturation, result.Saturation);
            Assert.Equal(value, result.Value);
        }
        
        [Theory]
        [InlineData(-1f, 0f,0f)]
        [InlineData(0f, -1f,0f)]
        [InlineData(0f, 0f,-1f)]
        [InlineData(361f, 0f,0f)]
        [InlineData(0f, 1.1f,0f)]
        [InlineData(0f, 0f,1.1f)]
        public void From_WrongInput_ShouldThrowException(float hue, float saturation, float value)
        {
            // assert
            Assert.Throws<InvalidHsvException>(() => HSV.From(hue, saturation, value));
        }
        
        
        [Theory]
        [InlineData(0f,0f,0f)]
        [InlineData(360f,1f,1f)]
        public void TryFrom_ValidInput_ShouldReturnOK(float hue, float saturation, float value)
        {
            // act
            var result = HSV.TryFrom(hue, saturation, value, out var hsv);
            
            // assert
            Assert.Equal(HSV.Validation.Ok, result);
            Assert.Equal(hue, hsv.Hue);
            Assert.Equal(saturation, hsv.Saturation);
            Assert.Equal(value, hsv.Value);
        }
        
        [Theory]
        [InlineData(-1f, 0f,0f, HSV.Validation.HueTooLow)]
        [InlineData(0f, -1f,0f, HSV.Validation.SaturationTooLow)]
        [InlineData(0f, 0f,-1f, HSV.Validation.ValueTooLow)]
        [InlineData(361f, 0f,0f, HSV.Validation.HueTooHigh)]
        [InlineData(0f, 1.1f,0f, HSV.Validation.SaturationToHigh)]
        [InlineData(0f, 0f,1.1f, HSV.Validation.ValueTooHigh)]
        public void TryFrom_WrongInput_ShouldReturnError(float hue, float saturation, float value, HSV.Validation expected)
        {
            // act
            var result = HSV.TryFrom(hue, saturation, value, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
