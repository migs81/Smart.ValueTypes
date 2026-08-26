using Smart.ValueTypes.Types.Security;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Security
{
    public class PasswordTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnEmptyPassword()
        {
            // arrange
            var expected = default(Password);
            
            // act
            var result = new Password();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.None)]
        [InlineData("abc", 3, 3, Password.PasswordRequirements.LowercaseLetters)]
        [InlineData("abc123", 6, int.MaxValue, Password.PasswordRequirements.LowercaseLetters | Password.PasswordRequirements.Numbers)]
        [InlineData("abcABC123$%&", 6, int.MaxValue, Password.PasswordRequirements.LowercaseLetters | Password.PasswordRequirements.Numbers | Password.PasswordRequirements.UppercaseLetters | Password.PasswordRequirements.Symbols)]
        public void Constructor_ValidInput_ShouldReturnObject(string input, int minValue, int maxValue, Password.PasswordRequirements requirement)
        {
            // act
            var password = new Password(input, minValue, maxValue, requirement);
            
            // assert
            Assert.Equal(input, password);
        }

        [Theory]
        [InlineData(null, 0, int.MaxValue, Password.PasswordRequirements.None)]
        [InlineData("", 1, int.MaxValue, Password.PasswordRequirements.None)]
        [InlineData("a", 0, 0, Password.PasswordRequirements.None)]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.LowercaseLetters)]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.UppercaseLetters)]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.Numbers)]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.Symbols)]
        public void Constructor_WrongInput_ShouldThrowInvalidPasswordException(string input, int minValue, int maxValue, Password.PasswordRequirements requirement)
        {
            // assert
            Assert.Throws<InvalidPasswordException>(() => new Password(input, minValue, maxValue, requirement));
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.None)]
        [InlineData("abc", 3, 3, Password.PasswordRequirements.LowercaseLetters)]
        [InlineData("abc123", 6, int.MaxValue, Password.PasswordRequirements.LowercaseLetters | Password.PasswordRequirements.Numbers)]
        [InlineData("abcABC123$%&", 6, int.MaxValue, Password.PasswordRequirements.LowercaseLetters | Password.PasswordRequirements.Numbers | Password.PasswordRequirements.UppercaseLetters | Password.PasswordRequirements.Symbols)]
        public void From_ValidInput_ShouldReturnObject(string input, int minValue, int maxValue, Password.PasswordRequirements requirement)
        {
            // act
            var password = Password.From(input, minValue, maxValue, requirement);
            
            // assert
            Assert.Equal(input, password);
        }

        [Theory]
        [InlineData(null, 0, int.MaxValue, Password.PasswordRequirements.None)]
        [InlineData("", 1, int.MaxValue, Password.PasswordRequirements.None)]
        [InlineData("a", 0, 0, Password.PasswordRequirements.None)]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.LowercaseLetters)]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.UppercaseLetters)]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.Numbers)]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.Symbols)]
        public void From_WrongInput_ShouldThrowInvalidPasswordException(string input, int minValue, int maxValue, Password.PasswordRequirements requirement)
        {
            // assert
            Assert.Throws<InvalidPasswordException>(() => Password.From(input, minValue, maxValue, requirement));
        }

        #endregion
        
        #region TryFrom

        [Theory]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.None)]
        [InlineData("abc", 3, 3, Password.PasswordRequirements.LowercaseLetters)]
        [InlineData("abc123", 6, int.MaxValue, Password.PasswordRequirements.LowercaseLetters | Password.PasswordRequirements.Numbers)]
        [InlineData("abcABC123$%&", 6, int.MaxValue, Password.PasswordRequirements.LowercaseLetters | Password.PasswordRequirements.Numbers | Password.PasswordRequirements.UppercaseLetters | Password.PasswordRequirements.Symbols)]
        public void TryFrom_ValidInput_ShouldBeTrue(string input, int minValue, int maxValue, Password.PasswordRequirements requirement)
        {
            var password = Password.TryFrom(input, minValue, maxValue, requirement, out _);
            Assert.Equal(Password.Validation.Ok, password);
        }

        [Theory]
        [InlineData(null, 0, int.MaxValue, Password.PasswordRequirements.None, Password.Validation.Null)]
        [InlineData("", 1, int.MaxValue, Password.PasswordRequirements.None, Password.Validation.TooShort)]
        [InlineData("a", 0, 0, Password.PasswordRequirements.None, Password.Validation.TooLong)]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.LowercaseLetters, Password.Validation.LowercaseLettersMissing)]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.UppercaseLetters, Password.Validation.UppercaseLettersMissing)]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.Numbers, Password.Validation.NumbersMissing)]
        [InlineData("", 0, int.MaxValue, Password.PasswordRequirements.Symbols, Password.Validation.SymbolsMissing)]
        public void TryFrom_WrongInput_ShouldBeFalse(string input, int minValue, int maxValue, Password.PasswordRequirements requirement, Password.Validation expected)
        {
            var password = Password.TryFrom(input, minValue, maxValue, requirement, out _);
            Assert.Equal(expected, password);
        }

        #endregion
        
        #region Equals

        [Theory]
        [InlineData("")]
        [InlineData("test")]
        [InlineData("test123")]
        public void Equals_ShouldBeTrue(string input)
        {
            var password = new Password(input);
            Assert.True(password.Equals(input));
        }

        #endregion
    }
}
