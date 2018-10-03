namespace QueueCLI.Background.Configurations
{
    public static class UsageConfiguration
    {
        public static string USAGE { get => @"
RabbitCL
Usage:
    rcl configuration --name=<name> --broker=<brokerType> --host=<host> --port=<port> --user=<user> --pass=<pass> --ssl=<ssl>
    rcl updateenv     --name=<name> [--broker=<brokerType> --host=<host> --port=<port> --user=<user> --pass=<pass> --ssl=<ssl>]
    rcl consume       -e --env=<environment> -q --queue=<queue> -a --ack=<ack> [-o --out=<destinationFolder>]
    rcl queue         -e --env=<environment> -c --command=<command> -q --queue=<queue> [--durable=<durable> --exclusive=<exclusive> --autodelete=<autodelete>]
    rcl bindings      -e --env=<environment> -c --command=<command> -q --queue=<queue> --exchange=<exchange> --keys=<keys> 
    rcl               (-h | --help)
    rcl --version
    rcl --config
Options:
    -h --help               Show this screen.
    --version               Show version.
    --config                Get current configuration settings.
    --name=NAME             Environment name.
    --broker=BROKER         Broker provider. Possible values: RabbitMQ
    --host=HOST             Broker host connection.
    --port=PORT             Broker port connection.
    --user=USER             Broker username.
    --pass=PASS             Broker password.
    --ssl=SSL               Broker enable SSL.
    --exchange=EXCHANGE     Exchange or Topic name.
    --durable=DURABLE       Set queue durable.
    --exclusive=EXCLUSIVE   Set queue exclusive.
    --autodelete=AUTODELETE Set queue autodelete.
    -c --command=COMMAND    Command to perform. Values: add, remove.
    -e --env=ENVIRONMENT    Environment instance.
    -q --queue=QUEUE        Broker queue name.
    -a --ack=ACK            Acknowledge message.
    -o --out=OUTPUT         Output folder."; }
    }
}
