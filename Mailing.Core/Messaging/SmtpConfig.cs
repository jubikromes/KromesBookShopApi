using System;
using System.Collections.Generic;
using System.Text;

namespace Mailing.Core.Messaging
{
    public class SmtpConfig
    {
        public bool EnableSSl { get; set; }
        public int Port { get; set; }
        public string Server { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Sender { get; set; }
        public bool UseDefaultCredentials { get; set; }
    }
}
