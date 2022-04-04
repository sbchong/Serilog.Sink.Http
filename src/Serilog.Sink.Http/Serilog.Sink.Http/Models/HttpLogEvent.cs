using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog.Sink.Http.Models
{
    public class HttpLogEvent
    {
        public LogEventLevel Level { get; set; }
        public string Message { get; set; }
        public DateTime ReleaseTime { get; set; }

        public HttpLogEvent(LogEventLevel level,string message)
        {
            Level = level;
            Message=message;
            ReleaseTime = DateTime.Now;
        }
    }
}
