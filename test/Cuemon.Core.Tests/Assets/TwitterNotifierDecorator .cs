namespace Cuemon.Assets;
public class TwitterNotifierDecorator : NotifierDecorator
{
    public TwitterNotifierDecorator(INotifier notifier) : base(notifier)
    {
    }

    public override string Send(string message)
    {
        return string.Concat(base.Send(message), " was send from Twitter (a decorated class).");
    }
}
