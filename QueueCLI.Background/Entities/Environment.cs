using QueueCLI.Background.Enums;
using System;

namespace QueueCLI.Background.Entities
{
    public class Environment
    {
        public Environment(string name, string brokerType, string host, string port, string username, string password, string ssl)
        {
            SetName(name);
            SetBrokerType(brokerType);
            SetHost(host);
            SetPort(port);
            SetUsername(username);
            SetPassword(password);
            SetSsl(ssl);
        }

        public string Name { get; private set; }
        public EBrokerType BrokerType { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool Ssl { get; private set; }

        public void SetBrokerType(string _brokerType)
        {
            if (string.IsNullOrEmpty(_brokerType))
                throw new Exception("BROKER TYPE IS REQUIRED");

            BrokerType = (EBrokerType)Enum.Parse(typeof(EBrokerType), _brokerType, true);
        }

        private void SetName(string _name)
        {
            if (string.IsNullOrEmpty(_name))
                throw new Exception("NAME IS REQUIRED");

            Name = _name;
        }

        public void SetHost(string _host)
        {
            if (string.IsNullOrEmpty(_host))
                throw new Exception("HOST IS REQUIRED");

            Host = _host;
        }

        public void SetPort(string _port)
        {
            if (string.IsNullOrEmpty(_port))
                throw new Exception("PORT IS REQUIRED");

            Port = int.Parse(_port);
        }

        public void SetUsername(string _user)
        {
            if (string.IsNullOrEmpty(_user))
                throw new Exception("USERNAME IS REQUIRED");

            Username = _user;
        }

        public void SetPassword(string _pass)
        {
            if (string.IsNullOrEmpty(_pass))
                throw new Exception("PASSWORD IS REQUIRED");

            Password = _pass;
        }

        public void SetSsl(string _ssl)
        {
            bool result;
            if (string.IsNullOrEmpty(_ssl) && !bool.TryParse(_ssl, out result))
                throw new Exception("SSL INFORMATION IS REQUIRED");

            Ssl = bool.Parse(_ssl);
        }
    }
}
