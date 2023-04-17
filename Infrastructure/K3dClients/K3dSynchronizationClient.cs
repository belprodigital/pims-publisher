using PimsPublisher.Infrastructure.HttpClients;
using PimsPublisher.Infrastructure.K3dClients.DataContract;
using System.Net.Http.Json;

namespace PimsPublisher.Infrastructure.K3dClients
{
    public class ClientOptions
    {
        public string HostOrigin { get; set; } 
        public string ServicePath { get; set; } 

        public string ApimSubscriptionKey { get; set; }

        public Oauth2ClientCredential ClientCredential { get; set; }
    }
    public class K3dSynchronizationClient
    {
        private ClientOptions clientOptions;
        public K3dSynchronizationClient(ClientOptions options) 
        {
            if(options == null) throw new ArgumentNullException("options"); 

            clientOptions = options;
        }

        private async Task<RestClient> CreateRestClient(HttpMethod method, string endpoint) 
        {
            RestClient client = RestClient.For(clientOptions.HostOrigin, clientOptions.ServicePath);

           var _ = await client
                 .MakeRequest(method, string.Empty, endpoint)
                 .WithSubscriptionKey(clientOptions.ApimSubscriptionKey)
                 .AuthroizeWithClientCredentialsAsync(clientOptions.ClientCredential, CancellationToken.None);

            return client;
        }

        public async Task<SynchSessionStatus?> PostSingleSessionBatch(SynchSessionBatching synchSessionBatching, CancellationToken cancellationToken)
        {
            RestClient client = await CreateRestClient(HttpMethod.Post, "projects");

            _ = client.RequestWithJsonContent(synchSessionBatching.ToJSON());

            HttpResponseMessage response = await client.SendAsync(cancellationToken);


            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SynchSessionStatus>(JsonConverterExtension.jsonSerializerOptions);

            }
            else
            {
                string message = $"RestClientException: HTTP_CODE → {response.StatusCode}, {Environment.NewLine} URL → {client.Request?.RequestUri}";
                throw new RestClientException(message);
            }
        }

    }
}
