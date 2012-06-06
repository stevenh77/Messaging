using GalaSoft.MvvmLight.Messaging;

namespace Common.Messages
{
    public class LogMessage : MessageBase
    {
        public string Body { get; set; }
    }

    public class MsmqLogMessage : LogMessage {}
    public class RabbitLogMessage : LogMessage {}
    public class ZeroMqLogMessage : LogMessage {}
}
