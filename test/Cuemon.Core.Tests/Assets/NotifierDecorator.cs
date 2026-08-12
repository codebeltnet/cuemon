namespace Cuemon.Assets;
public class NotifierDecorator : INotifier
{
    private readonly INotifier _notifier;

    public NotifierDecorator(INotifier notifier)
    {
        _notifier = notifier;
    }

    public virtual string Send(string message)
    {
        return _notifier.Send(message);
    }
}
