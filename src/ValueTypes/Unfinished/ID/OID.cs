using Migs.ValueTypes.Interfaces;
using System;
using System.Collections.Generic;

namespace Migs.ValueTypes.Unfinished.ID
{
    public readonly record struct OID : IValueType<string, OID>
    {
        #region fields

        private readonly string _value = "";
        private readonly int _id = 0;

        public enum Validation
        {
            Ok = 0,
            Null,
            Empty,
            IllegalCharacter,
            UnknownError
        }

        #endregion

        #region properties

        public static OID Default => new();

        #endregion

        #region constructor

        public OID() { }
        public OID(string value) => _value = value;
        public OID(int value)
        {
            _id = value;
            _value = Convert.ToBase64String(BitConverter.GetBytes(value));
        }
        private OID(ref string value) => _value = value;

        #endregion

        #region operator

        public static bool operator ==(OID left, string right) => left.Equals(right);
        public static bool operator !=(OID left, string right) => !left.Equals(right);

        public static implicit operator string(OID oid) => oid._value;
        public static implicit operator OID(string value) => new(value);

        #endregion

        #region public methods

        public bool Equals(string value) => EqualityComparer<string>.Default.Equals(value, value);

        public static OID New() => new();

        public static OID From(string hash) => new(hash);
        public static Validation TryFrom(string value, out OID output)
        {
            try
            {
                var result = Validate(ref value);
                if (result == Validation.Ok)
                {
                    output = new OID(ref value);
                    return Validation.Ok;
                }

                output = Default;
                return result;
            }
            catch (Exception)
            {
                output = Default;
                return Validation.UnknownError;
            }
        }

        public static Validation Validate(string value) => Validate(ref value);

        #endregion

        #region private methods

        private static Validation Validate(ref string value)
        {
            // not null
            if (value is null)
                return Validation.Null;

            // must be hex
            foreach (var c in value.AsSpan())
            {
                if ((c < '0' || c > '9') && (c < 'a' || c > 'f') && (c < 'A' || c > 'F'))
                    return Validation.IllegalCharacter;
            }

            return Validation.Ok;
        }

        #endregion
    }
}
