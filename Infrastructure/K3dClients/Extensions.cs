using PimsPublisher.Infrastructure.HttpClients;

namespace PimsPublisher.Infrastructure.K3dClients
{
    internal static class Extensions
    {
        public static HttpRequestMessage WithSubcription(this HttpRequestMessage requestMessage, Subcription subcription)
         => requestMessage
            .WithHeader("k3d-client-id", subcription.ClientId)
            .WithHeader("k3d-client-secret", subcription.ClientSecret);
        
    }
}
