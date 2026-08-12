using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Xunit;

namespace Cuemon.Extensions.Reflection.Assets;
[CLSCompliant(false)]
public class ClassWithAttributeDecorations
{
    [ContextStatic]
    private int _value = int.MaxValue;

    [XmlElement]
    public int Value => _value;

    public int ValueAlternative { get; } = int.MaxValue;

    [Description]
    public void Test()
    {

    }
}
