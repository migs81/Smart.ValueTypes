using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.Web
{
    /// <summary>
    /// Value type for Url's.
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidUrlException"></exception>
    public readonly record struct Url : IValueType<string, Url>
    {
        #region fields

        private readonly string? _value;

        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            MissingSchemeSeparator,
            UnsupportedScheme,
            MissingHost,
            HostTooLong,
            HostContainsIllegalCharacter,
            EmptyLabel,
            LabelTooLong,
            LabelStartsWithHyphen,
            LabelEndsWithHyphen,
            InvalidPort,
            PathContainsIllegalCharacter,
            InvalidPercentEncoding,
            ContainsUnencodedSpace,
            MultipleFragmentIdentifiers,
            FragmentContainsIllegalCharacter,
            QueryContainsIllegalCharacter,
            UnknownError,
        }
        
        #endregion

        #region properties

        public bool IsDefault => _value is null;

        public string Scheme
        {
            get
            {
                if (_value is null)
                    return "";

                var separator = _value.IndexOf("://", StringComparison.InvariantCulture);
                if (separator == -1)
                    return "";
                
                return _value[..separator];
            }
        }

        public string Host
        {
            get
            {
                if (_value is null)
                    return "";

                var span = _value.AsSpan();
                var hostPos = GetHostPosition(ref span);
                return _value[hostPos.Start..hostPos.End];
            }
        }

        public int Port
        {
            get
            {
                if (_value is null)
                    return 0;

                var span = _value.AsSpan();
                return GetPort(ref span);
            }
        }

        public string Path
        {
            get
            {
                if (_value is null)
                    return "";

                var span = _value.AsSpan();
                return GetPath(ref span);
            }
        }
        
        public string Query
        {
            get
            {
                if (_value is null)
                    return "";

                var span = _value.AsSpan();
                return GetQuery(ref span);
            }
        }
        
        public string Fragment
        {
            get
            {
                if (_value is null)
                    return "";

                var span = _value.AsSpan();
                return GetFragment(ref span);
            }
        }
        
        #endregion
        
        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Url"/> struct.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"></exception>
        public Url(string value)
        {
            var result = ValidateFormat(ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.Null => throw new ArgumentNullException(nameof(value)),
                    Validation.Empty => throw new ArgumentException("Argument can not be empty!", nameof(value)),
                    Validation.MissingSchemeSeparator => throw new InvalidUrlException($"The url {value} contains no scheme separator '://'!"),
                    Validation.UnsupportedScheme => throw new InvalidUrlException($"The url {value} contains no supported scheme!"),
                    Validation.MissingHost => throw new InvalidUrlException($"The url {value} contains no host!"),
                    Validation.HostTooLong => throw new InvalidUrlException($"The host of the url {value} exceeds the maximum allowed length of 253 characters!"),
                    Validation.HostContainsIllegalCharacter => throw new InvalidUrlException($"The host of the url {value} contains an illegal character!"),
                    Validation.EmptyLabel => throw new InvalidUrlException($"The url {value} contains an empty label!"),
                    Validation.LabelTooLong => throw new InvalidUrlException($"The host label of the url {value} exceeds the maximum allowed length of 63 characters."),
                    Validation.LabelStartsWithHyphen => throw new InvalidUrlException($"The host label of the url {value} starts with a hyphen!"),
                    Validation.LabelEndsWithHyphen => throw new InvalidUrlException($"The host label of the url {value} ends with a hyphen!"),
                    Validation.InvalidPort => throw new InvalidUrlException($"The port specified in the url {value} is not valid!"),
                    Validation.PathContainsIllegalCharacter => throw new InvalidUrlException($"The path in the url {value} contains a illegal characters!"),
                    Validation.InvalidPercentEncoding => throw new InvalidUrlException($"The url {value} contains incorrect percent encodings!"),
                    Validation.ContainsUnencodedSpace => throw new InvalidUrlException($"The url {value} contains incorrectly encoded spaces!"),
                    Validation.MultipleFragmentIdentifiers => throw new InvalidUrlException($"The url {value} contains multiple fragment identifiers!"),
                    Validation.QueryContainsIllegalCharacter => throw new InvalidUrlException($"The query in the url {value} contains a illegal characters!"),
                    Validation.FragmentContainsIllegalCharacter => throw new InvalidUrlException($"The fragment in the url {value} contains a illegal characters!"),
                    _ => new InvalidUrlException()
                };
            }

            _value = value;
        }
        
        // required for internal initialization
        private Url(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(Url left, string right) => left.Equals(right);
        public static bool operator !=(Url left, string right) => !left.Equals(right);

        public static implicit operator string(Url url) => url._value ?? "";
        public static implicit operator Url(string value) => new(value);

        #endregion

        #region public methods

        public static Url From(string value) => new(value);
        
        public static Validation TryFrom(string value, out Url output)
        {
            try
            {
                var result = ValidateFormat(ref value);
                if (result == Validation.Ok)
                {
                    output = new Url(ref value);
                    return Validation.Ok;
                }

                output = default;
                return result;
            }
            catch (Exception)
            {
                output = default;
                return Validation.UnknownError;
            }
        }
        
        public static Url Parse(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (value.Length == 0)
                throw new ArgumentException("Argument can not be empty!", nameof(value));
                
            // replace white spaces (only after the host)
            value = value.Trim();
            var span = value.AsSpan();
            var host = GetHostPosition(ref span);
            value = value[..host.End] + value[host.End..].Replace(" ", "%20");

            // validate it
            return new Url(value);
        }

        public static bool TryParse(string value, out Url output)
        {
            try
            {
                output = Parse(value);
                return true;
            }
            catch (Exception)
            {
                output = default;
                return false;
            }
        }
        
        public static Validation ValidateFormat(string value) => ValidateFormat(ref value);
        
        #endregion

        #region private methods

        private static Validation ValidateFormat(ref string value)
        {
            // ---------- general -----------
            if (value is null)
                return Validation.Null;
            
            if (value.Length == 0)
                return Validation.Empty;

            var span = value.AsSpan();
            
            // ----------- scheme -----------
            var host = GetHostPosition(ref span);
            if (host.Start == -1)
                return Validation.MissingSchemeSeparator;
            
            if (span[..(host.Start - 3)] is not "http" and not "https")
                return Validation.UnsupportedScheme;

            // ------------ host ------------
            if (host.End <= host.Start)
                return Validation.MissingHost;

            if (host.End - host.Start > 253)
                return Validation.HostTooLong;
            
            var result = ValidateHost(ref span, host.Start, host.End);
            if (result != Validation.Ok)
                return result;

            // stop?
            if (host.End >= span.Length) return Validation.Ok;
            
            // remember last position
            var pos = host.End;
            
            // ------------ port ------------
            if (span[pos] is ':')
            {
                if (pos == span.Length - 1)
                    return Validation.InvalidPort;

                var portEnd = pos + 1;
                do
                {
                    if (span[portEnd] is '/' or '?' or '#')
                        break;

                    portEnd++;
                } while (portEnd < span.Length);

                if (!int.TryParse(span[(pos + 1)..portEnd], out var port))
                    return Validation.InvalidPort;

                if (port == 0)
                    return Validation.InvalidPort;

                if (port > 65_535)
                    return Validation.InvalidPort;
            }
            
            // ----------- path ------------
            if (span[pos] is '/')
            {
                for (var i = pos + 1; i < span.Length; i++)
                {
                    if (span[i] is '?' or '#') // query or fragment
                        break;

                    if (char.IsControl(span[i]))
                        return Validation.PathContainsIllegalCharacter;

                    if (span[i] is ' ')
                        return Validation.ContainsUnencodedSpace;
                    
                    if (span[i] is '%')
                    {
                        if (i + 2 >= span.Length)
                            return Validation.InvalidPercentEncoding;

                        if (span[i + 1] is (< '0' or > '9') and (< 'a' or > 'f') and (< 'A' or > 'F'))
                            return Validation.InvalidPercentEncoding;

                        // step forward
                        i += 2;
                    }
                }
            }

            // ----------- query ------------
            if (span[pos] is '?')
            {
                for (var i = pos + 1; i < span.Length; i++)
                {
                    if (span[i] is '#') // fragment
                        break;

                    if (char.IsControl(span[i]))
                        return Validation.QueryContainsIllegalCharacter;

                    if (span[i] is ' ')
                        return Validation.ContainsUnencodedSpace;
                    
                    if (span[i] is '%')
                    {
                        if (i + 2 >= span.Length)
                            return Validation.InvalidPercentEncoding;

                        if (span[i + 1] is (< '0' or > '9') and (< 'a' or > 'f') and (< 'A' or > 'F'))
                            return Validation.InvalidPercentEncoding;

                        // step forward
                        i += 2;
                    }
                }
            }
            
            // ---------- fragment ----------
            if (span[pos] is '#')
            {
                for (var i = pos + 1; i < span.Length; i++)
                {
                    if (span[i] is '#')
                        return Validation.MultipleFragmentIdentifiers;

                    if (char.IsControl(span[i]))
                        return Validation.FragmentContainsIllegalCharacter;

                    if (span[i] is ' ')
                        return Validation.ContainsUnencodedSpace;
                    
                    if (span[i] is '%')
                    {
                        if (i + 2 >= span.Length)
                            return Validation.InvalidPercentEncoding;

                        if (span[i + 1] is (< '0' or > '9') and (< 'a' or > 'f') and (< 'A' or > 'F'))
                            return Validation.InvalidPercentEncoding;

                        // step forward
                        i += 2;
                    }
                }
            }

            return Validation.Ok;
        }

        private static Validation ValidateHost(ref ReadOnlySpan<char> span, int start, int end)
        {
            var lastLabelPos = start - 1;
            for (var i = start; i < end; i++)
            {
                // the first character of a label can not be a hyphen
                if (i == lastLabelPos + 1 && span[i] is '-')
                    return Validation.LabelStartsWithHyphen;
                
                // allowed characters
                if (span[i] is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or >= '0' and <= '9' or '-') continue;

                // label separator?
                if (span[i] is '.')
                {
                    if (i == lastLabelPos + 1)
                        return Validation.EmptyLabel;
                    
                    if (i - lastLabelPos - 1 > 63)
                        return Validation.LabelTooLong;
                    
                    if (span[i - 1] is '-')
                        return Validation.LabelEndsWithHyphen;

                    lastLabelPos = i;
                }
                else
                {
                    return Validation.HostContainsIllegalCharacter;
                }
            }

            // check last character
            if (span[end - 1] is '-')
                return Validation.LabelEndsWithHyphen;

            // if the host contains only one dot, do not allow it to be at the end
            if (span[end - 1] is '.' && span[start..end].Count('.') == 1)
                return Validation.EmptyLabel;
            
            return Validation.Ok;
        }

        private static (int Start, int End) GetHostPosition(ref ReadOnlySpan<char> span)
        {
            var start = span.IndexOf("://", StringComparison.InvariantCulture);
            if (start == -1)
                return (-1, -1);

            start += 3;
            for (var i = start; i < span.Length; i++)
            {
                if (span[i] is ':' or '/' or '?' or '#')
                    return (start, i);
            }

            return (start, span.Length);
        }

        private static int GetPort(ref ReadOnlySpan<char> span)
        {
            var host = GetHostPosition(ref span);
            if (host.End == span.Length || span[host.End] != ':')
                return 0;
            
            var stop = span.Length;
            for (var i = host.End + 1; i < span.Length; i++)
            {
                if (span[i] is >= '0' and <= '9') continue;

                stop = i;
                break;
            }

            var port = int.Parse(span[(host.End + 1)..stop]);
            return port;
        }
        
        private static string GetPath(ref ReadOnlySpan<char> span)
        {
            var host = GetHostPosition(ref span);
            if (host.End == span.Length)
                return "";

            // beginning of the path
            var start = span[host.End..].IndexOf('/');
            if (start == -1)
                return "";
            start = host.End + start + 1;
            
            // end of the path
            var query = span[start..].IndexOf('?');
            if (query != -1)
                return span[start..(start + query)].ToString();
            
            var fragment = span[start..].IndexOf('#');
            if (fragment != -1)
                return span[start..(start + fragment)].ToString();
            
            return span[start..].ToString();
        }

        private static string GetQuery(ref ReadOnlySpan<char> span)
        {
            var host = GetHostPosition(ref span);
            if (host.End == span.Length)
                return "";

            // beginning of the query
            var start = span[host.End..].IndexOf('?');
            if (start == -1)
                return "";
            start = host.End + start + 1;
            
            // end of the query
            var stop = span[start..].IndexOf('#');
            if (stop == -1)
                return span[start..].ToString();
            stop = start + stop;
            
            return span[start..stop].ToString();
        }

        private static string GetFragment(ref ReadOnlySpan<char> span)
        {
            var host = GetHostPosition(ref span);
            if (host.End == span.Length)
                return "";

            // beginning of the fragment
            var start = span[host.End..].IndexOf('#');
            if (start == -1)
                return "";
            
            return span[(host.End + start + 1)..].ToString();
        }
        
        #endregion
    }
    
    public class InvalidUrlException : Exception
    {
        public InvalidUrlException()
        {
        }

        public InvalidUrlException(string message) : base(message)
        {
        }
    }
}
