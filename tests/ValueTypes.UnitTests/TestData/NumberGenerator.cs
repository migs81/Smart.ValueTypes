using System;

namespace Migs.ValueTypes.UnitTests.TestData
{
    internal class NumberGenerator
    {
        #region integer

        public static int[] NextInt(uint amount, int minValue, int maxValue)
        {
            var array = new int[amount];
            for (int i = 0; i < amount; i++)
                array[i] = NextInt(minValue, maxValue);

            return array;
        }

        public static int NextInt() => Random.Shared.Next();

        public static int NextInt(int maxValue)
        {
            if (maxValue <= 0)
                throw new ArgumentException("The max value must be higher then zero!");
            Random.Shared.Next();
            return Random.Shared.Next(maxValue);
        }

        public static int NextInt(int minValue, int maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentException("The min value must be lower then max!");

            return Random.Shared.Next(minValue, maxValue);
        }

        #endregion

        #region double

        public static double[] NextDouble(int amount, double minValue, double maxValue)
        {
            var array = new double[amount];
            for (int i = 0; i < amount; i++)
                array[i] = NextDouble(minValue, maxValue);

            return array;
        }

        public static double NextDouble() => Random.Shared.NextDouble();

        public static double NextDouble(double minValue, double maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentException("The min value must be lower then max!");

            return Random.Shared.NextDouble() * (maxValue - minValue) + minValue;
        }

        #endregion
    }
}
