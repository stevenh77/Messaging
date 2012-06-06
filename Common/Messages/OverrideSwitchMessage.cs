using GalaSoft.MvvmLight.Messaging;

namespace Common.Messages
{
    public enum Switch
    {
        Off = 0,
        On
    }

    public class OverrideSwitchMessage : MessageBase
    {
        public Switch Switch { get; set; }
    }
}
