using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QueueCLI.Background.Entities;
using System.IO;
using System.Reflection;

namespace QueueCLI.Background.IO
{
    public class ConfigurationIO
    {
        private readonly string _filePath;
        private readonly string _folderPath;

        public ConfigurationIO()
        {
            _folderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            _filePath = $"{_folderPath}\\config.json";
        }

        public void Write(Configuration configuration)
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new StringEnumConverter());

            File.WriteAllText(_filePath, JsonConvert.SerializeObject(configuration, jsonSerializerSettings));
        }

        public Configuration Get()
        {
            if (File.Exists(_filePath))
            {
                var line = File.ReadAllLines(_filePath)[0];
                return JsonConvert.DeserializeObject<Configuration>(line);
            }
            return null;
        }
    }
}
