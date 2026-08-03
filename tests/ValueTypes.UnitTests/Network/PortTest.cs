using System;
using Smart.ValueTypes.Types.Network;
using Smart.ValueTypes.UnitTests.TestData;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Network
{
    public class PortTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(Port);
            
            // act
            var result = new Port();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(65_535)]
        public void Constructor_ValidInput_ShouldReturnObject(int value)
        {
            // act
            var result = new Port(value);
                
            // assert
            Assert.Equal(value, result, 0);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(65_536)]
        public void Constructor_WrongInput_ShouldThrowException(int input)
        {
            // assert
            Assert.Throws<InvalidPortException>(() => new Port(input));
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData(0)]
        [InlineData(65_535)]
        public void From_ValidInput_ShouldReturnObject(int value)
        {
            // act
            var result = Port.From(value);
                
            // assert
            Assert.Equal(value, result, 0);
        }
        
        [Theory]
        [InlineData(-1)]
        [InlineData(65_536)]
        public void From_WrongInput_ShouldThrowException(double input)
        {
            // assert
            Assert.Throws<InvalidPortException>(() => Port.From(input));
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData(0)]
        [InlineData(65_535)]
        public void TryFrom_ValidInput_ShouldReturnOK(int value)
        {
            // act
            var result = Port.TryFrom(value, out _);
                
            // assert
            Assert.Equal(Port.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(-1, Port.Validation.TooLow)]
        [InlineData(65_536, Port.Validation.TooHigh)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Port.Validation expected)
        {
            // act
            var result = Port.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
