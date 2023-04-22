namespace PimsPublisher.Infrastructure.MessageLog
{
    public enum MessageStates
    {
        NotPublished = 0,
        InProgress = 1,
        Published = 2,
        PublishedFailed = 3
    }
}
