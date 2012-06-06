namespace Server.Listeners
{
    public interface IListener
    {
        bool IsListening { get; set; }
        void Start();
        void Stop();
    }
}
