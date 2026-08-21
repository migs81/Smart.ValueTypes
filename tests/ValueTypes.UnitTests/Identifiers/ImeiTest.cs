using System;
using Smart.ValueTypes.Types.Identifiers;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Identifiers
{
    public class ImeiTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
	        // arrange
	        var expected = default(IMEI);
	        
	        // act
            var result = new IMEI();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("356609085048955")]
        [InlineData("120250063868430")]
        [InlineData("990002742214537")]
        [InlineData("134380046317850")]
        [InlineData("352590373324775")]
        [InlineData("135500067041027")]
        [InlineData("152490050037679")]
        [InlineData("122230092595020")]
        [InlineData("990001891327728")]
        [InlineData("353297077760598")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new IMEI(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => new IMEI(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData("356609085048955")]
        [InlineData("120250063868430")]
        [InlineData("990002742214537")]
        [InlineData("134380046317850")]
        [InlineData("352590373324775")]
        [InlineData("135500067041027")]
        [InlineData("152490050037679")]
        [InlineData("122230092595020")]
        [InlineData("990001891327728")]
        [InlineData("353297077760598")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = IMEI.From(input);
                
            // assert
            Assert.Equal(input.ToUpper(), result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
	        // act
            var result = Record.Exception(() => new IMEI(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData("356609085048955")]
        [InlineData("120250063868430")]
        [InlineData("990002742214537")]
        [InlineData("134380046317850")]
        [InlineData("352590373324775")]
        [InlineData("135500067041027")]
        [InlineData("152490050037679")]
        [InlineData("122230092595020")]
        [InlineData("990001891327728")]
        [InlineData("353297077760598")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = IMEI.TryFrom(input, out _);
                
            // assert
            Assert.Equal(IMEI.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, IMEI.Validation.Null)]
        [InlineData("", IMEI.Validation.Empty)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, IMEI.Validation expected)
        {
	        // act
            var result = IMEI.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
