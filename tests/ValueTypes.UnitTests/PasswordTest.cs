using Smart.ValueTypes.Types;
using System;
using Xunit;

namespace Smart.ValueTypes.UnitTests
{
    public class PasswordTest
    {
        #region test data

        #endregion

        #region tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnEmptyPassword()
        {
            var password = new Password();
            Assert.Equal(Password.Empty, password);
            Assert.Equal("", password);
        }

        [Theory]
        [InlineData("", 0, int.MaxValue, Password.Requirement.Nothing)]
        [InlineData("abc", 3, 3, Password.Requirement.LowercaseLetters)]
        [InlineData("abc123", 6, int.MaxValue, Password.Requirement.LowercaseLetters | Password.Requirement.Numbers)]
        [InlineData("abcABC123$%&", 6, int.MaxValue, Password.Requirement.LowercaseLetters | Password.Requirement.Numbers | Password.Requirement.UppercaseLetters | Password.Requirement.Symbols)]
        public void Constructor_ValidInput_ShouldReturnObject(string input, int minValue, int maxValue, Password.Requirement requirement)
        {
            var password = new Password(input, minValue, maxValue, requirement);
            Assert.Equal(input, password);
        }

        [Theory]
        [InlineData(null, 0, int.MaxValue, Password.Requirement.Nothing)]
        [InlineData("", 1, int.MaxValue, Password.Requirement.Nothing)]
        [InlineData("a", 0, 0, Password.Requirement.Nothing)]
        [InlineData("", 0, int.MaxValue, Password.Requirement.LowercaseLetters)]
        [InlineData("", 0, int.MaxValue, Password.Requirement.UppercaseLetters)]
        [InlineData("", 0, int.MaxValue, Password.Requirement.Numbers)]
        [InlineData("", 0, int.MaxValue, Password.Requirement.Symbols)]
        public void Constructor_WrongInput_ShouldThrowInvalidPasswordException(string input, int minValue, int maxValue, Password.Requirement requirement)
        {
            Assert.Throws<InvalidPasswordException>(() => new Password(input, minValue, maxValue, requirement));
        }

        [Theory]
        [InlineData("", 0, int.MaxValue, Password.Requirement.Nothing)]
        [InlineData("abc", 3, 3, Password.Requirement.LowercaseLetters)]
        [InlineData("abc123", 6, int.MaxValue, Password.Requirement.LowercaseLetters | Password.Requirement.Numbers)]
        [InlineData("abcABC123$%&", 6, int.MaxValue, Password.Requirement.LowercaseLetters | Password.Requirement.Numbers | Password.Requirement.UppercaseLetters | Password.Requirement.Symbols)]
        public void TryFrom_ValidInput_ShouldBeTrue(string input, int minValue, int maxValue, Password.Requirement requirement)
        {
            var password = Password.TryFrom(input, minValue, maxValue, requirement, out _);
            Assert.Equal(Password.Validation.Ok, password);
        }

        [Theory]
        [InlineData(null, 0, int.MaxValue, Password.Requirement.Nothing, Password.Validation.Null)]
        [InlineData("", 1, int.MaxValue, Password.Requirement.Nothing, Password.Validation.TooShort)]
        [InlineData("a", 0, 0, Password.Requirement.Nothing, Password.Validation.TooLong)]
        [InlineData("", 0, int.MaxValue, Password.Requirement.LowercaseLetters, Password.Validation.LowercaseLettersMissing)]
        [InlineData("", 0, int.MaxValue, Password.Requirement.UppercaseLetters, Password.Validation.UppercaseLettersMissing)]
        [InlineData("", 0, int.MaxValue, Password.Requirement.Numbers, Password.Validation.NumbersMissing)]
        [InlineData("", 0, int.MaxValue, Password.Requirement.Symbols, Password.Validation.SymbolsMissing)]
        public void TryFrom_WrongInput_ShouldBeFalse(string input, int minValue, int maxValue, Password.Requirement requirement, Password.Validation expected)
        {
            var password = Password.TryFrom(input, minValue, maxValue, requirement, out _);
            Assert.Equal(expected, password);
        }

        [Theory]
        [InlineData("")]
        [InlineData("test")]
        [InlineData("test123")]
        public void Equals_ShouldBeTrue(string input)
        {
            var password = new Password(input);
            Assert.True(password.Equals(input));
        }

        [Theory]
        [InlineData("", 0, int.MaxValue, Password.Requirement.Nothing)]
        [InlineData("abc", 3, 3, Password.Requirement.LowercaseLetters)]
        [InlineData("abc123", 6, int.MaxValue, Password.Requirement.LowercaseLetters | Password.Requirement.Numbers)]
        [InlineData("abcABC123$%&", 6, int.MaxValue, Password.Requirement.LowercaseLetters | Password.Requirement.Numbers | Password.Requirement.UppercaseLetters | Password.Requirement.Symbols)]
        public void From_ValidInput_ShouldReturnObject(string input, int minValue, int maxValue, Password.Requirement requirement)
        {
            var password = Password.From(input, minValue, maxValue, requirement);
            Assert.Equal(input, password);
        }

        [Theory]
        [InlineData(null, 0, int.MaxValue, Password.Requirement.Nothing)]
        [InlineData("", 1, int.MaxValue, Password.Requirement.Nothing)]
        [InlineData("a", 0, 0, Password.Requirement.Nothing)]
        [InlineData("", 0, int.MaxValue, Password.Requirement.LowercaseLetters)]
        [InlineData("", 0, int.MaxValue, Password.Requirement.UppercaseLetters)]
        [InlineData("", 0, int.MaxValue, Password.Requirement.Numbers)]
        [InlineData("", 0, int.MaxValue, Password.Requirement.Symbols)]
        public void From_WrongInput_ShouldThrowInvalidPasswordException(string input, int minValue, int maxValue, Password.Requirement requirement)
        {
            Assert.Throws<InvalidPasswordException>(() => Password.From(input, minValue, maxValue, requirement));
        }

        #endregion
    }
}
