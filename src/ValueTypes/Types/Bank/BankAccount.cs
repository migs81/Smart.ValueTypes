namespace Migs.ValueTypes.Types.Bank
{
    /// <summary>
    /// Aggregate for IBAN and BIC
    /// </summary>
    public readonly record struct BankAccount(IBAN IBAN, BIC BIC);
}
