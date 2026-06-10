namespace Migs.ValueTypes
{
    public readonly record struct RGB
    {
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }

        public RGB(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }

    public readonly record struct RGBA
    {
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public byte Alpha { get; }

        public RGBA(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
    }
}
