using System;
using System.Reflection;
using Cuemon.Configuration;
using Cuemon.Extensions.Runtime.Serialization;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Runtime;
/// <summary>
/// Specifies options that is related to <see cref="Hierarchy"/> and <see cref="HierarchySerializer"/> operations.
/// </summary>
/// <seealso cref="IParameterObject"/>
public class HierarchyOptions : IParameterObject
{
    private int _maxDepth;
    private int _maxCircularCalls;
    private Func<Type, bool> _skipPropertyType;
    private Func<PropertyInfo, bool> _skipProperty;
    private Func<object, bool> _hasCircularReference;
    private Func<object, PropertyInfo, object> _valueResolver;
    private MemberReflection _reflectionRules;

    /// <summary>
    /// Initializes a new instance of the <see cref="HierarchyOptions"/> class.
    /// </summary>
    public HierarchyOptions()
    {
        MaxDepth = 10;
        MaxCircularCalls = 2;
        ReflectionRules = new MemberReflection(true, true);
        SkipPropertyType = source =>
        {
            switch (Type.GetTypeCode(source))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Empty:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.String:
                    return true;
                default:
                    if (Decorator.Enclose(source).HasKeyValuePairImplementation()) { return true; }
                    if (Decorator.Enclose(source).HasTypes(typeof(MemberInfo))) { return true; }
                    return false;
            }
        };
        SkipProperty = property =>
        {
            return (property.PropertyType.GetTypeInfo().IsMarshalByRef ||
                    property.PropertyType.GetTypeInfo().IsSubclassOf(typeof(Delegate)) ||
                    property.Name.Equals("SyncRoot", StringComparison.Ordinal) ||
                    property.Name.Equals("IsReadOnly", StringComparison.Ordinal) ||
                    property.Name.Equals("IsFixedSize", StringComparison.Ordinal) ||
                    property.Name.Equals("IsSynchronized", StringComparison.Ordinal) ||
                    property.Name.Equals("Count", StringComparison.Ordinal) ||
                    property.Name.Equals("HResult", StringComparison.Ordinal) ||
                    property.Name.Equals("Parent", StringComparison.Ordinal) ||
                    property.Name.Equals("TargetSite", StringComparison.Ordinal));
        };
        HasCircularReference = i => Decorator.Enclose(i.GetType()).HasCircularReference(i);
        ValueResolver = (s, i) => Decorator.RawEnclose(s).DefaultPropertyValueResolver(i);
    }

    /// <summary>
    /// Gets or sets the maximum depth to safely traverse an object hierarchy. Default is 10.
    /// </summary>
    /// <value>The maximum depth to safely traverse an object hierarchy.</value>
    public int MaxDepth
    {
        get => _maxDepth;
        set
        {
            Validator.ThrowIfLowerThan(value, 0, nameof(value));
            _maxDepth = value;
        }
    }

    /// <summary>
    /// Gets or sets the maximum amount of times an object is allowed to make circular calls. Default is 2.
    /// </summary>
    /// <value>The maximum amount of times an object is allowed to make circular calls.</value>
    public int MaxCircularCalls
    {
        get => _maxCircularCalls;
        set
        {
            Validator.ThrowIfLowerThan(value, 0, nameof(value));
            _maxCircularCalls = value;
        }
    }

    /// <summary>
    /// Gets or sets the binding constraints for reflection based member searching.
    /// </summary>
    /// <value>The binding constraints for reflection based member searching.</value>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> cannot be null.
    /// </exception>
    public MemberReflection ReflectionRules
    {
        get => _reflectionRules;
        set
        {
            Validator.ThrowIfNull(value);
            _reflectionRules = value;
        }
    }

    /// <summary>
    /// Gets or sets the function delegate that is invoked just before public properties is being iterated and whose return <see cref="Type"/> determine if the properties should be skipped or not.
    /// </summary>
    /// <value>A <see cref="Func{TResult}"/> that determines if a given property <see cref="Type"/> should be skipped or not.</value>
    public Func<Type, bool> SkipPropertyType
    {
        get => _skipPropertyType;
        set
        {
            Validator.ThrowIfNull(value);
            _skipPropertyType = value;
        }
    }

    /// <summary>
    /// Gets or sets the function delegate that is invoked every time a public property is iterated and whose <see cref="PropertyInfo"/> determine if that property should be skipped or not.
    /// </summary>
    /// <value>A <see cref="Func{TResult}"/> that determines if a given <see cref="PropertyInfo"/> should be skipped or not.</value>
    public Func<PropertyInfo, bool> SkipProperty
    {
        get => _skipProperty;
        set
        {
            Validator.ThrowIfNull(value);
            _skipProperty = value;
        }
    }

    /// <summary>
    /// Gets or sets the function delegate that is invoked when a property has a value and whose return value suggest a circular reference.
    /// </summary>
    /// <value>A <see cref="Func{TResult}"/> that determines if an object is suggesting a circular reference.</value>
    public Func<object, bool> HasCircularReference
    {
        get => _hasCircularReference;
        set
        {
            Validator.ThrowIfNull(value);
            _hasCircularReference = value;
        }
    }

    /// <summary>
    /// Gets or sets the function delegate that is invoked when a property can be read and is of same type as the underlying <see cref="Type"/> of the source object.
    /// </summary>
    /// <value>A function delegate that is invoked when a property can be read and is of same type as the underlying <see cref="Type"/> of the source object.</value>
    public Func<object, PropertyInfo, object> ValueResolver
    {
        get => _valueResolver;
        set
        {
            Validator.ThrowIfNull(value);
            _valueResolver = value;
        }
    }
}
