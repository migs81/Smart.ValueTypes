using Migs.ValueTypes.Types.Hashes;
using System;
using Xunit;

namespace Migs.ValueTypes.UnitTests.Hashes
{
    public class MD5Test
    {
        #region test data

        private static readonly string[] _validValues =
        [
            "d41d8cd98f00b204e9800998ecf8427e", "098f6bcd4621d373cade4e832627b4f6", "5eb63bbbe01eeed093cb22bb8f5acdc3",
            "e4d909c290d0fb1ca068ffaddf22cbd0", "a87ff679a2f3e71d9181a67b7542122c", "9e107d9d372bb6826bd81d3542a419d6",
            "45c48cce2e2d7fbdea1afc51c7c6ad26", "6512bd43d9caa6e02c990b0a82652dca", "c20ad4d76fe97759aa27a0c99bff6710",
            "c51ce410c124a10e0db5e4b97fc2af39", "aab3238922bcc25a6f606eb525ffdc56", "9bf31c7ff062936a96d3c8bd1f8f2ff3",
            "c74d97b01eae257e44aa9d5bade97baf", "70efdf2ec9b086079795c442636b55fb", "6f4922f45568161a8cdf4ad2299f6d23",
            "1f0e3dad99908345f7439f8ffabdffc4", "98f13708210194c475687be6106a3b84", "3c59dc048e8850243be8079a5c74d079",
            "b6d767d2f8ed5d21a44b0e5886680cb9", "37693cfc748049e45d87b8c7d8b9aacd", "1ff1de774005f8da13f42943881c655f",
            "8e296a067a37563370ded05f5a3bf3ec", "4e732ced3463d06de0ca9a15b6153677", "02e74f10e0327ad868d138f2b4fdd6f0",
            "33e75ff09dd601bbe69f351039152189", "6ea9ab1baa0efb9e19094440c317e21b", "34173cb38f07f89ddbebc2ac9128303f",
            "c16a5320fa475530d9583c34fd356ef5", "6364d3f0f495b6ab9dcf8d3b5c6e0b01", "182be0c5cdcd5072bb1864cdee4d3d6e",
        ];

        #endregion

        #region tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new MD5();
            Assert.Equal(MD5.Default, result);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = new MD5(value);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidMD5Exception))]
        [InlineData("0000000000000000000000000000000z", typeof(InvalidMD5Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new MD5(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = MD5.From(value);
                Assert.Equal(value, result);
            }
        }

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in _validValues)
            {
                var result = MD5.TryFrom(value, out _);
                Assert.Equal(MD5.Validation.Ok, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidMD5Exception))]
        [InlineData("0000000000000000000000000000000z", typeof(InvalidMD5Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new MD5(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(null, MD5.Validation.Null)]
        [InlineData("", MD5.Validation.WrongLength)]
        [InlineData("0000000000000000000000000000000z", MD5.Validation.IllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, MD5.Validation expected)
        {
            var result = MD5.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
