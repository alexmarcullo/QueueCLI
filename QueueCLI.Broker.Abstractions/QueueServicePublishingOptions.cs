namespace QueueCLI.Broker.Abstractions
{
    public class QueueServicePublishingOptions
    {
        public string RoutingKey { get; set; }
        public string ExchangeName { get; set; }
    }
}
