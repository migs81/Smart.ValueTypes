namespace Migs.ValueTypes.Types.GeoCoordinate
{
    /// <summary>
    /// Aggregate for Longitude and Latitude.
    /// </summary>
    public readonly record struct GeoCoordinate
    {
        public Longitude Longitude { get; }
        public Latitude Latitude { get; }
    }
}
