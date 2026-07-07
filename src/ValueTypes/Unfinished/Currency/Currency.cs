using System;
using System.Collections.Generic;
using System.Globalization;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Unfinished.Currency
{
    public readonly record struct Currency : IValueType<string, string, string, Currency>
    {
        #region properties

        public string Name { get; }
        public string Code { get; }
        public string Symbol { get; }

        #endregion

        #region constructor

        public Currency()
        {
            Name = "";
            Code = "";
            Symbol = "";
        }
        public Currency(RegionInfo region)
        {
            _ = region ?? throw new ArgumentNullException(nameof(region));

            Name = region.CurrencyEnglishName;
            Code = region.ISOCurrencySymbol;
            Symbol = region.CurrencySymbol;
        }
        public Currency(string regionName)
        {
            if (regionName is null)
                throw new ArgumentNullException(nameof(regionName));

            if (regionName.Length == 0)
                throw new ArgumentException("Argument can not be empty!", nameof(regionName));

            RegionInfo region = new(regionName);

            Name = region.CurrencyEnglishName;
            Code = region.ISOCurrencySymbol;
            Symbol = region.CurrencySymbol;
        }
        private Currency(ref RegionInfo region)
        {
            Name = region.CurrencyEnglishName;
            Code = region.ISOCurrencySymbol;
            Symbol = region.CurrencySymbol;
        }

        #endregion

        #region operator

        public static implicit operator string(Currency currency) => currency.Name;

        #endregion

        #region public methods

        public static Currency From(RegionInfo region) => new(region);
        public static Currency From(string regionName) => new(regionName);
        public static bool TryFrom(string regionName, out Currency? output)
        {
            try
            {
                if (regionName is not null && regionName.Length > 0)
                {
                    var region = new RegionInfo(regionName);
                    output = new(ref region);
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
    }
}
