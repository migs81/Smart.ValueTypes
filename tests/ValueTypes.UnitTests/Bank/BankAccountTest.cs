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
        var iban = IBAN.Empty;
        var bic = BIC.Empty;
        
        // act
        var account = new BankAccount(iban, bic);
        
        // assert
        Assert.Equal(iban, account.IBAN);
        Assert.Equal(bic, account.BIC);
    }

    [Fact]
    public void Constructor_NullInput_ShouldThrowException()
    {
        // arrange
        var iban = IBAN.Empty;
        var bic = BIC.Empty;
        
        // assert
        Assert.Throws<ArgumentNullException>(() =>  new BankAccount(null, bic));
        Assert.Throws<ArgumentNullException>(() =>  new BankAccount(iban, null));
    }
    
    #endregion
}