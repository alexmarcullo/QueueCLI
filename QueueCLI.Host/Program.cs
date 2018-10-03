using Autofac;
using DocoptNet;
using QueueCLI.Background.Configurations;
using QueueCLI.Background.Entities;
using QueueCLI.Background.Enums;
using QueueCLI.Background.Factories;
using QueueCLI.Background.IO;
using QueueCLI.Broker.Abstractions;
using QueueCLI.Broker.ActiveMQ.DependencyInjection;
using QueueCLI.Broker.RabbitMQ.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueueCLI.Host
{
    class Program
    {
        private static Configuration _configuration { get => new ConfigurationIO().Get(); }
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            var arguments = new Docopt().Apply(UsageConfiguration.USAGE, args, version: "Queue CLI 1.0.0", exit: true);

            var config = _configuration;

            var builder = new ContainerBuilder();

            EnvironmentFactory(arguments, config, builder);

            Container = builder.Build();

            try
            {
                var command = CommandFactory.Create(arguments, () => { return Container.Resolve<IQueueService>(); });
                try
                {
                    if (command != null)
                    {
                        //Console.WriteLine(command.Name);
                        command.Execute(arguments);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] ({command.Name}) => {ex.Message}");
                }
            }
            catch (Autofac.Core.Registration.ComponentNotRegisteredException ex)
            {
                Console.WriteLine($"[ERROR] (COMPONENT NOT REGISTRED) => {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] (COMMAND FACTORY) => {ex.Message}");
            }
        }

        private static void EnvironmentFactory(IDictionary<string, ValueObject> arguments, Configuration config, ContainerBuilder builder)
        {
            if (config != null && arguments["--env"] != null)
            {
                var environmentArgument = arguments["--env"].Value.ToString();

                var environment = config.Environments.FirstOrDefault(x => x.Name == environmentArgument);
                if (environment != null)
                {
                    switch (environment.BrokerType)
                    {
                        case EBrokerType.RabbitMQ:
                            builder.AddRabbitMQ(options =>
                            {
                                options.Host = environment.Host;
                                options.Port = environment.Port;
                                options.EnableSsl = environment.Ssl;
                                options.Username = environment.Username;
                                options.Password = environment.Password;
                            });
                            break;
                        case EBrokerType.ActiveMQ:
                            builder.AddActiveMQ(options =>
                            {
                                options.Host = environment.Host;
                                options.Port = environment.Port;
                                options.EnableSsl = environment.Ssl;
                                options.Username = environment.Username;
                                options.Password = environment.Password;
                            });
                            break;
                    }
                }
            }
        }
    }
}
