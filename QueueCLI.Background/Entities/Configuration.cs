using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueueCLI.Background.Entities
{
    public class Configuration
    {
        public Configuration()
        {
            _environments = new List<Environment>();
        }

        private List<Environment> _environments;
        public IEnumerable<Environment> Environments { get => _environments; }

        public void AddEnvironment(Environment environment)
        {
            if (this.Environments.Any(x => x.Name == environment.Name))
                throw new Exception("ENVIRONMENT NAME ALREADY EXISTS.");

            this._environments.Add(environment);
        }

        public void UpdateEnvironment(Environment environment)
        {
            var env = _environments.FirstOrDefault(x => x.Name == environment.Name);
            if (env != null)
            {
                _environments.Remove(env);
                _environments.Add(environment);
            }
            else
                throw new Exception("ENVIRONMENT NOT EXIST.");
        }

        public void RemoveEnvironment(string name)
        {
            var env = _environments.FirstOrDefault(x => x.Name == name);
            if (env != null)
                _environments.Remove(env);
        }

        public Environment GetEnvironment(string name)
        {
            return _environments.FirstOrDefault(x => x.Name == name);
        }

        public override string ToString()
            => JsonConvert.SerializeObject(this);
    }
}
