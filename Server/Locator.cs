using Server.UI;

namespace Server
{
    public class Locator
    {
        public MainWindowViewModel MainWindowViewModel
        {
            get { return new MainWindowViewModel(); }
        }
    }
}