using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Migs.ValueTypes.Interfaces;

namespace Migs.ValueTypes.Unfinished
{
    /// <summary>
    /// Value type for file paths.
    /// </summary>
    /// <seealso cref="IValueType&lt;string, FilePath&gt;" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidFilePathException"></exception>
    public readonly record struct FilePath : IValueType<string, FilePath>
    {
        #region fields

        private readonly string _value;
        private enum ValidationError
        {
            None = 0,
            Null,
            Empty,
            TooLong,
            ContainsIllegalCharacter,
        }

        #endregion

        #region constructor

        public FilePath() => _value = @"\";
        public FilePath(string value)
        {
            var (Success, Error) = IsFilePath(ref value);
            if (!Success)
            {
                throw Error switch
                {
                    ValidationError.Null => throw new ArgumentNullException(nameof(value)),
                    ValidationError.Empty => throw new ArgumentException("Argument can not be empty!", nameof(value)),
                    ValidationError.TooLong => throw new InvalidFilePathException($"The path '{value}' is too long!"),
                    ValidationError.ContainsIllegalCharacter => throw new InvalidFilePathException($"The path '{value}' contains illegal characters!"),
                    _ => new InvalidFilePathException()
                };
            }

            _value = value;
        }
        private FilePath(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(FilePath left, string right) => left.Equals(right);
        public static bool operator !=(FilePath left, string right) => !left.Equals(right);

        public static implicit operator string(FilePath left) => left._value;
        public static implicit operator FilePath(string value) => new(value);
        public static implicit operator FilePath(FileInfo fileInfo) => new(fileInfo.FullName);

        #endregion

        #region public methods

        public FileInfo GetInfo() => new(_value);
        public FilePath Combine(params string[] args) => Path.Combine(args.Prepend(_value).ToArray());

        public bool Equals(string other) => EqualityComparer<string>.Default.Equals(_value, other);

        public static FilePath Parse(string value) => new(value);
        public static bool TryParse(string value, out FilePath? output)
        {
            try
            {
                if (IsFilePath(ref value).Success)
                {
                    output = new FilePath(ref value);
                    return true;
                }

                output = null;
                return false;
            }
            catch (Exception)
            {
                output = null;
                return false;
            }
        }

        #endregion

        #region private methods

        private static (bool Success, ValidationError Error) IsFilePath(ref string value)
        {
            if (value is null)
                return (false, ValidationError.Null);

            if (value.Length == 0)
                return (false, ValidationError.Empty);

            // illegal characters
            foreach (var c in value)
            {
                if (c < 32)
                    return (false, ValidationError.ContainsIllegalCharacter);
            }

            return (true, ValidationError.None);
        }

        #endregion
    }

    public class InvalidFilePathException : Exception
    {
        public InvalidFilePathException()
        {
        }

        public InvalidFilePathException(string message) : base(message)
        {
        }
    }
}
