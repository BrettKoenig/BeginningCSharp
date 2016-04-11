using System;
using System.IO;
using BadExample.Service.Interfaces;
using BadExample.Service.Services;
using NSubstitute;
using Xunit;

namespace BadExample.Test.Services
{
    public class FileWrapperServiceTest
    {
        private IFileWrapperService _target;
        private readonly IInventoryProcessor _mockInventoryProcessor;
        private readonly string _filePath;
        private readonly string _directoryPath;

        public FileWrapperServiceTest()
        {
            _mockInventoryProcessor = Substitute.For<IInventoryProcessor>();
            var basePath = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            _filePath = basePath + @"\FileCreateTest\test.txt";
            _directoryPath = basePath + @"\FileCreateTest";
        }
        private bool Setup()
        {
            try
            {
                Directory.CreateDirectory(_directoryPath);
                var file = File.Create(_filePath);
                file.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void Teardown()
        {
            try
            {
                File.Delete(_filePath);
                Directory.Delete(_directoryPath);
            }
            catch (Exception)
            {
                // ignored
            }
        }
        [Fact]
        public void DeleteFile_Success()
        {
            //Arrange
            var shouldRun = Setup();
            if (shouldRun == false)
                return;

            var fileWrapperService = new FileWrapperService(_mockInventoryProcessor);

            //Act
            var result = fileWrapperService.DeleteLocalFile(_filePath);

            //Assert
            Teardown();
            Assert.True(result);
        }
        //public FileWrapperService(IInventoryProcessor inventoryProcessor)
        //{
        //    _inventoryProcessor = inventoryProcessor;
        //}
    }
}
