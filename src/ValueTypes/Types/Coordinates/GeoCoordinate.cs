namespace Migs.ValueTypes.Types.Coordinates
{
    /// <summary>
    /// Aggregate for Longitude and Latitude.
    /// </summary>
    public readonly record struct GeoCoordinate(Longitude Longitude, Latitude Latitude);
}
