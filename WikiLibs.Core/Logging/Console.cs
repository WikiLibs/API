using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WikiLibs.Core.Logging
{
    public class Console : AbstractLogger
    {
        private string _prevSection;

        public Console()
        {
        }

        public override void WriteMessage(LogMessage msg)
        {
            switch (msg.Type)
            {
                case LogMessage.EType.WARNING:
                    System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogMessage.EType.INFO:
                    System.Console.ResetColor();
                    break;
                case LogMessage.EType.ERROR:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogMessage.EType.DEBUG:
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
            }
            var str = "";
            if (msg.Section != null)
            {
                var prefix = "";
                for (int i = 1; i != msg.SectionLayer; ++i)
                    prefix += "    ";
                if (_prevSection != msg.Section)
                    System.Console.WriteLine(prefix + "=> " + msg.Section);
                str += prefix + "    ";
                _prevSection = msg.Section;
            }
            str += msg.Time + " [" + msg.Category + " | " + msg.Type.ToString() + "] " + msg.Message;
            System.Console.WriteLine(str);
        }
    }
}
