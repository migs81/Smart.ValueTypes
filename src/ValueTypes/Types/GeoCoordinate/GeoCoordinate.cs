namespace Migs.ValueTypes.Types.GeoCoordinate
{
    /// <summary>
    /// Aggregate for Longitude and Latitude.
    /// </summary>
    public readonly record struct GeoCoordinate(Longitude Longitude, Latitude Latitude);
}
