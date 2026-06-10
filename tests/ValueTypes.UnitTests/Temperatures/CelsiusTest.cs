using Migs.ValueTypes.Types.Temperatures;
using Migs.ValueTypes.UnitTests.TestData;
using System;
using System.Diagnostics;
using Xunit;

namespace Migs.ValueTypes.UnitTests.Temperatures
{
    public class CelsiusTest
    {
        #region test data

        private static readonly Temp[] _validTemperatures = TemperatureGenerator.CreateTemperatures(1000);
        private const double _tooLowValue = Celsius.MinValue - 1;

        #endregion

        #region constructor tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new Celsius();
            Assert.Equal(Celsius.Default, result);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(7)]
        [InlineData(99)]
        public void Constructor_InputInteger_ShouldReturnObject(int value)
        {
            var result = new Celsius(value);
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void Constructor_InputDouble_ShouldReturnObject()
        {
            foreach (var temp in _validTemperatures)
            {
                var result = new Celsius(temp.Celsius);
                Assert.Equal(temp.Celsius, result, 0);
            }
        }

        [Fact]
        public void Constructor_InputKelvin_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Kelvin kelvin = new(temp.Kelvin);

                // act
                Celsius celsius = new(kelvin);

                // assert
                Assert.Equal(temp.Celsius, celsius, 0);
            }
        }

        [Fact]
        public void Constructor_InputFahrenheit_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                Celsius celsius = new(fahrenheit);

                // assert
                Assert.Equal(temp.Celsius, celsius, 1);
            }
        }

        [Theory]
        [InlineData(_tooLowValue, typeof(InvalidCelsiusException))]
        public void Constructor_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            var result = Record.Exception(() => new Celsius(input));
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
            var result = Celsius.From(value);
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void From_ValidDouble_ShouldReturnObject()
        {
            foreach (var temp in _validTemperatures)
            {
                var result = Celsius.From(temp.Celsius);
                Assert.Equal(temp.Celsius, result, 0);
            }
        }

        [Fact]
        public void TryFrom_ValidDouble_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var result = Celsius.TryFrom(temp.Celsius, out _);
                Assert.Equal(Celsius.Validation.OK, result);
            }
        }

        [Fact]
        public void TryFrom_ValidKelvin_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var kelvin = new Kelvin(temp.Kelvin);
                var result = Celsius.TryFrom(kelvin, out _);
                Assert.Equal(Celsius.Validation.OK, result);
            }
        }

        [Fact]
        public void TryFrom_ValidFahrenheit_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var fahrenheit = new Fahrenheit(temp.Fahrenheit);
                var result = Celsius.TryFrom(fahrenheit, out _);
                Assert.Equal(Celsius.Validation.OK, result);
            }
        }

        [Fact]
        public void TryFrom_ValidReamur_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var reaumur = new Reaumur(temp.Reaumur);
                var result = Celsius.TryFrom(reaumur, out _);
                Assert.Equal(Celsius.Validation.OK, result);
            }
        }

        [Theory]
        [InlineData(_tooLowValue, typeof(InvalidCelsiusException))]
        public void From_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            var result = Record.Exception(() => Celsius.From(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(_tooLowValue, Celsius.Validation.TooLow)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Celsius.Validation expected)
        {
            var result = Celsius.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ImplicitOperator_ToKelvin_ShouldReturnConvertedObject()
        {
            foreach(var temp in _validTemperatures)
            {
                // arrange
                Celsius celsius = Celsius.From(temp.Celsius);

                // act
                Kelvin kelvin = celsius;

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
            }
        }

        [Fact]
        public void ImplicitOperator_ToFahrenheit_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Celsius celsius = Celsius.From(temp.Celsius);

                // act
                Fahrenheit fahrenheit = celsius;

                // assert
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
                Celsius celsius = Celsius.From(kelvin);

                // assert
                Assert.Equal(temp.Celsius, celsius, 0);
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
                Celsius celsius = Celsius.From(fahrenheit);

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
                Celsius celsius = Celsius.From(temp.Celsius);

                // act
                double kelvin = celsius.ToKelvin();

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
            }
        }

        [Fact]
        public void ToFahrenheit_ValidInput_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Celsius celsius = Celsius.From(temp.Celsius);

                // act
                double fahrenheit = celsius.ToFahrenheit();

                // assert
                Assert.Equal(temp.Fahrenheit, fahrenheit, 0);
            }
        }

        #endregion
    }
}
