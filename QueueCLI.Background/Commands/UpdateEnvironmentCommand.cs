using DocoptNet;
using QueueCLI.Background.IO;
using System;
using System.Collections.Generic;

namespace QueueCLI.Background.Commands
{
    public class UpdateEnvironmentCommand : Command
    {
        private readonly ConfigurationIO _configurationIO;
        public UpdateEnvironmentCommand()
        {
            _configurationIO = new ConfigurationIO();
        }

        public override string Name => "UPDATE ENVIRONMENT";

        public override void Execute(IDictionary<string, ValueObject> arguments)
        {
            var configuration = _configurationIO.Get();

            if (configuration == null)
                throw new Exception("CONFIGURATION FILE NOT FOUND.");

            var name = arguments["--name"] != null ? arguments["--name"].Value.ToString() : "";
            var broker = arguments["--broker"] != null ? arguments["--broker"].Value.ToString() : "";
            var host = arguments["--host"] != null ? arguments["--host"].Value.ToString() : "";
            var port = arguments["--port"] != null ? arguments["--port"].Value.ToString() : "";
            var user = arguments["--user"] != null ? arguments["--user"].Value.ToString() : "";
            var pass = arguments["--pass"] != null ? arguments["--pass"].Value.ToString() : "";
            var ssl = arguments["--ssl"] != null ? arguments["--ssl"].Value.ToString() : "";

            var environment = configuration.GetEnvironment(name);

            if (environment != null)
            {
                if (!String.IsNullOrEmpty(broker))
                    environment.SetBrokerType(broker);
                if (!String.IsNullOrEmpty(host))
                    environment.SetHost(host);
                if (!String.IsNullOrEmpty(port))
                    environment.SetPort(port);
                if (!String.IsNullOrEmpty(user))
                    environment.SetUsername(user);
                if (!String.IsNullOrEmpty(pass))
                    environment.SetPassword(pass);
                if (!String.IsNullOrEmpty(ssl))
                    environment.SetSsl(ssl);

                configuration.UpdateEnvironment(environment);

                _configurationIO.Write(configuration);
            }
        }

    }
}
