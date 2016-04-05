using System;
using System.Collections.Generic;
using System.Net.Mail;
using BadExample.Service.Interfaces;
using S22.Imap;

namespace BadExample.Service.Services
{
    public class EmailClientService : IEmailClientService
    {
        private readonly ImapClient _client;
        public EmailClientService()
        {
            _client = new ImapClient("imap.gmail.com", 993, true);
        }

        public bool Login(string username, string password)
        {
            try
            {
                _client.Login(username, password, AuthMethod.Auto);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return false;
            }
            return true;
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
