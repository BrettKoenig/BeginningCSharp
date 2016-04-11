using System.IO;
using System.Net.Mail;

namespace BadExample.Service.Interfaces
{
    public interface IFileWrapperService
    {
        bool DeleteLocalFile(string filePath);
        FileStream CreateLocalFile(Attachment emailAttachment, string tempFileLocation);
        void ProcessLinesInFile(FileStream fileToReadFrom);
    }
}