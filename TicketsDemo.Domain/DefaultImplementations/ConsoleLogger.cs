using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.Domain.Interfaces;

namespace TicketsDemo.Domain.DefaultImplementations
{
    public class ConsoleLogger:ILogger
    {
        public void Log(string message, LogSeverity severity)
        {

            var wrtStr = String.Format(" {0}[{1}]: {2}", severity, DateTime.Now, message);
            System.Diagnostics.Debug.WriteLine(wrtStr);
        }
    }
}
