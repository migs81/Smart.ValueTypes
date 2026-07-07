namespace Smart.ValueTypes.UnitTests.TestData
{
    internal static class Converter
    {
        public static class Celsius
        {
            public static double ToKelvin(double celsius) => celsius + 273.15d;
            public static double ToFahrenheit(double celsius) => (celsius * 9d / 5d) + 32d;
            public static double ToReaumur(double celsius) => celsius * 4 / 5;
        }
        public static class Kelvin
        {
            public static double ToCelsius(double kelvin) => kelvin - 273.15d;
            public static double ToFahrenheit(double kelvin) => (kelvin - 273.15d) * 9d / 5d + 32d;
            public static double ToReaumur(double kelvin) => (kelvin - 273.15) * 4 / 5;
        }
        public static class Fahrenheit
        {
            public static double ToCelsius(double fahrenheit) => (fahrenheit - 32d) * 5d / 9d;
            public static double ToKelvin(double fahrenheit) => (fahrenheit - 32d) * 5d / 9d + 273.15d;
            public static double ToReaumur(double fahrenheit) => (fahrenheit - 32) * 4 / 9;
        }
        public static class Reaumur
        {
            public static double ToCelsius(double reaumur) => reaumur * 5 / 4;
            public static double ToKelvin(double reaumur) => (reaumur * 9 / 8) + 273.15;
            public static double ToFahrenheit(double reaumur) => (reaumur * 9 / 4) + 32;
        }
    }
}
