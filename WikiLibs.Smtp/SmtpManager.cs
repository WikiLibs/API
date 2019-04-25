using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules;

namespace WikiLibs.Smtp
{
    [Module(Interface = typeof(ISmtpManager))]
    public class SmtpManager : ISmtpManager
    {
        private readonly Config _config;

        public SmtpManager(Config cfg)
        {
            _config = cfg;
        }

        public void SendEmailMessage(EmailMessage msg)
        {
            SmtpClient client = new SmtpClient(_config.Host + _config.Port);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_config.Username, _config.Password);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_config.From);
            mailMessage.To.Add(msg.To);
            mailMessage.Body = msg.Body;
            mailMessage.Subject = "WikiLibs - " + msg.Subject;
            client.Send(mailMessage);
        }
    }
}
