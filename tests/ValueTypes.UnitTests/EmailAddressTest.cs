using Migs.ValueTypes.Types;
using System;
using Xunit;

namespace Migs.ValueTypes.UnitTests
{
    public class EmailAddressTest
    {
        #region test data

        private const string TOO_SHORT = "ab";
        private const string TOO_LONG = ""
            + "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            + "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            + "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            + "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            + "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            + "@.com"; // 255 characters
        private const string NO_AT_SIGN = "name_domain.com";

        private const string LOCAL_PART_STARTS_WITH_DOT = ".name@domain.com";
        private const string LOCAL_PART_ENDS_WITH_DOT = "name.@domain.com";
        private const string LOCAL_PART_TOO_SHORT = "@domain.com";
        private const string LOCAL_PART_CONTAINS_ILLEGAL_CHARACTER = "firstname~lastname@domain.com";
        private const string LOCAL_PART_TOO_LONG = ""
            + "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
            + "xxxxxxxxxxxxxxx"
            + "@.com"; // 65 characters for the name part

        private const string DOMAIN_PART_TOO_SHORT = "name@";
        private const string DOMAIN_PART_CONTAINS_ILLEGAL_CHARACTER = "firstname.lastname@domain~xyz.com";

        private static readonly string[] _validValues =
        [
            "name@domain.com",
            "firstname.lastname@company.gv.com",
            "firstname.lastname123@company123.to",
            "firstname_lastname@domainxyz.com",
            "firstname-lastname@domain-xyz.com",
            "Firstname.Lastname@DOMAIN.XYZ.COM",

            //"simple@example.com",
            //"very.common@example.com",
            //"abc@example.co.uk",
            //"disposable.style.email.with+symbol@example.com",
            //"other.email-with-hyphen@example.com",
            //"fully-qualified-domain@example.com",
            //"user.name+tag+sorting@example.com",
            //"example-indeed@strange-example.com",
            //"example-indeed@strange-example.inininini",
            //"1234567890123456789012345678901234567890123456789012345678901234+x@example.com",
        ];

        #endregion

        #region tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var email = new EmailAddress();
            Assert.Equal(EmailAddress.Default, email);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var email = new EmailAddress(value);
                Assert.Equal(value, email);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(TOO_SHORT, typeof(InvalidEmailAddressException))]
        [InlineData(TOO_LONG, typeof(InvalidEmailAddressException))]
        [InlineData(LOCAL_PART_STARTS_WITH_DOT, typeof(InvalidEmailAddressException))]
        [InlineData(LOCAL_PART_ENDS_WITH_DOT, typeof(InvalidEmailAddressException))]
        [InlineData(NO_AT_SIGN, typeof(InvalidEmailAddressException))]
        [InlineData(LOCAL_PART_TOO_SHORT, typeof(InvalidEmailAddressException))]
        [InlineData(LOCAL_PART_TOO_LONG, typeof(InvalidEmailAddressException))]
        [InlineData(DOMAIN_PART_TOO_SHORT, typeof(InvalidEmailAddressException))]
        [InlineData(LOCAL_PART_CONTAINS_ILLEGAL_CHARACTER, typeof(InvalidEmailAddressException))]
        [InlineData(DOMAIN_PART_CONTAINS_ILLEGAL_CHARACTER, typeof(InvalidEmailAddressException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new EmailAddress(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var email = EmailAddress.From(value);
                Assert.Equal(value, email);
            }
        }

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in _validValues)
            {
                var result = EmailAddress.TryFrom(value, out _);
                Assert.Equal(EmailAddress.Validation.Ok, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(TOO_SHORT, typeof(InvalidEmailAddressException))]
        [InlineData(TOO_LONG, typeof(InvalidEmailAddressException))]
        [InlineData(LOCAL_PART_STARTS_WITH_DOT, typeof(InvalidEmailAddressException))]
        [InlineData(LOCAL_PART_ENDS_WITH_DOT, typeof(InvalidEmailAddressException))]
        [InlineData(NO_AT_SIGN, typeof(InvalidEmailAddressException))]
        [InlineData(LOCAL_PART_TOO_SHORT, typeof(InvalidEmailAddressException))]
        [InlineData(LOCAL_PART_TOO_LONG, typeof(InvalidEmailAddressException))]
        [InlineData(DOMAIN_PART_TOO_SHORT, typeof(InvalidEmailAddressException))]
        [InlineData(LOCAL_PART_CONTAINS_ILLEGAL_CHARACTER, typeof(InvalidEmailAddressException))]
        [InlineData(DOMAIN_PART_CONTAINS_ILLEGAL_CHARACTER, typeof(InvalidEmailAddressException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new EmailAddress(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(null, EmailAddress.Validation.Null)]
        [InlineData("", EmailAddress.Validation.Empty)]
        [InlineData(TOO_SHORT, EmailAddress.Validation.TooShort)]
        [InlineData(TOO_LONG, EmailAddress.Validation.TooLong)]
        [InlineData(LOCAL_PART_STARTS_WITH_DOT, EmailAddress.Validation.LocalPartStartsWithDot)]
        [InlineData(LOCAL_PART_ENDS_WITH_DOT, EmailAddress.Validation.LocalPartEndsWithDot)]
        [InlineData(NO_AT_SIGN, EmailAddress.Validation.NoAtSign)]
        [InlineData(LOCAL_PART_TOO_SHORT, EmailAddress.Validation.LocalPartTooShort)]
        [InlineData(LOCAL_PART_TOO_LONG, EmailAddress.Validation.LocalPartTooLong)]
        [InlineData(DOMAIN_PART_TOO_SHORT, EmailAddress.Validation.DomainPartTooShort)]
        [InlineData(LOCAL_PART_CONTAINS_ILLEGAL_CHARACTER, EmailAddress.Validation.LocalPartContainsIllegalCharacter)]
        [InlineData(DOMAIN_PART_CONTAINS_ILLEGAL_CHARACTER, EmailAddress.Validation.DomainPartContainsIllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, EmailAddress.Validation expected)
        {
            var result = EmailAddress.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
