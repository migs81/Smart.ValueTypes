using Smart.ValueTypes.Types.NationalInsuranceNumbers;
using Smart.ValueTypes.UnitTests.TestData;
using System;
using Xunit;

namespace Smart.ValueTypes.UnitTests.NationalInsuranceNumbers
{
    public class SVNRTest
    {
        #region test data

        private static readonly string[] _validValues = SVNRGenerator.GenerateTestSVNRs(1000, int.MinValue, int.MaxValue);

        private const string _wrongValue = "1234010190";

        public static string CorruptChecksum(string svnr) => svnr[..9] + ((svnr[9] - '0' + 1) % 10);

        #endregion

        #region tests

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            var result = new SVNR();
            Assert.Equal(SVNR.Empty, result);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = new SVNR(value);
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(_wrongValue, typeof(InvalidSvnrException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new SVNR(input));
            Assert.Equal(expectedException, result.GetType());
        }


        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in _validValues)
            {
                var result = SVNR.From(value);
                Assert.Equal(value, result);
            }
        }

        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in _validValues)
            {
                var result = SVNR.TryFrom(value, out _);
                Assert.Equal(SVNR.Validation.Ok, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1234", typeof(InvalidSvnrException))]
        [InlineData(_wrongValue, typeof(InvalidSvnrException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            var result = Record.Exception(() => new SVNR(input));
            Assert.Equal(expectedException, result.GetType());
        }

        [Theory]
        [InlineData(null, SVNR.Validation.Null)]
        [InlineData("", SVNR.Validation.Empty)]
        [InlineData("1234", SVNR.Validation.WrongLength)]
        [InlineData(_wrongValue, SVNR.Validation.WrongChecksum)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, SVNR.Validation expected)
        {
            var result = SVNR.TryFrom(input, out _);
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
