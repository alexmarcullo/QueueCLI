namespace QueueCLI.Broker.Abstractions
{
    public class QueueMessage
    {
        public QueueMessage(object id, string payload)
        {
            Id = id;
            Payload = payload;
        }

        public object Id { get; private set; }
        public string Payload { get; private set; }
    }
}
