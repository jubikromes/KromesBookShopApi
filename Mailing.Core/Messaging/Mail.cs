using Mailing.Core.Messaging.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Mailing.Core.Messaging
{
    public sealed class Mail : MailBase
    {
        private Mail()
        {
            IsBodyHtml = true;
            To = new List<string>();
            CC = new List<string>();
            Bcc = new List<string>();
            Attachments = new List<Attachment>();
        }

        public Mail(string sender, string subject, params string[] to)
            : this()
        {
            Sender = sender;
            Subject = subject;

            foreach (var rec in to)
                To.Add(rec);
        }
    }
}
