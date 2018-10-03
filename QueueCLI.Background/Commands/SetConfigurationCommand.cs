using DocoptNet;
using QueueCLI.Background.Entities;
using QueueCLI.Background.IO;
using System.Collections.Generic;

namespace QueueCLI.Background.Commands
{
    public class SetConfigurationCommand : Command
    {
        private readonly ConfigurationIO _configurationIO;

        public SetConfigurationCommand()
        {
            _configurationIO = new ConfigurationIO();
        }

        public override string Name => "SET CONFIGURATION";

        public override void Execute(IDictionary<string, ValueObject> arguments)
        {
            var name = arguments["--name"] != null ? arguments["--name"].Value.ToString() : "";
            var broker = arguments["--broker"] != null ? arguments["--broker"].Value.ToString() : "";
            var host = arguments["--host"] != null ? arguments["--host"].Value.ToString() : "";
            var port = arguments["--port"] != null ? arguments["--port"].Value.ToString() : "";
            var user = arguments["--user"] != null ? arguments["--user"].Value.ToString() : "";
            var pass = arguments["--pass"] != null ? arguments["--pass"].Value.ToString() : "";
            var ssl = arguments["--ssl"] != null ? arguments["--ssl"].Value.ToString() : "";
            var environment = new Environment(name, broker, host, port, user, pass, ssl);

            var configuration = _configurationIO.Get();

            if (configuration == null)
                configuration = new Configuration();

            configuration.AddEnvironment(environment);

            _configurationIO.Write(configuration);
        }
    }
}
