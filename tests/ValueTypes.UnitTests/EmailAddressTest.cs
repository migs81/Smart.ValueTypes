using Smart.ValueTypes.Types;
using System;
using Xunit;

namespace Smart.ValueTypes.UnitTests
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
        private const string LocalPartContainsIllegalCharacter = "firstname~lastname@domain.com";
        private const string LocalPartTooLong = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" +
                                                "xxxxxxxxxxxxxxxxxx@.com"; // 65 characters for the name part

        private const string DomainPartTooShort = "name@";
        private const string DomainPartContainsIllegalCharacter = "firstname.lastname@domain~xyz.com";

        private static readonly string[] ValidValues =
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

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in ValidValues)
            {
                // act
                var email = new EmailAddress(value);
                
                // assert
                Assert.Equal(value, email);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(TooShort, typeof(InvalidEmailAddressException))]
        [InlineData(TooLong, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartStartsWithDot, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartEndsWithDot, typeof(InvalidEmailAddressException))]
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

        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in ValidValues)
            {
                // act
                var email = EmailAddress.From(value);
                
                // assert
                Assert.Equal(value, email);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(TooShort, typeof(InvalidEmailAddressException))]
        [InlineData(TooLong, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartStartsWithDot, typeof(InvalidEmailAddressException))]
        [InlineData(LocalPartEndsWithDot, typeof(InvalidEmailAddressException))]
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

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in ValidValues)
            {
                // act
                var result = EmailAddress.TryFrom(value, out _);
                
                // assert
                Assert.Equal(EmailAddress.Validation.Ok, result);
            }
        }

        [Theory]
        [InlineData(null, EmailAddress.Validation.Null)]
        [InlineData("", EmailAddress.Validation.Empty)]
        [InlineData(TooShort, EmailAddress.Validation.TooShort)]
        [InlineData(TooLong, EmailAddress.Validation.TooLong)]
        [InlineData(LocalPartStartsWithDot, EmailAddress.Validation.LocalPartStartsWithDot)]
        [InlineData(LocalPartEndsWithDot, EmailAddress.Validation.LocalPartEndsWithDot)]
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
