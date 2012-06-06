using System;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Common;
using RabbitMQ.Client;

namespace Client.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public const int MessageCount = 10000;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnRabbitClick(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
                                      {
                                          var cf = new ConnectionFactory
                                                       {Address = "localhost:" + Settings.RabbitPortNumber};

                                          using (var conn = cf.CreateConnection())
                                          using (var channel = conn.CreateModel())
                                          {
                                              for (var i = 0; i < MessageCount; i++)
                                              {
                                                  channel.BasicPublish("amq.direct", Settings.RabbitQueueName, null,
                                                                       Encoding.UTF8.GetBytes("hello from the client!"));
                                              }
                                          }
                                      });
        }

        private void BtnMsmqClick(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
                                      {
                                          var msMq
                                              = !MessageQueue.Exists(Settings.MsmqQueueName)
                                                    ? MessageQueue.Create(Settings.MsmqQueueName)
                                                    : new MessageQueue(Settings.MsmqQueueName);

                                          try
                                          {
                                              for (var i = 0; i < MessageCount; i++)
                                              {
                                                  msMq.Send("Sending data to MSMQ at " + DateTime.Now.ToString());
                                              }
                                          }
                                          catch (MessageQueueException ee)
                                          {
                                              Console.Write(ee.ToString());
                                          }
                                          catch (Exception eee)
                                          {
                                              Console.Write(eee.ToString());
                                          }
                                          finally
                                          {
                                              msMq.Close();
                                          }
                                      });
        }

        private void BtnZeroMqClick(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
                                      {
                                          using (var ctx = new ZMQ.Context(1))
                                          {
                                              var socket = ctx.Socket(ZMQ.REQ);
                                              socket.Connect(Settings.ZeroMqClientAddress);

                                              for (var i = 0; i < MessageCount; i++)
                                              {
                                                  socket.Send(Encoding.ASCII.GetBytes("Hello"));

                                                  byte[] message;
                                                  socket.Recv(out message);
                                              }
                                          }
                                      });
        }

        private void BtnSendAllClick(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => BtnMsmqClick(this, null));
            Task.Factory.StartNew(() => BtnRabbitClick(this, null));
            Task.Factory.StartNew(() => BtnZeroMqClick(this, null));
        }
    }
}