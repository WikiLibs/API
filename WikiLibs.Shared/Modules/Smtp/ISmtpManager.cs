using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiLibs.Shared.Modules.Smtp
{
    public interface ISmtpManager : IModule
    {
        Task<bool> SendAsync(Mail msg);
    }
}
