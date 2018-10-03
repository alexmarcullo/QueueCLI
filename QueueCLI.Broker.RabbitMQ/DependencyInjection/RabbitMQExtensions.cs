using Autofac;
using QueueCLI.Broker.Abstractions;
using System;

namespace QueueCLI.Broker.RabbitMQ.DependencyInjection
{
    public static class RabbitMQExtensions
    {
        public static void AddRabbitMQ(this ContainerBuilder builder, Action<RabbitMQOptions> configurations)
        {
            var options = new RabbitMQOptions();
            configurations.Invoke(options);

            builder.Register(c =>
            {
                return new QueueService(options);
            }).As<IQueueService>();
        }
    }
}
