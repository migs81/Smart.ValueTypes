using System;
using Smart.ValueTypes.Interfaces;

namespace Smart.ValueTypes.Types.ColorModels
{
    /// <summary>
    /// Value type for the YCbCr color model.
    /// Luma: 0 – 1
    /// BlueDifference: -0.5 – +0.5
    /// RedDifference: -0.5 – +0.5
    /// </summary>
    /// <seealso cref="IValueType{TValue,TThis}" />
    /// <exception cref="InvalidYCbCrException"></exception>
    public readonly record struct YCbCr : IValueType<float, float, float, YCbCr>
    {
        #region fields

        public enum Validation
        {
            Ok = 0,
            LumaTooLow,
            LumaTooHigh,
            BlueDifferenceTooLow,
            BlueDifferenceToHigh,
            RedDifferenceTooLow,
            RedDifferenceTooHigh,
            UnknownError
        }

        #endregion

        #region properties

        public static YCbCr Empty => new();
        
        public float Luma { get; }

        public float BlueDifference { get; }

        public float RedDifference { get; }

        #endregion

        #region constructor
        
        /// <summary>
        /// Initializes a new instance of the <see cref="YCbCr"/> struct.
        /// </summary>
        public YCbCr()
        {
            Luma = 0f;
            BlueDifference = 0f;
            RedDifference = 0f;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="YCbCr"/> struct.
        /// </summary>
        /// <param name="luma">0 - 100</param>
        /// <param name="blueDifference">-128 - +127</param>
        /// <param name="redDifference">-128 - +127</param>
        /// <exception cref="InvalidYCbCrException"></exception>
        public YCbCr(float luma, float blueDifference, float redDifference)
        {
            var result = ValidateFormat(ref luma, ref blueDifference, ref redDifference);
            if (result != Validation.Ok)
            {
                throw result switch
                {
                    Validation.LumaTooLow => new InvalidYCbCrException($"The value '{luma}' is too low for {nameof(Luma)}!"),
                    Validation.LumaTooHigh => new InvalidYCbCrException($"The value '{luma}' is too high for {nameof(Luma)}!"),
                    Validation.BlueDifferenceTooLow => new InvalidYCbCrException($"The value '{blueDifference}' is too low for {nameof(BlueDifference)}!"),
                    Validation.BlueDifferenceToHigh => new InvalidYCbCrException($"The value '{blueDifference}' is too high for {nameof(BlueDifference)}!"),
                    Validation.RedDifferenceTooLow => new InvalidYCbCrException($"The value '{redDifference}' is too low for {nameof(RedDifference)}!"),
                    Validation.RedDifferenceTooHigh => new InvalidYCbCrException($"The value '{redDifference}' is too high for {nameof(RedDifference)}!"),
                    _ => new InvalidYCbCrException(),
                };
            }

            Luma = luma;
            BlueDifference = blueDifference;
            RedDifference = redDifference;
        }

        // required for internal initialization
        private YCbCr(ref float luma, ref float blueDifference, ref float redDifference)
        {
            Luma = luma;
            BlueDifference = blueDifference;
            RedDifference = redDifference;
        }

        #endregion

        #region public methods

        public static YCbCr From(float luma, float blueDifference, float redDifference) => new(luma, blueDifference, redDifference);

        public static Validation TryFrom(float luma, float blueDifference, float redDifference, out YCbCr output)
        {
            try
            {
                var result = ValidateFormat(ref luma, ref blueDifference, ref redDifference);
                if (result == Validation.Ok)
                {
                    output = new YCbCr(ref luma, ref blueDifference, ref redDifference);
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

        public static Validation ValidateFormat(float luma, float blueDifference, float redDifference) =>
            ValidateFormat(ref luma, ref blueDifference, ref redDifference);

        #endregion

        #region private methods

        private static Validation ValidateFormat(ref float luma, ref float blueDifference, ref float redDifference)
        {
            // luma
            if (luma is < 0f)
                return Validation.LumaTooLow;
            
            if (luma > 1f)
                return Validation.LumaTooHigh;
            
            // blueDifference
            if (blueDifference is < -0.5f)
                return Validation.BlueDifferenceTooLow;
            
            if (blueDifference > 0.5f)
                return Validation.BlueDifferenceToHigh;
            
            // redDifference
            if (redDifference is < -0.5f)
                return Validation.RedDifferenceTooLow;
            
            if (redDifference > 0.5f)
                return Validation.RedDifferenceTooHigh;
            
            return Validation.Ok;
        }

        #endregion
    }

    public class InvalidYCbCrException : Exception
    {
        public InvalidYCbCrException()
        {
        }

        public InvalidYCbCrException(string message) : base(message)
        {
        }
    }
}
