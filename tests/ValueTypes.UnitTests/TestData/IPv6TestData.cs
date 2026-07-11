namespace Smart.ValueTypes.UnitTests.TestData;

public class IPv6TestData
{
    public const string TooShortValue = "0";
    public const string TooLongValue = "0000:0000:0000:0000:0000:0000:0000:0000:0000";
    public const string MultipleColons = "0:::0:0:0:0:0:0";
    public const string SegmentTooLong = "0:00000000:0:0:0:0:0:0.";
    public const string SegmentNotHex = "0:G:0:0:0:0:0:0";
    public const string EndsWithColon = "0:0:0:0:0:0:0:0:";
}