﻿// http://www.switchonthecode.com/tutorials/wpf-snippet-detecting-binding-errors

using System.Diagnostics;
using System.Text;
using System.Windows;

namespace Common.Xaml
{
    public class BindingErrorTraceListener : DefaultTraceListener
    {
        private static BindingErrorTraceListener _listener;


        private readonly StringBuilder _message = new StringBuilder();

        private BindingErrorTraceListener()
        {
        }

        public override void Write(string message)
        {
            _message.Append(message);
        }

        public override void WriteLine(string message)
        {
            _message.Append(message);

            var final = _message.ToString();
            _message.Length = 0;

            MessageBox.Show(final,
              "Binding Error",
              MessageBoxButton.OK,
              MessageBoxImage.Error);
        }

        public static void CloseTrace()
        {
            if (_listener == null)
            {
                return;
            }

            _listener.Flush();
            _listener.Close();
            PresentationTraceSources.DataBindingSource.Listeners.Remove(_listener);
            _listener = null;
        }

        [Conditional("DEBUG")]
        public static void SetTrace()
        {
            SetTrace(SourceLevels.Error, TraceOptions.None);
        }

        public static void SetTrace(SourceLevels level, TraceOptions options)
        {
            if (_listener == null)
            {
                _listener = new BindingErrorTraceListener();
                PresentationTraceSources.DataBindingSource.Listeners.Add(_listener);
            }

            _listener.TraceOutputOptions = options;
            PresentationTraceSources.DataBindingSource.Switch.Level = level;
        }
    }
}
