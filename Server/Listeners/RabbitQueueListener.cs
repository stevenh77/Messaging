using System;
using System.Diagnostics;
using System.Text;
using Common.Messages;
using GalaSoft.MvvmLight.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace Server.Listeners
{
    class RabbitQueueListener : IListener
    {
        public bool IsListening { get; set; }
        private readonly int _port;
        private Subscription _subscription;
        private string _queueName;

        public RabbitQueueListener(int port, string queueName)
        {
            _port = port;
            _queueName = queueName;
        }

        public void Start()
        {
            try
            {
                if (IsListening) return;

                var serverAddress = "localhost:" + _port;

                var connectionFactory = new ConnectionFactory { Address = serverAddress };

                using (var connection = connectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        Log("Opening listener");
                        string queueName = channel.QueueDeclare(_queueName, false, false, false, null);
                        channel.QueueBind(queueName, "amq.direct", queueName, null);

                        using (_subscription = new Subscription(channel, queueName))
                        {
                            IsListening = true;
                            while (IsListening)
                            {
                                foreach (BasicDeliverEventArgs eventArgs in _subscription)
                                {
                                    //Log(Encoding.UTF8.GetString(eventArgs.Body));
                                    Messenger.Default.Send(new RabbitClientMessage());
                                    _subscription.Ack();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void Stop()
        {
            if (!IsListening) return;
            _subscription.Close();
            IsListening = false;
            Log("Listener closed.");
        }

        private void Log(string message)
        {
            Debug.WriteLine(message);
            Messenger.Default.Send(new RabbitLogMessage() { Body = message });
        }
    }
}
