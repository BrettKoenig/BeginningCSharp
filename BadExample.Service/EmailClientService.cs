using System.Collections.Generic;
using System.Net.Mail;
using S22.Imap;

namespace BadExample.Service
{
    public class EmailClientService : IEmailClientService
    {
        private readonly ImapClient _client;
        public EmailClientService(string username, string password)
        {
            _client = new ImapClient("imap.gmail.com", 993, true);
            _client.Login(username, password, AuthMethod.Auto);
        }
        public IEnumerable<uint> GetEmailsFromSender(string fromEmail)
        {
            return _client.Search(SearchCondition.From(fromEmail));
        }

        public AttachmentCollection GetAttachmentsByEmail(uint email)
        {
            return _client.GetMessage(email).Attachments;
        }
    }
}
