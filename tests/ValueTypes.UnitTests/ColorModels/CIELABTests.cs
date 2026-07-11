using Smart.ValueTypes.Types.ColorModels;
using Xunit;

namespace Smart.ValueTypes.UnitTests.ColorModels
{
    public class CIELABTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // act
            var result = new CIELAB();
            
            // assert
            Assert.Equal(CIELAB.Empty, result);
        }

        [Theory]
        [InlineData(0f,-128f,-128f)]
        [InlineData(100f,127f,127f)]
        public void Constructor_ValidInput_ShouldReturnObject(float hue, float saturation, float  lightness)
        {
            // act
            var result = new CIELAB(hue, saturation, lightness);
            
            // assert
            Assert.Equal(hue, result.Lightness);
            Assert.Equal(saturation, result.GreenToRed);
            Assert.Equal(lightness, result.BlueToYellow);
        }

        [Theory]
        [InlineData(-1f, 0f,0f)]
        [InlineData(0f, -129f,0f)]
        [InlineData(0f, 0f,-129f)]
        [InlineData(101f, 0f,0f)]
        [InlineData(0f, 128f,0f)]
        [InlineData(0f, 0f,128f)]
        public void Constructor_WrongInput_ShouldThrowException(float hue, float saturation, float lightness)
        {
            // assert
            Assert.Throws<InvalidCielabException>(() => new CIELAB(hue, saturation, lightness));
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData(0f,-128f,-128f)]
        [InlineData(100f,127f,127f)]
        public void From_ValidInput_ShouldReturnObject(float hue, float saturation, float  lightness)
        {
            // act
            var result = CIELAB.From(hue, saturation, lightness);
            
            // assert
            Assert.Equal(hue, result.Lightness);
            Assert.Equal(saturation, result.GreenToRed);
            Assert.Equal(lightness, result.BlueToYellow);
        }
        
        [Theory]
        [InlineData(-1f, 0f,0f)]
        [InlineData(0f, -129f,0f)]
        [InlineData(0f, 0f,-129f)]
        [InlineData(101f, 0f,0f)]
        [InlineData(0f, 128f,0f)]
        [InlineData(0f, 0f,128f)]
        public void From_WrongInput_ShouldThrowException(float hue, float saturation, float lightness)
        {
            // assert
            Assert.Throws<InvalidCielabException>(() => CIELAB.From(hue, saturation, lightness));
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData(0f,-128f,-128f)]
        [InlineData(100f,127f,127f)]
        public void TryFrom_ValidInput_ShouldReturnOK(float hue, float saturation, float lightness)
        {
            // act
            var result = CIELAB.TryFrom(hue, saturation, lightness, out var hsl);
            
            // assert
            Assert.Equal(CIELAB.Validation.Ok, result);
            Assert.Equal(hue, hsl.Lightness);
            Assert.Equal(saturation, hsl.GreenToRed);
            Assert.Equal(lightness, hsl.BlueToYellow);
        }
        
        [Theory]
        [InlineData(-1f, 0f,0f, CIELAB.Validation.LightnessTooLow)]
        [InlineData(0f, -129f,0f, CIELAB.Validation.GreenToRedTooLow)]
        [InlineData(0f, 0f,-129f, CIELAB.Validation.BlueToYellowTooLow)]
        [InlineData(101f, 0f,0f, CIELAB.Validation.LightnessTooHigh)]
        [InlineData(0f, 128f,0f, CIELAB.Validation.GreenToRedToHigh)]
        [InlineData(0f, 0f,128f, CIELAB.Validation.BlueToYellowTooHigh)]
        public void TryFrom_WrongInput_ShouldReturnError(float hue, float saturation, float lightness, CIELAB.Validation expected)
        {
            // act
            var result = CIELAB.TryFrom(hue, saturation, lightness, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
