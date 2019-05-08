using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WikiLibs.Smtp;

namespace WikiLibs.API.Tests
{
    [TestFixture]
    public class SmtpTests
    {
        private SmtpManager _smtpManager;

        [SetUp]
        public void Setup()
        {
            _smtpManager = new SmtpManager(new Config()
            {
                Host = "smtp.mailtrap.io",
                Port = 587,
                FromName = "WikiLibs",
                FromEmail = "8413e528f1-4eac74@inbox.mailtrap.io",
                Username = "3c1b959eae2f25",
                Password = "cdf97d3fa8f732"
            });
        }

        [Test]
        public void SendMail()
        {
            _smtpManager.SendEmailMessage(new Shared.Modules.EmailMessage()
            {
                To = "nicolas1.fraysse@epitech.eu",
                Subject = "TestMessage UTC:" + DateTime.UtcNow.ToString(),
                Body = "<h1>This is a test message</h1>"
                    + "<br />"
                    + "<p>Smtp module version " + typeof(SmtpManager).Assembly.GetName().Version.ToString() + "<p>"
                    + "<br />"
                    + "<p>The WikiLibs Team</p>"
            });
        }
    }
}
