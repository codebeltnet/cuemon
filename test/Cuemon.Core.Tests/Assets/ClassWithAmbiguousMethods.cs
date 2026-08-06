using System;

namespace Cuemon.Assets;
public class ClassWithAmbiguousMethods
{
    public void MethodA()
    {

    }

    public void MethodA(int i)
    {

    }

    public void MethodA(int i, string s)
    {

    }

    public void MethodA(Guid id)
    {

    }

    public void MethodB()
    {

    }
}
