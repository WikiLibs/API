using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiLibs.Shared.Modules.Smtp;

namespace WikiLibs.API.Tests.Helper
{
    public class TestingSmtp : ISmtpManager
    {
        public int SentEmailCount { get; private set; } = 0;
        public Mail LastSendEmail { get; private set; } = null;

        public Task SendAsync(Mail msg)
        {
            LastSendEmail = msg;
            ++SentEmailCount;
            return (Task.CompletedTask);
        }
    }
}
