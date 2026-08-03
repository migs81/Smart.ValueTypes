using System;
using Smart.ValueTypes.Types.Web;
using Smart.ValueTypes.UnitTests.TestData;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Web
{
    public class MimeTypeTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(MimeType);
            
            // act
            var result = new MimeType();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }
            
        [Theory]
        [MemberData(nameof(MimeTypeTestData.ValidValues), MemberType = typeof(MimeTypeTestData))]
        public void Constructor_ValidInput_ShouldReturnObject(MimeTypeTestCase testCase)
        {
            // act
            var result = new MimeType(testCase.MimeType);
                
            // assert
            Assert.Equal(testCase.MimeType, result);
            Assert.Equal(testCase.Type, result.Type);
            Assert.Equal(testCase.Subtype, result.Subtype);
            Assert.Equal(testCase.Parameter, result.Parameter);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("applicationjson", typeof(InvalidMimeTypeException))] // type separator is missing
        [InlineData("/json", typeof(InvalidMimeTypeException))] // type too short
        [InlineData("applicat=ion/json", typeof(InvalidMimeTypeException))] // type contains invalid character
        [InlineData("application//json", typeof(InvalidMimeTypeException))] // subtype contains invalid character
        [InlineData("application/js/on", typeof(InvalidMimeTypeException))] // subtype contains invalid character
        [InlineData("application/", typeof(InvalidMimeTypeException))] // subtype too short
        [InlineData("application/js?on", typeof(InvalidMimeTypeException))] // type contains invalid character
        [InlineData("application/json;", typeof(InvalidMimeTypeException))] // invalid parameter
        [InlineData("application/json;charset:UTF-8", typeof(InvalidMimeTypeException))] // invalid parameter
        [InlineData("application/json;charset==UTF-8", typeof(InvalidMimeTypeException))] // invalid parameter
        [InlineData("application/json;charset=UTF-8;boundary-myBoundary", typeof(InvalidMimeTypeException))] // invalid parameter
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new MimeType(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion

        #region From
        
        [Theory]
        [MemberData(nameof(MimeTypeTestData.ValidValues), MemberType = typeof(MimeTypeTestData))]
        public void From_ValidInput_ShouldReturnObject(MimeTypeTestCase testCase)
        {
            // act
            var result = MimeType.From(testCase.MimeType);
                
            // assert
            Assert.Equal(testCase.MimeType, result);
            Assert.Equal(testCase.Type, result.Type);
            Assert.Equal(testCase.Subtype, result.Subtype);
            Assert.Equal(testCase.Parameter, result.Parameter);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData("applicationjson", typeof(InvalidMimeTypeException))] // type separator is missing
        [InlineData("/json", typeof(InvalidMimeTypeException))] // type too short
        [InlineData("applicat=ion/json", typeof(InvalidMimeTypeException))] // type contains invalid character
        [InlineData("application//json", typeof(InvalidMimeTypeException))] // subtype contains invalid character
        [InlineData("application/js/on", typeof(InvalidMimeTypeException))] // subtype contains invalid character
        [InlineData("application/", typeof(InvalidMimeTypeException))] // subtype too short
        [InlineData("application/js?on", typeof(InvalidMimeTypeException))] // type contains invalid character
        [InlineData("application/json;", typeof(InvalidMimeTypeException))] // invalid parameter
        [InlineData("application/json;charset:UTF-8", typeof(InvalidMimeTypeException))] // invalid parameter
        [InlineData("application/json;charset==UTF-8", typeof(InvalidMimeTypeException))] // invalid parameter
        [InlineData("application/json;charset=UTF-8;boundary-myBoundary", typeof(InvalidMimeTypeException))] // invalid parameter
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => MimeType.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [MemberData(nameof(MimeTypeTestData.ValidValues), MemberType = typeof(MimeTypeTestData))]
        public void TryFrom_ValidInput_ShouldReturnOk(MimeTypeTestCase testCase)
        {
            // act
            var result = MimeType.TryFrom(testCase.MimeType, out _);
                
            // assert
            Assert.Equal(MimeType.Validation.Ok, result);
        }
        
        [Theory]
        [InlineData(null, MimeType.Validation.Null)]
        [InlineData("", MimeType.Validation.Empty)]
        [InlineData("applicationjson", MimeType.Validation.IncorrectNumberOfTypeSeparators)] // separator is missing
        [InlineData("application//json", MimeType.Validation.IncorrectNumberOfTypeSeparators)] // too many separators
        [InlineData("/json", MimeType.Validation.TypeTooShort)] // type too short
        [InlineData("applicat=ion/json", MimeType.Validation.TypeContainsIllegalCharacter)] // type contains invalid character
        [InlineData("application/", MimeType.Validation.SubtypeTooShort)] // subtype too short
        [InlineData("application/js?on", MimeType.Validation.SubtypeContainsIllegalCharacter)] // subtype contains invalid character
        [InlineData("application/json;", MimeType.Validation.ParameterTooShort)] // invalid parameter
        [InlineData("application/json;charset:UTF-8", MimeType.Validation.InvalidParameter)] // invalid parameter
        [InlineData("application/json;charset==UTF-8", MimeType.Validation.InvalidParameter)] // invalid parameter
        [InlineData("application/json;charset=UTF-8;boundary-myBoundary", MimeType.Validation.InvalidParameter)] // invalid parameter
        public void TryFrom_WrongInput_ShouldThrowException(string input, MimeType.Validation expected)
        {
            // act
            var result = MimeType.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
    }
}
