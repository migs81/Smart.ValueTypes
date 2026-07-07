using Smart.ValueTypes.Types.Temperatures;

namespace Smart.ValueTypes.UnitTests.TestData
{
    internal class TemperatureGenerator
    {
        public static Temp[] CreateTemperatures(uint amount)
        {
            var array = new Temp[amount];
            for (int i = 0; i < amount; i++)
            {
                double celsius = NumberGenerator.NextDouble(Celsius.MinValue, 1_000_000);
                double kelvin = Converter.Celsius.ToKelvin(celsius);
                double fahrenheit = Converter.Celsius.ToFahrenheit(celsius);
                double reaumur = Converter.Celsius.ToReaumur(celsius);
                array[i] = new Temp(celsius, kelvin, fahrenheit, reaumur);
            }

            return array;
        }
    }

    internal readonly struct Temp(double celsius, double kelvin, double fahrenheit, double reaumur)
    {
        public double Celsius { get; } = celsius;
        public double Kelvin { get; } = kelvin;
        public double Fahrenheit { get; } = fahrenheit;
        public double Reaumur { get; } = reaumur;
    }
}
