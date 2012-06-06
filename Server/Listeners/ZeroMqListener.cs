using System;
using System.Text;
using Common.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace Server.Listeners
{
    public class ZeroMqListener : IListener
    {
        public bool IsListening { get; set; }
        private readonly string _zeroMqAddress;
        public ZeroMqListener(string zeroMqAddress)
        {
            _zeroMqAddress = zeroMqAddress;
        }

        public void Start()
        {
            if (IsListening) return;

            Messenger.Default.Send(new ZeroMqLogMessage() { Body = "Opening listener" });
            IsListening = true;
            
            try
            {
                using (var ctx = new ZMQ.Context(1))
                {
                    
                    var socket = ctx.Socket(ZMQ.REP);
                    socket.Bind(_zeroMqAddress);

                    while (IsListening)
                    {
                        byte[] message;
                        socket.Recv(out message);
                        Messenger.Default.Send(new ZeroMqClientMessage());

                        socket.Send(Encoding.ASCII.GetBytes(""));
                    }
                }
            }
            catch (Exception e)
            {
                Messenger.Default.Send(new ZeroMqLogMessage() { Body = e.Message });
            }
        }

        public void Stop()
        {
            if (!IsListening) return;
            IsListening = false;
            Messenger.Default.Send(new ZeroMqLogMessage() { Body = "Listener closed." });
        }
    }
}
