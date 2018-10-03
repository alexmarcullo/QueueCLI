using Autofac;
using QueueCLI.Broker.Abstractions;
using System;

namespace QueueCLI.Broker.ActiveMQ.DependencyInjection
{
    public static class ActiveMQExtensions
    {
        public static void AddActiveMQ(this ContainerBuilder builder, Action<ActiveMQOptions> configurations)
        {
            var options = new ActiveMQOptions();
            configurations.Invoke(options);

            builder.Register(c =>
            {
                return new QueueService(options);
            }).As<IQueueService>();
        }
    }
}
