using System;
using Smart.ValueTypes.Types.IO;
using Xunit;

namespace Smart.ValueTypes.UnitTests.IO
{
    public class WindowsFilePathTest
    {
        #region constructor

        [Fact]
        public void Constructor_NoInput_ShouldReturnDefaultObject()
        {
            // act
            var result = new WindowsFilePath();
            
            // assert
            Assert.Equal(WindowsFilePath.Empty, result);
        }

        [Theory]
        [InlineData(@"C:\File.txt")]
        [InlineData(@"C:\Temp\File.txt")]
        [InlineData(@"C:\Temp\Subfolder\File.txt")]
        [InlineData(@"D:\Data\Test.pdf")]
        [InlineData(@"Z:\")]
        [InlineData(@"Temp\File.txt")]
        [InlineData(@"Subfolder\File.txt")]
        [InlineData(@".\File.txt")]
        [InlineData(@".\Temp\File.txt")]
        [InlineData(@"..\File.txt")]
        [InlineData(@"..\Temp\File.txt")]
        [InlineData(@"..\..\Temp\File.txt")]
        [InlineData(@"\\Server\Share\File.txt")]
        [InlineData(@"\\Server\Share\Folder\File.txt")]
        [InlineData(@"C:Temp\File.txt")]
        [InlineData(@"My Folder\File Name.txt")]
        [InlineData(@"My_Folder\File_Name.txt")]
        [InlineData(@"My-Folder\File-Name.txt")]
        [InlineData(@"123\456\789.txt")]
        [InlineData(@"C:\123\456\File123.txt")]
        [InlineData("C:File.txt")]
        [InlineData("file.txt")]
        [InlineData("README")]
        [InlineData("archive.tar.gz")]
        [InlineData("C:/Temp/File.txt")]
        [InlineData("Temp/File.txt")]
        public void Constructor_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = new WindowsFilePath(input);
            
            // assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(" ", typeof(ArgumentException))]
        [InlineData("   ", typeof(ArgumentException))]
        [InlineData(@"C:\Temp\File<1>.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File>1.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File:1.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File""1.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File|1.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File?1.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File*1.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File<.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File>.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File:.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File"".txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File|.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File?.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File*.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C::\Temp\File.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"::\Temp\File.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp:File.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Folder:Name\File.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\CON", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\con", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\CON.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\PRN", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\PrN", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\PRN.log", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\AUX", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\NUL", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\COM1", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\COM9.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\LPT1", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\LPT9.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\Folder.", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File.txt.", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\Folder ", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File.txt ", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\\File.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\\Temp\File.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File
.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData("C:\\Temp\\File\t.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData("C:\\Temp\\File\0.txt", typeof(InvalidWindowsFilePathException))]
        public void Constructor_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => new WindowsFilePath(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }

        #endregion
        
        #region From
        
        [Theory]
        [InlineData(@"C:\File.txt")]
        [InlineData(@"C:\Temp\File.txt")]
        [InlineData(@"C:\Temp\Subfolder\File.txt")]
        [InlineData(@"D:\Data\Test.pdf")]
        [InlineData(@"Z:\")]
        [InlineData(@"Temp\File.txt")]
        [InlineData(@"Subfolder\File.txt")]
        [InlineData(@".\File.txt")]
        [InlineData(@".\Temp\File.txt")]
        [InlineData(@"..\File.txt")]
        [InlineData(@"..\Temp\File.txt")]
        [InlineData(@"..\..\Temp\File.txt")]
        [InlineData(@"\\Server\Share\File.txt")]
        [InlineData(@"\\Server\Share\Folder\File.txt")]
        [InlineData(@"C:Temp\File.txt")]
        [InlineData(@"My Folder\File Name.txt")]
        [InlineData(@"My_Folder\File_Name.txt")]
        [InlineData(@"My-Folder\File-Name.txt")]
        [InlineData(@"123\456\789.txt")]
        [InlineData(@"C:\123\456\File123.txt")]
        [InlineData("C:File.txt")]
        [InlineData("file.txt")]
        [InlineData("README")]
        [InlineData("archive.tar.gz")]
        [InlineData("C:/Temp/File.txt")]
        [InlineData("Temp/File.txt")]
        public void From_ValidInput_ShouldReturnObject(string input)
        {
            // act
            var result = WindowsFilePath.From(input);
            
            // assert
            Assert.Equal(input, result);
        }
        
        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        [InlineData(" ", typeof(ArgumentException))]
        [InlineData("   ", typeof(ArgumentException))]
        [InlineData(@"C:\Temp\File<1>.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File>1.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File:1.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File""1.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File|1.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File?1.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File*1.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File<.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File>.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File:.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File"".txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File|.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File?.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp\File*.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C::\Temp\File.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"::\Temp\File.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Temp:File.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"Folder:Name\File.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\CON", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\con", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\CON.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\PRN", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\PrN", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\PRN.log", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\AUX", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\NUL", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\COM1", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\COM9.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\LPT1", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\LPT9.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\Folder.", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File.txt.", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\Folder ", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File.txt ", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\\File.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\\Temp\File.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData(@"C:\Temp\File
.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData("C:\\Temp\\File\t.txt", typeof(InvalidWindowsFilePathException))]
        [InlineData("C:\\Temp\\File\0.txt", typeof(InvalidWindowsFilePathException))]
        public void From_WrongInput_ShouldThrowException(string input, Type expectedException)
        {
            // act
            var result = Record.Exception(() => WindowsFilePath.From(input));
            
            // assert
            Assert.Equal(expectedException, result?.GetType());
        }
        
        #endregion
        
        #region TryFrom
        
        [Theory]
        [InlineData(@"C:\File.txt")]
        [InlineData(@"C:\Temp\File.txt")]
        [InlineData(@"C:\Temp\Subfolder\File.txt")]
        [InlineData(@"D:\Data\Test.pdf")]
        [InlineData(@"Z:\")]
        [InlineData(@"Temp\File.txt")]
        [InlineData(@"Subfolder\File.txt")]
        [InlineData(@".\File.txt")]
        [InlineData(@".\Temp\File.txt")]
        [InlineData(@"..\File.txt")]
        [InlineData(@"..\Temp\File.txt")]
        [InlineData(@"..\..\Temp\File.txt")]
        [InlineData(@"\\Server\Share\File.txt")]
        [InlineData(@"\\Server\Share\Folder\File.txt")]
        [InlineData(@"C:Temp\File.txt")]
        [InlineData(@"My Folder\File Name.txt")]
        [InlineData(@"My_Folder\File_Name.txt")]
        [InlineData(@"My-Folder\File-Name.txt")]
        [InlineData(@"123\456\789.txt")]
        [InlineData(@"C:\123\456\File123.txt")]
        [InlineData("C:File.txt")]
        [InlineData("file.txt")]
        [InlineData("README")]
        [InlineData("archive.tar.gz")]
        [InlineData("C:/Temp/File.txt")]
        [InlineData("Temp/File.txt")]
        public void TryFrom_ValidInput_ShouldReturnOK(string input)
        {
            // act
            var result = WindowsFilePath.TryFrom(input, out _);
            
            // assert
            Assert.Equal(WindowsFilePath.Validation.Ok, result);
        }
        
[Theory]
        [InlineData(null, WindowsFilePath.Validation.Null)]
        [InlineData("", WindowsFilePath.Validation.Empty)]
        [InlineData(" ", WindowsFilePath.Validation.WhiteSpaceOnly)]
        [InlineData("   ", WindowsFilePath.Validation.WhiteSpaceOnly)]
        [InlineData(@"C:\Temp\File<1>.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData(@"C:\Temp\File>1.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData(@"C:\Temp\File:1.txt", WindowsFilePath.Validation.ContainsMultipleColons)]
        [InlineData(@"C:\Temp\File""1.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData(@"C:\Temp\File|1.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData(@"C:\Temp\File?1.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData(@"C:\Temp\File*1.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData(@"Temp\File<.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData(@"Temp\File>.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData(@"Temp\File:.txt", WindowsFilePath.Validation.WrongColonPlacement)]
        [InlineData(@"Temp\File"".txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData(@"Temp\File|.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData(@"Temp\File?.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData(@"Temp\File*.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData(@"C::\Temp\File.txt", WindowsFilePath.Validation.ContainsMultipleColons)]
        [InlineData(@"::\Temp\File.txt", WindowsFilePath.Validation.ContainsMultipleColons)]
        [InlineData(@"Temp:File.txt", WindowsFilePath.Validation.WrongColonPlacement)]
        [InlineData(@"Folder:Name\File.txt", WindowsFilePath.Validation.WrongColonPlacement)]
        [InlineData(@"C:\Temp\CON", WindowsFilePath.Validation.ContainsReservedName)]
        [InlineData(@"C:\Temp\con", WindowsFilePath.Validation.ContainsReservedName)]
        [InlineData(@"C:\Temp\CON.txt", WindowsFilePath.Validation.ContainsReservedName)]
        [InlineData(@"C:\Temp\PRN", WindowsFilePath.Validation.ContainsReservedName)]
        [InlineData(@"C:\Temp\PrN", WindowsFilePath.Validation.ContainsReservedName)]
        [InlineData(@"C:\Temp\PRN.log", WindowsFilePath.Validation.ContainsReservedName)]
        [InlineData(@"C:\Temp\AUX", WindowsFilePath.Validation.ContainsReservedName)]
        [InlineData(@"C:\Temp\NUL", WindowsFilePath.Validation.ContainsReservedName)]
        [InlineData(@"C:\Temp\COM1", WindowsFilePath.Validation.ContainsReservedName)]
        [InlineData(@"C:\Temp\COM9.txt", WindowsFilePath.Validation.ContainsReservedName)]
        [InlineData(@"C:\Temp\LPT1", WindowsFilePath.Validation.ContainsReservedName)]
        [InlineData(@"C:\Temp\LPT9.txt", WindowsFilePath.Validation.ContainsReservedName)]
        [InlineData(@"C:\Temp\Folder.", WindowsFilePath.Validation.InvalidSegmentEnding)]
        [InlineData(@"C:\Temp\File.txt.", WindowsFilePath.Validation.InvalidSegmentEnding)]
        [InlineData(@"C:\Temp\Folder ", WindowsFilePath.Validation.InvalidSegmentEnding)]
        [InlineData(@"C:\Temp\File.txt ", WindowsFilePath.Validation.InvalidSegmentEnding)]
        [InlineData(@"C:\Temp\\File.txt", WindowsFilePath.Validation.ContainsMultipleBackslashes)]
        [InlineData(@"C:\\Temp\File.txt", WindowsFilePath.Validation.ContainsMultipleBackslashes)]
        [InlineData(@"C:\Temp\File
.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData("C:\\Temp\\File\t.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        [InlineData("C:\\Temp\\File\0.txt", WindowsFilePath.Validation.ContainsIllegalCharacter)]
        public void TryFrom_WrongInput_ShouldReturnError(string input, WindowsFilePath.Validation expected)
        {
            // act
            var result = WindowsFilePath.TryFrom(input, out _);
            
            // assert
            Assert.Equal(expected, result);
        }
        
        #endregion

        #region Parse

        [Theory]
        [InlineData(@"C:\\Windows\\explorer.exe", @"C:\Windows\explorer.exe")]
        [InlineData(@"C:/Windows/explorer.exe", @"C:\Windows\explorer.exe")]
        [InlineData(@" C:\Windows\explorer.exe", @"C:\Windows\explorer.exe")]
        public void Parse_ValidInput_ShouldReturnObject(string input, string expected)
        {
            // act
            var result = WindowsFilePath.Parse(input);
            
            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Parse_NullInput_ShouldThrowException()
        {
            // assert
            Assert.Throws<ArgumentNullException>(() => WindowsFilePath.Parse(null));
        }
        
        #endregion
        
        #region Parse

        [Theory]
        [InlineData(@"C:\\Windows\\explorer.exe", @"C:\Windows\explorer.exe")]
        [InlineData(@"C:/Windows/explorer.exe", @"C:\Windows\explorer.exe")]
        [InlineData(@" C:\Windows\explorer.exe", @"C:\Windows\explorer.exe")]
        public void TryParse_ValidInput_ShouldReturnTrue(string input, string expected)
        {
            // act
            var result = WindowsFilePath.TryParse(input, out var path);
            
            // assert
            Assert.True(result);
            Assert.Equal(expected, path);
        }

        [Fact]
        public void TryParse_NullInput_ShouldReturnFalse()
        {
            // act
            var result = WindowsFilePath.TryParse(null, out _);
            
            // assert
            Assert.False(result);
        }
        
        #endregion
    }
}
