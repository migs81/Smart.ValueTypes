using Smart.ValueTypes.Types.Coordinates;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Coordinates
{
    public class GeoCoordinateTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // act
            var result = new GeoCoordinate();
            
            // assert
            Assert.Equal(default(Latitude), result.Latitude, 0);
            Assert.Equal(default(Longitude), result.Longitude, 0);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            // arrange
            var longitude = new Longitude(10);
            var latitude = new Latitude(10);
            
            // act
            var result = new GeoCoordinate(longitude, latitude);
            
            // assert
            Assert.Equal(latitude, result.Latitude);
            Assert.Equal(longitude, result.Longitude);
        }

        #endregion
    }
}
