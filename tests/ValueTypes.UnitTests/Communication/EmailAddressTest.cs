using System;
using Smart.ValueTypes.Types.Communication;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Communication
{
    public class EmailAddressTest
    {
        #region test data

        private const string TooShort = "ab";
        private const string TooLong = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                                       "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                                       "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                                       "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx@.com"; // 255 characters
        private const string NoAtSign = "name_domain.com";

        private const string LocalPartStartsWithDot = ".name@domain.com";
        private const string LocalPartEndsWithDot = "name.@domain.com";
        private const string LocalPartTooShort = "@domain.com";
        private const string LocalPartContainsTwoDots = "user..name@domain.com";
        private const string LocalPartContainsIllegalCharacter = "firstname lastname@domain.com";
        private const string LocalPartTooLong = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                                                "xxxxxxxxxxxxxxxxxx@.com"; // 65 characters for the name part

        private const string DomainPartTooShort = "name@";
        private const string DomainPartContainsIllegalCharacter = "firstname.lastname@domain~xyz.com";
        
        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(EmailAddress);
            
            // act
            var result = new EmailAddress();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("user@example.com")]
        [InlineData("john.doe@example.com")]
        [InlineData("john-doe@example.com")]
        [InlineData("john_doe@example.com")]
        [InlineData("john+newsletter@example.com")]
        [InlineData("user123@example.com")]
        [InlineData("123456@example.com")]
        [InlineData("a@example.com")]
        [InlineData("firstname.lastname@sub.example.com")]
        [InlineData("user@localhost")]
        [InlineData("user@example.co.uk")]
        [InlineData("user@example.travel")]
        [InlineData("user@my-domain.com")]
        [InlineData("user.name+tag123@sub-domain.example.org")]
        [InlineData("customer-service@company.com")]
        [InlineData("x_y-z+123@example.net")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var email = new EmailAddress(input);
                
            // assert
            Assert.Equal(input, email);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(TooShort, typeof(InvalidEmailAddressException))]
        [InlineData(TooLong, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartStartsWithDot, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartEndsWithDot, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartContainsTwoDots, typeof(InvalidEmailAddressException))]
        [InlineData(NoAtSign, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartTooShort, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartTooLong, typeof(InvalidEmailAddressException))]
        [InlineData(DomainPartTooShort, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartContainsIllegalCharacter, typeof(InvalidEmailAddressException))]
        [InlineData(DomainPartContainsIllegalCharacter, typeof(InvalidEmailAddressException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new EmailAddress(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData("user@example.com")]
        [InlineData("john.doe@example.com")]
        [InlineData("john-doe@example.com")]
        [InlineData("john_doe@example.com")]
        [InlineData("john+newsletter@example.com")]
        [InlineData("user123@example.com")]
        [InlineData("123456@example.com")]
        [InlineData("a@example.com")]
        [InlineData("firstname.lastname@sub.example.com")]
        [InlineData("user@localhost")]
        [InlineData("user@example.co.uk")]
        [InlineData("user@example.travel")]
        [InlineData("user@my-domain.com")]
        [InlineData("user.name+tag123@sub-domain.example.org")]
        [InlineData("customer-service@company.com")]
        [InlineData("x_y-z+123@example.net")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var email = EmailAddress.From(input);
                
            // assert
            Assert.Equal(input, email);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(TooShort, typeof(InvalidEmailAddressException))]
        [InlineData(TooLong, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartStartsWithDot, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartEndsWithDot, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartContainsTwoDots, typeof(InvalidEmailAddressException))]
        [InlineData(NoAtSign, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartTooShort, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartTooLong, typeof(InvalidEmailAddressException))]
        [InlineData(DomainPartTooShort, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartContainsIllegalCharacter, typeof(InvalidEmailAddressException))]
        [InlineData(DomainPartContainsIllegalCharacter, typeof(InvalidEmailAddressException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new EmailAddress(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom

        [Theory]
        [InlineData("user@example.com")]
        [InlineData("john.doe@example.com")]
        [InlineData("john-doe@example.com")]
        [InlineData("john_doe@example.com")]
        [InlineData("john+newsletter@example.com")]
        [InlineData("user123@example.com")]
        [InlineData("123456@example.com")]
        [InlineData("a@example.com")]
        [InlineData("firstname.lastname@sub.example.com")]
        [InlineData("user@localhost")]
        [InlineData("user@example.co.uk")]
        [InlineData("user@example.travel")]
        [InlineData("user@my-domain.com")]
        [InlineData("user.name+tag123@sub-domain.example.org")]
        [InlineData("customer-service@company.com")]
        [InlineData("x_y-z+123@example.net")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = EmailAddress.TryFrom(input, out _);
                
            // assert
            Assert.Equal(EmailAddress.Validation.Ok, result);
        }

        [Theory]
        [InlineData(null, EmailAddress.Validation.Null)]
        [InlineData("", EmailAddress.Validation.Empty)]
        [InlineData(TooShort, EmailAddress.Validation.TooShort)]
        [InlineData(TooLong, EmailAddress.Validation.TooLong)]
        [InlineData(LocalPartStartsWithDot, EmailAddress.Validation.LocalPartStartsWithDot)]
        [InlineData(LocalPartEndsWithDot, EmailAddress.Validation.LocalPartEndsWithDot)]
        [InlineData(LocalPartContainsTwoDots, EmailAddress.Validation.LocalPartContainsTwoDotsTogether)]
        [InlineData(NoAtSign, EmailAddress.Validation.NoAtSign)]
        [InlineData(LocalPartTooShort, EmailAddress.Validation.LocalPartTooShort)]
        [InlineData(LocalPartTooLong, EmailAddress.Validation.LocalPartTooLong)]
        [InlineData(DomainPartTooShort, EmailAddress.Validation.DomainPartTooShort)]
        [InlineData(LocalPartContainsIllegalCharacter, EmailAddress.Validation.LocalPartContainsIllegalCharacter)]
        [InlineData(DomainPartContainsIllegalCharacter, EmailAddress.Validation.DomainPartContainsIllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, EmailAddress.Validation expected)
        {
            // act
            var result = EmailAddress.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
