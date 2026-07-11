using Smart.ValueTypes.Types.Temperatures;
using Smart.ValueTypes.UnitTests.TestData;
using System;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Temperatures
{
    public class ReaumurTest
    {
        #region test data

        private static readonly Temp[] ValidTemperatures = TemperatureGenerator.CreateTemperatures(1000);
        private const double TooLowValue = Reaumur.MinValue - 1;

        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // act
            var result = new Reaumur();
            
            // assert
            Assert.Equal(Reaumur.Zero, result);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(7)]
        [InlineData(99)]
        public void Constructor_InputInteger_ShouldReturnObject(int value)
        {
            // act
            var result = new Reaumur(value);
            
            // assert
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void Constructor_InputDouble_ShouldReturnObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // act
                var result = new Reaumur(temp.Reaumur);
                
                // assert
                Assert.Equal(temp.Reaumur, result, 0);
            }
        }

        [Fact]
        public void Constructor_InputCelsius_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
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
            foreach (var temp in ValidTemperatures)
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
        [InlineData(TooLowValue, typeof(InvalidReaumurException))]
        public void Constructor_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new Reaumur(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion

        #region From

        [Theory]
        [InlineData(-10)]
        [InlineData(7)]
        [InlineData(99)]
        public void From_ValidInteger_ShouldReturnObject(int value)
        {
            // act
            var result = Reaumur.From(value);
            
            // assert
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void From_ValidDouble_ShouldReturnObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // act
                var result = Reaumur.From(temp.Reaumur);
                
                // assert
                Assert.Equal(temp.Reaumur, result, 0);
            }
        }
        
        [Fact]
        public void From_Celsius_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                Celsius celsius = new(temp.Celsius);

                // act
                var kelvin = Reaumur.From(celsius);

                // assert
                Assert.Equal(temp.Reaumur, kelvin, 0);
            }
        }

        [Fact]
        public void From_Fahrenheit_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                var kelvin = Reaumur.From(fahrenheit);

                // assert
                Assert.Equal(temp.Reaumur, kelvin, 0);
            }
        }
        
        #endregion
        
        #region TryFrom

        [Fact]
        public void TryFrom_ValidDouble_ShouldReturnOK()
        {
            foreach (var temp in ValidTemperatures)
            {
                // act
                var result = Reaumur.TryFrom(temp.Reaumur, out _);
                
                // assert
                Assert.Equal(Reaumur.Validation.Ok, result);
            }
        }

        [Fact]
        public void TryFrom_ValidCelsius_ShouldReturnOK()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                var celsius = new Celsius(temp.Celsius);
                
                // act
                var result = Reaumur.TryFrom(celsius, out _);
                
                // assert
                Assert.Equal(Reaumur.Validation.Ok, result);
            }
        }

        [Fact]
        public void TryFrom_ValidKelvin_ShouldReturnOK()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                var kelvin = new Kelvin(temp.Kelvin);
                
                // act
                var result = Reaumur.TryFrom(kelvin, out _);
                
                // assert
                Assert.Equal(Reaumur.Validation.Ok, result);
            }
        }

        [Fact]
        public void TryFrom_ValidFahrenheit_ShouldReturnOK()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                var fahrenheit = new Fahrenheit(temp.Fahrenheit);
                
                // act
                var result = Reaumur.TryFrom(fahrenheit, out _);
                
                // assert
                Assert.Equal(Reaumur.Validation.Ok, result);
            }
        }

        [Theory]
        [InlineData(TooLowValue, typeof(InvalidReaumurException))]
        public void From_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new Reaumur(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        [Theory]
        [InlineData(TooLowValue, Reaumur.Validation.TooLow)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Reaumur.Validation expected)
        {
            // act
            var result = Reaumur.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
        
        #region Operator

        [Fact]
        public void ImplicitOperator_ToCelsius_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
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
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                Reaumur kelvin = new(temp.Reaumur);

                // act
                Fahrenheit fahrenheit = kelvin;

                // assert
                Assert.Equal(temp.Fahrenheit, fahrenheit, 0);
            }
        }

        #endregion
        
        #region To

        [Fact]
        public void ToCelsius_ValidInput_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                Reaumur kelvin = new(temp.Reaumur);

                // act
                var celsius = kelvin.ToCelsius();

                // assert
                Assert.Equal(temp.Celsius, celsius, 0);
            }
        }

        [Fact]
        public void ToFahrenheit_ValidInput_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                Reaumur kelvin = new(temp.Reaumur);

                // act
                var fahrenheit = kelvin.ToFahrenheit();

                // assert
                Assert.Equal(temp.Fahrenheit, fahrenheit, 0);
            }
        }

        #endregion
    }
}
