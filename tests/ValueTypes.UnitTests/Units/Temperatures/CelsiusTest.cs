using System;
using Smart.ValueTypes.Types.Units.Temperatures;
using Smart.ValueTypes.UnitTests.TestData;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Units.Temperatures
{
    public class CelsiusTest
    {
        #region test data

        private static readonly Temp[] ValidTemperatures = TemperatureGenerator.CreateTemperatures(1000);
        private const double TooLowValue = Celsius.MinValue - 1;

        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(Celsius);
            
            // act
            var result = new Celsius();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData(-10)]
        [InlineData(7)]
        [InlineData(99)]
        public void Constructor_InputInteger_ShouldReturnObject(int value)
        {
            // act
            var result = new Celsius(value);
            
            // assert
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void Constructor_InputDouble_ShouldReturnObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // act
                var result = new Celsius(temp.Celsius);
                
                // assert
                Assert.Equal(temp.Celsius, result, 0);
            }
        }

        [Fact]
        public void Constructor_InputKelvin_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
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
            foreach (var temp in ValidTemperatures)
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
        [InlineData(TooLowValue, typeof(InvalidCelsiusException))]
        public void Constructor_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new Celsius(input));
            
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
            var result = Celsius.From(value);
            
            // assert
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void From_ValidDouble_ShouldReturnObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // act
                var result = Celsius.From(temp.Celsius);
                
                // assert
                Assert.Equal(temp.Celsius, result, 0);
            }
        }

        
        [Fact]
        public void From_Kelvin_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                Kelvin kelvin = new(temp.Kelvin);

                // act
                var celsius = Celsius.From(kelvin);

                // assert
                Assert.Equal(temp.Celsius, celsius, 0);
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
                var celsius = Celsius.From(fahrenheit);

                // assert
                Assert.Equal(temp.Celsius, celsius, 0);
            }
        }
        
        [Theory]
        [InlineData(TooLowValue, typeof(InvalidCelsiusException))]
        public void From_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => Celsius.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom

        [Fact]
        public void TryFrom_ValidDouble_ShouldReturnOK()
        {
            foreach (var temp in ValidTemperatures)
            {
                // act
                var result = Celsius.TryFrom(temp.Celsius, out _);
                
                // assert
                Assert.Equal(Celsius.Validation.Ok, result);
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
                var result = Celsius.TryFrom(kelvin, out _);
                
                // assert
                Assert.Equal(Celsius.Validation.Ok, result);
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
                var result = Celsius.TryFrom(fahrenheit, out _);
                
                // assert
                Assert.Equal(Celsius.Validation.Ok, result);
            }
        }

        [Fact]
        public void TryFrom_ValidReamur_ShouldReturnOK()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                var reaumur = new Reaumur(temp.Reaumur);
                
                // act
                var result = Celsius.TryFrom(reaumur, out _);
                
                // assert
                Assert.Equal(Celsius.Validation.Ok, result);
            }
        }

        [Theory]
        [InlineData(TooLowValue, Celsius.Validation.TooLow)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Celsius.Validation expected)
        {
            // act
            var result = Celsius.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
        
        #region Operator

        [Fact]
        public void ImplicitOperator_ToKelvin_ShouldReturnConvertedObject()
        {
            foreach(var temp in ValidTemperatures)
            {
                // arrange
                var celsius = Celsius.From(temp.Celsius);

                // act
                Kelvin kelvin = celsius;

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
            }
        }

        [Fact]
        public void ImplicitOperator_ToFahrenheit_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                var celsius = Celsius.From(temp.Celsius);

                // act
                Fahrenheit fahrenheit = celsius;

                // assert
                Assert.Equal(temp.Fahrenheit, fahrenheit, 0);
            }
        }
        
        #endregion
        
        #region To

        [Fact]
        public void ToKelvin_ValidInput_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                var celsius = Celsius.From(temp.Celsius);

                // act
                var kelvin = celsius.ToKelvin();

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
            }
        }

        [Fact]
        public void ToFahrenheit_ValidInput_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                var celsius = Celsius.From(temp.Celsius);

                // act
                var fahrenheit = celsius.ToFahrenheit();

                // assert
                Assert.Equal(temp.Fahrenheit, fahrenheit, 0);
            }
        }

        #endregion
    }
}
