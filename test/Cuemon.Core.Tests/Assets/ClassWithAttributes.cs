using System;
using System.Runtime.Serialization;

namespace Cuemon.Assets;
[DataContract]
public class ClassWithAttributes
{
    [DataMember]
    public int Id { get; set; }

    [Obsolete]
    public string Name { get; set; }
}
