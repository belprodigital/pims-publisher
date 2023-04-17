using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;

namespace PimsPublisher.Infrastructure.HttpClients
{
    public static class HttpRequestMessageExtension
    {

        public static HttpRequestMessage WithHeader(this HttpRequestMessage httpRequest, string name, string value)
        {
            httpRequest.Headers.Add(name, value);
            return httpRequest;
        }

        private static HttpRequestMessage WithContent(this HttpRequestMessage httpRequest, string content, Encoding encoding, string mediaType)
        {
            httpRequest.Content = new StringContent(content, encoding, mediaType);

            return httpRequest;
        }

        public static HttpRequestMessage WithJsonContent(this HttpRequestMessage httpRequest, string jsonContent)
            => httpRequest.WithContent(jsonContent, Encoding.UTF8, HttpMediaType.Json);

        public static HttpRequestMessage WithApplicationFormContent(this HttpRequestMessage httpRequest, Dictionary<string,string> formParams)
        {
            httpRequest.Content = new FormUrlEncodedContent(formParams);

            return httpRequest;
        }
            

    }
}
