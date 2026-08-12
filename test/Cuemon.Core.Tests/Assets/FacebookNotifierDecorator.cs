namespace Cuemon.Assets;
public class FacebookNotifierDecorator : NotifierDecorator
{
    public FacebookNotifierDecorator(INotifier notifier) : base(notifier)
    {
    }

    public override string Send(string message)
    {
        return string.Concat(base.Send(message), " was send from Facebook (a decorated class).");
    }
}
