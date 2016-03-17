using System.Configuration;
using BadExample.Service;

namespace BadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var emailAttachmentProcessor = new EmailAttachmentProcessor(
                ConfigurationManager.AppSettings["EmailUsername"],
                ConfigurationManager.AppSettings["EmailPassword"],
                ConfigurationManager.AppSettings["SearchFromEmail"],
                ConfigurationManager.AppSettings["TempFileLocation"],
                ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString,
                ConfigurationManager.AppSettings["AWSAccessKey"],
                ConfigurationManager.AppSettings["AWSSecretKey"]
                );

            emailAttachmentProcessor.ProcessEmails();
        }
    }
}