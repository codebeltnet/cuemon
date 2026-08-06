namespace Cuemon.Assets;
public static class NotifierDecoratorExtensions
{
    public static string Send(this IDecorator<INotifier> decorator, bool facebook = false, bool slack = false, bool twitter = false)
    {
        var stack = new NotifierDecorator(decorator.Inner);
        if (facebook)
        {
            stack = new FacebookNotifierDecorator(stack);
        }

        if (twitter)
        {
            stack = new TwitterNotifierDecorator(stack);
        }

        if (slack)
        {
            stack = new SlackNotifierDecorator(stack);
        }

        return stack.Send("Unit Testing the Decorator Pattern: ");
    }
}
