using System.IO;
using System.Net.Mail;

namespace BadExample.Service
{
    public interface IFileWrapperService
    {
        void DeleteLocalFile(string filePath);
        FileStream CreateLocalFile(Attachment emailAttachment, string tempFileLocation);
        void ProcessLinesInFile(FileStream fileToReadFrom);
    }
}