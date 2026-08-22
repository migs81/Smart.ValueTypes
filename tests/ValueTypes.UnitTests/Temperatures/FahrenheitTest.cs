using Smart.ValueTypes.Types.Temperatures;
using Smart.ValueTypes.UnitTests.TestData;
using System;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Temperatures
{
    public class FahrenheitTest
    {
        #region test data

        private static readonly Temp[] ValidTemperatures = TemperatureGenerator.CreateTemperatures(1000);
        private const double TooLowValue = Fahrenheit.MinValue - 1;

        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(Fahrenheit);
            
            // act
            var result = new Fahrenheit();
            
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
            var result = new Fahrenheit(value);
            
            // assert
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void Constructor_InputDouble_ShouldReturnObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // act
                var result = new Fahrenheit(temp.Fahrenheit);
                
                // assert
                Assert.Equal(temp.Fahrenheit, result, 0);
            }
        }

        [Fact]
        public void Constructor_InputCelsius_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
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
            foreach (var temp in ValidTemperatures)
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
        [InlineData(TooLowValue, typeof(InvalidFahrenheitException))]
        public void Constructor_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new Fahrenheit(input));
            
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
            var result = Fahrenheit.From(value);
            
            // assert
            Assert.Equal(value, result, 0);
        }

        [Fact]
        public void From_ValidDouble_ShouldReturnObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // act
                var result = Fahrenheit.From(temp.Fahrenheit);
                
                // assert
                Assert.Equal(temp.Fahrenheit, result, 0);
            }
        }

        [Theory]
        [InlineData(TooLowValue, typeof(InvalidFahrenheitException))]
        public void From_WrongInput_ShouldThrowException(double input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => Fahrenheit.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        
        [Fact]
        public void From_Celsius_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                var celsius = new Celsius(temp.Celsius);

                // act
                var fahrenheit = Fahrenheit.From(celsius);

                // assert
                Assert.Equal(temp.Fahrenheit, fahrenheit, 0);
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
                var fahrenheit = Fahrenheit.From(kelvin);

                // assert
                Assert.Equal(temp.Fahrenheit, fahrenheit, 0);
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
                var result = Fahrenheit.TryFrom(temp.Fahrenheit, out _);
                
                // assert
                Assert.Equal(Fahrenheit.Validation.Ok, result);
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
                var result = Fahrenheit.TryFrom(celsius, out _);
                
                // assert
                Assert.Equal(Fahrenheit.Validation.Ok, result);
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
                var result = Fahrenheit.TryFrom(kelvin, out _);
                
                // assert
                Assert.Equal(Fahrenheit.Validation.Ok, result);
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
                var result = Fahrenheit.TryFrom(reaumur, out _);
                
                // assert
                Assert.Equal(Fahrenheit.Validation.Ok, result);
            }
        }


        [Theory]
        [InlineData(TooLowValue, Fahrenheit.Validation.TooLow)]
        public void TryFrom_WrongInput_ShouldReturnError(double input, Fahrenheit.Validation expected)
        {
            // act
            var result = Fahrenheit.TryFrom(input, out _);
            
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
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                Kelvin kelvin = fahrenheit;

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
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
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                var celsius = fahrenheit.ToCelsius();

                // assert
                Assert.Equal(temp.Celsius, celsius, 0);
            }
        }

        [Fact]
        public void ToKelvin_ValidInput_ShouldReturnConvertedObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                Fahrenheit fahrenheit = new(temp.Fahrenheit);

                // act
                var kelvin = fahrenheit.ToKelvin();

                // assert
                Assert.Equal(temp.Kelvin, kelvin, 0);
            }
        }

        #endregion
    }
}
