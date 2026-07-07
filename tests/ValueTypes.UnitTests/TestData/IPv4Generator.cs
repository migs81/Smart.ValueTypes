namespace Smart.ValueTypes.UnitTests.TestData
{
    internal class IPv4Generator
    {
        public static string[] CreateAddresses(uint amount)
        {
            var values = new string[amount];
            for (int i = 0; i < values.Length; i++)
                values[i] = CreateAddress();

            return values;
        }

        public static string CreateAddress()
        {
            int[] array =
            [
                NumberGenerator.NextInt(0, 255),
                NumberGenerator.NextInt(0, 255),
                NumberGenerator.NextInt(0, 255),
                NumberGenerator.NextInt(0, 255),
            ];

            return string.Join(".", array);
        }
    }
}
