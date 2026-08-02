using System;
using System.IO;
using System.Linq;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.IO
{
    /// <summary>
    /// Value type for file paths.
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidWindowsFilePathException"></exception>
    public readonly record struct WindowsFilePath : IValueType<string, WindowsFilePath>
    {
        #region fields

        private readonly string? _value;
        
        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            WhiteSpaceOnly,
            ContainsIllegalCharacter,
            ContainsMultipleColons,
            WrongColonPlacement,
            ContainsMultipleBackslashes,
            ContainsReservedName,
            InvalidSegmentEnding,
            UnknownError
        }

        #endregion

        #region properties

        public bool IsDefault => _value is null;
        
        #endregion
        
        #region constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="value"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="WindowsFilePath"></exception>
        public WindowsFilePath(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => throw new ArgumentNullException(nameof(value)),
                    Validation.Empty => throw new ArgumentException("Argument can not be empty!", nameof(value)),
                    Validation.WhiteSpaceOnly => throw new ArgumentException("Argument cannot consist solely of white spaces!", nameof(value)),
                    Validation.ContainsIllegalCharacter => throw new InvalidWindowsFilePathException($"The path '{value}' contains illegal characters!"),
                    Validation.ContainsMultipleColons => throw new InvalidWindowsFilePathException($"The path '{value}' contains more than one colon!"),
                    Validation.WrongColonPlacement => throw new InvalidWindowsFilePathException($"The path '{value}' contains a colon in the wrong place!"),
                    Validation.ContainsMultipleBackslashes => throw new InvalidWindowsFilePathException($"The path '{value}' contains multiple backslashes!"),
                    Validation.ContainsReservedName => throw new InvalidWindowsFilePathException($"The path '{value}' contains reserved names!"),
                    Validation.InvalidSegmentEnding => throw new InvalidWindowsFilePathException($"The path '{value}' contains a segment that ends with an invalid character!"),
                    _ => new InvalidWindowsFilePathException()
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private WindowsFilePath(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(WindowsFilePath left, string right) => left.Equals(right);
        public static bool operator !=(WindowsFilePath left, string right) => !left.Equals(right);

        public static implicit operator string(WindowsFilePath left) => left._value ?? "";
        public static implicit operator WindowsFilePath(string value) => new(value);
        public static implicit operator WindowsFilePath(FileInfo fileInfo) => new(fileInfo.FullName);

        #endregion

        #region public methods

        public WindowsFilePath Combine(params string[] args)
        {
            if (args is null)
                return this;
            
            return Path.Combine(args.Where(w => w is not null).Prepend(_value ?? "").ToArray());
        }

        public static WindowsFilePath From(string value) => new(value);
        
        public static Validation TryFrom(string value, out WindowsFilePath output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new WindowsFilePath(ref value);
                    return Validation.Ok;
                }

                output = new WindowsFilePath();
                return result;
            }
            catch (Exception)
            {
                output = new WindowsFilePath();
                return Validation.UnknownError;
            }
        }

        public static WindowsFilePath Parse(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            // trim and replace slashes
            value = value.Trim()
                         .Replace('/', '\\');
            
            // replace multiple backslashes
            while (value.Contains(@"\\"))
                value = value.Replace(@"\\", @"\");
            
            return new WindowsFilePath(value);
        }

        public static bool TryParse(string value, out WindowsFilePath output)
        {
            try
            {
                output = Parse(value);
                return true;
            }
            catch (Exception)
            {
                output = new WindowsFilePath();
                return false;
            }
        }
        
        public static Validation ValidateFormat(string value) => ValidateFormat(ref value);
        
        #endregion

        #region private methods

        /// <summary>
        /// Determines whether the specified value is a file path.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Validation ValidateFormat(ref string value)
        {
            // ---------- general -----------
            if (value is null)
                return Validation.Null;
            
            if (value.Length == 0)
                return Validation.Empty;
            
            // ------- white space ----------
            var span = value.AsSpan();
            if (IsWhiteSpaceOnly(ref span))
                return Validation.WhiteSpaceOnly;
            
            // ----- illegal characters -----
            foreach (var c in span)
            {
                if (c is '<' or '>' or '"' or '|' or '?' or '*' or '\0' || char.IsControl(c))
                    return Validation.ContainsIllegalCharacter;
            }
            
            // ----------- colon ------------
            var colonCount = value.Count(':');
            if (colonCount > 1)
                return Validation.ContainsMultipleColons;
            if (colonCount == 1 && value[1] != ':')
                return Validation.WrongColonPlacement;
            
            // ---- multiple backslashes ----
            if (span[2..].IndexOf(@"\\") != -1)
                return Validation.ContainsMultipleBackslashes;
            
            // ------- check segments -------
            var result = CheckSegments(ref span); // reserved names and endings
            if (result != Validation.Ok)
                return result;
            
            return Validation.Ok;
        }
        
        private static Validation CheckSegments(ref ReadOnlySpan<char> span)
        {
            var segment = NextSegment(ref span, 0);
            while (!segment.IsLastSegment)
            {
                // reserved name?
                if (IsReservedName(ref span, segment.From, segment.To))
                    return Validation.ContainsReservedName;
                
                // ends with?
                if (segment.To is ' ' or '.')
                    return Validation.InvalidSegmentEnding;
                
                segment = NextSegment(ref span, segment.To + 1);
            }

            // check last segment
            if (IsReservedName(ref span, segment.From, segment.To))
                return Validation.ContainsReservedName;
            
            // ends with?
            if (span[^1] is ' ' or '.')
                return Validation.InvalidSegmentEnding;
            
            return Validation.Ok;
        }

        private static bool IsReservedName(ref ReadOnlySpan<char> span, int from, int to)
        {
            switch (to - from) // segment length
            {
                // CON
                case 3 when span[from] is 'C' or 'c' &&
                            span[from + 1] is 'O' or 'o' &&
                            span[from + 2] is 'N' or 'n':
                // PRN
                case 3 when span[from] is 'P' or 'p' &&
                            span[from + 1] is 'R' or 'r' &&
                            span[from + 2] is 'N' or 'n':
                // AUX
                case 3 when span[from] is 'A' or 'a' &&
                            span[from + 1] is 'U' or 'u' &&
                            span[from + 2] is 'X' or 'x':
                // NUL
                case 3 when span[from] is 'N' or 'n' &&
                            span[from + 1] is 'U' or 'u' &&
                            span[from + 2] is 'L' or 'l':
                // COM1 - COM9
                case 4 when span[from] is 'C' or 'c' &&
                            span[from + 1] is 'O' or 'o' &&
                            span[from + 2] is 'M' or 'm' &&
                            span[from + 3] is >= '1' and <= '9':
                // LPT1 - LPT9
                case 4 when span[from] is 'L' or 'l' &&
                            span[from + 1] is 'P' or 'p' &&
                            span[from + 2] is 'T' or 't' &&
                            span[from + 3] is >= '1' and <= '9':
                    return true;
                default:
                    return false;
            }
        }
        
        private static bool IsWhiteSpaceOnly(ref ReadOnlySpan<char> span)
        {
            foreach (var c in span)
                if (!char.IsWhiteSpace(c))
                    return false;

            return span.Length > 0;
        }
        
        private static (int From, int To, bool IsLastSegment) NextSegment(ref ReadOnlySpan<char> span, int start)
        {
            for (var i = start; i < span.Length; i++)
            {
                if (span[i] is '\\' or '/' or '.')
                    return (start, i, false);
            }

            return (start, span.Length, true);
        }
        
        #endregion
    }

    public class InvalidWindowsFilePathException : Exception
    {
        public InvalidWindowsFilePathException()
        {
        }

        public InvalidWindowsFilePathException(string message) : base(message)
        {
        }
    }
}
