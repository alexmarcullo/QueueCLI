using QueueCLI.Broker.Abstractions;
using QueueCLI.Broker.RabbitMQ.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace QueueCLI.Broker.RabbitMQ
{
    public class QueueService : IQueueService
    {
        private readonly RabbitMQOptions _options;
        private readonly ConnectionFactory _factory;

        private IConnection _connection { get; set; }
        private IModel _requestChannel { get; set; }
        private IModel _publishChannel { get; set; }

        public QueueService(RabbitMQOptions options)
        {
            _options = options;

            var protocol = "amqp";

            var sslOption = new SslOption();
            if (_options.EnableSsl)
            {
                sslOption.Enabled = true;
                sslOption.ServerName = _options.Host;
                sslOption.AcceptablePolicyErrors |= System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch;

                protocol = "amqps";
            }

            _factory = new ConnectionFactory()
            {
                Ssl = sslOption,
                TopologyRecoveryEnabled = false,
                AutomaticRecoveryEnabled = true,
                Uri = new Uri($"{protocol}://{_options.Username}:{_options.Password}@{_options.Host}:{_options.Port}"),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
            };
            _factory.Ssl.CertificateValidationCallback 
                += (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

            _connection = _factory.CreateConnection();
            _requestChannel = _connection.CreateModel();
        }

        public void Acknowledge(QueueMessage message)
        {
            _requestChannel.BasicAck((ulong)message.Id, true);
        }

        public void Bind(string queue, string[] exchanges, string[] routeKeys)
        {
            if (routeKeys != null)
            {
                foreach (var exchange in exchanges)
                {
                    foreach (var key in routeKeys)
                    {
                        try
                        {
                            _requestChannel.QueueBind(queue, exchange, key);
                        }
                        catch (OperationInterruptedException ex)
                        {
                            throw new Exception(ex.ShutdownReason.ReplyText);
                        }

                    }
                }
            }
        }

        public void Clean(string name)
        {
            throw new NotImplementedException();
        }

        public void Create(string queue, bool durable, bool exclusive, bool autoDelete, CreateQueueSettings settings)
        {
            var arguments = new Dictionary<string, object>();

            if (settings.TTL != TimeSpan.Zero)
                arguments.Add("x-message-ttl", (int)settings.TTL.TotalMilliseconds);

            if (settings.AutoExpire != TimeSpan.Zero)
                arguments.Add("x-expires", (int)settings.AutoExpire.TotalMilliseconds);

            if (settings.MaxLength > 0)
                arguments.Add("x-max-length", settings.MaxLength);

            if (settings.MaxSizeBytes > 0)
                arguments.Add("x-max-length-bytes", settings.MaxSizeBytes);

            if (settings.MaxPriority > 0)
                arguments.Add("x-max-priority", settings.MaxPriority);

            if (!string.IsNullOrEmpty(settings.DeadLetterExchange))
                arguments.Add("x-dead-letter-exchange", settings.DeadLetterExchange);

            if (!string.IsNullOrEmpty(settings.DeadLetterRoutingKey))
                arguments.Add("x-dead-letter-routing-key", settings.DeadLetterRoutingKey);

            _requestChannel.QueueDeclare(queue, durable, false, autoDelete, arguments);
        }

        public ICollection<QueueMessage> Get(string queueName, QueueServiceGetOptions options = null)
        {
            var list = new List<QueueMessage>();
            while (true)
            {
                var message = _requestChannel.BasicGet(queueName, false);
                if (message != null)
                    list.Add(new QueueMessage(message.DeliveryTag, Encoding.UTF8.GetString(message.Body)));
                else
                    break;
            }
            return list;
        }

        public void Publish(string message, QueueServicePublishingOptions options = null)
        {
            using (var publishChannel = _connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(message);
                publishChannel.BasicPublish(exchange: options.ExchangeName, routingKey: options.RoutingKey, basicProperties: null, body: body);
            }
        }

        public void Purge(string queue)
        {
            _requestChannel.QueuePurge(queue);
        }

        public void Unbind(string queue, string[] exchanges, string[] routeKeys)
        {
            if (routeKeys != null)
            {
                foreach (var exchange in exchanges)
                {
                    foreach (var key in routeKeys)
                    {
                        try
                        {
                            _requestChannel.QueueUnbind(queue, exchange, key);
                        }
                        catch (OperationInterruptedException ex)
                        {
                            throw new Exception(ex.ShutdownReason.ReplyText);
                        }

                    }
                }
            }
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();

            GC.SuppressFinalize(this);
        }

        public void Remove(string queue)
        {
            _requestChannel.QueueDelete(queue);
        }
    }
}
