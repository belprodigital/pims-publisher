using MediatR;
using PimsPublisher.Application.Cqs;
using PimsPublisher.Domains.Entities;

namespace PimsPublisher.Application
{
    public class CreateSynchronizationSessionCommand: ICommand<Guid>
    {
        public DateTime StartTime { get; set; } = DateTime.Now;
        public string ProjectCode {get;set;} = string.Empty;
        public string ModelCode {get;set;} = string.Empty;
    }

    internal class CreateSynchronizationSessionCommandHandler : IRequestHandler<CreateSynchronizationSessionCommand, Guid>
    {
        private readonly ISynchronizationSessionRepository _synchSessionRepository;
        private readonly IPimsAttributesDataService _pimsAttributesDataService;

        public CreateSynchronizationSessionCommandHandler(ISynchronizationSessionRepository synchronizationSessionRepository,IPimsAttributesDataService pimsAttributesDataService)
        {
            _synchSessionRepository = synchronizationSessionRepository;
            _pimsAttributesDataService = pimsAttributesDataService;
        }
        public async Task<Guid> Handle(CreateSynchronizationSessionCommand cmd, CancellationToken cancellationToken)
        {
            int totalItems = await _pimsAttributesDataService.GetTotalSynchronizationItem(cmd.StartTime, cmd.ProjectCode, cmd.ModelCode);
            
            SynchronizationSession session = SynchronizationSession.InitNewSynchronizationSession(cmd.ProjectCode, cmd.ModelCode, cmd.StartTime, totalItems);

            Guid sessionId = _synchSessionRepository.Add(session);

            await _synchSessionRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return sessionId;
        }
    }
}
