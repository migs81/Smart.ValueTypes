namespace Smart.ValueTypes.UnitTests.TestData;

public static class IPv4TestData
{
    public const string TooShortValue = "1.1.1";
    public const string TooLongValue = "255.255.255.255.0";
    public const string StartsWithDotValue = ".127.0.0.1";
    public const string EndsWithDotValue = "127.0.0.1.";
    public const string ContainsIllegalCharacterValue = "127.a.0.1";
    public const string InvalidSegmentNumberValue = "1.0.0.256";
    public const string InvalidSegmentCountValue = "0.0.0.0.0";
}