using System.Collections.Generic;
using System.Net.Mail;

namespace BadExample.Service
{
    public interface IEmailClientService
    {
        IEnumerable<uint> GetEmailsFromSender(string fromEmail);
        AttachmentCollection GetAttachmentsByEmail(uint email);
    }
}