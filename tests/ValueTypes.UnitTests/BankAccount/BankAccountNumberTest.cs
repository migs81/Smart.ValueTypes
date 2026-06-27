using System;
using Migs.ValueTypes.Types.BankAccount;
using Xunit;

namespace Migs.ValueTypes.UnitTests.BankAccount;

public class BankAccountNumberTest
{
    #region tests

    [Fact]
    public void Constructor_ValidInput_ShouldReturnObject()
    {
        // arrange
        var iban = IBAN.Default;
        var bic = BIC.Default;
        
        // act
        var account = new BankAccountNumber(iban, bic);
        
        // assert
        Assert.Equal(iban, account.IBAN);
        Assert.Equal(bic, account.BIC);
    }

    [Fact]
    public void Constructor_NullInput_ShouldThrowException()
    {
        // arrange
        var iban = IBAN.Default;
        var bic = BIC.Default;
        
        // assert
        Assert.Throws<ArgumentNullException>(() =>  new BankAccountNumber(null, bic));
        Assert.Throws<ArgumentNullException>(() =>  new BankAccountNumber(iban, null));
    }
    
    #endregion
}