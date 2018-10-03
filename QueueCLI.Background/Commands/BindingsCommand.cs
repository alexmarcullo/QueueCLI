using DocoptNet;
using QueueCLI.Broker.Abstractions;
using System;
using System.Collections.Generic;

namespace QueueCLI.Background.Commands
{
    public class BindingsCommand : Command
    {
        private readonly IQueueService _queueService;
        public override string Name => "BINDINGS COMMANDS";

        public BindingsCommand(IQueueService queueService)
        {
            this._queueService = queueService;
        }

        public override void Execute(IDictionary<string, ValueObject> arguments)
        {
            var command = arguments["--command"] != null ? arguments["--command"].Value.ToString() : "";
            var queue = arguments["--queue"] != null ? arguments["--queue"].Value.ToString() : "";
            var exchangeValues = arguments["--exchange"] != null ? arguments["--exchange"].Value.ToString() : "";
            var keyValues = arguments["--keys"] != null ? arguments["--keys"].Value.ToString() : "";
            var keys = keyValues.Split(',');
            var exchanges = exchangeValues.Split(',');

            using (_queueService)
            {
                switch (command)
                {
                    case "add":
                        _queueService.Bind(queue, exchanges, keys);
                        break;
                    case "remove":
                        _queueService.Unbind(queue, exchanges, keys);
                        break;
                    default:
                        throw new Exception("UNKNOW COMMAND.");
                }
            }
        }
    }
}
