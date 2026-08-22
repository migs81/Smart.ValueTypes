using System;
using Smart.ValueTypes.Types.Security.Hashes;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Security.Hashes
{
    public class Md5Test
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(MD5);
            
            // act
            var result = new MD5();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("d41d8cd98f00b204e9800998ecf8427e")]
        [InlineData("a87ff679a2f3e71d9181a67b7542122c")]
        [InlineData("aab3238922bcc25a6f606eb525ffdc56")]
        [InlineData("1ff1de774005f8da13f42943881c655f")]
        [InlineData("1f0e3dad99908345f7439f8ffabdffc4")]
        [InlineData("6ea9ab1baa0efb9e19094440c317e21b")]
        [InlineData("c16a5320fa475530d9583c34fd356ef5")]
        [InlineData("34173cb38f07f89ddbebc2ac9128303f")]
        [InlineData("02e74f10e0327ad868d138f2b4fdd6f0")]
        [InlineData("9e107d9d372bb6826bd81d3542a419d6")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new MD5(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidMd5Exception))]
        [InlineData("0000000000000000000000000000000z", typeof(InvalidMd5Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new MD5(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData("d41d8cd98f00b204e9800998ecf8427e")]
        [InlineData("a87ff679a2f3e71d9181a67b7542122c")]
        [InlineData("aab3238922bcc25a6f606eb525ffdc56")]
        [InlineData("1ff1de774005f8da13f42943881c655f")]
        [InlineData("1f0e3dad99908345f7439f8ffabdffc4")]
        [InlineData("6ea9ab1baa0efb9e19094440c317e21b")]
        [InlineData("c16a5320fa475530d9583c34fd356ef5")]
        [InlineData("34173cb38f07f89ddbebc2ac9128303f")]
        [InlineData("02e74f10e0327ad868d138f2b4fdd6f0")]
        [InlineData("9e107d9d372bb6826bd81d3542a419d6")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = MD5.From(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidMd5Exception))]
        [InlineData("0000000000000000000000000000000z", typeof(InvalidMd5Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => MD5.From(input));
            
            // act
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom

        [Theory]
        [InlineData("d41d8cd98f00b204e9800998ecf8427e")]
        [InlineData("a87ff679a2f3e71d9181a67b7542122c")]
        [InlineData("aab3238922bcc25a6f606eb525ffdc56")]
        [InlineData("1ff1de774005f8da13f42943881c655f")]
        [InlineData("1f0e3dad99908345f7439f8ffabdffc4")]
        [InlineData("6ea9ab1baa0efb9e19094440c317e21b")]
        [InlineData("c16a5320fa475530d9583c34fd356ef5")]
        [InlineData("34173cb38f07f89ddbebc2ac9128303f")]
        [InlineData("02e74f10e0327ad868d138f2b4fdd6f0")]
        [InlineData("9e107d9d372bb6826bd81d3542a419d6")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = MD5.TryFrom(input, out _);
                
            // assert
            Assert.Equal(MD5.Validation.Ok, result);
        }

        [Theory]
        [InlineData(null, MD5.Validation.Null)]
        [InlineData("", MD5.Validation.WrongLength)]
        [InlineData("0000000000000000000000000000000z", MD5.Validation.IllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, MD5.Validation expected)
        {
            // act
            var result = MD5.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
    }
}
