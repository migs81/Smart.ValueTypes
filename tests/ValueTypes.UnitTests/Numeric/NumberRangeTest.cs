using Smart.ValueTypes.Types.Numeric;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Numeric
{
    public class NumberRangeTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(NumberRange<int>);
            
            // act
            var result = new NumberRange<int>();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData( 1, 10)]
        public void Constructor_ValidInput_ShouldReturnObject(int minValue, int maxValue)
        {
            // act
            var result = new NumberRange<int>(minValue, maxValue);
            
            // assert
            Assert.Equal(minValue, result.Min);
            Assert.Equal(maxValue, result.Max);
        }

        [Theory]
        [InlineData( 1, 0)] // InvalidRange
        [InlineData( 10, 5)] // InvalidRange
        [InlineData( -1, -2)] // InvalidRange
        public void Constructor_WrongInput_ShouldThrowException(int minValue, int maxValue)
        {
            // assert
            Assert.Throws<InvalidNumberRangeException>(() => new NumberRange<int>(minValue, maxValue));
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData(0, 0)]
        [InlineData( 1, 10)]
        public void From_ValidInput_ShouldReturnObject(int minValue, int maxValue)
        {
            // act
            var result = NumberRange<int>.From(minValue, maxValue);
            
            // assert
            Assert.Equal(minValue, result.Min);
            Assert.Equal(maxValue, result.Max);
        }
        
        [Theory]
        [InlineData( 1, 0)] // InvalidRange
        [InlineData( 10, 5)] // InvalidRange
        [InlineData( -1, -2)] // InvalidRange
        public void From_WrongInput_ShouldThrowException(int minValue, int maxValue)
        {
            // assert
            Assert.Throws<InvalidNumberRangeException>(() => NumberRange<int>.From(minValue, maxValue));
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData( 0, 0)]
        [InlineData( 1, 10)]
        public void TryFrom_ValidInput_ShouldReturnOK(int minValue, int maxValue)
        {
            // act
            var result = NumberRange<int>.TryFrom(minValue, maxValue, out _);
            
            // assert
            Assert.Equal(NumberRange<int>.Validation.Ok,  result);
        }
        
        [Theory]
        [InlineData( 1, 0, NumberRange<int>.Validation.InvalidRange)]
        [InlineData( 10, 5, NumberRange<int>.Validation.InvalidRange)]  
        [InlineData( -1, -2, NumberRange<int>.Validation.InvalidRange)]
        public void TryFrom_WrongInput_ShouldReturnError(int minValue, int maxValue, NumberRange<int>.Validation expected)
        {
            // act
            var result = NumberRange<int>.TryFrom(minValue, maxValue, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
