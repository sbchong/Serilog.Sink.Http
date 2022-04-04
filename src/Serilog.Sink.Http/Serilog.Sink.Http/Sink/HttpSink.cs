using Newtonsoft.Json;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sink.Http.Models;
using System;
using System.Net.Http;
using System.Text;

namespace Serilog.Sink.Http.Sink
{
    public class HttpSink : ILogEventSink
    {
        private readonly Uri _uri;
        private readonly IFormatProvider _formatProvider;

        public HttpSink(Uri uri, IFormatProvider formatProvider)
        {
            _uri = uri;
            _formatProvider = formatProvider;
        }
        public HttpSink(string url, IFormatProvider formatProvider)
        {
            _uri = new Uri(url);
            _formatProvider = formatProvider;
        }


        public async void Emit(LogEvent logEvent)
        {
            var message = logEvent.RenderMessage(_formatProvider);
            //Console.WriteLine(DateTimeOffset.Now.ToString() + " " + message);
            var req = new HttpLogEvent(logEvent.Level, message);
            var s = JsonConvert.SerializeObject(req);
            await new HttpClient().PostAsync(_uri, new StringContent(s, Encoding.UTF8, "application/json"));
        }
    }
}
