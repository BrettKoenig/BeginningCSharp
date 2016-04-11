using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using BadExample.Service.Interfaces;

namespace BadExample.Service
{
    public class EmailAttachmentProcessor : IEmailAttachmentProcessor
    {
        private readonly IEmailClientService _emailClientService;
        private readonly IFileWrapperService _fileWrapperService;
        private readonly IAmazonWebService _amazonWebService;

        public EmailAttachmentProcessor(IEmailClientService emailClientService, IFileWrapperService fileWrapperService, IAmazonWebService amazonWebService)
        {
            _emailClientService = emailClientService;
            _emailClientService.Login(ConfigurationManager.AppSettings["EmailUsername"], ConfigurationManager.AppSettings["EmailPassword"]);
            _fileWrapperService = fileWrapperService;
            _amazonWebService = amazonWebService;
        }
        [ExcludeFromCodeCoverage]
        public void ProcessEmailAttachments()
        {
            //Get Emails from Gmail
            var emailsFromSender = _emailClientService.GetEmailsFromSender(ConfigurationManager.AppSettings["SearchFromEmail"]);

            foreach (var email in emailsFromSender)
            {
                //Get attachments on email
                var attachmentsOnEmail = _emailClientService.GetAttachmentsByEmail(email);

                foreach (var attachment in attachmentsOnEmail)
                {
                    var localFile = _fileWrapperService.CreateLocalFile(attachment, ConfigurationManager.AppSettings["TempFileLocation"]);

                    //Read Info from files
                    _fileWrapperService.ProcessLinesInFile(localFile);

                    //Store file on s3
                    _amazonWebService.UploadFileToBucket(localFile.Name, localFile.Name, ConfigurationManager.AppSettings["AWSBucketName"]);

                    //Delete file locally
                    _fileWrapperService.DeleteLocalFile(localFile.Name);
                }
            }
        }
    }
}