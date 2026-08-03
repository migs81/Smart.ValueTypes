using System;
using Smart.ValueTypes.Types.Bank;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Bank;

public class BankAccountTest
{
    #region constructor

    [Fact]
    public void Constructor_ValidInput_ShouldReturnObject()
    {
        // arrange
        var iban = new IBAN();
        var bic = new BIC();
        
        // act
        var account = new BankAccount(iban, bic);
        
        // assert
        Assert.Equal(iban, account.IBAN);
        Assert.Equal(bic, account.BIC);
        Assert.True(account.IBAN.IsDefault);
        Assert.True(account.BIC.IsDefault);
    }

    [Fact]
    public void Constructor_NullInput_ShouldThrowException()
    {
        // arrange
        var iban = new IBAN();
        var bic = new BIC();
        
        // assert
        Assert.Throws<ArgumentNullException>(() =>  new BankAccount(null, bic));
        Assert.Throws<ArgumentNullException>(() =>  new BankAccount(iban, null));
    }
    
    #endregion
}