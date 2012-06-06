using System;
using System.Threading.Tasks;
using Common.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Server.Listeners;

namespace Server.EndPoints
{
    public class MessageEndPoint<T, TY> : ObservableObject
        where T : ClientMessage
        where TY : LogMessage
    {
        #region Fields

        private readonly IListener _listener;
        private readonly object _locker;

        #endregion

        #region Constructor

        public MessageEndPoint(IListener listener)
        {
            _listener = listener;
            _locker = new object();
            Init();
        }

        #endregion

        #region Messaging

        public void Init()
        {
            Messenger.Default.Register<T>(this, ClientMessageReceived);
            Messenger.Default.Register<TY>(this, LogMessageReceived);
            Messenger.Default.Register<OverrideSwitchMessage>(this, SwitchMessageReceived);
        }

        private void SwitchMessageReceived(OverrideSwitchMessage message)
        {
            if (message.Switch == Switch.Off)
            {
                IsListening = false;
            }
            else
            {
                IsListening = true;
            }
        }

        protected void LogMessageReceived(TY message)
        {
            this.DisplayLog = DisplayLog + Environment.NewLine + message.Body;
        }

        protected void ClientMessageReceived(T message)
        {
            this.DisplayCount++;
         }

        #endregion

        #region Properties

        #region IsListening

        public const string IsListeningPropertyName = "IsListening";
        private bool _isListening;
        public bool IsListening
        {
            get { return _isListening; }
            set
            {
                if (_isListening == value) return;
                _isListening = value;
                RaisePropertyChanged(() => IsListening);
                ToggleListener();
            }
        }

        #endregion

        #region DisplayText

        public const string DisplayTextPropertyName = "DisplayText";
        private string _displayText;
        public string DisplayText
        {
            get { return _displayText; }
            set
            {
                if (_displayText == value) return;
                _displayText = value;
                RaisePropertyChanged(() => DisplayText);
            }
        }

        #endregion

        #region DisplayCount

        public const string DisplayCountPropertyName = "DisplayCount";
        private int _displayCount;
        public int DisplayCount
        {
            get { return _displayCount; }
            set
            {
                lock (_locker)
                {
                    if (_displayCount == value) return;
                    _displayCount = value;
                    RaisePropertyChanged(() => DisplayCount);
                }
            }
        }
        #endregion

        #region DisplayLog

        public const string DisplayLogPropertyName = "DisplayLog";
        private string _displayLog = string.Empty;
        public string DisplayLog
        {
            get { return _displayLog; }
            set
            {
                if (_displayLog == value) return;
                _displayLog = value;
                RaisePropertyChanged(() => DisplayLog);
                IsLogChanged = true;
            }
        }

        #endregion

        #region IsLogChanged

        public const string IsLogChangedPropertyName = "IsLogChanged";
        private bool _isLogChanged;
        public bool IsLogChanged
        {
            get { return _isLogChanged; }
            set
            {
                if (_isLogChanged == value) return;
                _isLogChanged = value;
                RaisePropertyChanged(() => IsLogChanged);
            }
        }

        #endregion

        #region ToolTip

        public const string ToolTipPropertyName = "ToolTip";
        private string _toolTip = string.Empty;
        public string ToolTip
        {
            get { return _toolTip; }
            set
            {
                if (_toolTip == value) return;
                _toolTip = value;
                RaisePropertyChanged(() => ToolTip);
            }
        }

        #endregion

        #endregion

        #region Commands

        private void ToggleListener()
        {
            if (null == _listener) return;

            if (_listener.IsListening)
                Task.Factory.StartNew(_listener.Stop);
            else
                Task.Factory.StartNew(_listener.Start);
        }

        #endregion
    }
}