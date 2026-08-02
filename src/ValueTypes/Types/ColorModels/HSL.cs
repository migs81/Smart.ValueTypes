using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.ColorModels
{
    /// <summary>
    /// Value type for the HSL color model.
    /// Hue: 0 – 360°
    /// Saturation: 0 – 1
    /// Lightness: 0 – 1
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="InvalidHslException"></exception>
    public readonly record struct HSL : IValueType<float, float, float, HSL>
    {
        #region fields

        public enum Validation
        {
            Ok = 0,
            HueTooLow,
            HueTooHigh,
            SaturationTooLow,
            SaturationToHigh,
            LightnessTooLow,
            LightnessTooHigh,
            UnknownError
        }

        #endregion

        #region properties

        public bool IsDefault => Hue == 0f && Saturation == 0f && Lightness == 0f;
        
        public float Hue { get; }

        public float Saturation { get; }

        public float Lightness { get; }

        #endregion

        #region constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HSL"/> struct.
        /// </summary>
        /// <param name="hue">0 – 360°</param>
        /// <param name="saturation">0 – 1</param>
        /// <param name="lightness">0 – 1</param>
        /// <exception cref="InvalidHslException"></exception>
        public HSL(float hue, float saturation, float lightness)
        {
            var result = ValidateFormat(ref hue, ref saturation, ref lightness);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.HueTooLow => new InvalidHslException($"The value '{hue}' is too low for {nameof(Hue)}!"),
                    Validation.HueTooHigh => new InvalidHslException($"The value '{hue}' is too high for {nameof(Hue)}!"),
                    Validation.SaturationTooLow => new InvalidHslException($"The value '{saturation}' is too low for {nameof(Saturation)}!"),
                    Validation.SaturationToHigh => new InvalidHslException($"The value '{saturation}' is too high for {nameof(Saturation)}!"),
                    Validation.LightnessTooLow => new InvalidHslException($"The value '{lightness}' is too low for {nameof(Lightness)}!"),
                    Validation.LightnessTooHigh => new InvalidHslException($"The value '{lightness}' is too high for {nameof(Lightness)}!"),
                    _ => new InvalidHslException(),
                };
            }

            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
        }

        // required for internal initialization
        private HSL(ref float hue, ref float saturation, ref float lightness)
        {
            Hue = hue;
            Saturation = saturation;
            Lightness = lightness;
        }

        #endregion

        #region public methods

        public static HSL From(float hue, float saturation, float lightness) => new(hue, saturation, lightness);

        public static Validation TryFrom(float hue, float saturation, float lightness, out HSL output)
        {
            try
            {
                var result = ValidateFormat(ref hue, ref saturation, ref lightness);
                if (result == Validation.Ok)
                {
                    output = new HSL(ref hue, ref saturation, ref lightness);
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

        public static Validation ValidateFormat(float hue, float saturation, float lightness) =>
            ValidateFormat(ref hue, ref saturation, ref lightness);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref float hue, ref float saturation, ref float lightness)
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
            
            // lightness
            if (lightness is < 0)
                return Validation.LightnessTooLow;
            
            if (lightness > 1)
                return Validation.LightnessTooHigh;
            
            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidHslException : Exception
    {
        public InvalidHslException()
        {
        }

        public InvalidHslException(string message) : base(message)
        {
        }
    }
}
