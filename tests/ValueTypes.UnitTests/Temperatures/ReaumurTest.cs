using Smart.ValueTypes.Types.Temperatures;
using Smart.ValueTypes.UnitTests.TestData;
using System;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Temperatures
{
    public class ReaumurTest
    {
        #region test data

        private static readonly Temp[] _validTemperatures = TemperatureGenerator.CreateTemperatures(1000);
        private const double _tooLowValue = Reaumur.MinValue - 1;

        #endregion

        #region constructor tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new Reaumur();
            Assert.Equal(Reaumur.Zero, result);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(7)]
        [InlineData(99)]
        public void Constructor_InputInteger_ShouldReturnObject(int value)
        {
            var result = new Reaumur(value);
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void Constructor_InputDouble_ShouldReturnObject()
        {
            foreach (var temp in _validTemperatures)
            {
                var result = new Reaumur(temp.Reaumur);
                Assert.Equal(temp.Reaumur, result, 0);
            }
        }

        [Fact]
        public void Constructor_InputCelsius_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Celsius celsius = new(temp.Celsius);

                // act
                Reaumur kelvin = new(celsius);

                // assert
                Assert.Equal(temp.Reaumur, kelvin, 0);
            }
        }

        [Fact]
        public void Constructor_InputFahrenheitValues_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                Reaumur kelvin = new(fahrenheit);

                // assert
                Assert.Equal(temp.Reaumur, kelvin, 0);
            }
        }

        [Theory]
        [InlineData(_tooLowValue, typeof(InvalidReaumurException))]
        public void Constructor_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            var result = Record.Exception(() => new Reaumur(input));
            Assert.Equal(expectedException, result.GetType());
        }

        #endregion

        #region other tests

        [Theory]
        [InlineData(-10)]
        [InlineData(7)]
        [InlineData(99)]
        public void From_ValidInteger_ShouldReturnObject(int value)
        {
            var result = Reaumur.From(value);
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void From_ValidDouble_ShouldReturnObject()
        {
            foreach (var temp in _validTemperatures)
            {
                var result = Reaumur.From(temp.Reaumur);
                Assert.Equal(temp.Reaumur, result, 0);
            }
        }

        [Fact]
        public void TryFrom_ValidDouble_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var result = Reaumur.TryFrom(temp.Reaumur, out _);
                Assert.Equal(Reaumur.Validation.Ok, result);
            }
        }

        [Fact]
        public void TryFrom_ValidCelsius_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var celsius = new Celsius(temp.Celsius);
                var result = Reaumur.TryFrom(celsius, out _);
                Assert.Equal(Reaumur.Validation.Ok, result);
            }
        }

        [Fact]
        public void TryFrom_ValidKelvin_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var kelvin = new Kelvin(temp.Kelvin);
                var result = Reaumur.TryFrom(kelvin, out _);
                Assert.Equal(Reaumur.Validation.Ok, result);
            }
        }

        [Fact]
        public void TryFrom_ValidFahrenheit_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var fahrenheit = new Fahrenheit(temp.Fahrenheit);
                var result = Reaumur.TryFrom(fahrenheit, out _);
                Assert.Equal(Reaumur.Validation.Ok, result);
            }
        }

        [Theory]
        [InlineData(_tooLowValue, typeof(InvalidReaumurException))]
        public void From_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            var result = Record.Exception(() => new Reaumur(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(_tooLowValue, Reaumur.Validation.TooLow)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Reaumur.Validation expected)
        {
            var result = Reaumur.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ImplicitOperator_ToCelsius_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Reaumur kelvin = new(temp.Reaumur);

                // act
                Celsius celsius = kelvin;

                // assert
                Assert.Equal(temp.Celsius, celsius, 0);
            }
        }

        [Fact]
        public void ImplicitOperator_ToFahrenheit_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Reaumur kelvin = new(temp.Reaumur);

                // act
                Fahrenheit fahrenheit = kelvin;

                // assert
                Assert.Equal(temp.Fahrenheit, fahrenheit, 0);
            }
        }

        [Fact]
        public void From_Celsius_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Celsius celsius = new(temp.Celsius);

                // act
                Reaumur kelvin = Reaumur.From(celsius);

                // assert
                Assert.Equal(temp.Reaumur, kelvin, 0);
            }
        }

        [Fact]
        public void From_Fahrenheit_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                Reaumur kelvin = Reaumur.From(fahrenheit);

                // assert
                Assert.Equal(temp.Reaumur, kelvin, 0);
            }
        }

        [Fact]
        public void ToCelsius_ValidInput_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Reaumur kelvin = new(temp.Reaumur);

                // act
                double celsius = kelvin.ToCelsius();

                // assert
                Assert.Equal(temp.Celsius, celsius, 0);
            }
        }

        [Fact]
        public void ToFahrenheit_ValidInput_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Reaumur kelvin = new(temp.Reaumur);

                // act
                double fahrenheit = kelvin.ToFahrenheit();

                // assert
                Assert.Equal(temp.Fahrenheit, fahrenheit, 0);
            }
        }

        #endregion
    }
}
