using System;
using Migs.ValueTypes.Interfaces;

namespace Migs.ValueTypes.Unfinished
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
            if (day is <= 0 or > 31)
                throw new ArgumentOutOfRangeException(nameof(day));

            if (month is <= 0 or > 12)
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
