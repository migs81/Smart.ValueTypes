using Migs.ValueTypes.Types.Temperatures;
using Migs.ValueTypes.UnitTests.TestData;
using System;
using Xunit;

namespace Migs.ValueTypes.UnitTests.Temperatures
{
    public class FahrenheitTest
    {
        #region test data

        private static readonly Temp[] _validTemperatures = TemperatureGenerator.CreateTemperatures(1000);
        private const double _tooLowValue = Fahrenheit.MinValue - 1;

        #endregion

        #region constructor tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new Fahrenheit();
            Assert.Equal(Fahrenheit.Default, result);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(7)]
        [InlineData(99)]
        public void Constructor_InputInteger_ShouldReturnObject(int value)
        {
            var result = new Fahrenheit(value);
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void Constructor_InputDouble_ShouldReturnObject()
        {
            foreach (var temp in _validTemperatures)
            {
                var result = new Fahrenheit(temp.Fahrenheit);
                Assert.Equal(temp.Fahrenheit, result, 0);
            }
        }

        [Fact]
        public void Constructor_InputCelsius_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                Celsius celsius = new(fahrenheit);

                // assert
                Assert.Equal(temp.Celsius, celsius, 0);
            }
        }

        [Fact]
        public void Constructor_InputKelvin_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                Kelvin kelvin = new(fahrenheit);

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
            }
        }

        [Theory]
        [InlineData(_tooLowValue, typeof(InvalidFahrenheitException))]
        public void Constructor_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            var result = Record.Exception(() => new Fahrenheit(input));
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
            var result = Fahrenheit.From(value);
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void From_ValidDouble_ShouldReturnObject()
        {
            foreach (var temp in _validTemperatures)
            {
                var result = Fahrenheit.From(temp.Fahrenheit);
                Assert.Equal(temp.Fahrenheit, result, 0);
            }
        }

        [Fact]
        public void TryFrom_ValidDouble_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var result = Fahrenheit.TryFrom(temp.Fahrenheit, out _);
                Assert.Equal(Fahrenheit.Validation.Ok, result);
            }
        }

        [Fact]
        public void TryFrom_ValidCelsius_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var celsius = new Celsius(temp.Celsius);
                var result = Fahrenheit.TryFrom(celsius, out _);
                Assert.Equal(Fahrenheit.Validation.Ok, result);
            }
        }

        [Fact]
        public void TryFrom_ValidKelvin_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var kelvin = new Kelvin(temp.Kelvin);
                var result = Fahrenheit.TryFrom(kelvin, out _);
                Assert.Equal(Fahrenheit.Validation.Ok, result);
            }
        }

        [Fact]
        public void TryFrom_ValidReamur_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var reaumur = new Reaumur(temp.Reaumur);
                var result = Fahrenheit.TryFrom(reaumur, out _);
                Assert.Equal(Fahrenheit.Validation.Ok, result);
            }
        }

        [Theory]
        [InlineData(_tooLowValue, typeof(InvalidFahrenheitException))]
        public void From_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            var result = Record.Exception(() => new Fahrenheit(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(_tooLowValue, Fahrenheit.Validation.TooLow)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Fahrenheit.Validation expected)
        {
            var result = Fahrenheit.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ImplicitOperator_ToCelsius_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                Celsius celsius = fahrenheit;

                // assert
                Assert.Equal(temp.Celsius, celsius, 0);
            }
        }

        [Fact]
        public void ImplicitOperator_ToKelvin_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                Kelvin kelvin = fahrenheit;

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
            }
        }

        [Fact]
        public void From_Celsius_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                Celsius celsius = new(temp.Celsius);

                // conversion
                Fahrenheit fahrenheit = Fahrenheit.From(celsius);

                // expected values
                Assert.Equal(temp.Fahrenheit, fahrenheit, 0);
            }
        }

        [Fact]
        public void From_Kelvin_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Kelvin kelvin = new(temp.Kelvin);

                // act
                Fahrenheit fahrenheit = Fahrenheit.From(kelvin);

                // assert
                Assert.Equal(temp.Fahrenheit, fahrenheit, 0);
            }
        }

        [Fact]
        public void ToCelsius_ValidInput_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                double celsius = fahrenheit.ToCelsius();

                // assert
                Assert.Equal(temp.Celsius, celsius, 0);
            }
        }

        [Fact]
        public void ToKelvin_ValidInput_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                double kelvin = fahrenheit.ToKelvin();

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
            }
        }

        #endregion
    }
}
