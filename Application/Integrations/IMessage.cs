namespace PimsPublisher.Application.Integrations
{
    public interface IMessage
    {
        Guid Id { get; }
        Guid EntityId { get; }
        DateTime Occurred { get; }
    }
}
