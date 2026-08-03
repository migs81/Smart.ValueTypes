using Smart.ValueTypes.Types.Hashes;
using System;
using Xunit;

namespace Smart.ValueTypes.UnitTests.Hashes
{
    public class SHA512Test
    {
        #region test data

        private static readonly ValueTuple<string, string> ValidValue = new("test", "ee26b0dd4af7e749aa1a8ee3c10ae992" +
                                                                                    "3f618980772e473f8819a5d4940e0db2" +
                                                                                    "7ac185f8a0e1d5f84f88bc887fd67b14" +
                                                                                    "3732c304cc5fa9ad8e6f57f50028a8ff");

        private const string WrongCharacter = "gggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg" +
                                              "gggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg";
        
        #endregion

        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // arrange
            var expected = default(SHA512);

            // act
            var result = new SHA512();
            
            // assert
            Assert.Equal(expected, result);
            Assert.True(result.IsDefault);
        }

        [Theory]
        [InlineData("1f40fc92da241694750979ee6cf582f2d5d7d28e18335de05abc54d0560e0f5302860c652bf08d560252aa5e74210546f369fbbbce8c12cfc7957b2652fe9a75")]
        [InlineData("5267768822ee624d48fce15ec5ca79cbd602cb7f4c2157a516556991f22ef8c7b5ef7b18d1ff41c59370efb0858651d44a936c11b7b144c48fe04df3c6a3e8da")]
        [InlineData("acc28db2beb7b42baa1cb0243d401ccb4e3fce44d7b02879a52799aadff541522d8822598b2fa664f9d5156c00c924805d75c3868bd56c2acb81d37e98e35adc")]
        [InlineData("48fb10b15f3d44a09dc82d02b06581e0c0c69478c9fd2cf8f9093659019a1687baecdbb38c9e72b12169dc4148690f87467f9154f5931c5df665c6496cbfd5f5")]
        [InlineData("87c568e037a5fa50b1bc911e8ee19a77c4dd3c22bce9932f86fdd8a216afe1681c89737fada6859e91047eece711ec16da62d6ccb9fd0de2c51f132347350d8c")]
        [InlineData("711c22448e721e5491d8245b49425aa861f1fc4a15287f0735e203799b65cffec50b5abd0fddd91cd643aeb3b530d48f05e258e7e230a94ed5025c1387bb4e1b")]
        [InlineData("19f142b018f307bfdf1c7009d15a29417c96d8678d2982eebce4961b2e67eeb118a8ebb1d75b70087c3e65bc793450e3fe4a10002befa2d038e5aed4796937f2")]
        [InlineData("2241bc8fc70705b42efead371fd4982c5ba69917e5b4b895810002644f0386da9c3131793458c2bf47608480d64a07278133c99912e0ba2daf23098f3520eb97")]
        [InlineData("507b553b106b1b9963b7affb34e5ed14bc1160bbdea24c094405b306bdcb2520823a0c7db7da4b51cf45cbdbad519eeca9affd7103b131d1e65a4974ba56b18d")]
        [InlineData("fcd8780493d9d11d29031b928a9da358a6f48627fff0cb7e80fb8107de86e0c365dc9cf8fe2fc05a6ce6d75803b78ac894d82a396042312a995ef63b5dd4dd11")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new SHA512(input);
                
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidSha512Exception))]
        [InlineData(WrongCharacter, typeof(InvalidSha512Exception))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new SHA512(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
            
        [Theory]
        [InlineData("1f40fc92da241694750979ee6cf582f2d5d7d28e18335de05abc54d0560e0f5302860c652bf08d560252aa5e74210546f369fbbbce8c12cfc7957b2652fe9a75")]
        [InlineData("5267768822ee624d48fce15ec5ca79cbd602cb7f4c2157a516556991f22ef8c7b5ef7b18d1ff41c59370efb0858651d44a936c11b7b144c48fe04df3c6a3e8da")]
        [InlineData("acc28db2beb7b42baa1cb0243d401ccb4e3fce44d7b02879a52799aadff541522d8822598b2fa664f9d5156c00c924805d75c3868bd56c2acb81d37e98e35adc")]
        [InlineData("48fb10b15f3d44a09dc82d02b06581e0c0c69478c9fd2cf8f9093659019a1687baecdbb38c9e72b12169dc4148690f87467f9154f5931c5df665c6496cbfd5f5")]
        [InlineData("87c568e037a5fa50b1bc911e8ee19a77c4dd3c22bce9932f86fdd8a216afe1681c89737fada6859e91047eece711ec16da62d6ccb9fd0de2c51f132347350d8c")]
        [InlineData("711c22448e721e5491d8245b49425aa861f1fc4a15287f0735e203799b65cffec50b5abd0fddd91cd643aeb3b530d48f05e258e7e230a94ed5025c1387bb4e1b")]
        [InlineData("19f142b018f307bfdf1c7009d15a29417c96d8678d2982eebce4961b2e67eeb118a8ebb1d75b70087c3e65bc793450e3fe4a10002befa2d038e5aed4796937f2")]
        [InlineData("2241bc8fc70705b42efead371fd4982c5ba69917e5b4b895810002644f0386da9c3131793458c2bf47608480d64a07278133c99912e0ba2daf23098f3520eb97")]
        [InlineData("507b553b106b1b9963b7affb34e5ed14bc1160bbdea24c094405b306bdcb2520823a0c7db7da4b51cf45cbdbad519eeca9affd7103b131d1e65a4974ba56b18d")]
        [InlineData("fcd8780493d9d11d29031b928a9da358a6f48627fff0cb7e80fb8107de86e0c365dc9cf8fe2fc05a6ce6d75803b78ac894d82a396042312a995ef63b5dd4dd11")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = SHA512.From(input);
                
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(InvalidSha512Exception))]
        [InlineData(WrongCharacter, typeof(InvalidSha512Exception))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new SHA512(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData("1f40fc92da241694750979ee6cf582f2d5d7d28e18335de05abc54d0560e0f5302860c652bf08d560252aa5e74210546f369fbbbce8c12cfc7957b2652fe9a75")]
        [InlineData("5267768822ee624d48fce15ec5ca79cbd602cb7f4c2157a516556991f22ef8c7b5ef7b18d1ff41c59370efb0858651d44a936c11b7b144c48fe04df3c6a3e8da")]
        [InlineData("acc28db2beb7b42baa1cb0243d401ccb4e3fce44d7b02879a52799aadff541522d8822598b2fa664f9d5156c00c924805d75c3868bd56c2acb81d37e98e35adc")]
        [InlineData("48fb10b15f3d44a09dc82d02b06581e0c0c69478c9fd2cf8f9093659019a1687baecdbb38c9e72b12169dc4148690f87467f9154f5931c5df665c6496cbfd5f5")]
        [InlineData("87c568e037a5fa50b1bc911e8ee19a77c4dd3c22bce9932f86fdd8a216afe1681c89737fada6859e91047eece711ec16da62d6ccb9fd0de2c51f132347350d8c")]
        [InlineData("711c22448e721e5491d8245b49425aa861f1fc4a15287f0735e203799b65cffec50b5abd0fddd91cd643aeb3b530d48f05e258e7e230a94ed5025c1387bb4e1b")]
        [InlineData("19f142b018f307bfdf1c7009d15a29417c96d8678d2982eebce4961b2e67eeb118a8ebb1d75b70087c3e65bc793450e3fe4a10002befa2d038e5aed4796937f2")]
        [InlineData("2241bc8fc70705b42efead371fd4982c5ba69917e5b4b895810002644f0386da9c3131793458c2bf47608480d64a07278133c99912e0ba2daf23098f3520eb97")]
        [InlineData("507b553b106b1b9963b7affb34e5ed14bc1160bbdea24c094405b306bdcb2520823a0c7db7da4b51cf45cbdbad519eeca9affd7103b131d1e65a4974ba56b18d")]
        [InlineData("fcd8780493d9d11d29031b928a9da358a6f48627fff0cb7e80fb8107de86e0c365dc9cf8fe2fc05a6ce6d75803b78ac894d82a396042312a995ef63b5dd4dd11")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = SHA512.TryFrom(input, out _);
                
            // assert
            Assert.Equal(SHA512.Validation.Ok, result);
        }

        [Theory]
        [InlineData(null, SHA512.Validation.Null)]
        [InlineData("", SHA512.Validation.WrongLength)]
        [InlineData(WrongCharacter, SHA512.Validation.IllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, SHA512.Validation expected)
        {
            // act
            var result = SHA512.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion
        
        #region Create

        [Fact]
        public void Create_ValidInput_ShouldReturnObject()
        {
            // act
            var result = SHA512.Create(ValidValue.Item1);
            
            // assert
            Assert.Equal(ValidValue.Item2.ToUpper(), result);
        }

        #endregion
        
        #region TryCreate

        [Fact]
        public void TryCreate_ValidInput_ShouldReturnTrue()
        {
            // act
            var result = SHA512.TryCreate(ValidValue.Item1, out _);
            
            // assert
            Assert.True(result);
        }

        #endregion
    }
}
