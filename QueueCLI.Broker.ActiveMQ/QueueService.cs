using Amqp;
using QueueCLI.Broker.Abstractions;
using QueueCLI.Broker.ActiveMQ.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace QueueCLI.Broker.ActiveMQ
{
    public class QueueService : IQueueService
    {
        private readonly ActiveMQOptions _options;
        private Session _senderSession;
        private Session _receiverSession;
        private Connection _connection;

        public QueueService(ActiveMQOptions options)
        {
            _options = options;

            Task.Run(async () =>
            {
                var protocol = "amqp";
                if (_options.EnableSsl)
                    protocol = "amqps";

                var address = new Address($"{protocol}://{_options.Username}:{_options.Password}@{_options.Host}:{_options.Port}");
                var connectionFactory = new ConnectionFactory();
                connectionFactory.SSL.RemoteCertificateValidationCallback 
                    += (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

                _connection = await connectionFactory.CreateAsync(address);
                _senderSession = new Session(_connection);
                _receiverSession = new Session(_connection);

            }).Wait();
        }

        public void Acknowledge(QueueMessage message)
        {
            throw new NotImplementedException();
        }

        public void Bind(string queue, string[] exchanges, string[] routeKeys)
        {
            throw new NotImplementedException();
        }

        public void Clean(string name)
        {
            throw new NotImplementedException();
        }

        public void Create(string queue, bool durable, bool exclusive, bool autoDelete, CreateQueueSettings settings)
        {
            throw new NotImplementedException();
        }

        public ICollection<QueueMessage> Get(string queueName, QueueServiceGetOptions options = null)
        {
            var list = new List<QueueMessage>();
            var receiver = new ReceiverLink(_receiverSession, "", queueName);
            while (true)
            {
                var message = receiver.Receive(new TimeSpan(0,0,1));
                if (message != null)
                    list.Add(new QueueMessage(message.DeliveryTag, message.Body.ToString()));
                else
                    break;

                if (options!= null && options.AutoAcknowlodge)
                    receiver.Accept(message);
            }
            return list;
        }

        public void Publish(string message, QueueServicePublishingOptions options = null)
        {
            throw new NotImplementedException();
        }

        public void Purge(string queue)
        {
            throw new NotImplementedException();
        }

        public void Remove(string queue)
        {
            throw new NotImplementedException();
        }

        public void Unbind(string queue, string[] exchanges, string[] routeKeys)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _senderSession?.Close();
            _receiverSession?.Close();
            _connection.Close();
        }
    }
}
