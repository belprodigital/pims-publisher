using Microsoft.Identity.Abstractions;
using PimsPublisher.Domains.Entities;
using PimsPublisher.Application.Adapters;
using PimsPublisher.Infrastructure.K3dClients.DataContract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;

namespace PimsPublisher.Infrastructure.K3dClients
{
    public class SynchronizationApi : ISynchronizationApi
    {

        private readonly IDownstreamApi api;
        private readonly ILogger<SynchronizationApi> _logger;
        public SynchronizationApi(IDownstreamApi downstreamApi, ILoggerFactory loggerFactory)
        {
            api = downstreamApi ?? throw new ArgumentNullException(nameof(downstreamApi));
            _logger = loggerFactory?.CreateLogger< SynchronizationApi>() ?? throw new ArgumentNullException( nameof(loggerFactory));
        }


        public async Task<Domains.Entities.SynchronizationStatus> PostASynchronizationBatch(SynchronizationBatchEntity batch, CancellationToken cancellationToken)
        {
            var batchModel = batch.ToContractModel();

            _logger.LogTrace("---POSTING {0}", JsonConvert.SerializeObject(batchModel));

            Func<object?, HttpContent?> modelSerializer = (object? input) =>
            {
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = System.Text.Json.JsonSerializer.Serialize(input, options);
                return new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            };

            var status = await api.PostForAppAsync<SynchSessionBatching, DataContract.SynchronizationStatus>("K3dApim", batchModel, overrideOption =>
            {
                overrideOption.RelativePath = $"api/core/synchronization/projects/{batch.ProjectCode}/models/{batch.ModelCode}/session/{batch.SyncId}";
                overrideOption.Serializer = modelSerializer;

            });

            return status?.ToSynchronizationStatusEntity() ?? new Domains.Entities.SynchronizationStatus();
        }
    }
}
