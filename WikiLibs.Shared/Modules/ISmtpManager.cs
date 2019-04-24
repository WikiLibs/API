using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiLibs.Shared.Modules
{
    public struct EmailMessage
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
    }

    public interface ISmtpManager : IModule
    {
        void SendEmailMessage(EmailMessage msg);
    }
}
