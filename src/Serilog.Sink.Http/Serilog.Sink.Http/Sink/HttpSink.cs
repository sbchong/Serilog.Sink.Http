using Serilog.Core;
using Serilog.Events;
using Serilog.Sink.Http.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Serilog.Sink.Http.Sink
{
    public class HttpSink : ILogEventSink
    {
        private readonly HttpClient _httpClient;
        private readonly Uri _uri;
        private readonly IFormatProvider _formatProvider;

        public HttpSink()
        {
            _httpClient = new HttpClient();
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


        public async void Emit(LogEvent logEvent)
        {
            var message = logEvent.RenderMessage(_formatProvider);
            //Console.WriteLine(DateTimeOffset.Now.ToString() + " " + message);
            var model = new HttpLogEvent(logEvent.Level, message);
            var buffer = JsonSerializer.SerializeToUtf8Bytes(model);
            var content = new ByteArrayContent(buffer);
            var request = new HttpRequestMessage(HttpMethod.Post, _uri);
            request.Content = content;
            request.Headers.Add("Content-Type", "application/json");
            await _httpClient.SendAsync(request);
        }
    }
}
