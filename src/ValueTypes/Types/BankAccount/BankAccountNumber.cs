namespace Migs.ValueTypes.Types
{
    /// <summary>
    /// Aggregate for IBAN and BIC
    /// </summary>
    public readonly record struct BankAccountNumber
    {
        public IBAN IBAN { get; }
        public BIC BIC { get; }
    }
}
