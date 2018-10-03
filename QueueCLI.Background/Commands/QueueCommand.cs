using DocoptNet;
using QueueCLI.Broker.Abstractions;
using System;
using System.Collections.Generic;

namespace QueueCLI.Background.Commands
{
    public class QueueCommand : Command
    {
        private readonly IQueueService _queueService;

        public QueueCommand(IQueueService queueService)
        {
            this._queueService = queueService;
        }

        public override string Name => "QUEUE COMMANDS";

        public override void Execute(IDictionary<string, ValueObject> arguments)
        {
            var command = arguments["--command"] != null ? arguments["--command"].Value.ToString() : "";
            var queue = arguments["--queue"] != null ? arguments["--queue"].Value.ToString() : "";
            var durableValue = arguments["--durable"] != null ? arguments["--durable"].Value.ToString() : "";
            var exclusiveValue = arguments["--exclusive"] != null ? arguments["--exclusive"].Value.ToString() : "";
            var autoDeleteValue = arguments["--autodelete"] != null ? arguments["--autodelete"].Value.ToString() : "";

            using (_queueService)
            {
                switch (command)
                {
                    case "add":
                        if (!bool.TryParse(durableValue, out bool durable))
                            throw new Exception("DURABLE PARAMETER IS INVALID.");
                        if (!bool.TryParse(exclusiveValue, out bool exclusive))
                            throw new Exception("EXCLUSIVE PARAMETER IS INVALID.");
                        if (!bool.TryParse(autoDeleteValue, out bool autodelete))
                            throw new Exception("AUTODELETE PARAMETER IS INVALID.");
                        _queueService.Create(queue, durable, exclusive, autodelete, new CreateQueueSettings());
                        break;
                    case "remove":
                        _queueService.Remove(queue);
                        break;
                    default:
                        throw new Exception("UNKNOW COMMAND.");
                }
            }
        }
    }
}
