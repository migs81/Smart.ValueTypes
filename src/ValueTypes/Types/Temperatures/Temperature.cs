namespace Migs.ValueTypes.Types.Temperatures
{
    public readonly record struct Temperature
    {
        #region properties

        public Celsius Celsius { get; }
        public Kelvin Kelvin { get; }
        public Fahrenheit Fahrenheit { get; }
        public Reaumur Reaumur { get; }

        #endregion

        #region constructor

        public Temperature(Celsius celsius)
        {
            Celsius = celsius;

            Kelvin = Celsius.ToKelvin();
            Fahrenheit = Celsius.ToFahrenheit();
            Reaumur = Celsius.ToReaumur();
        }

        public Temperature(Kelvin kelvin)
        {
            Kelvin = kelvin;

            Celsius = Kelvin.ToCelsius();
            Fahrenheit = Kelvin.ToFahrenheit();
            Reaumur = Kelvin.ToReaumur();
        }

        public Temperature(Fahrenheit fahrenheit)
        {
            Fahrenheit = fahrenheit;

            Celsius = Fahrenheit.ToCelsius();
            Kelvin = Fahrenheit.ToKelvin();
            Reaumur = Fahrenheit.ToReaumur();
        }

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
