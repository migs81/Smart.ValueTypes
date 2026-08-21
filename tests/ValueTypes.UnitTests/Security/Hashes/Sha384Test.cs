using System;
using Smart.ValueTypes.Types.Security.Hashes;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Security.Hashes
{
    public class Sha384Test
    {
        #region test data

        private static readonly ValueTuple<string, string> ValidValue = new("test",
            "768412320f7b0aa5812fce428dc4706b3cae50e02a64caa16a782249bfe8efc4b7ef1ccb126255d196047dfedf17a0a9");

        private const string WrongCharacter = 
            "gggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg";
        
        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(SHA384);

            // act
            var result = new SHA384();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("54a59b9f22b0b80880d8427e548b7c23abd873486e1f035dce9cd697e85175033caa88e6d57bc35efae0b5afd3145f31")]
        [InlineData("98a906182cdcfb1eb4eb47117600f68958e2ddd140248b47984f4bde6587b89c8215c3da895a336e94ad1aca39015c40")]
        [InlineData("40f98a05660bf871802ee59964de1945bd731a45cc7f48e4dadd92f34a7eeec089e149ad8c2434f11792e588b740d997")]
        [InlineData("8ac10705a78a2dcd15fa577bac70762708597a02e130d8a6192d73dababd2b14502dbeee29d0e22bc341a0c42af6a4fb")]
        [InlineData("8d182905e535537a32cc0c475403d1fe78ee541a40d61e0b306d7541ed8dbb63d550dab383d0fca0e23448af99bffe10")]
        [InlineData("3a31ca443b6cae3717b4a4f19972bc92413645380e4990e4fe4dc322386494fadaa2b63b4f62be6e2b7077e982bb7ada")]
        [InlineData("400351f8d278fdf7e765ccb2943e8a99548a68ae0235b96fa47cb758a354ddd1201d034199439c49ff22cfc0904bc2e2")]
        [InlineData("a4eb0778c79fce94c02126543cba398d645b2fd4c6ff6a02eecc026bbe0cc0dd666279722b7615bc15b4c9126b941c04")]
        [InlineData("2051ff7a91bcc6245fe7c3c4bdfcb0538553f73c54100c686a6fc0279354d12ecb4b7589a60a516c4fabadbeb622f397")]
        [InlineData("417316736caf4fc0f2a09f5d6e4c90d78b768fe08add82c5db9f0a22809bd6a719e3ad367f149d92b5455e952b79613b")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new SHA384(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidSha384Exception))]
        [InlineData(WrongCharacter, typeof(InvalidSha384Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new SHA384(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From

        [Theory]
        [InlineData("54a59b9f22b0b80880d8427e548b7c23abd873486e1f035dce9cd697e85175033caa88e6d57bc35efae0b5afd3145f31")]
        [InlineData("98a906182cdcfb1eb4eb47117600f68958e2ddd140248b47984f4bde6587b89c8215c3da895a336e94ad1aca39015c40")]
        [InlineData("40f98a05660bf871802ee59964de1945bd731a45cc7f48e4dadd92f34a7eeec089e149ad8c2434f11792e588b740d997")]
        [InlineData("8ac10705a78a2dcd15fa577bac70762708597a02e130d8a6192d73dababd2b14502dbeee29d0e22bc341a0c42af6a4fb")]
        [InlineData("8d182905e535537a32cc0c475403d1fe78ee541a40d61e0b306d7541ed8dbb63d550dab383d0fca0e23448af99bffe10")]
        [InlineData("3a31ca443b6cae3717b4a4f19972bc92413645380e4990e4fe4dc322386494fadaa2b63b4f62be6e2b7077e982bb7ada")]
        [InlineData("400351f8d278fdf7e765ccb2943e8a99548a68ae0235b96fa47cb758a354ddd1201d034199439c49ff22cfc0904bc2e2")]
        [InlineData("a4eb0778c79fce94c02126543cba398d645b2fd4c6ff6a02eecc026bbe0cc0dd666279722b7615bc15b4c9126b941c04")]
        [InlineData("2051ff7a91bcc6245fe7c3c4bdfcb0538553f73c54100c686a6fc0279354d12ecb4b7589a60a516c4fabadbeb622f397")]
        [InlineData("417316736caf4fc0f2a09f5d6e4c90d78b768fe08add82c5db9f0a22809bd6a719e3ad367f149d92b5455e952b79613b")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = SHA384.From(input);
                
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidSha384Exception))]
        [InlineData(WrongCharacter, typeof(InvalidSha384Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new SHA384(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom

        [Theory]
        [InlineData("54a59b9f22b0b80880d8427e548b7c23abd873486e1f035dce9cd697e85175033caa88e6d57bc35efae0b5afd3145f31")]
        [InlineData("98a906182cdcfb1eb4eb47117600f68958e2ddd140248b47984f4bde6587b89c8215c3da895a336e94ad1aca39015c40")]
        [InlineData("40f98a05660bf871802ee59964de1945bd731a45cc7f48e4dadd92f34a7eeec089e149ad8c2434f11792e588b740d997")]
        [InlineData("8ac10705a78a2dcd15fa577bac70762708597a02e130d8a6192d73dababd2b14502dbeee29d0e22bc341a0c42af6a4fb")]
        [InlineData("8d182905e535537a32cc0c475403d1fe78ee541a40d61e0b306d7541ed8dbb63d550dab383d0fca0e23448af99bffe10")]
        [InlineData("3a31ca443b6cae3717b4a4f19972bc92413645380e4990e4fe4dc322386494fadaa2b63b4f62be6e2b7077e982bb7ada")]
        [InlineData("400351f8d278fdf7e765ccb2943e8a99548a68ae0235b96fa47cb758a354ddd1201d034199439c49ff22cfc0904bc2e2")]
        [InlineData("a4eb0778c79fce94c02126543cba398d645b2fd4c6ff6a02eecc026bbe0cc0dd666279722b7615bc15b4c9126b941c04")]
        [InlineData("2051ff7a91bcc6245fe7c3c4bdfcb0538553f73c54100c686a6fc0279354d12ecb4b7589a60a516c4fabadbeb622f397")]
        [InlineData("417316736caf4fc0f2a09f5d6e4c90d78b768fe08add82c5db9f0a22809bd6a719e3ad367f149d92b5455e952b79613b")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = SHA384.TryFrom(input, out _);
                
            // assert
            Assert.Equal(SHA384.Validation.Ok, result);
        }

        [Theory]
        [InlineData(null, SHA384.Validation.Null)]
        [InlineData("", SHA384.Validation.WrongLength)]
        [InlineData(WrongCharacter, SHA384.Validation.IllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, SHA384.Validation expected)
        {
            // act
            var result = SHA384.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
        
        #region Create

        [Fact]
        public void Create_ValidInput_ShouldReturnObject()
        {
            // act
            var result = SHA384.Create(ValidValue.Item1);
            
            // assert
            Assert.Equal(ValidValue.Item2.ToUpper(), result);
        }

        #endregion
        
        #region TryCreate

        [Fact]
        public void TryCreate_ValidInput_ShouldReturnTrue()
        {
            // act
            var result = SHA384.TryCreate(ValidValue.Item1, out _);
            
            // assert
            Assert.True(result);
        }

        #endregion
    }
}
