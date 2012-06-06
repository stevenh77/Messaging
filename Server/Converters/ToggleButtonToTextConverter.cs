using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Server.Converters
{
    public class ToggleButtonToTextConverter : MarkupExtension, IValueConverter
    {
        private static ToggleButtonToTextConverter _toggleButtonToTextConverter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _toggleButtonToTextConverter ?? (_toggleButtonToTextConverter = new ToggleButtonToTextConverter());
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool) value ? "ON" : "OFF";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}