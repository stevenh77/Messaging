using System;
using System.Messaging;
using Common.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace Server.Listeners
{
    public class MsmqListener : IListener
    {
        public bool IsListening { get; set; }

        private readonly MessageQueue _mq;

        public MsmqListener(string queueName)
        {
            _mq = MessageQueue.Exists(queueName)
                ? new MessageQueue(queueName)
                : MessageQueue.Create(queueName);

            _mq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
        }

        public void Start()
        {
            if (IsListening) return;

            Messenger.Default.Send(new MsmqLogMessage() { Body = "Opening listener" });
            IsListening = true;
            while (IsListening)
            {
                try
                {
                    var message = _mq.Receive().Body;
                    Messenger.Default.Send(new MsmqClientMessage());
                }
                catch (MessageQueueException mqe)
                {
                    Messenger.Default.Send(new MsmqLogMessage() { Body = mqe.Message });
                }
                catch (Exception e)
                {
                    Messenger.Default.Send(new MsmqLogMessage() { Body = e.Message });
                }
            }
        }

        public void Stop()
        {
            if (!IsListening) return;
            _mq.Close();
            IsListening = false;
            Messenger.Default.Send(new MsmqLogMessage() { Body = "Listener closed." });
        }
    }
}
