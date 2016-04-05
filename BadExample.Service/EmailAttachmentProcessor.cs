using System;
using BadExample.Service.Interfaces;
using BadExample.Service.Services;

namespace BadExample.Service
{
    public class EmailAttachmentProcessor : IEmailAttachmentProcessor
    {
        private readonly string _searchFromEmail;
        private readonly string _tempFileLocation;
        private readonly string _awsBucketName;
        private readonly EmailClientService _emailClientService;
        private readonly FileWrapperService _fileWrapperService;
        private readonly AmazonWebService _amazonWebService;

        public EmailAttachmentProcessor(string emailUsername, string emailPassword, string searchFromEmail, string tempFileLocation, string connectionString, string awsBucketName, string awsAccessKey, string awsSecretKey)
        {
            _searchFromEmail = searchFromEmail;
            _tempFileLocation = tempFileLocation;
            _awsBucketName = awsBucketName;
            _emailClientService = new EmailClientService(emailUsername, emailPassword);
            _fileWrapperService = new FileWrapperService(connectionString);
            _amazonWebService = new AmazonWebService(awsAccessKey, awsSecretKey);
        }
        public void ProcessEmailAttachments()
        {
            //Get Emails from Gmail
            var emailsFromSender = _emailClientService.GetEmailsFromSender(_searchFromEmail);

            foreach (var email in emailsFromSender)
            {
                //Get attachments on email
                var attachmentsOnEmail = _emailClientService.GetAttachmentsByEmail(email);

                foreach (var attachment in attachmentsOnEmail)
                {
                    var localFile = _fileWrapperService.CreateLocalFile(attachment, _tempFileLocation);

                    //Read Info from files
                    _fileWrapperService.ProcessLinesInFile(localFile);

                    //Store file on s3
                    _amazonWebService.UploadFileToBucket(localFile.Name, localFile.Name, _awsBucketName);

                    //Delete file locally
                    _fileWrapperService.DeleteLocalFile(localFile.Name);
                }
            }
        }
    }
}