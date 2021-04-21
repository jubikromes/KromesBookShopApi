using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Mailing.Core.Messaging.Abstractions
{
    public abstract class MailBase
    {
        public bool BodyIsFile { get; set; }
        public string Body { get; set; }
        public string BodyPath { get; set; }
        public string Subject { get; set; }
        public string Sender { get; set; }
        public string SenderDisplayName { get; set; }
        public bool IsBodyHtml { get; set; }
        public ICollection<string> To { get; set; }
        public ICollection<string> Bcc { get; set; }
        public ICollection<string> CC { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
    }
}
