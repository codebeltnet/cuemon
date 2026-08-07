namespace Cuemon.Net;
internal static class Infrastructure
{
    internal static bool NotEncoded(char c)
    {
        return (c == '!' || c == '(' || c == ')' || c == '*' || c == '-' || c == '.' || c == '_');
    }
}
