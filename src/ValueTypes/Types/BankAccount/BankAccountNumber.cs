namespace Migs.ValueTypes.Types.BankAccount
{
    /// <summary>
    /// Aggregate for IBAN and BIC
    /// </summary>
    public readonly record struct BankAccountNumber(IBAN IBAN, BIC BIC);
}
