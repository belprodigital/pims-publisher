using PimsPublisher.Infrastructure.HttpClients;

namespace PimsPublisher.Infrastructure.K3dClients
{
    internal static class Extensions
    {
        public static HttpRequestMessage WithSubscription(this HttpRequestMessage requestMessage, Subscription subscription)
         => requestMessage
            .WithHeader("k3d-client-id", subscription.ClientId)
            .WithHeader("k3d-client-secret", subscription.ClientSecret);
        
    }
}
