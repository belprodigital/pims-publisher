using PimsPublisher.Application.Integrations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PimsPublisher.Infrastructure.IntegrationMessages
{
    public class IntegrationMessageLogEntry
    {
        private IntegrationMessageLogEntry() { }
        public IntegrationMessageLogEntry(IMessage msg, Guid transactionId)
        {
            MessageId = msg.Id;
            OccurredTime = msg.Occurred;
            MessageTypeName = msg.GetType().FullName;
            MessageTypeShortName = msg.GetType().Name;
            Payload = JsonSerializer.Serialize(msg, msg.GetType());
            State = MessageStates.NotPublished;
            TimesSent = 0;
            TransactionId = transactionId.ToString();
            EntityId = msg.EntityId;
        }
        public Guid MessageId { get; private set; }
        public string MessageTypeName { get; private set; }
        public string MessageTypeShortName {get; private set;}
        public MessageStates State { get; set; }
        public int TimesSent { get; set; }
        public DateTime OccurredTime { get;  set; }
        public string Payload { get;  set; }
        public string TransactionId { get;  set; }
        public Guid EntityId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public IMessage IntegrationEvent {get; private set;}

        public  IntegrationMessageLogEntry DeserializePayload(Type eventType)
        {
            if(eventType==null)
                throw new System.ArgumentNullException(nameof(eventType));
            IntegrationEvent = JsonSerializer.Deserialize(this.Payload, eventType) as IMessage;

            return this;
        }
    }
}
