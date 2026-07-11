using System;
using Smart.ValueTypes.Types.Temperatures;
using Smart.ValueTypes.UnitTests.TestData;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Temperatures
{
    public class TemperatureTest
    {
        #region test data

        private static readonly Temp[] ValidTemperatures = TemperatureGenerator.CreateTemperatures(1000);

        #endregion
        
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            const double celsius = 0d;
            var fahrenheit = Converter.Celsius.ToFahrenheit(celsius);
            var kelvin = Converter.Celsius.ToKelvin(celsius);
            var reaumur = Converter.Celsius.ToReaumur(celsius);
            
            // act
            var result = new Temperature();
            
            // assert
            Assert.Equal(celsius, result.Celsius, 0);
            Assert.Equal(fahrenheit, result.Fahrenheit, 0);
            Assert.Equal(kelvin, result.Kelvin, 0);
            Assert.Equal(reaumur, result.Reaumur, 0);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var temp in ValidTemperatures)
            {
                // arrange
                var celsius = new Celsius(temp.Celsius);
                
                // act
                var result = new Temperature(celsius);
            
                // assert
                Assert.Equal(temp.Celsius, result.Celsius, 0);
                Assert.Equal(temp.Fahrenheit, result.Fahrenheit, 0);
                Assert.Equal(temp.Kelvin, result.Kelvin, 0);
                Assert.Equal(temp.Reaumur, result.Reaumur, 0);
            }
        }

        #endregion
    }
}
