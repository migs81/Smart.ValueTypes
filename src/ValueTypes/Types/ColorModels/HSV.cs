using System;
using Migs.ValueTypes.Interfaces;

namespace Migs.ValueTypes.Types.ColorModels
{
    /// <summary>
    /// Value type for the HSV color model.
    /// Hue: 0 – 360°
    /// Saturation: 0 – 1
    /// Value: 0 – 1
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="InvalidHsvException"></exception>
    public readonly record struct HSV : IValueType<float, float, float, HSV>
    {
        #region fields

        public enum Validation
        {
            Ok = 0,
            HueTooLow,
            HueTooHigh,
            SaturationTooLow,
            SaturationToHigh,
            ValueTooLow,
            ValueTooHigh,
            UnknownError
        }

        #endregion

        #region properties

        public static HSV Empty => new();
        
        public float Hue { get; }

        public float Saturation { get; }

        public float Value { get; }

        #endregion

        #region constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HSV"/> struct.
        /// </summary>
        public HSV()
        {
            Hue = 0f;
            Saturation = 0f;
            Value = 0f;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HSV"/> struct.
        /// </summary>
        /// <param name="hue">0 – 360°</param>
        /// <param name="saturation">0 – 1</param>
        /// <param name="value">0 – 1</param>
        /// <exception cref="InvalidHsvException"></exception>
        public HSV(float hue, float saturation, float value)
        {
            var result = ValidateFormat(ref hue, ref saturation, ref value);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.HueTooLow => new InvalidHsvException($"The value '{hue}' is too low for {nameof(Hue)}!"),
                    Validation.HueTooHigh => new InvalidHsvException($"The value '{hue}' is too high for {nameof(Hue)}!"),
                    Validation.SaturationTooLow => new InvalidHsvException($"The value '{saturation}' is too low for {nameof(Saturation)}!"),
                    Validation.SaturationToHigh => new InvalidHsvException($"The value '{saturation}' is too high for {nameof(Saturation)}!"),
                    Validation.ValueTooLow => new InvalidHsvException($"The value '{value}' is too low for {nameof(Value)}!"),
                    Validation.ValueTooHigh => new InvalidHsvException($"The value '{value}' is too high for {nameof(Value)}!"),
                    _ => new InvalidHsvException(),
                };
            }

            Hue = hue;
            Saturation = saturation;
            Value = value;
        }

        // required for internal initialization
        private HSV(ref float hue, ref float saturation, ref float value)
        {
            Hue = hue;
            Saturation = saturation;
            Value = value;
        }

        #endregion

        #region public methods

        public static HSV From(float hue, float saturation, float value) => new(hue, saturation, value);

        public static Validation TryFrom(float hue, float saturation, float value, out HSV output)
        {
            try
            {
                var result = ValidateFormat(ref hue, ref saturation, ref value);
                if (result == Validation.Ok)
                {
                    output = new HSV(ref hue, ref saturation, ref value);
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

        public static Validation ValidateFormat(float hue, float saturation, float value) =>
            ValidateFormat(ref hue, ref saturation, ref value);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref float hue, ref float saturation, ref float value)
        {
            // hue
            if (hue is < 0)
                return Validation.HueTooLow;
            
            if (hue > 360)
                return Validation.HueTooHigh;
            
            // saturation
            if (saturation is < 0)
                return Validation.SaturationTooLow;
            
            if (saturation > 1)
                return Validation.SaturationToHigh;
            
            // value
            if (value is < 0)
                return Validation.ValueTooLow;
            
            if (value > 1)
                return Validation.ValueTooHigh;
            
            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidHsvException : Exception
    {
        public InvalidHsvException()
        {
        }

        public InvalidHsvException(string message) : base(message)
        {
        }
    }
}
