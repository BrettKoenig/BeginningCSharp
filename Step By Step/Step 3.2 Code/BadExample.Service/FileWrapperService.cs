﻿using System.IO;
using System.Net.Mail;

namespace BadExample.Service
{
    public class FileWrapperService
    {
        private readonly string _connectionString;
        public FileWrapperService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void DeleteLocalFile(string filePath)
        {
            File.Delete(filePath);
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
            var inventoryProcessor = new InventoryProcessor(_connectionString);
            string line = string.Empty;
            StreamReader fileStream = new StreamReader(fileToReadFrom);
            while ((line = fileStream.ReadLine()) != null)
            {
                inventoryProcessor.ProcessLineItem(line);
            }
            fileStream.Close();
        }
    }
}
