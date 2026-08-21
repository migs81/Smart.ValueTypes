using System;

namespace Smart.ValueTypes.UnitTests.TestData
{
    internal static class SvnrGenerator
    {
        public static string[] GenerateTestSVNRs(uint amount, int minValue, int maxValue)
        {
            var array = new string[amount];
            for (var i = 0; i < amount; i++)
            {
                var seed = NumberGenerator.NextInt(minValue, maxValue);
                array[i] = GenerateTestSVNR(seed);
            }

            return array;
        }

        public static string GenerateTestSVNR(int seed)
        {
            var random = new Random(seed);

            while (true)
            {
                // number sequence
                var firstDigit = random.Next(1, 10);
                var secondDigit = random.Next(0, 10);
                var thirdDigit = random.Next(0, 10);

                var part1 = $"{firstDigit}{secondDigit}{thirdDigit}";

                // date
                var day = random.Next(1, 29);
                var month = random.Next(1, 13);
                var year = random.Next(0, 100);

                var birthDate = $"{day:00}{month:00}{year:00}";
                var calculationBase = part1 + birthDate; // 9 digits

                int[] weights = { 3, 7, 9, 5, 8, 4, 2, 1, 6 };
                var sum = 0;

                for (var i = 0; i < 9; i++)
                {
                    sum += (calculationBase[i] - '0') * weights[i];
                }

                var checkDigit = sum % 11;

                // regenerate if check digit is 10 (invalid)
                if (checkDigit == 10)
                    continue;

                return part1 + checkDigit + birthDate;
            }
        }
    }
}
