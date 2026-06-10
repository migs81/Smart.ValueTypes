namespace Migs.ValueTypes.Types
{
    /// <summary>
    /// Aggregate for IBAN and BIC
    /// </summary>
    public readonly record struct BankAccountNumber
    {
        public readonly IBAN IBAN { get; }
        public readonly BIC BIC { get; }
    }
}
