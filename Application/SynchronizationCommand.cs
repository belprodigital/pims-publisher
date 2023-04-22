using Hangfire;
using MediatR;
using PimsPublisher.Application.Adapters;
using PimsPublisher.Application.Cqs;
using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Application
{
    public class CreateSynchronizationSessionCommand : ICommand<Guid>
    {
        public DateTime StartTime { get; set; } = DateTime.Now;
        public string ProjectCode { get; set; } = string.Empty;
        public string ModelCode { get; set; } = string.Empty;

        private CreateSynchronizationSessionCommand(string projectCode, string modelCode)
        {
            StartTime = DateTime.Now;
            ProjectCode = projectCode;
            ModelCode = modelCode;
        }
        public static CreateSynchronizationSessionCommand For(string projectCode, string modelCode)
        {
             return new CreateSynchronizationSessionCommand(projectCode, modelCode);
        }
    }

    internal class CreateSynchronizationSessionCommandHandler : IRequestHandler<CreateSynchronizationSessionCommand, Guid>
    {
        private readonly ISynchronizationRepository _synchronizationRepository;
        private readonly IPimsAttributesDataService _pimsAttributesDataService;

        public CreateSynchronizationSessionCommandHandler(ISynchronizationRepository synchronizationSessionRepository, IPimsAttributesDataService pimsAttributesDataService)
        {
            _synchronizationRepository = synchronizationSessionRepository;
            _pimsAttributesDataService = pimsAttributesDataService;
        }
        public async Task<Guid> Handle(CreateSynchronizationSessionCommand cmd, CancellationToken cancellationToken)
        {
            int totalItems = await _pimsAttributesDataService.GetTotalSynchronizationItem(cmd.StartTime, cmd.ProjectCode, cmd.ModelCode);
            
            SynchronizationAggregate synchronization = SynchronizationAggregate.InitNewSynchronizationSession(cmd.ProjectCode, cmd.ModelCode, cmd.StartTime, totalItems);

            Guid syncId = _synchronizationRepository.Add(synchronization);

            await _synchronizationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            //foreach (var batch in synchronization.Batches)
            //{
            //    batch.JobId = BackgroundJob.Enqueue<SynchronizationService>(service => service.PostSynchronizationBatch(batch, cancellationToken));
            //}

            return syncId;
        }
    }
}
