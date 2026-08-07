using System;

namespace Cuemon.Extensions.Reflection.Assets;
public class CustomException : AggregateException
{
    public CustomException()
    {
        Code = 42;
        CodePhrase = "FortyTwo";
    }

    public int Code { get; }

    public string CodePhrase { get; }
}
