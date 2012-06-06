using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Common.Controls
{
    [TemplatePart(Name="PART_DISPLAY_ICON", Type=typeof(Image))]
    [TemplatePart(Name = "PART_DISPLAY_COUNT_CONTAINER", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_DISPLAY_TITLE_CONTAINER", Type = typeof(TextBlock))]
    [Description("Represents a metro styled tile that displays an icon, a count and a title")]
    public class MetroTile : Control
    {
        #region Constructor
        
        static MetroTile()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroTile),
                new FrameworkPropertyMetadata(typeof(MetroTile)));

            CommandManager.RegisterClassCommandBinding(
                typeof(MetroTile),
                new CommandBinding(ResetCountCommand, OnResetCountCommand));
        }

        #endregion

        #region Dependency Properties

        #region DisplayIcon

        public static readonly DependencyProperty DisplayIconProperty =
            DependencyProperty.Register("DisplayIcon",
                                        typeof (ImageSource),
                                        typeof (MetroTile),
                                        new PropertyMetadata(null));

        [Description("The icon displayed in the tile."), Category("Common Properties")]
        public ImageSource DisplayIcon
        {
            get { return (ImageSource)this.GetValue(DisplayIconProperty); }
            set { this.SetValue(DisplayIconProperty, value); }
        }

        #endregion

        #region DisplayCount

        public static readonly DependencyProperty DisplayCountProperty =
            DependencyProperty.Register("DisplayCount",
                                        typeof(int),
                                        typeof(MetroTile),
                                        new UIPropertyMetadata(0));

        [Description("The count displayed in the tile."), Category("Common Properties")]
        public int DisplayCount
        {
            get { return (int)this.GetValue(DisplayCountProperty); }
            set { this.SetValue(DisplayCountProperty, value); }
        }

        #endregion

        #region DisplayText
        
        [Description("The text displayed in the tile."), Category("Common Properties")]
        public static readonly DependencyProperty DisplayTextProperty =
            DependencyProperty.Register("DisplayText",
                                        typeof(string),
                                        typeof(MetroTile),
                                        new PropertyMetadata("Not set"));

        public string DisplayText
        {
            get { return (string)this.GetValue(DisplayTextProperty); }
            set { this.SetValue(DisplayTextProperty, value); }
        }
        #endregion

        #endregion

        #region Events

        public static readonly RoutedEvent DisplayCountChangedEvent =
            EventManager.RegisterRoutedEvent("DisplayCountChanged",
                                            RoutingStrategy.Bubble,
                                            typeof(RoutedEventHandler),
                                            typeof(MetroTile));

        public event RoutedEventHandler DisplayCountChanged
        {
            add { AddHandler(DisplayCountChangedEvent, value); }
            remove { RemoveHandler(DisplayCountChangedEvent, value); }
        }

        protected virtual void OnDisplayCountChanged(int oldValue, int newValue)
        {
            //  1. Pair of events: A preview that tunnels and a main event that bubbles
            //  2. We could also create our own RoutedEventArgs that includes oldValue & new Value
            
            var previewEvent = new RoutedEventArgs(PreviewDisplayCountChangedEvent);
            RaiseEvent(previewEvent);

            var e = new RoutedEventArgs(DisplayCountChangedEvent);
            RaiseEvent(e);
        }

        public static readonly RoutedEvent PreviewDisplayCountChangedEvent =
            EventManager.RegisterRoutedEvent("PreviewDisplayCountChanged",
                                            RoutingStrategy.Tunnel,
                                            typeof(RoutedEventHandler),
                                            typeof(MetroTile));

        public event RoutedEventHandler PreviewDisplayCountChanged
        {
            add { AddHandler(PreviewDisplayCountChangedEvent, value); }
            remove { RemoveHandler(PreviewDisplayCountChangedEvent, value); }
        }

        #endregion

        #region Commands

        public static readonly ICommand ResetCountCommand =
            new RoutedUICommand("ResetCount", "ResetCount", typeof(MetroTile));

        private static void OnResetCountCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var target = (MetroTile)sender;
            target.DisplayCount = 0;
        }

        #endregion

    }
}