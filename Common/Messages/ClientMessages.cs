using GalaSoft.MvvmLight.Messaging;

namespace Common.Messages
{
    public class ClientMessage : MessageBase {}

    public class MsmqClientMessage : ClientMessage {}
    public class RabbitClientMessage : ClientMessage {}
    public class ZeroMqClientMessage : ClientMessage {}
}
