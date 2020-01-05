using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace WikiLibs.API.Tests.Helper
{
    static class LogUtils
    {
        public static ILogger<T> FakeLogger<T>()
        {
            ILogger<T> log = new Logger<T>(new NullLoggerFactory());
            return (log);
        }
    }
}
