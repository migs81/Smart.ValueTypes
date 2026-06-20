using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Migs.ValueTypes.Generic
{
    /// <summary>
    /// Generic structure for a specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="IValueType{TValue,TThis}" />
    public readonly record struct ValueType<TValue> : IValueType<TValue, ValueType<TValue>>
        where TValue : notnull
    {
        public TValue Value { get; }

        public ValueType([DisallowNull] TValue value) => Value = value;
        public ValueType([DisallowNull] TValue value, Func<TValue, bool> predicate)
        {
            if (!predicate(value))
                throw new ArgumentException($"The given value '{value}' is not valid!");

            Value = value;
        }

        public static bool operator ==(ValueType<TValue> left, TValue right) => left.Equals(right);
        public static bool operator !=(ValueType<TValue> left, TValue right) => !left.Equals(right);
        public static implicit operator TValue(ValueType<TValue> arg) => arg.Value;

        public bool Equals(TValue other) => EqualityComparer<TValue>.Default.Equals(Value, other);
    }

    /// <summary>
    /// Static methods for the generic ValueTypes.
    /// </summary>
    public readonly record struct ValueType
    {
        public static ValueType<TValue> From<TValue>(TValue value) where TValue : notnull => new(value);
        public static bool TryFrom<TValue>(TValue value, Func<TValue, bool> predicate, out ValueType<TValue>? output) where TValue : notnull
        {
            try
            {
                output = new ValueType<TValue>(value, predicate);
                return true;
            }
            catch (Exception)
            {
                output = null;
                return false;
            }
        }
    }
}