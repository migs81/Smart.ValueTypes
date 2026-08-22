using System;
using Smart.ValueTypes.Types.Text;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Text
{
    public class NonEmptyStringTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(NonEmptyString);
            
            // act
            var result = new NonEmptyString();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("Hello world")]
        [InlineData("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!\"§$%&/()=?`°'#*+-_.,;:")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new NonEmptyString(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new NonEmptyString(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData("a")]
        [InlineData("Hello world")]
        [InlineData("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!\"§$%&/()=?`°'#*+-_.,;:")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = NonEmptyString.From(input);
                
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => NonEmptyString.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData("a")]
        [InlineData("Hello world")]
        [InlineData("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!\"§$%&/()=?`°'#*+-_.,;:")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = NonEmptyString.TryFrom(input, out _);
                
            // assert
            Assert.Equal(NonEmptyString.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, NonEmptyString.Validation.Null)]
        [InlineData("", NonEmptyString.Validation.Empty)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, NonEmptyString.Validation expected)
        {
            // act
            var result = NonEmptyString.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
    }
}
