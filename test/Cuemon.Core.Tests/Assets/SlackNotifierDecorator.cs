namespace Cuemon.Assets;
public class SlackNotifierDecorator : NotifierDecorator
{
    public SlackNotifierDecorator(INotifier notifier) : base(notifier)
    {
    }

    public override string Send(string message)
    {
        return string.Concat(base.Send(message), " was send from Slack (a decorated class).");
    }
}
