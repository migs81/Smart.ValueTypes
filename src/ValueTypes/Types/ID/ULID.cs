using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.ID
{
    /// <summary>
    /// Value type for Universally Unique Lexicographically Sortable Identifier (ULID).
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidUlidException"></exception>
    public readonly record struct ULID : IValueType<string, ULID>
    {
        #region fields

        private readonly string? _value;
        
        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            WrongLength,
            InvalidFirstCharacter,
            IllegalCharacter,
            UnknownError
        }

        #endregion

        #region properties

        public bool IsDefault => _value is null;

        public DateTimeOffset TimeStamp
        {
            get
            {
                if (_value is null) return default;
                
                const string alphabet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
                long timestamp = 0;

                foreach (var c in _value[..10])
                    timestamp = (timestamp << 5) | (uint)alphabet.IndexOf(c);

                return DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
            }
        }

        #endregion

        #region constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ULID"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidUlidException"></exception>
        public ULID(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be null or empty!", nameof(value)),
                    Validation.WrongLength => new InvalidUlidException($"The value '{value}' is not 26 characters long!"),
                    Validation.InvalidFirstCharacter => new InvalidUlidException($"The first character of the value '{value}' is not in the range '0' to '7'"),
                    Validation.IllegalCharacter => new InvalidUlidException($"The value '{value}' contains an illegal character!"),
                    _ => new InvalidUlidException(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private ULID(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(ULID left, string right) => left.Equals(right);
        public static bool operator !=(ULID left, string right) => !left.Equals(right);

        public static implicit operator string(ULID imei) => imei._value ?? "";
        public static implicit operator ULID(string value) => new(value);

        #endregion

        #region public methods

        public static ULID From(string value) => new(value);
        
        public static Validation TryFrom(string value, out ULID output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                    output = new ULID(ref value);
                else
                    output = default;

                return result;
            }
            catch (Exception)
            {
                output = default;
                return Validation.UnknownError;
            }
        }

        public static Validation ValidateFormat(string value) => ValidateFormat(ref value);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref string value)
        {
            if (value is null)
                return Validation.Null;

            if (value.Length == 0)
                return Validation.Empty;
            
            if (value.Length != 26)
                return Validation.WrongLength;
            
            // -------- characters ---------
            var span = value.AsSpan();
            
            if (span[0] is < '0' or > '7')
                return Validation.InvalidFirstCharacter;
            
            foreach (var c in span[1..])
            {
                if (c is >= 'A' and <= 'Z' and not 'I' and not 'L' and not 'O' and not 'I') continue; // Base32
                if (c is >= '0' and <= '9') continue;
                if (c is '!' or '#' or '$' or '&' or '-' or '^' or '_' or '.' or '+') continue;
                
                return Validation.IllegalCharacter;
            }
            
            return Validation.Ok;
        }
        
        #endregion
    }

    public class InvalidUlidException : Exception
    {
        public InvalidUlidException()
        {
        }

        public InvalidUlidException(string message) : base(message)
        {
        }
    }
}
