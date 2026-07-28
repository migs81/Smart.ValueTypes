using System;
using Smart.ValueTypes.Types.Network;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Network
{
    public class SlugTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // act
            var result = new Slug();
            
            // assert
            Assert.Equal(Slug.Empty, result);
        }

        [Theory]
        [InlineData("about-us")]
        [InlineData("product-123")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new Slug(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("-about-us", typeof(InvalidSlugException))]
        [InlineData("about-us-", typeof(InvalidSlugException))]
        [InlineData("about--us", typeof(InvalidSlugException))]
        [InlineData("About-us", typeof(InvalidSlugException))]
        [InlineData("about+us", typeof(InvalidSlugException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new Slug(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData("about-us")]
        [InlineData("product-123")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = Slug.From(input);
                
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("-about-us", typeof(InvalidSlugException))]
        [InlineData("about-us-", typeof(InvalidSlugException))]
        [InlineData("about--us", typeof(InvalidSlugException))]
        [InlineData("About-us", typeof(InvalidSlugException))]
        [InlineData("about+us", typeof(InvalidSlugException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => Slug.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region TryFrom

        [Theory]
        [InlineData("about-us")]
        [InlineData("product-123")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = Slug.TryFrom(input, out _);
                
            // assert
            Assert.Equal(Slug.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, Slug.Validation.Null)]
        [InlineData("", Slug.Validation.Empty)]
        [InlineData("-about-us", Slug.Validation.InvalidHyphenPlacement)]
        [InlineData("about-us-", Slug.Validation.InvalidHyphenPlacement)]
        [InlineData("about--us", Slug.Validation.InvalidHyphenPlacement)]
        [InlineData("About-us", Slug.Validation.IllegalCharacter)]
        [InlineData("about+us", Slug.Validation.IllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, Slug.Validation expected)
        {
            // act
            var result = Slug.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }

        #endregion
        
        #region Parse
        
        [Theory]
        [InlineData("About Us", "about-us")]
        [InlineData("Hello World", "hello-world")]
        [InlineData("  Hello World  ", "hello-world")]
        [InlineData("C# Grundlagen", "c-grundlagen")]
        [InlineData("Über uns", "ueber-uns")]
        [InlineData("Müller & Söhne", "mueller-soehne")]
        [InlineData("Café Crème", "cafe-creme")]
        [InlineData("My   Product", "my-product")]
        [InlineData("Hello---World", "hello-world")]
        [InlineData("HELLO WORLD", "hello-world")]
        [InlineData("  Multiple   Spaces  ", "multiple-spaces")]
        [InlineData("Das ist ein Test!", "das-ist-ein-test")]
        [InlineData("100% Coverage", "100-coverage")]
        [InlineData("hello", "hello")]
        [InlineData("Hello", "hello")]
        [InlineData("HELLO", "hello")]
        [InlineData("Hello   World", "hello-world")]
        [InlineData("hello-world", "hello-world")]
        [InlineData("hello--world", "hello-world")]
        [InlineData("hello---world", "hello-world")]
        [InlineData("-hello-world", "hello-world")]
        [InlineData("hello-world-", "hello-world")]
        [InlineData("--hello--world--", "hello-world")]
        [InlineData("hello_world", "hello-world")]
        [InlineData("hello.world", "hello-world")]
        [InlineData("hello/world", "hello-world")]
        [InlineData("hello\\world", "hello-world")]
        [InlineData("hello@world", "hello-world")]
        [InlineData("hello#world", "hello-world")]
        [InlineData("hello+world", "hello-world")]
        [InlineData("hello&world", "hello-world")]
        [InlineData("hello=world", "hello-world")]
        [InlineData("hello?world", "hello-world")]
        [InlineData("hello!world", "hello-world")]
        [InlineData("hello,world", "hello-world")]
        [InlineData("hello:world", "hello-world")]
        [InlineData("hello;world", "hello-world")]
        [InlineData("hello(world)", "hello-world")]
        [InlineData("hello[world]", "hello-world")]
        [InlineData("hello{world}", "hello-world")]
        [InlineData("hello\"world", "hello-world")]
        [InlineData("hello'world", "hello-world")]
        [InlineData("123", "123")]
        [InlineData("123 456", "123-456")]
        [InlineData("version 2", "version-2")]
        [InlineData("v2.1", "v2-1")]
        [InlineData("C#", "c")]
        [InlineData("C++", "c")]
        [InlineData(".NET 9", "net-9")]
        [InlineData("ASP.NET Core", "asp-net-core")]
        [InlineData("Über", "ueber")]
        [InlineData("Österreich", "oesterreich")]
        [InlineData("Äpfel", "aepfel")]
        [InlineData("Fußball", "fussball")]
        [InlineData("Müller", "mueller")]
        [InlineData("Café", "cafe")]
        [InlineData("Crème brûlée", "creme-brulee")]
        [InlineData("São Paulo", "sao-paulo")]
        [InlineData("François", "francois")]
        [InlineData("naïve", "naive")]
        [InlineData("piñata", "pinata")]
        [InlineData("___hello___", "hello")]
        [InlineData("***hello***", "hello")]
        [InlineData("...hello...", "hello")]
        [InlineData("###hello###", "hello")]
        public void Parse_ValidInput_ShouldReturnObject(string input, string expected)
        {
            // act
            var result = Slug.Parse(input);
                
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
    }
}
