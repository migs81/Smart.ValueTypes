using Migs.ValueTypes.Types.ID;
using System;
using Xunit;

namespace Migs.ValueTypes.UnitTests.ID
{
    public class IMSITest
    {
        #region test data

        #endregion

        #region tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new IMSI();
            Assert.Equal(IMSI.Default, result);
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("123456789012345")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            var result = new IMSI(input);
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("12345", typeof(InvalidIMEIException))]
        [InlineData("1234567890123456", typeof(InvalidIMEIException))]
        [InlineData("a23456", typeof(InvalidIMEIException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new IMSI(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("123456789012345")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            var result = IMSI.From(input);
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("123456789012345")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            var result = IMSI.TryFrom(input, out _);
            Assert.Equal(IMSI.Validation.Ok, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("12345", typeof(InvalidIMEIException))]
        [InlineData("1234567890123456", typeof(InvalidIMEIException))]
        [InlineData("a23456", typeof(InvalidIMEIException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new IMSI(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(null, IMSI.Validation.Null)]
        [InlineData("", IMSI.Validation.Empty)]
        [InlineData("12345", IMSI.Validation.TooShort)]
        [InlineData("1234567890123456", IMSI.Validation.TooLong)]
        [InlineData("a23456", IMSI.Validation.IllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, IMSI.Validation expected)
        {
            var result = IMSI.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
