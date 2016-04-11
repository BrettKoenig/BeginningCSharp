using System.Collections.Generic;
using System.Net.Mail;

namespace BadExample.Service.Interfaces
{
    public interface IEmailClientService
    {
        IEnumerable<uint> GetEmailsFromSender(string fromEmail);
        AttachmentCollection GetAttachmentsByEmail(uint email);
        bool Login(string username, string password);
    }
}