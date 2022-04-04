using Serilog.Configuration;
using Serilog.Sink.Http.Sink;
using System;

namespace Serilog.Sink.Http
{
    public static class LoggerConfigurationHttpExtensions
    {
        public static LoggerConfiguration Http(this LoggerSinkConfiguration loggerConfiguration,
                                               Uri uri,
                                               IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException(nameof(loggerConfiguration));
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            return loggerConfiguration.Sink(
                new HttpSink(uri, formatProvider));
        }

        public static LoggerConfiguration Http(this LoggerSinkConfiguration loggerConfiguration,
                                               string url,
                                               IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException(nameof(loggerConfiguration));
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException(nameof(url));

            return loggerConfiguration.Sink(
                new HttpSink(url, formatProvider));
        }
    }
}
