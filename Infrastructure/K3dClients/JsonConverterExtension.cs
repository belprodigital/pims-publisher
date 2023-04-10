using System.Text.Json;
using PimsPublisher.Infrastructure.K3dClients.DataContract;

namespace PimsPublisher.Infrastructure.K3dClients
{
    internal static class JsonConverterExtension
    {
        internal static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true
        };
        internal static string ToJSON(this SynchSessionBatching synchSessionBatching)
             => JsonSerializer.Serialize<SynchSessionBatching>(synchSessionBatching, jsonSerializerOptions);
    }
}
