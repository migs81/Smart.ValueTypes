using Smart.ValueTypes.Types.NationalInsuranceNumbers;
using Smart.ValueTypes.UnitTests.TestData;
using System;
using Xunit;

namespace Smart.ValueTypes.UnitTests.NationalInsuranceNumbers
{
    public class SvnrTest
    {
        #region test data

        private static readonly string[] ValidValues = SvnrGenerator.GenerateTestSVNRs(1000, int.MinValue, int.MaxValue);

        private const string WrongValue = "1234010190";

        public static string CorruptChecksum(string svnr) => svnr[..9] + ((svnr[9] - '0' + 1) % 10);

        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(SVNR);
            
            // act
            var result = new SVNR();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldReturnObject()
        {
            foreach (var value in ValidValues)
            {
                // act
                var result = new SVNR(value);
                
                // assert
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(WrongValue, typeof(InvalidSvnrException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new SVNR(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Fact]
        public void From_ValidInput_ShouldReturnObject()
        {
            foreach (var value in ValidValues)
            {
                // act
                var result = SVNR.From(value);
                
                // assert
                Assert.Equal(value, result);
            }
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("1234", typeof(InvalidSvnrException))]
        [InlineData(WrongValue, typeof(InvalidSvnrException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => SVNR.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region TryFrom
        
        [Fact]
        public void TryFrom_ValidInput_ShouldReturnOK()
        {
            foreach (var value in ValidValues)
            {
                // act
                var result = SVNR.TryFrom(value, out _);
                
                // assert
                Assert.Equal(SVNR.Validation.Ok, result);
            }
        }
        
        [Theory]
        [InlineData(null, SVNR.Validation.Null)]
        [InlineData("", SVNR.Validation.Empty)]
        [InlineData("1234", SVNR.Validation.WrongLength)]
        [InlineData(WrongValue, SVNR.Validation.WrongChecksum)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, SVNR.Validation expected)
        {
            // act
            var result = SVNR.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
