using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PimsPublisher.Infrastructure.HttpClients
{
    internal class RestClient
    {
        private HttpClient _httpClient;
        private HttpRequestMessage _httpRequest = null;

        public HttpRequestMessage Request { get { return _httpRequest; } }

        public string ServicePath { get; private set; } 

        private RestClient(string host, string servicePath)
        {
            ServicePath = servicePath;

            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(host)
            };

            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
           
        }

        internal static RestClient For(string host, string servicePath)
        {
            if (IsValidHost(host))
                return new RestClient(host, servicePath);
            else
                throw new ArgumentException("INVALID_HOST_NAME", "host");

        }

        internal HttpRequestMessage MakeRequest(HttpMethod method, string apiVersion, string endpoint)
        {
            StringBuilder relativeUrlBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(ServicePath))
            {
                relativeUrlBuilder.Append($"/{ServicePath}");
            }

            if (!string.IsNullOrEmpty(apiVersion))
            {
                relativeUrlBuilder.Append($"/{apiVersion}");
            }

            relativeUrlBuilder.Append($"/{endpoint}");

            _httpRequest = new HttpRequestMessage(method, relativeUrlBuilder.ToString());

            return _httpRequest;
        }

        internal async Task<HttpResponseMessage> SendAsync(CancellationToken cancellationToken)
        {
            if(_httpClient == null)
            {
                throw new MissingMemberException("HttpClient -> _httpClient is not created yet");
            }

            if(_httpRequest == null)
            {
                throw new MissingMemberException("HttpRequestMessage -> _httpRequest is not created yet");
            }

            return await _httpClient.SendAsync(_httpRequest, cancellationToken);
        }

        private static bool IsValidHost(string host) => Uri.IsWellFormedUriString(host, UriKind.Absolute);
        
    }
}
