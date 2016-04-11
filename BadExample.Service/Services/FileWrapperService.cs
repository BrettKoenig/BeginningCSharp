using System;
using System.IO;
using System.Net.Mail;
using BadExample.Service.Interfaces;

namespace BadExample.Service.Services
{
    public class FileWrapperService : IFileWrapperService
    {
        private readonly IInventoryProcessor _inventoryProcessor;
        public FileWrapperService(IInventoryProcessor inventoryProcessor)
        {
            _inventoryProcessor = inventoryProcessor;
        }
        public bool DeleteLocalFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                //could log something here
            }
            return false;
        }

        public FileStream CreateLocalFile(Attachment emailAttachment, string tempFileLocation)
        {
            var localFile = File.Create(tempFileLocation + emailAttachment.Name);
            emailAttachment.ContentStream.Seek(0, SeekOrigin.Begin);
            emailAttachment.ContentStream.CopyTo(localFile);
            localFile.Close();
            return localFile;
        }

        public void ProcessLinesInFile(FileStream fileToReadFrom)
        {
            string line;
            StreamReader fileStream = new StreamReader(fileToReadFrom);
            while ((line = fileStream.ReadLine()) != null)
            {
                _inventoryProcessor.ProcessLineItem(line);
            }
            fileStream.Close();
        }
    }
}
