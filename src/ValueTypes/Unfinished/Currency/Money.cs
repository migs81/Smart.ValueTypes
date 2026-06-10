using Migs.ValueTypes.Interfaces;
using System.Collections.Generic;

namespace Migs.ValueTypes
{
    public readonly record struct Money : IValueType<decimal, Currency, Money>
    {
        #region fields

        private readonly decimal _amount;

        public const decimal MaxValue = decimal.MaxValue;
        public const decimal MinValue = decimal.MinValue;

        #endregion

        #region properties

        public Currency Currency { get; }

        #endregion

        #region constructor

        public Money() : this(0d, new Currency()) { }
        public Money(int amount, Currency currency) : this((decimal)amount, currency) { }
        public Money(long amount, Currency currency) : this((decimal)amount, currency) { }
        public Money(double amount, Currency currency) : this((decimal)amount, currency) { }
        public Money(float amount, Currency currency) : this((decimal)amount, currency) { }
        public Money(uint amount, Currency currency) : this((decimal)amount, currency) { }
        public Money(ulong amount, Currency currency) : this((decimal)amount, currency) { }
        public Money(decimal amount, Currency currency)
        {
            _amount = amount;
            Currency = currency;
        }

        #endregion

        #region operator

        public static bool operator ==(Money left, decimal right) => left.Equals(right);
        public static bool operator !=(Money left, decimal right) => !left.Equals(right);

        public static implicit operator decimal(Money left) => left._amount;

        #endregion

        #region public methods

        public static Money From(decimal amount, Currency currency) => new(amount, currency);
        public static Money From(double amount, Currency currency) => new((decimal)amount, currency);
        public static Money From(int amount, Currency currency) => new((decimal)amount, currency);
        public static Money From(float amount, Currency currency) => new((decimal)amount, currency);
        public static Money From(long amount, Currency currency) => new((decimal)amount, currency);
        public static Money From(uint amount, Currency currency) => new((decimal)amount, currency);
        public static Money From(ulong amount, Currency currency) => new((decimal)amount, currency);

        public bool Equals(decimal value, Currency currency)
        {
            return EqualityComparer<decimal>.Default.Equals(_amount, value)
                && EqualityComparer<string>.Default.Equals(Currency, currency);
        }

        //public string ToString(string format) => _amount.ToString(format);
        //public string ToString(string format, IFormatProvider provider) => _amount.ToString(format, provider);
        //public string ToString(IFormatProvider provider) => _amount.ToString(provider);

        #endregion
    }
}
