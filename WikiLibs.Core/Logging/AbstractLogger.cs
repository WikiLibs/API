using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WikiLibs.Core.Logging
{
    public struct LogMessage
    {
        public enum EType
        {
            ERROR,
            WARNING,
            DEBUG,
            INFO
        }

        public string Message { get; set; }
        public string Time { get; set; }
        public string Section { get; set; }
        public string Category { get; set; }
        public EType Type { get; set; }
        public int SectionLayer { get; set; }
    }

    public abstract class AbstractLogger : ILoggerProvider
    {
        private ConcurrentQueue<LogMessage> _messages = new ConcurrentQueue<LogMessage>();
        private CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        internal class Logger : ILogger
        {
            internal class AutoPop : IDisposable
            {
                private Stack<string> _stack;

                public AutoPop(Stack<string> st)
                {
                    _stack = st;
                }

                public void Dispose()
                {
                    _stack.Pop();
                }
            }

            private Stack<string> _scopes = new Stack<string>();
            private string _category;
            private AbstractLogger _provider;

            public Logger(string cat, AbstractLogger logger)
            {
                _category = cat;
                _provider = logger;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                _scopes.Push(state.ToString());
                return (new AutoPop(_scopes));
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return (logLevel != LogLevel.None && logLevel != LogLevel.Trace);
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var time = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz");
                var section = _scopes.Count == 0 ? null : _scopes.Peek();
                LogMessage.EType type;
                var msg = formatter(state, exception);

                if (exception != null)
                    msg += exception.ToString();
                switch (logLevel)
                {
                    case LogLevel.Information:
                        type = LogMessage.EType.INFO;
                        break;
                    case LogLevel.Warning:
                        type = LogMessage.EType.WARNING;
                        break;
                    case LogLevel.Debug:
                        type = LogMessage.EType.DEBUG;
                        break;
                    default:
                        type = LogMessage.EType.ERROR;
                        break;
                }
                _provider.AddMessage(new LogMessage()
                {
                    Category = _category,
                    Message = msg,
                    Section = section,
                    Time = time,
                    Type = type,
                    SectionLayer = _scopes.Count
                });
            }
        }

        public AbstractLogger()
        {
            Task.Factory.StartNew(ProcessLogs, TaskContinuationOptions.LongRunning);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return (new Logger(categoryName, this));
        }

        public void Dispose()
        {
            _cancellationToken.Cancel();
        }

        private async Task ProcessLogs(object _)
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                LogMessage msg;
                while (_messages.TryDequeue(out msg))
                    WriteMessage(msg);
                await Task.Delay(500);
            }
        }

        protected void AddMessage(LogMessage msg)
        {
            _messages.Enqueue(msg);
        }

        public abstract void WriteMessage(LogMessage msg);
    }
}
