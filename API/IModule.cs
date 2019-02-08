using Microsoft.Extensions.Configuration;
using System;

namespace API
{
    public interface IModule
    {
        void LoadConfig(IConfiguration cfg);
    }
}
