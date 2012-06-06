using System.Windows.Input;
using Common;
using Common.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Server.EndPoints;
using Server.Listeners;

namespace Server.UI
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Properties

        #region OverrideSwitch

        public const string OverrideSwitchPropertyName = "OverrideSwitch";
        private bool? _overrideSwitch = false;
        public bool? OverrideSwitch
        {
            get { return _overrideSwitch; }
            set
            {
                if (_overrideSwitch == value) return;
                _overrideSwitch = value;
                RaisePropertyChanged(() => OverrideSwitch);
                if (OverrideSwitch == null) return;

                Messenger.Default.Send((bool) OverrideSwitch
                    ? new OverrideSwitchMessage() {Switch = Switch.On}
                    : new OverrideSwitchMessage() {Switch = Switch.Off});
            }
        }

        #endregion

        #endregion

        public ICommand CloseCommand { get; set; }

        public MessageEndPoint<MsmqClientMessage, MsmqLogMessage> MsmqEndPoint { get; set; }
        public MessageEndPoint<RabbitClientMessage, RabbitLogMessage> RabbitEndPoint { get; set; }
        public MessageEndPoint<ZeroMqClientMessage, ZeroMqLogMessage> ZeroMqEndPoint { get; set; }

        #region Constructor

        public MainWindowViewModel()
        {
            InitMsmqEndPoint(Settings.MsmqQueueName);
            InitRabbitEndPoint(Settings.RabbitPortNumber, Settings.RabbitQueueName);
            InitZeroMqEndPoint(Settings.ZeroMqServerAddress);

            CloseCommand = new RelayCommand(CloseCommandExecute);
        }

        #endregion

        private void InitMsmqEndPoint(string queueName)
        {
            IListener listener = new MsmqListener(queueName);
            MsmqEndPoint = new MessageEndPoint<MsmqClientMessage, MsmqLogMessage>(listener)
            {
                DisplayText = "Microsoft Message Queue",
                ToolTip = string.Format("Listening on msmq queue ({0})", queueName)
            };
        }

        private void InitRabbitEndPoint(int port, string queueName)
        {
            IListener listener = new RabbitQueueListener(port, queueName);
            RabbitEndPoint = new MessageEndPoint<RabbitClientMessage, RabbitLogMessage>(listener)
            {
                DisplayText = "RabbitMQ",
                ToolTip = string.Format("Listening on port {0} to queue {1} and binding it to amq.direct", port, queueName)
            };
        }

        private void InitZeroMqEndPoint(string address)
        {
            IListener listener = new ZeroMqListener(address);
            ZeroMqEndPoint = new MessageEndPoint<ZeroMqClientMessage, ZeroMqLogMessage>(listener)
            {
                DisplayText = "ZeroMq",
                ToolTip = "Listening on " + address
            };
        }

        private void CloseCommandExecute()
        {
            Application.Current.Shutdown();
        }
    }
}