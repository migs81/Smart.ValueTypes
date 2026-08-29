using Smart.ValueTypes.Types.Units;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Units
{
    public class ByteSizeTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(ByteSize);
            
            // act
            var result = new ByteSize();
            
            // assert
            Assert.Equal(expected, result);
            
            // bits
            Assert.Equal(0m, result.Bits);
            Assert.Equal(0m, result.Kibibits);
            Assert.Equal(0m, result.Mebibits);
            Assert.Equal(0m, result.Gibibits);
            Assert.Equal(0m, result.Tebibits);
            Assert.Equal(0m, result.Kilobytes);
            Assert.Equal(0m, result.Megabits);
            Assert.Equal(0m, result.Gigabits);
            Assert.Equal(0m, result.Terabits);
            
            // bytes
            Assert.Equal(0m, result.Kibibytes);
            Assert.Equal(0m, result.Mebibytes);
            Assert.Equal(0m, result.Gibibytes);
            Assert.Equal(0m, result.Tebibytes);
            Assert.Equal(0m, result.Kilobytes);
            Assert.Equal(0m, result.Megabytes);
            Assert.Equal(0m, result.Gigabytes);
            Assert.Equal(0m, result.Terabytes);
        }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        public void Constructor_ValidInput_ShouldReturnObject(ulong value)
        {
            // act
            var result = new ByteSize(value);
            
            // assert
            Assert.Equal(value, (ulong)result);
        }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(1000)]
        [InlineData(1_000_000)]
        [InlineData(1_000_000_000)]
        [InlineData(ulong.MaxValue)]
        public void Constructor_ValidInput_ShouldContainCorrectProperties(ulong value)
        {
            // act
            var result = new ByteSize(value);
            var bits = value * 8m;
            
            var kibibits = value * 8m / 1024m;
            var mebibits = value * 8m / (1024m * 1024m);
            var gibibits = value * 8m / (1024m * 1024m * 1024m);
            var tebibits = value * 8m / (1024m * 1024m * 1024m * 1024m);
            
            var kilobits = value * 8m / 1000m;
            var megabits = value * 8m / (1000m * 1000m);
            var gigabits = value * 8m / (1000m * 1000m * 1000m);
            var terabits = value * 8m / (1000m * 1000m * 1000m * 1000m);
            
            var kibibytes = value / 1024m;
            var mebibytes = value / (1024m * 1024m);
            var gibibytes = value / (1024m * 1024m * 1024m);
            var tebibytes = value / (1024m * 1024m * 1024m * 1024m);
                     
            var kilobytes = value / 1000m;
            var megabytes = value / (1000m * 1000m);
            var gigabytes = value / (1000m * 1000m * 1000m);
            var terabytes = value / (1000m * 1000m * 1000m * 1000m);
            
            // assert
            Assert.Equal(value, (ulong)result);
            
            // bits
            Assert.Equal(bits, result.Bits);
            
            Assert.Equal(kibibits, result.Kibibits);
            Assert.Equal(mebibits, result.Mebibits);
            Assert.Equal(gibibits, result.Gibibits);
            Assert.Equal(tebibits, result.Tebibits);
            
            Assert.Equal(kilobits, result.Kilobits);
            Assert.Equal(megabits, result.Megabits);
            Assert.Equal(gigabits, result.Gigabits);
            Assert.Equal(terabits, result.Terabits);
            
            // bytes
            Assert.Equal(kibibytes, result.Kibibytes);
            Assert.Equal(mebibytes, result.Mebibytes);
            Assert.Equal(gibibytes, result.Gibibytes);
            Assert.Equal(tebibytes, result.Tebibytes);
            
            Assert.Equal(kilobytes, result.Kilobytes);
            Assert.Equal(megabytes, result.Megabytes);
            Assert.Equal(gigabytes, result.Gigabytes);
            Assert.Equal(terabytes, result.Terabytes);
        }
        
        #endregion
        
        #region From

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(uint.MaxValue)]
        public void From_ValidInput_Uint_ShouldReturnObject(uint value)
        {
            // act
            var result = ByteSize.From(value);
            
            // assert
            Assert.Equal(value, (ulong)result);
        }

        [Theory]
        [InlineData(ulong.MinValue)]
        [InlineData(ulong.MaxValue)]
        public void From_ValidInput_Ulong_ShouldReturnObject(ulong value)
        {
            // act
            var result = ByteSize.From(value);
            
            // assert
            Assert.Equal(value, (ulong)result);
        }

        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(10U)]
        [InlineData(uint.MaxValue)]
        public void FromKibibytes_ValidInput_ShouldReturnObject(uint value)
        {
            // arrange
            var expected = value * 1024UL;
            
            // act
            var result = ByteSize.FromKibibytes(value);
            
            // assert
            Assert.Equal(expected, (ulong)result);
        }
        
        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(10U)]
        [InlineData(uint.MaxValue)]
        public void FromMebibytes_ValidInput_ShouldReturnObject(uint value)
        {
            // arrange
            var expected = value * 1024UL * 1024UL;
            
            // act
            var result = ByteSize.FromMebibytes(value);
            
            // assert
            Assert.Equal(expected, (ulong)result);
        }
        
        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(10U)]
        [InlineData(uint.MaxValue)]
        public void FromGibibytes_ValidInput_ShouldReturnObject(uint value)
        {
            // arrange
            var expected = value * 1024UL * 1024UL* 1024UL;
            
            // act
            var result = ByteSize.FromGibibytes(value);
            
            // assert
            Assert.Equal(expected, (ulong)result);
        }
        
        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(10U)]
        [InlineData(uint.MaxValue)]
        public void FromTebibytes_ValidInput_ShouldReturnObject(uint value)
        {
            // arrange
            var expected = value * 1024UL * 1024UL * 1024UL * 1024UL;
            
            // act
            var result = ByteSize.FromTebibytes(value);
            
            // assert
            Assert.Equal(expected, (ulong)result);
        }
        
        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(10U)]
        [InlineData(uint.MaxValue)]
        public void FromKilobytes_ValidInput_ShouldReturnObject(uint value)
        {
            // arrange
            var expected = value * 1000UL;
            
            // act
            var result = ByteSize.FromKilobytes(value);
            
            // assert
            Assert.Equal(expected, (ulong)result);
        }
        
        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(10U)]
        [InlineData(uint.MaxValue)]
        public void FromMegabytes_ValidInput_ShouldReturnObject(uint value)
        {
            // arrange
            var expected = value * 1000UL * 1000UL;
            
            // act
            var result = ByteSize.FromMegabytes(value);
            
            // assert
            Assert.Equal(expected, (ulong)result);
        }
        
        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(10U)]
        [InlineData(uint.MaxValue)]
        public void FromGigabytes_ValidInput_ShouldReturnObject(uint value)
        {
            // arrange
            var expected = value * 1000UL * 1000UL* 1000UL;
            
            // act
            var result = ByteSize.FromGigabytes(value);
            
            // assert
            Assert.Equal(expected, (ulong)result);
        }
        
        [Theory]
        [InlineData(uint.MinValue)]
        [InlineData(10U)]
        [InlineData(uint.MaxValue)]
        public void FromTerabytes_ValidInput_ShouldReturnObject(uint value)
        {
            // arrange
            var expected = value * 1000UL * 1000UL * 1000UL * 1000UL;
            
            // act
            var result = ByteSize.FromTerabytes(value);
            
            // assert
            Assert.Equal(expected, (ulong)result);
        }
        
        #endregion
    }
}
