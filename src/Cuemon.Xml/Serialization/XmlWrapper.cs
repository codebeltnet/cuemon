using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using Cuemon.Extensions;

namespace Cuemon.Xml.Serialization;
/// <summary>
/// Provide ways to override the default XML serialization.
/// </summary>
[XmlWrapper]
public abstract class XmlWrapper : Wrapper<object>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlWrapper"/> class.
    /// </summary>
    /// <param name="instance">The instance to override normal serialization naming logic.</param>
    protected XmlWrapper(object instance) : base(instance)
    {
    }

    /// <summary>
    /// Gets or sets the name of the instance used in XML serialization. Overrides normal serialization logic.
    /// </summary>
    /// <value>The name of the instance used in XML serialization.</value>
    public abstract XmlQualifiedEntity InstanceName { get; set; }

    /// <summary>
    /// Gets a collection of key/value pairs that provide additional user-defined information about this wrapper object.
    /// </summary>
    /// <value>An object that implements the <see cref="IDictionary{TKey,TValue}"/> interface and contains a collection of user-defined key/value pairs.</value>
    [XmlIgnore]
    public override IDictionary<string, object> Data => base.Data;

    /// <summary>
    /// Gets a value indicating whether this instance has a member reference.
    /// </summary>
    /// <value><c>true</c> if this instance has a member reference; otherwise, <c>false</c>.</value>
    [XmlIgnore]
    public override bool HasMemberReference => base.HasMemberReference;

    /// <summary>
    /// Gets the type of the object that this wrapper represents.
    /// </summary>
    /// <value>The type of the that this wrapper represents.</value>
    [XmlIgnore]
    public override Type InstanceType
    {
        get => base.InstanceType;
        protected set => base.InstanceType = value;
    }

    /// <summary>
    /// Gets the member from where <see cref="Wrapper{T}.Instance"/> was referenced.
    /// </summary>
    /// <value>The member from where <see cref="Wrapper{T}.Instance"/> was referenced.</value>
    [XmlIgnore]
    public override MemberInfo MemberReference
    {
        get => base.MemberReference;
        protected set => base.MemberReference = value;
    }
}
