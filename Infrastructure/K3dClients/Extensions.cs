using PimsPublisher.Infrastructure.HttpClients;

namespace PimsPublisher.Infrastructure.K3dClients
{
    internal static class Extensions
    {
        public static HttpRequestMessage WithSubscriptionKey(this HttpRequestMessage requestMessage, string apimSubscriptionKey)
           => requestMessage.WithHeader("k3d-apim-key", apimSubscriptionKey);

        public static async Task<HttpRequestMessage> AuthroizeWithClientCredentialsAsync(this HttpRequestMessage requestMessage, Oauth2ClientCredential clientCredential, CancellationToken cancellationToken)
        {
            string accessToken = await Oauth2Client.AppAccessToken(clientCredential, cancellationToken);
            requestMessage.WithHeader("Authorization", $"Bearer {accessToken}" );
            
            return requestMessage;

        }
    }
}
