using Smart.ValueTypes.Types.Numeric;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Numeric
{
    public class BoundedNumberTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // act
            var result = new BoundedNumber<int>();
            
            // assert
            Assert.Equal(BoundedNumber<int>.Empty, result);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 2)]
        public void Constructor_ValidInput_ShouldReturnObject(int value, int minValue, int maxValue)
        {
            // act
            var result = new BoundedNumber<int>(value, minValue, maxValue);
            
            // assert
            Assert.Equal(value, result);
            Assert.Equal(minValue, result.Min);
            Assert.Equal(maxValue, result.Max);
        }

        [Theory]
        [InlineData(1, 2, 0)] // InvalidBounds
        [InlineData(0, 1, 2)] // ValueTooLow
        [InlineData(2, 1, 0)] // ValueTooHigh
        public void Constructor_WrongInput_ShouldThrowException(int value, int minValue, int maxValue)
        {
            // assert
            Assert.Throws<InvalidBoundedNumberException>(() => new BoundedNumber<int>(value, minValue, maxValue));
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 2)]
        public void From_ValidInput_ShouldReturnObject(int value, int minValue, int maxValue)
        {
            // act
            var result = BoundedNumber<int>.From(value, minValue, maxValue);
            
            // assert
            Assert.Equal(value, result);
            Assert.Equal(minValue, result.Min);
            Assert.Equal(maxValue, result.Max);
        }
        
        [Theory]
        [InlineData(1, 2, 0)] // InvalidBounds
        [InlineData(0, 1, 2)] // ValueTooLow
        [InlineData(2, 1, 0)] // ValueTooHigh
        public void From_WrongInput_ShouldThrowException(int value, int minValue, int maxValue)
        {
            // assert
            Assert.Throws<InvalidBoundedNumberException>(() => BoundedNumber<int>.From(value, minValue, maxValue));
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 2)]
        public void TryFrom_ValidInput_ShouldReturnOK(int value, int minValue, int maxValue)
        {
            // act
            var result = BoundedNumber<int>.TryFrom(value, minValue, maxValue, out _);
            
            // assert
            Assert.Equal(BoundedNumber<int>.Validation.Ok,  result);
        }
        
        [Theory]
        [InlineData(1, 2, 0, BoundedNumber<int>.Validation.InvalidBounds)]
        [InlineData(0, 1, 2, BoundedNumber<int>.Validation.ValueTooLow)]  
        [InlineData(2, 0, 1, BoundedNumber<int>.Validation.ValueTooHigh)]
        public void TryFrom_WrongInput_ShouldReturnError(int value, int minValue, int maxValue, BoundedNumber<int>.Validation expected)
        {
            // act
            var result = BoundedNumber<int>.TryFrom(value, minValue, maxValue, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
