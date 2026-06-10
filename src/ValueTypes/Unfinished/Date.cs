using Migs.ValueTypes.Interfaces;
using System;

namespace Migs.ValueTypes
{
    public readonly record struct Date : IValueType<byte, byte, int, Date>
    {
        public byte Day { get; }
        public byte Month { get; }
        public int Year { get; }

        public Date()
        {
            Day = 1;
            Month = 1;
            Year = 1;
        }
        public Date(byte day, byte month, int year)
        {
            if (day <= 0 || day > 31)
                throw new ArgumentOutOfRangeException(nameof(day));

            if (month <= 0 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month));

            Day = day;
            Month = month;
            Year = year;
        }

        public bool Equals(byte day, byte month, int year)
        {
            // TO DO
            return false;
        }
    }
}
