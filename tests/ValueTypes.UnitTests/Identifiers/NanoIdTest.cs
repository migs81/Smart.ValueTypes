using System;
using Smart.ValueTypes.Types.Identifiers;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Identifiers
{
    public class NanoIdTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(NanoId);

	        // act
            var result = new NanoId();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("V1StGXR8_Z5jdHi6B-myT")]
        [InlineData("x7Kp2mQ9fR4aL8NcD3wZb")]
        [InlineData("nF5Qv8T2kL9Xc4HjR6mPa")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new NanoId(input);
            
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("00000000000000000000", typeof(InvalidNanoIdException))]
        [InlineData("0000000000000000000000", typeof(InvalidNanoIdException))]
        [InlineData("+00000000000000000000", typeof(InvalidNanoIdException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => new NanoId(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData("V1StGXR8_Z5jdHi6B-myT")]
        [InlineData("x7Kp2mQ9fR4aL8NcD3wZb")]
        [InlineData("nF5Qv8T2kL9Xc4HjR6mPa")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = NanoId.From(input);
            
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("00000000000000000000", typeof(InvalidNanoIdException))]
        [InlineData("0000000000000000000000", typeof(InvalidNanoIdException))]
        [InlineData("+00000000000000000000", typeof(InvalidNanoIdException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => NanoId.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData("V1StGXR8_Z5jdHi6B-myT")]
        [InlineData("x7Kp2mQ9fR4aL8NcD3wZb")]
        [InlineData("nF5Qv8T2kL9Xc4HjR6mPa")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = NanoId.TryFrom(input, out _);
                
            // assert
            Assert.Equal(NanoId.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, NanoId.Validation.Null)]
        [InlineData("", NanoId.Validation.Empty)]
        [InlineData("00000000000000000000", NanoId.Validation.WrongLength)]
        [InlineData("0000000000000000000000", NanoId.Validation.WrongLength)]
        [InlineData("+00000000000000000000", NanoId.Validation.IllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, NanoId.Validation expected)
        {
	        // act
            var result = NanoId.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
