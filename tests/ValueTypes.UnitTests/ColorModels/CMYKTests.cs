using Smart.ValueTypes.Types.ColorModels;
using Xunit;

namespace Smart.ValueTypes.UnitTests.ColorModels
{
    public class CMYKTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(CMYK);
            
            // act
            var result = new CMYK();
            
            // assert
            Assert.Equal(expected, result);
            Assert.Equal(0f, result.Cyan);
            Assert.Equal(0f, result.Magenta);
            Assert.Equal(0f, result.Yellow);
            Assert.Equal(0f, result.Key);
        }

        [Theory]
        [InlineData(0f,0f,0f, 0f)]
        [InlineData(1f,1f,1f, 1f)]
        public void Constructor_ValidInput_ShouldReturnObject(float cyan, float magenta, float  yellow, float key)
        {
            // act
            var result = new CMYK(cyan, magenta, yellow, key);
            
            // assert
            Assert.Equal(cyan, result.Cyan);
            Assert.Equal(magenta, result.Magenta);
            Assert.Equal(yellow, result.Yellow);
            Assert.Equal(key, result.Key);
        }

        [Theory]
        [InlineData(-1f, 0f,0f, 0f)]
        [InlineData(0f, -1f,0f, 0f)]
        [InlineData(0f, 0f,-1f, 0f)]
        [InlineData(0f, 0f,0f, -1f)]
        [InlineData(1.1f, 0f,0f, 0f)]
        [InlineData(0f, 1.1f,0f, 0f)]
        [InlineData(0f, 0f,1.1f, 0f)]
        [InlineData(0f, 0f,0f, 1.1f)]
        public void Constructor_WrongInput_ShouldThrowException(float cyan, float magenta, float  yellow, float key)
        {
            // assert
            Assert.Throws<InvalidCmykException>(() => new CMYK(cyan, magenta, yellow, key));
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData(0f,0f,0f, 0f)]
        [InlineData(1f,1f,1f, 1f)]
        public void From_ValidInput_ShouldReturnObject(float cyan, float magenta, float  yellow, float key)
        {
            // act
            var result = CMYK.From(cyan, magenta, yellow, key);
            
            // assert
            Assert.Equal(cyan, result.Cyan);
            Assert.Equal(magenta, result.Magenta);
            Assert.Equal(yellow, result.Yellow);
            Assert.Equal(key, result.Key);
        }
        
        [Theory]
        [InlineData(-1f, 0f,0f, 0f)]
        [InlineData(0f, -1f,0f, 0f)]
        [InlineData(0f, 0f,-1f, 0f)]
        [InlineData(0f, 0f,0f, -1f)]
        [InlineData(1.1f, 0f, 0f, 0f)]
        [InlineData(0f, 1.1f,0f, 0f)]
        [InlineData(0f, 0f,1.1f, 0f)]
        [InlineData(0f, 0f,0f, 1.1f)]
        public void From_WrongInput_ShouldThrowException(float cyan, float magenta, float  yellow, float key)
        {
            // assert
            Assert.Throws<InvalidCmykException>(() => CMYK.From(cyan, magenta, yellow, key));
        }
        
        #endregion
        
        #region TryFrom

        [Theory]
        [InlineData(0f,0f,0f, 0f)]
        [InlineData(1f,1f,1f, 1f)]
        public void TryFrom_ValidInput_ShouldReturnOK(float cyan, float magenta, float  yellow, float key)
        {
            // act
            var result = CMYK.TryFrom(cyan, magenta, yellow, key, out var hsv);
            
            // assert
            Assert.Equal(CMYK.Validation.Ok, result);
            Assert.Equal(cyan, hsv.Cyan);
            Assert.Equal(magenta, hsv.Magenta);
            Assert.Equal(yellow, hsv.Yellow);
            Assert.Equal(key, hsv.Key);
        }
        
        [Theory]
        [InlineData(-1f, 0f,0f, 0f, CMYK.Validation.CyanTooLow)]
        [InlineData(0f, -1f,0f, 0f, CMYK.Validation.MagentaTooLow)]
        [InlineData(0f, 0f,-1f, 0f, CMYK.Validation.YellowTooLow)]
        [InlineData(0f, 0f,0f, -1f, CMYK.Validation.KeyTooLow)]
        [InlineData(361f, 0f,0f, 0f, CMYK.Validation.CyanTooHigh)]
        [InlineData(0f, 1.1f,0f, 0f, CMYK.Validation.MagentaToHigh)]
        [InlineData(0f, 0f,1.1f, 0f, CMYK.Validation.YellowTooHigh)]
        [InlineData(0f, 0f,0f, 1.1f, CMYK.Validation.KeyTooHigh)]
        public void TryFrom_WrongInput_ShouldReturnError(float cyan, float magenta, float  yellow, float key, CMYK.Validation expected)
        {
            // act
            var result = CMYK.TryFrom(cyan, magenta, yellow, key, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
