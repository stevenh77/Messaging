namespace Common
{
    public class Settings
    {
        public const string MsmqQueueName = @".\private$\Queue#1";
        public const int RabbitPortNumber = 5672;
        public const string RabbitQueueName = "Queue#2";
        public const string ZeroMqServerAddress = "tcp://*:5555";
        public const string ZeroMqClientAddress = "tcp://localhost:5555";
    }
}
