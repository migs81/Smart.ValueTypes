using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.ColorModels
{
    /// <summary>
    /// Value type for the CIELAB color model.
    /// Lightness: 0 – 100
    /// GreenToRed: -128 - +127
    /// BlueToYellow: -128 - +127
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="InvalidCielabException"></exception>
    public readonly record struct CIELAB : IValueType<float, float, float, CIELAB>
    {
        #region fields

        public enum Validation
        {
            Ok = 0,
            LightnessTooLow,
            LightnessTooHigh,
            GreenToRedTooLow,
            GreenToRedToHigh,
            BlueToYellowTooLow,
            BlueToYellowTooHigh,
            UnknownError
        }

        #endregion

        #region properties

        public static CIELAB Empty => new();
        
        public float Lightness { get; }

        public float GreenToRed { get; }

        public float BlueToYellow { get; }

        #endregion

        #region constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CIELAB"/> struct.
        /// </summary>
        public CIELAB()
        {
            Lightness = 0f;
            GreenToRed = 0f;
            BlueToYellow = 0f;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CIELAB"/> struct.
        /// </summary>
        /// <param name="lightness">0 - 100</param>
        /// <param name="greenToRed">-128 - +127</param>
        /// <param name="blueToYellow">-128 - +127</param>
        /// <exception cref="InvalidCielabException"></exception>
        public CIELAB(float lightness, float greenToRed, float blueToYellow)
        {
            var result = ValidateFormat(ref lightness, ref greenToRed, ref blueToYellow);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.LightnessTooLow => new InvalidCielabException($"The value '{lightness}' is too low for {nameof(Lightness)}!"),
                    Validation.LightnessTooHigh => new InvalidCielabException($"The value '{lightness}' is too high for {nameof(Lightness)}!"),
                    Validation.GreenToRedTooLow => new InvalidCielabException($"The value '{greenToRed}' is too low for {nameof(GreenToRed)}!"),
                    Validation.GreenToRedToHigh => new InvalidCielabException($"The value '{greenToRed}' is too high for {nameof(GreenToRed)}!"),
                    Validation.BlueToYellowTooLow => new InvalidCielabException($"The value '{blueToYellow}' is too low for {nameof(BlueToYellow)}!"),
                    Validation.BlueToYellowTooHigh => new InvalidCielabException($"The value '{blueToYellow}' is too high for {nameof(BlueToYellow)}!"),
                    _ => new InvalidCielabException(),
                };
            }

            Lightness = lightness;
            GreenToRed = greenToRed;
            BlueToYellow = blueToYellow;
        }

        // required for internal initialization
        private CIELAB(ref float lightness, ref float greenToRed, ref float blueToYellow)
        {
            Lightness = lightness;
            GreenToRed = greenToRed;
            BlueToYellow = blueToYellow;
        }

        #endregion

        #region public methods

        public static CIELAB From(float lightness, float greenToRed, float blueToYellow) => new(lightness, greenToRed, blueToYellow);

        public static Validation TryFrom(float lightness, float greenToRed, float blueToYellow, out CIELAB output)
        {
            try
            {
                var result = ValidateFormat(ref lightness, ref greenToRed, ref blueToYellow);
                if (result == Validation.Ok)
                {
                    output = new CIELAB(ref lightness, ref greenToRed, ref blueToYellow);
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

        public static Validation ValidateFormat(float lightness, float greenToRed, float blueToYellow) =>
            ValidateFormat(ref lightness, ref greenToRed, ref blueToYellow);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref float lightness, ref float greenToRed, ref float blueToYellow)
        {
            // lightness
            if (lightness is < 0)
                return Validation.LightnessTooLow;
            
            if (lightness > 100)
                return Validation.LightnessTooHigh;
            
            // greenToRed
            if (greenToRed is < -128)
                return Validation.GreenToRedTooLow;
            
            if (greenToRed > 127)
                return Validation.GreenToRedToHigh;
            
            // blueToYellow
            if (blueToYellow is < -128)
                return Validation.BlueToYellowTooLow;
            
            if (blueToYellow > 127)
                return Validation.BlueToYellowTooHigh;
            
            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidCielabException : Exception
    {
        public InvalidCielabException()
        {
        }

        public InvalidCielabException(string message) : base(message)
        {
        }
    }
}
