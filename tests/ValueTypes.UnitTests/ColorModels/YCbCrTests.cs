using Smart.ValueTypes.Types.ColorModels;
using Xunit;

namespace Smart.ValueTypes.UnitTests.ColorModels
{
    public class YCbCrTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // act
            var result = new YCbCr();
            
            // assert
            Assert.Equal(YCbCr.Empty, result);
        }

        [Theory]
        [InlineData(0f,-0.5f,-0.5f)]
        [InlineData(1f,0.5f,0.5f)]
        public void Constructor_ValidInput_ShouldReturnObject(float hue, float saturation, float  lightness)
        {
            // act
            var result = new YCbCr(hue, saturation, lightness);
            
            // assert
            Assert.Equal(hue, result.Luma);
            Assert.Equal(saturation, result.BlueDifference);
            Assert.Equal(lightness, result.RedDifference);
        }

        [Theory]
        [InlineData(-0.1f, 0f,0f)]
        [InlineData(0f, -0.6f,0f)]
        [InlineData(0f, 0f,-0.6f)]
        [InlineData(1.1f, 0f,0f)]
        [InlineData(0f, 0.6f,0f)]
        [InlineData(0f, 0f,0.6f)]
        public void Constructor_WrongInput_ShouldThrowException(float hue, float saturation, float lightness)
        {
            // assert
            Assert.Throws<InvalidYCbCrException>(() => new YCbCr(hue, saturation, lightness));
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData(0f,-0.5f,-0.5f)]
        [InlineData(1f,0.5f,0.5f)]
        public void From_ValidInput_ShouldReturnObject(float hue, float saturation, float  lightness)
        {
            // act
            var result = YCbCr.From(hue, saturation, lightness);
            
            // assert
            Assert.Equal(hue, result.Luma);
            Assert.Equal(saturation, result.BlueDifference);
            Assert.Equal(lightness, result.RedDifference);
        }
        
        [Theory]
        [InlineData(-0.1f, 0f,0f)]
        [InlineData(0f, -0.6f,0f)]
        [InlineData(0f, 0f,-0.6f)]
        [InlineData(1.1f, 0f,0f)]
        [InlineData(0f, 0.6f,0f)]
        [InlineData(0f, 0f,0.6f)]
        public void From_WrongInput_ShouldThrowException(float hue, float saturation, float lightness)
        {
            // assert
            Assert.Throws<InvalidYCbCrException>(() => YCbCr.From(hue, saturation, lightness));
        }
        
        #endregion
        
        #region TryFrom

        [Theory]
        [InlineData(0f,-0.5f,-0.5f)]
        [InlineData(1f,0.5f,0.5f)]
        public void TryFrom_ValidInput_ShouldReturnOK(float hue, float saturation, float lightness)
        {
            // act
            var result = YCbCr.TryFrom(hue, saturation, lightness, out var hsl);
            
            // assert
            Assert.Equal(YCbCr.Validation.Ok, result);
            Assert.Equal(hue, hsl.Luma);
            Assert.Equal(saturation, hsl.BlueDifference);
            Assert.Equal(lightness, hsl.RedDifference);
        }
        
        [Theory]
        [InlineData(-1.1f, 0f,0f, YCbCr.Validation.LumaTooLow)]
        [InlineData(0f, -0.6f,0f, YCbCr.Validation.BlueDifferenceTooLow)]
        [InlineData(0f, 0f,-0.6f, YCbCr.Validation.RedDifferenceTooLow)]
        [InlineData(1.1f, 0f,0f, YCbCr.Validation.LumaTooHigh)]
        [InlineData(0f, 0.6f,0f, YCbCr.Validation.BlueDifferenceToHigh)]
        [InlineData(0f, 0f,0.6f, YCbCr.Validation.RedDifferenceTooHigh)]
        public void TryFrom_WrongInput_ShouldReturnError(float hue, float saturation, float lightness, YCbCr.Validation expected)
        {
            // act
            var result = YCbCr.TryFrom(hue, saturation, lightness, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
