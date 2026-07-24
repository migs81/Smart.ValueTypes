using System;
using Smart.ValueTypes.Types.Network;
using Smart.ValueTypes.UnitTests.TestData;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Network
{
    public class MimeTypeTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // act
            var result = new MimeType();
            
            // assert
            Assert.Equal(MimeType.Empty, result);
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

        // #region From
        //
        // [Fact]
        // public void From_ValidInput_ShouldReturnObject()
        // {
        //     foreach (var value in ValidValues)
        //     {
        //         // act
        //         var result = MimeType.From(value);
        //         
        //         // assert
        //         Assert.Equal(value, result);
        //     }
        // }
        //
        // [Theory]
        // [InlineData(null, typeof(ArgumentNullException))]
        // [InlineData("", typeof(ArgumentException))]
        // [InlineData(MimeTypeTestData.TooShortValue, typeof(InvalidMimeTypeException))]
        // [InlineData(MimeTypeTestData.TooLongValue, typeof(InvalidMimeTypeException))]
        // [InlineData(MimeTypeTestData.StartsWithDotValue, typeof(InvalidMimeTypeException))]
        // [InlineData(MimeTypeTestData.EndsWithDotValue, typeof(InvalidMimeTypeException))]
        // [InlineData(MimeTypeTestData.ContainsIllegalCharacterValue, typeof(InvalidMimeTypeException))]
        // [InlineData(MimeTypeTestData.InvalidSegmentNumberValue, typeof(InvalidMimeTypeException))]
        // [InlineData(MimeTypeTestData.InvalidSegmentCountValue, typeof(InvalidMimeTypeException))]
        // public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        // {
        //     // act
        //     var result = Record.Exception(() => new MimeType(input));
        //     
        //     // assert
        //     Assert.Equal(expectedException, result?.GetType());
        // }
        //
        // #endregion
        //
        // #region TryFrom
        //
        // [Fact]
        // public void TryFrom_ValidInput_ShouldReturnOK()
        // {
        //     foreach (var value in ValidValues)
        //     {
        //         var result = MimeType.TryFrom(value, out _);
        //         Assert.Equal(MimeType.Validation.Ok, result);
        //     }
        // }
        //
        // [Theory]
        // [InlineData(null, MimeType.Validation.Null)]
        // [InlineData("", MimeType.Validation.Empty)]
        // [InlineData(MimeTypeTestData.TooShortValue, MimeType.Validation.TooShort)]
        // [InlineData(MimeTypeTestData.TooLongValue, MimeType.Validation.TooLong)]
        // [InlineData(MimeTypeTestData.StartsWithDotValue, MimeType.Validation.StartsWithDot)]
        // [InlineData(MimeTypeTestData.EndsWithDotValue, MimeType.Validation.EndsWithDot)]
        // [InlineData(MimeTypeTestData.ContainsIllegalCharacterValue, MimeType.Validation.ContainsIllegalCharacter)]
        // [InlineData(MimeTypeTestData.InvalidSegmentNumberValue, MimeType.Validation.InvalidSegmentNumber)]
        // [InlineData(MimeTypeTestData.InvalidSegmentCountValue, MimeType.Validation.InvalidSegmentCount)]
        // public void TryFrom_WrongInput_ShouldReturnError(string input, MimeType.Validation expected)
        // {
        //     var result = MimeType.TryFrom(input, out _);
        //     Assert.Equal(expected, result);
        // }
        //
        // #endregion
    }
}
