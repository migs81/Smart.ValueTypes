using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.ID
{
    /// <summary>
    /// Value type for Universally Unique Lexicographically Sortable Identifier (NanoId).
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidNanoIdException"></exception>
    public readonly record struct NanoId : IValueType<string, NanoId>
    {
        #region fields

        private readonly string _value;

        private const string Default = "000000000000000000000";
        
        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            WrongLength,
            IllegalCharacter,
            UnknownError
        }

        #endregion

        #region properties

        public static NanoId Empty => new();

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NanoId"/> struct.
        /// </summary>
        public NanoId() => _value = Default;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NanoId"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidNanoIdException"></exception>
        public NanoId(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be null or empty!", nameof(value)),
                    Validation.WrongLength => new InvalidNanoIdException($"The value '{value}' is not 21 characters long!"),
                    Validation.IllegalCharacter => new InvalidNanoIdException($"The value '{value}' contains an illegal character!"),
                    _ => new InvalidNanoIdException(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private NanoId(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(NanoId left, string right) => left.Equals(right);
        public static bool operator !=(NanoId left, string right) => !left.Equals(right);

        public static implicit operator string(NanoId imei) => imei._value;
        public static implicit operator NanoId(string value) => new(value);

        #endregion

        #region public methods

        public static NanoId New() => new();

        public static NanoId From(string value) => new(value);
        
        public static Validation TryFrom(string value, out NanoId output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                    output = new NanoId(ref value);
                else
                    output = Empty;

                return result;
            }
            catch (Exception)
            {
                output = Empty;
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
            
            if (value.Length != 21)
                return Validation.WrongLength;
            
            // -------- characters ---------
            var span = value.AsSpan();
            
            foreach (var c in span)
            {
                if (c is >= 'a' and <= 'z') continue;
                if (c is >= 'A' and <= 'Z') continue;
                if (c is >= '0' and <= '9') continue;
                if (c is '-' or '_') continue;
                
                return Validation.IllegalCharacter;
            }
            
            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidNanoIdException : Exception
    {
        public InvalidNanoIdException()
        {
        }

        public InvalidNanoIdException(string message) : base(message)
        {
        }
    }
}
