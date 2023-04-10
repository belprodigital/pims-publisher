using PimsPublisher.Infrastructure.HttpClients;
using PimsPublisher.Infrastructure.K3dClients.DataContract;
using System.Net.Http.Json;

namespace PimsPublisher.Infrastructure.K3dClients
{
    public class Subscription
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public Subscription(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
    public class ClientOptions
    {
        public string HostOrigin { get; set; } 
        public string ServicePath { get; set; } 

        public Subscription Subscription { get; set; }

        public ClientOptions(string hostOrigin, string servicePath)
        {
            HostOrigin = hostOrigin;
            ServicePath = servicePath;
        }
    }
    public class K3dSynchronizationClient
    {
        private ClientOptions clientOptions;
        public K3dSynchronizationClient(ClientOptions options) 
        {
            if(options == null) throw new ArgumentNullException("options"); 

            clientOptions = options;
        }

        private RestClient CreateRestClient(HttpMethod method, string endpoint) 
        {
            RestClient client = RestClient.For(clientOptions.HostOrigin, clientOptions.ServicePath);

            client
                .MakeRequest(method, string.Empty, endpoint)
                .WithSubscription(clientOptions.Subscription);

            return client;
        }

        public async Task<SynchSessionStatus?> PostSingleSessionBatch(SynchSessionBatching synchSessionBatching, CancellationToken cancellationToken)
        {
            RestClient client = CreateRestClient(HttpMethod.Post, "projects");

            client.Request.WithJsonContent(synchSessionBatching.ToJSON());

            HttpResponseMessage response = await client.SendAsync(cancellationToken);


            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SynchSessionStatus>(JsonConverterExtension.jsonSerializerOptions);

            }
            else
            {
                string message = $"RestClientException: HTTP_CODE → {response.StatusCode}, {Environment.NewLine} URL → {client.Request.RequestUri}";
                throw new RestClientException(message);
            }
        }

    }
}
