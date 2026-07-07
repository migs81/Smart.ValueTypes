namespace Smart.ValueTypes.Unfinished.ColorModels
{
    /*
     * Color models
     */
    
    public readonly record struct RGB(byte Red, byte Green, byte Blue);

    public readonly record struct RGBA(byte Red, byte Green, byte Blue, byte Alpha);
    
    // /// <summary>
    // /// Hue: 0 – 360°
    // /// Saturation: 0 – 1
    // /// Value/Brightness: 0 – 1
    // /// </summary>
    // /// <param name="Hue"></param>
    // /// <param name="Saturation"></param>
    // /// <param name="Value"></param>
    // public readonly record struct HSV(float Hue, float Saturation, float Value);
    //
    // /// <summary>
    // /// C, M, Y, K: 0 – 1
    // /// </summary>
    // /// <param name="cyan"></param>
    // /// <param name="magenta"></param>
    // /// <param name="yellow"></param>
    // /// <param name="key"></param>
    // public readonly record struct CMYK(float Cyan, float Magenta, float Yellow, float Key);
    //
    // /// <summary>
    // /// L*: 0 – 100
    // /// a*: ca. -128 – +127
    // /// b*: ca. -128 – +127
    // /// </summary>
    // /// <param name="l"></param>
    // /// <param name="a"></param>
    // /// <param name="b"></param>
    // public readonly record struct CIELAB(float Lightness, float GreenToRed, float BlueToYellow);
    //
    // /// <summary>
    // /// Y: 0 – 1 (oder 16–235 in TV-range)
    // /// Cb/Cr: -0.5 – +0.5 (oder 16–240 skaliert)
    // /// </summary>
    // /// <param name="y"></param>
    // /// <param name="cb"></param>
    // /// <param name="cr"></param>
    // public readonly record struct YCbCr(float Luma, float BlueDifference, float RedDifference);
    //
    // /// <summary>
    // /// 0 - 255
    // /// </summary>
    // /// <param name="Luma"></param>
    // public readonly record struct Gray(byte Luma);
}
