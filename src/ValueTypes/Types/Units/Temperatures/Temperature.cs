namespace Smart.ValueTypes.Types.Units.Temperatures
{
    /// <summary>
    /// Represents a temperature value with support for multiple units (Celsius, Fahrenheit, Kelvin)
    /// </summary>
    /// <exception cref="InvalidCelsiusException"></exception>
    /// <exception cref="InvalidKelvinException"></exception>
    /// <exception cref="InvalidFahrenheitException"></exception>
    /// <exception cref="InvalidReaumurException"></exception>
    public readonly record struct Temperature
    {
        #region properties

        public Celsius Celsius { get; }
        public Kelvin Kelvin { get; }
        public Fahrenheit Fahrenheit { get; }
        public Reaumur Reaumur { get; }

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Temperature"/> struct.
        /// </summary>
        /// <param name="celsius"></param>
        public Temperature(Celsius celsius)
        {
            Celsius = celsius;

            Kelvin = Celsius.ToKelvin();
            Fahrenheit = Celsius.ToFahrenheit();
            Reaumur = Celsius.ToReaumur();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Temperature"/> struct.
        /// </summary>
        /// <param name="kelvin"></param>
        public Temperature(Kelvin kelvin)
        {
            Kelvin = kelvin;

            Celsius = Kelvin.ToCelsius();
            Fahrenheit = Kelvin.ToFahrenheit();
            Reaumur = Kelvin.ToReaumur();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Temperature"/> struct.
        /// </summary>
        /// <param name="fahrenheit"></param>
        public Temperature(Fahrenheit fahrenheit)
        {
            Fahrenheit = fahrenheit;

            Celsius = Fahrenheit.ToCelsius();
            Kelvin = Fahrenheit.ToKelvin();
            Reaumur = Fahrenheit.ToReaumur();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Temperature"/> struct.
        /// </summary>
        /// <param name="reaumur"></param>
        public Temperature(Reaumur reaumur)
        {
            Reaumur = reaumur;

            Fahrenheit = Reaumur.ToFahrenheit();
            Celsius = Reaumur.ToCelsius();
            Kelvin = Reaumur.ToKelvin();
        }

        #endregion
    }
}
