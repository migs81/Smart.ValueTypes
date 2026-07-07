using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.ColorModels
{
    /// <summary>
    /// Value type for the CMYK color model.
    /// Cyan: 0 – 1
    /// Magenta: 0 – 1
    /// Yellow: 0 – 1
    /// Key: 0 – 1
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="InvalidCmykException"></exception>
    public readonly record struct CMYK : IValueType<float, float, float, float, CMYK>
    {
        #region fields

        public enum Validation
        {
            Ok = 0,
            CyanTooLow,
            CyanTooHigh,
            MagentaTooLow,
            MagentaToHigh,
            YellowTooLow,
            YellowTooHigh,
            KeyTooLow,
            KeyTooHigh,
            UnknownError
        }

        #endregion

        #region properties

        public static CMYK Empty => new();
        
        public float Cyan { get; }

        public float Magenta { get; }

        public float Yellow { get; }
        
        public float Key { get; }

        #endregion

        #region constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CMYK"/> struct.
        /// </summary>
        public CMYK()
        {
            Cyan = 0f;
            Magenta = 0f;
            Yellow = 0f;
            Key = 0f;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CMYK"/> struct.
        /// </summary>
        /// <param name="cyan">0 – 1</param>
        /// <param name="magenta">0 – 1</param>
        /// <param name="yellow">0 – 1</param>
        /// <param name="key">0 – 1</param>
        /// <exception cref="InvalidCmykException"></exception>
        public CMYK(float cyan, float magenta, float yellow, float key)
        {
            var result = ValidateFormat(ref cyan, ref magenta, ref yellow, ref key);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.CyanTooLow => new InvalidCmykException($"The value '{cyan}' is too low for {nameof(Cyan)}!"),
                    Validation.CyanTooHigh => new InvalidCmykException($"The value '{cyan}' is too high for {nameof(Cyan)}!"),
                    Validation.MagentaTooLow => new InvalidCmykException($"The value '{magenta}' is too low for {nameof(Yellow)}!"),
                    Validation.MagentaToHigh => new InvalidCmykException($"The value '{magenta}' is too high for {nameof(Yellow)}!"),
                    Validation.YellowTooLow => new InvalidCmykException($"The value '{yellow}' is too low for {nameof(Magenta)}!"),
                    Validation.YellowTooHigh => new InvalidCmykException($"The value '{yellow}' is too high for {nameof(Magenta)}!"),
                    Validation.KeyTooLow => new InvalidCmykException($"The value '{key}' is too low for {nameof(Key)}!"),
                    Validation.KeyTooHigh => new InvalidCmykException($"The value '{key}' is too high for {nameof(Key)}!"),
                    _ => new InvalidCmykException(),
                };
            }

            Cyan = cyan;
            Magenta = magenta;
            Yellow = yellow;
            Key = key;
        }

        // required for internal initialization
        private CMYK(ref float cyan, ref float magenta, ref float yellow, ref float key)
        {
            Cyan = cyan;
            Magenta = magenta;
            Yellow = yellow;
            Key = key;
        }

        #endregion

        #region public methods

        public static CMYK From(float cyan, float magenta, float yellow, float key) => new(cyan, magenta, yellow, key);

        public static Validation TryFrom(float cyan, float magenta, float yellow, float key, out CMYK output)
        {
            try
            {
                var result = ValidateFormat(ref cyan, ref magenta, ref yellow, ref key);
                if (result == Validation.Ok)
                {
                    output = new CMYK(ref cyan, ref magenta, ref yellow, ref key);
                    return Validation.Ok;
                }

                output = Empty;
                return result;
            }
            catch (Exception)
            {
                output = Empty;
                return Validation.UnknownError;
            }
        }

        public static Validation ValidateFormat(float cyan, float magenta, float yellow, float key) =>
            ValidateFormat(ref cyan, ref magenta, ref yellow, ref key);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref float cyan, ref float magenta, ref float yellow, ref float key)
        {
            // cyan
            if (cyan is < 0)
                return Validation.CyanTooLow;
            
            if (cyan > 1)
                return Validation.CyanTooHigh;
            
            // magenta
            if (magenta is < 0)
                return Validation.MagentaTooLow;
            
            if (magenta > 1)
                return Validation.MagentaToHigh;
            
            // yellow
            if (yellow is < 0)
                return Validation.YellowTooLow;
            
            if (yellow > 1)
                return Validation.YellowTooHigh;
            
            // key
            if (key is < 0)
                return Validation.KeyTooLow;
            
            if (key > 1)
                return Validation.KeyTooHigh;
            
            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidCmykException : Exception
    {
        public InvalidCmykException()
        {
        }

        public InvalidCmykException(string message) : base(message)
        {
        }
    }
}
