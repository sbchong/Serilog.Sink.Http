using Serilog.Core;
using Serilog.Events;
using Serilog.Sink.Http.Models;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Serilog.Sink.Http.Sink
{
    public class HttpSink : ILogEventSink
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _uri;
        private readonly IFormatProvider _formatProvider;

        private readonly ConcurrentQueue<LogEvent> logEvents = new ConcurrentQueue<LogEvent>();

        public HttpSink()
        {
            _httpClient = new HttpClient();
            Task.Run(() => PostLog());
        }

        public HttpSink(Uri uri, IFormatProvider formatProvider) : this()
        {
            _uri = uri;
            _formatProvider = formatProvider;
        }
        public HttpSink(string url, IFormatProvider formatProvider) : this()
        {
            _uri = new Uri(url);
            _formatProvider = formatProvider;
        }


        public void Emit(LogEvent logEvent)
        {
            logEvents.Enqueue(logEvent);
        }

        private async void PostLog()
        {
            while (true)
            {
                if (logEvents.TryDequeue(out LogEvent logEvent))
                {
                    var message = logEvent.RenderMessage(_formatProvider);
                    var model = new HttpLogEvent(logEvent.Level, message);
                    var buffer = JsonSerializer.SerializeToUtf8Bytes(model);
                    var content = new ByteArrayContent(buffer);
                    var request = new HttpRequestMessage(HttpMethod.Post, _uri);
                    request.Content = content;
                    content.Headers.Add("Content-Type", "application/json");
                    await _httpClient.SendAsync(request);
                }
            }
        }
    }
}
