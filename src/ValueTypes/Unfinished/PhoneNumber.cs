using System;
using System.Text.RegularExpressions;

namespace Migs.ValueTypes
{
    public readonly record struct PhoneNumber
    {
        public string CountryCode { get; }
        public string AreaCode { get; }
        public string Number { get; }

        public PhoneNumber(string countryCode, string areaCode, string number)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
                throw new ArgumentException("Country code cannot be null or empty", nameof(countryCode));

            if (string.IsNullOrWhiteSpace(areaCode))
                throw new ArgumentException("Area code cannot be null or empty", nameof(areaCode));

            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("Number cannot be null or empty", nameof(number));

            // Validate the format of the phone number
            var regex = new Regex(@"^\+[0-9]{1,3}\.[0-9]{4,14}(?:x.+)?$");

            if (!regex.IsMatch($"{countryCode}.{areaCode}{number}"))
                throw new ArgumentException("Invalid phone number format");

            CountryCode = countryCode;
            AreaCode = areaCode;
            Number = number;
        }
    }
}
