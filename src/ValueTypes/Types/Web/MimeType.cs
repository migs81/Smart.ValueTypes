using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Web
{
    /// <summary>
    /// Value type for International Mobile Equipment Identity (MimeType).
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidMimeTypeException"></exception>
    public readonly record struct MimeType : IValueType<string, MimeType>
    {
        #region fields

        private readonly string? _value;
        
        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            IncorrectNumberOfTypeSeparators,
            TypeTooShort,
            TypeContainsIllegalCharacter,
            SubtypeTooShort,
            SubtypeContainsIllegalCharacter,
            ParameterTooShort,
            InvalidParameter,
            UnknownError
        }

        #endregion

        #region properties

        public bool IsDefault => _value is null;

        public string Type => _value is not null ? _value[.._value.IndexOf('/')] : "";

        public string Subtype
        {
            get
            {
                if (_value is null) return "";
                
                var slashPos = _value.IndexOf('/');
                var semicolonPos = _value.IndexOf(';', slashPos + 1);

                if (semicolonPos == -1)
                    semicolonPos = _value.Length;
                
                return _value[(slashPos + 1)..semicolonPos].Trim();
            }
        }

        public string Parameter
        {
            get
            {
                if (_value is null) return "";
                
                var semicolonPos = _value.IndexOf(';');
                if (semicolonPos == -1)
                    return "";

                return _value[(semicolonPos + 1)..].Trim();
            }
        }

        #endregion

        #region constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MimeType"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidMimeTypeException"></exception>
        public MimeType(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => new ArgumentNullException(nameof(value)),
                    Validation.Empty => new ArgumentException($"Argument can not be null or empty!", nameof(value)),
                    Validation.IncorrectNumberOfTypeSeparators => new InvalidMimeTypeException($"The value '{value}' has no valid type part!"),
                    Validation.TypeTooShort => new InvalidMimeTypeException($"The value '{value}' is too short for the type part!"),
                    Validation.TypeContainsIllegalCharacter => new InvalidMimeTypeException($"The type part '{value}' contains an illegal character!"),
                    Validation.SubtypeTooShort => new InvalidMimeTypeException($"The value '{value}' is too short for the subtype part!"),
                    Validation.SubtypeContainsIllegalCharacter => new InvalidMimeTypeException($"The subtype part '{value}' contains an illegal character!"),
                    Validation.ParameterTooShort => new InvalidMimeTypeException($"The value '{value}' is too short for a parameter!"),
                    Validation.InvalidParameter => new InvalidMimeTypeException($"The value '{value}' is not a valid parameter!"),
                    _ => new InvalidMimeTypeException(),
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private MimeType(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(MimeType left, string right) => left.Equals(right);
        public static bool operator !=(MimeType left, string right) => !left.Equals(right);

        public static implicit operator string(MimeType imei) => imei._value ?? "";
        public static implicit operator MimeType(string value) => new(value);

        #endregion

        #region public methods

        public static MimeType From(string value) => new(value);
        
        public static Validation TryFrom(string value, out MimeType output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                    output = new MimeType(ref value);
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
            // ----------- general ----------------
            if (value is null)
                return Validation.Null;
        
            if (value.Length == 0)
                return Validation.Empty;
        
            var span = value.AsSpan();
            
            // ------------- type -----------------
            if (span.Count('/') != 1)
                return Validation.IncorrectNumberOfTypeSeparators;
            
            var slashPos = span.IndexOf('/');
            if (slashPos == 0)
                return Validation.TypeTooShort;

            if (!ValidateCharacters(ref span, 0, slashPos))
                return Validation.TypeContainsIllegalCharacter;
            
            // ------------ subtype ---------------
            return ValidateSubtype(ref span, slashPos);
        }

        private static Validation ValidateSubtype(ref ReadOnlySpan<char> span, int slashPos)
        {
            var semicolonPos = span[slashPos..].IndexOf(';');
            if (semicolonPos == -1)
            {
                // minimum length
                if (slashPos == span.Length - 1)
                    return Validation.SubtypeTooShort;

                // characters
                if (!ValidateCharacters(ref span, slashPos + 1, span.Length - slashPos - 1))
                    return Validation.SubtypeContainsIllegalCharacter;
            }
            else
            {
                semicolonPos += slashPos;
                
                // minimum length
                if (semicolonPos == slashPos + 1)
                    return Validation.SubtypeTooShort;

                // characters
                if (!ValidateCharacters(ref span, slashPos + 1, semicolonPos - slashPos - 1))
                    return Validation.SubtypeContainsIllegalCharacter;
                
                // ----------- parameter --------------
                var result = ValidateParameter(ref span, semicolonPos);
                if (result != Validation.Ok) return result;
            }

            return Validation.Ok;
        }

        private static Validation ValidateParameter(ref ReadOnlySpan<char> span, int semicolonPos)
        {
            var start = semicolonPos + 1;
            var length = span[start..].IndexOf(';');
            while (length != -1) // for multiple parameters
            {
                // minimum length
                if (length < 3)
                    return Validation.ParameterTooShort;
                
                // must contain exactly one =
                if (span[start..(start + length)].Count('=') != 1)
                    return Validation.InvalidParameter;
                
                // parameter can not start or end with a =
                if (span[start] == '=' || span[(start + length)] == '=')
                    return Validation.InvalidParameter;
                
                start += length + 1;
                length = span[start..].IndexOf(';');
            }
                
            // check last parameter
            length = span.Length - 1 - start;
            if (length < 3)
                return Validation.ParameterTooShort;
                
            // must contain exactly one =
            if (span[start..].Count('=') != 1)
                return Validation.InvalidParameter;
                
            // parameter can not start or end with a =
            if (span[start] == '=' || span[(start + length)] == '=')
                return Validation.InvalidParameter;

            return Validation.Ok;
        }
        
        private static bool ValidateCharacters(ref ReadOnlySpan<char> value, int startIndex, int length)
        {
            for (var index = startIndex; index < startIndex + length; index++)
            {
                if (value[index] is >= 'a' and <= 'z') continue;
                if (value[index] is >= 'A' and <= 'Z') continue;
                if (value[index] is >= '0' and <= '9') continue;
                if (value[index] is '!' or '#' or '$' or '&' or '-' or '^' or '_' or '.' or '+') continue;

                return false;
            }

            return true;
        }
        
        #endregion
    }

    public class InvalidMimeTypeException : Exception
    {
        public InvalidMimeTypeException()
        {
        }

        public InvalidMimeTypeException(string message) : base(message)
        {
        }
    }
}
