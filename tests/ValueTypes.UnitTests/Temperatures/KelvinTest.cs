using Migs.ValueTypes.Types.Temperatures;
using Migs.ValueTypes.UnitTests.TestData;
using System;
using Xunit;

namespace Migs.ValueTypes.UnitTests.Temperatures
{
    public class KelvinTest
    {
        #region test data

        private static readonly Temp[] _validTemperatures = TemperatureGenerator.CreateTemperatures(1000);
        private const double _tooLowValue = Kelvin.MinValue - 1;

        #endregion

        #region constructor tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new Kelvin();
            Assert.Equal(Kelvin.Default, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(99)]
        public void Constructor_InputInteger_ShouldReturnObject(int value)
        {
            var result = new Kelvin(value);
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void Constructor_InputDouble_ShouldReturnObject()
        {
            foreach (var temp in _validTemperatures)
            {
                var result = new Kelvin(temp.Kelvin);
                Assert.Equal(temp.Kelvin, result, 0);
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
                Kelvin kelvin = new(celsius);

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
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
                Kelvin kelvin = new(fahrenheit);

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
            }
        }

        [Theory]
        [InlineData(_tooLowValue, typeof(InvalidKelvinException))]
        public void Constructor_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            var result = Record.Exception(() => new Kelvin(input));
            Assert.Equal(expectedException, result.GetType());
        }

        #endregion

        #region other tests

        [Theory]
        [InlineData(0)]
        [InlineData(7)]
        [InlineData(99)]
        public void From_ValidInteger_ShouldReturnObject(int value)
        {
            var result = Kelvin.From(value);
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void From_ValidDouble_ShouldReturnObject()
        {
            foreach (var temp in _validTemperatures)
            {
                var result = Kelvin.From(temp.Kelvin);
                Assert.Equal(temp.Kelvin, result, 0);
            }
        }

        [Fact]
        public void TryFrom_ValidDouble_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var result = Kelvin.TryFrom(temp.Kelvin, out _);
                Assert.Equal(Kelvin.Validation.OK, result);
            }
        }

        [Fact]
        public void TryFrom_ValidCelsius_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var celsius = new Celsius(temp.Celsius);
                var result = Kelvin.TryFrom(celsius, out _);
                Assert.Equal(Kelvin.Validation.OK, result);
            }
        }

        [Fact]
        public void TryFrom_ValidFahrenheit_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var fahrenheit = new Fahrenheit(temp.Fahrenheit);
                var result = Kelvin.TryFrom(fahrenheit, out _);
                Assert.Equal(Kelvin.Validation.OK, result);
            }
        }

        [Fact]
        public void TryFrom_ValidReamur_ShouldReturnOK()
        {
            foreach (var temp in _validTemperatures)
            {
                var reaumur = new Reaumur(temp.Reaumur);
                var result = Kelvin.TryFrom(reaumur, out _);
                Assert.Equal(Kelvin.Validation.OK, result);
            }
        }

        [Theory]
        [InlineData(_tooLowValue, typeof(InvalidKelvinException))]
        public void From_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            var result = Record.Exception(() => new Kelvin(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(_tooLowValue, Kelvin.Validation.TooLow)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Kelvin.Validation expected)
        {
            var result = Kelvin.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ImplicitOperator_ToCelsius_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Kelvin kelvin = new(temp.Kelvin);

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
                Kelvin kelvin = new(temp.Kelvin);

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
                Kelvin kelvin = Kelvin.From(celsius);

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
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
                Kelvin kelvin = Kelvin.From(fahrenheit);

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
            }
        }

        [Fact]
        public void ToCelsius_ValidInput_ShouldReturnConvertedObject()
        {
            foreach (var temp in _validTemperatures)
            {
                // arrange
                Kelvin kelvin = new(temp.Kelvin);

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
                Kelvin kelvin = new(temp.Kelvin);

                // act
                double fahrenheit = kelvin.ToFahrenheit();

                // assert
                Assert.Equal(temp.Fahrenheit, fahrenheit, 0);
            }
        }

        #endregion
    }
}
