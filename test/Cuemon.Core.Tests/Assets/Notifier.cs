namespace Cuemon.Assets;
public class Notifier : INotifier
{
    public Notifier()
    {

    }

    public string Send(string message)
    {
        return string.Concat(message, " was send from Notifier (base class - not a decorator).");
    }
}
