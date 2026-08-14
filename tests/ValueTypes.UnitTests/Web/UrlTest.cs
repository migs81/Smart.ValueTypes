using System;
using Smart.ValueTypes.Types.Web;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Web
{
    public class UrlTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(Url);
            
            // act
            var result = new Url();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        // Host - basic
        [InlineData("https://a.com")]
        [InlineData("https://a")]
        [InlineData("https://1")]
        [InlineData("https://123.com")]
        [InlineData("https://a1.com")]
        [InlineData("https://1a.com")]
        [InlineData("https://example123.com")]
        [InlineData("http://localhost")]
        [InlineData("https://192.168.1.1")]

        // Host - hyphens
        [InlineData("https://a-b.com")]
        [InlineData("https://a--b.com")]
        [InlineData("https://a1-b2.com")]
        [InlineData("https://my-domain.example.com")]
        [InlineData("https://x-domain.example.com")]

        // Host - subdomains
        [InlineData("https://www.example.com")]
        [InlineData("https://sub.example.com")]

        // Host - maximum label length
        [InlineData("https://aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.com")]
        [InlineData("https://aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb.ccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc.ddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd")]
        
        // Scheme
        [InlineData("http://example.com")]
        [InlineData("https://example.com")]

        // Port
        [InlineData("https://example.com:1")]
        [InlineData("https://example.com:80")]
        [InlineData("https://example.com:443")]
        [InlineData("https://example.com:00080")]
        [InlineData("https://example.com:65535")]
        [InlineData("https://example.com:65535/path")]
        [InlineData("https://example.com:1/path?query=1#fragment")]

        // Path
        [InlineData("https://example.com/")]
        [InlineData("https://example.com/path")]
        [InlineData("https://example.com/path/to/resource")]
        [InlineData("https://example.com//path")]
        [InlineData("https://example.com/path/")]
        [InlineData("https://example.com/path%2Ftest")]
        [InlineData("https://example.com/%20")]

        // Query
        [InlineData("https://example.com?")]
        [InlineData("https://example.com?query=test")]
        [InlineData("https://example.com?query")]
        [InlineData("https://example.com?a=1&b=2")]
        [InlineData("https://example.com?name=Max&age=30")]
        [InlineData("https://example.com?name=Max%20Mustermann")]
        [InlineData("https://example.com/search?q=hello+world")]

        // Fragment
        [InlineData("https://example.com#")]
        [InlineData("https://example.com#fragment")]
        [InlineData("https://example.com#hello%20world")]
        [InlineData("https://example.com/#top")]

        // Path + Query + Fragment
        [InlineData("https://example.com/path?foo=bar#section")]
        [InlineData("https://example.com/path/to/file.html?x=1#section")]
        [InlineData("https://example.com?query#fragment")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new Url(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        // Argument validation
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]

        // Schema
        [InlineData("http://", typeof(InvalidUrlException))]
        [InlineData("https://", typeof(InvalidUrlException))]
        [InlineData("example.com", typeof(InvalidUrlException))]
        [InlineData("www.example.com", typeof(InvalidUrlException))]
        [InlineData("://example.com", typeof(InvalidUrlException))]
        [InlineData("http:/example.com", typeof(InvalidUrlException))]
        [InlineData("https:/example.com", typeof(InvalidUrlException))]
        [InlineData("ftp://example.com", typeof(InvalidUrlException))]
        [InlineData("mailto:user@example.com", typeof(InvalidUrlException))]

        // Host
        [InlineData("http://exam ple.com", typeof(InvalidUrlException))]
        [InlineData("https:// example.com", typeof(InvalidUrlException))]
        [InlineData("https://example .com", typeof(InvalidUrlException))]
        [InlineData("https://-example", typeof(InvalidUrlException))]
        [InlineData("https://example-", typeof(InvalidUrlException))]
        [InlineData("https://example.", typeof(InvalidUrlException))]
        [InlineData("https://example..com", typeof(InvalidUrlException))]
        [InlineData("https://-example.com", typeof(InvalidUrlException))]
        [InlineData("https://example-.com", typeof(InvalidUrlException))]
        [InlineData("https://.example.com", typeof(InvalidUrlException))]
        
        // Port
        [InlineData("https://example.com:", typeof(InvalidUrlException))]
        [InlineData("https://example.com:abc", typeof(InvalidUrlException))]
        [InlineData("https://example.com:0", typeof(InvalidUrlException))]
        [InlineData("https://example.com:65536", typeof(InvalidUrlException))]
        [InlineData("https://example.com:80abc", typeof(InvalidUrlException))]
        [InlineData("https://example.com:65536/path", typeof(InvalidUrlException))]
        [InlineData("https://example.com:65536/path?query=1#fragment", typeof(InvalidUrlException))]

        // Path
        [InlineData("https://example.com/path with spaces", typeof(InvalidUrlException))]
        [InlineData("https://example.com/%", typeof(InvalidUrlException))]
        [InlineData("https://example.com/%2", typeof(InvalidUrlException))]
        [InlineData("https://example.com/%GG", typeof(InvalidUrlException))]
        
        // Query
        [InlineData("https://example.com?x=hello world", typeof(InvalidUrlException))]
        [InlineData("https://example.com?x=%", typeof(InvalidUrlException))]
        [InlineData("https://example.com?x=%GG", typeof(InvalidUrlException))]
        
        // Fragment
        [InlineData("https://example.com#hello world", typeof(InvalidUrlException))]
        [InlineData("https://example.com#foo#bar", typeof(InvalidUrlException))]
        [InlineData("https://example.com#foo%", typeof(InvalidUrlException))]
        [InlineData("https://example.com#foo%GG", typeof(InvalidUrlException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new Url(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        // #region From
        //
        // [Theory]
        // [InlineData("")]
        // [InlineData("")]
        // public void From_ValidInput_ShouldReturnObject(string input)
        // {
        //     // act
        //     var result = Url.From(input);
        //         
        //     // assert
        //     Assert.Equal(input, result);
        // }
        //
        // [Theory]
        // [InlineData(null, typeof(ArgumentNullException))]
        // [InlineData("", typeof(ArgumentException))]
        // [InlineData("", typeof(InvalidUrlException))]
        // [InlineData("", typeof(InvalidUrlException))]
        // [InlineData("", typeof(InvalidUrlException))]
        // [InlineData("", typeof(InvalidUrlException))]
        // [InlineData("", typeof(InvalidUrlException))]
        // public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        // {
        //     // act
        //     var result = Record.Exception(() => Url.From(input));
        //     
        //     // assert
        //     Assert.Equal(expectedException, result?.GetType());
        // }
        //
        // #endregion
        //
        // #region TryFrom
        //
        // [Theory]
        // [InlineData("")]
        // [InlineData("")]
        // public void TryFrom_ValidInput_ShouldReturnOK(string input)
        // {
        //     // act
        //     var result = Url.TryFrom(input, out _);
        //         
        //     // assert
        //     Assert.Equal(Url.Validation.Ok, result);
        // }
        //
        // [Theory]
        // [InlineData(null, Url.Validation.Null)]
        // [InlineData("", Url.Validation.Empty)]
        // [InlineData("", Url.Validation.InvalidHyphenPlacement)]
        // [InlineData("", Url.Validation.InvalidHyphenPlacement)]
        // [InlineData("", Url.Validation.InvalidHyphenPlacement)]
        // [InlineData("", Url.Validation.IllegalCharacter)]
        // [InlineData("", Url.Validation.IllegalCharacter)]
        // public void TryFrom_WrongInput_ShouldReturnError(string input, Url.Validation expected)
        // {
        //     // act
        //     var result = Url.TryFrom(input, out _);
        //     
        //     // assert
        //     Assert.Equal(expected, result);
        // }
        //
        // #endregion
        //
        // #region Parse
        //
        // [Theory]
        // [InlineData("", "")]
        // [InlineData("", "hello-world")]
        // [InlineData("", "hello-world")]
        // [InlineData("", "c-grundlagen")]
        // public void Parse_ValidInput_ShouldReturnObject(string input, string expected)
        // {
        //     // act
        //     var result = Url.Parse(input);
        //         
        //     // assert
        //     Assert.Equal(expected, result);
        // }
        //
        // [Theory]
        // [InlineData(null, typeof(ArgumentNullException))]
        // [InlineData("", typeof(ArgumentException))]
        // [InlineData(" ", typeof(ArgumentException))]
        // public void Parse_WrongInput_ShouldThrowException(string input, Type expectedException)
        // {
        //     // act
        //     var result = Record.Exception(() => Url.Parse(input));
        //     
        //     // assert
        //     Assert.Equal(expectedException, result?.GetType());
        // }
        //
        // #endregion
        //
        // #region TryParse
        //
        // [Theory]
        // [InlineData("", "")]
        // [InlineData("", "hello-world")]
        // [InlineData("", "hello-world")]
        // [InlineData("", "c-grundlagen")]
        // public void TryParse_ValidInput_ShouldReturnTrue(string input, string expected)
        // {
        //     // act
        //     var result = Url.TryParse(input, out var url);
        //         
        //     // assert
        //     Assert.True(result);
        //     Assert.Equal(expected, url);
        // }
        //
        // [Theory]
        // [InlineData(null)]
        // [InlineData("")]
        // [InlineData(" ")]
        // public void TryParse_WrongInput_ShouldReturnFalse(string input)
        // {
        //     // act
        //     var result = Url.TryParse(input, out _);
        //     
        //     // assert
        //     Assert.False(result);
        // }
        //
        // #endregion
    }
}
