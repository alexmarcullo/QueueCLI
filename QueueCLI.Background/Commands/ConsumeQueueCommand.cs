using DocoptNet;
using Newtonsoft.Json;
using QueueCLI.Broker.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;

namespace QueueCLI.Background.Commands
{
    public class ConsumeQueueCommand : Command
    {
        private readonly IQueueService _queueService;

        public ConsumeQueueCommand(IQueueService queueService)
        {
            _queueService = queueService;
        }

        public override string Name => "CONSUME QUEUE CONFIGURATION";

        public override void Execute(IDictionary<string, ValueObject> arguments)
        {
            var queue = arguments["--queue"] != null ? arguments["--queue"].Value.ToString() : "";
            var ack = arguments["--ack"] != null ? arguments["--ack"].Value.ToString() : "";
            var output = arguments["--out"] != null ? arguments["--out"].Value.ToString() : "";

            string folder = output == "." || String.IsNullOrEmpty(output) ? "." : output;
            if (!Directory.Exists(folder))
                throw new Exception("OUTPUT PATH DOES NOT EXISTIS");

            var messages = _queueService.Get(queue);

            foreach (var message in messages)
            {
                if (ack == "true")
                    _queueService.Acknowledge(message);

                string extension = IsJsonFormat(message.Payload) ? "json" : "txt";
                string file = $"{folder}\\{queue}-{message.Id}-{DateTime.Now.ToString("yyyyMMddHHmmss.fff")}.{extension}";

                File.WriteAllText(file, message.Payload);
            }

            _queueService.Dispose();
        }

        private bool IsJsonFormat(string payload)
        {
            payload = payload.Trim();
            if ((payload.StartsWith("{") && payload.EndsWith("}")) || //For object
                (payload.StartsWith("[") && payload.EndsWith("]"))) //For array
            {
                try
                {
                    JsonConvert.DeserializeObject(payload);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
