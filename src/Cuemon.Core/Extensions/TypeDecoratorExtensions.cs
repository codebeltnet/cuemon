using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;

namespace Cuemon;
/// <summary>
/// Extension methods for the <see cref="Type"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class TypeDecoratorExtensions
{
    private static ConcurrentDictionary<string, bool> ComplexValueTypeLookup { get; } = new();

    private static readonly Action<MemberReflectionOptions> DefaultMemberReflectionSetup = o =>
    {
        o.ExcludeStatic = true;
        o.ExcludeInheritancePath = true;
    };

    /// <summary>
    /// Retrieves a collection that represents all properties defined on the enclosed <see cref="Type"/> of the specified <paramref name="decorator"/> and its inheritance chain.
    /// </summary>
    /// <param name="decorator">The <see cref="Type"/> to extend.</param>
    /// <param name="setup">The <see cref="MemberReflectionOptions" /> which may be configured.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains all <see cref="PropertyInfo"/> objects on the enclosed <see cref="Type"/> of the specified <paramref name="decorator"/> and its inheritance chain.</returns>
    public static IEnumerable<PropertyInfo> GetAllProperties(this IDecorator<Type> decorator, Action<MemberReflectionOptions> setup = null)
    {
        return GetInheritedTypes(decorator).SelectMany(type => type.GetProperties(MemberReflection.CreateFlags(setup ?? DefaultMemberReflectionSetup))).Distinct(DynamicEqualityComparer.Create<PropertyInfo>(pi => pi.Name.GetHashCode(), (pi1, pi2) => pi1.Name.Equals(pi2.Name, StringComparison.Ordinal)));
    }

    /// <summary>
    /// Retrieves a collection that represents all fields defined on the enclosed <see cref="Type"/> of the specified <paramref name="decorator"/> and its inheritance chain.
    /// </summary>
    /// <param name="decorator">The <see cref="Type"/> to extend.</param>
    /// <param name="setup">The <see cref="MemberReflectionOptions" /> which may be configured.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains all <see cref="FieldInfo"/> objects on the enclosed <see cref="Type"/> of the specified <paramref name="decorator"/> and its inheritance chain.</returns>
    public static IEnumerable<FieldInfo> GetAllFields(this IDecorator<Type> decorator, Action<MemberReflectionOptions> setup = null)
    {
        return GetInheritedTypes(decorator).SelectMany(type => type.GetFields(MemberReflection.CreateFlags(setup ?? DefaultMemberReflectionSetup))).Distinct(DynamicEqualityComparer.Create<FieldInfo>(pi => pi.Name.GetHashCode(), (pi1, pi2) => pi1.Name.Equals(pi2.Name, StringComparison.Ordinal)));
    }

    /// <summary>
    /// Retrieves a collection that represents all events defined on the enclosed <see cref="Type"/> of the specified <paramref name="decorator"/> and its inheritance chain.
    /// </summary>
    /// <param name="decorator">The <see cref="Type"/> to extend.</param>
    /// <param name="setup">The <see cref="MemberReflectionOptions" /> which may be configured.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains all <see cref="EventInfo"/> objects on the enclosed <see cref="Type"/> of the specified <paramref name="decorator"/> and its inheritance chain.</returns>
    public static IEnumerable<EventInfo> GetAllEvents(this IDecorator<Type> decorator, Action<MemberReflectionOptions> setup = null)
    {
        return GetInheritedTypes(decorator).SelectMany(type => type.GetEvents(MemberReflection.CreateFlags(setup ?? DefaultMemberReflectionSetup))).Distinct(DynamicEqualityComparer.Create<EventInfo>(pi => pi.Name.GetHashCode(), (pi1, pi2) => pi1.Name.Equals(pi2.Name, StringComparison.Ordinal)));
    }

    /// <summary>
    /// Retrieves a collection that represents all methods defined on the enclosed <see cref="Type"/> of the specified <paramref name="decorator"/> and its inheritance chain.
    /// </summary>
    /// <param name="decorator">The <see cref="Type"/> to extend.</param>
    /// <param name="setup">The <see cref="MemberReflectionOptions" /> which may be configured.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains all <see cref="MethodInfo"/> objects on the enclosed <see cref="Type"/> of the specified <paramref name="decorator"/> and its inheritance chain.</returns>
    public static IEnumerable<MethodInfo> GetAllMethods(this IDecorator<Type> decorator, Action<MemberReflectionOptions> setup = null)
    {
        return GetInheritedTypes(decorator).SelectMany(type => type.GetMethods(MemberReflection.CreateFlags(setup ?? DefaultMemberReflectionSetup))).Distinct(DynamicEqualityComparer.Create<MethodInfo>(pi => pi.Name.GetHashCode(), (pi1, pi2) => pi1.Name.Equals(pi2.Name, StringComparison.Ordinal)));
    }

    /// <summary>
    /// Retrieves a collection that represents all the properties defined on the enclosed <see cref="Type"/> of the <paramref name="decorator"/> except those defined on <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type to exclude properties on the enclosed <see cref="Type"/> of the <paramref name="decorator"/>.</typeparam>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>A collection of properties for the enclosed <see cref="Type"/> of the <paramref name="decorator"/> except those defined on <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IEnumerable<PropertyInfo> GetRuntimePropertiesExceptOf<T>(this IDecorator<Type> decorator)
    {
        Validator.ThrowIfNull(decorator);
        var baseProperties = typeof(T).GetRuntimeProperties();
        var typeProperties = decorator.Inner.GetRuntimeProperties();
        return typeProperties.Except(baseProperties, DynamicEqualityComparer.Create<PropertyInfo>(pi => Generate.HashCode32(FormattableString.Invariant($"{pi.Name}-{pi.PropertyType.Name}")), (x, y) => x.Name == y.Name && x.PropertyType.Name == y.PropertyType.Name));
    }

    /// <summary>
    /// Determines whether the enclosed <see cref="Type"/> of the <paramref name="decorator"/> contains one or more of the specified <paramref name="types"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="types">The types to be matched against.</param>
    /// <returns><c>true</c> if the enclosed <see cref="Type"/> of the <paramref name="decorator"/> contains one or more of the specified <paramref name="types"/>; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="types"/> cannot be null.
    /// </exception>
    public static bool HasTypes(this IDecorator<Type> decorator, params Type[] types)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(types);
        foreach (var tt in types)
        {
            var st = decorator.Inner;
            while (st != null)
            {
                if (st.IsGenericType && tt == st.GetGenericTypeDefinition()) { return true; }
                if (st == tt) { return true; }
                st = st.BaseType;
            }
        }
        return false;
    }

    /// <summary>
    /// Determines whether the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements one or more of the specified <paramref name="attributeTypes"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="attributeTypes">The attribute types to be matched against.</param>
    /// <returns>
    /// 	<c>true</c> if the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements one or more of the specified <paramref name="attributeTypes"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="attributeTypes"/> cannot be null.
    /// </exception>
    public static bool HasAttribute(this IDecorator<Type> decorator, params Type[] attributeTypes)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(attributeTypes);
        foreach (var attributeType in attributeTypes) { if (decorator.Inner.GetCustomAttributes(attributeType, true).Length != 0) { return true; } }
        foreach (var m in decorator.Inner.GetMembers())
        {
            if (Decorator.Enclose(m).HasAttribute(attributeTypes)) { return true; }
        }
        return false;
    }

    /// <summary>
    /// Determines whether the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements one or more of the specified <paramref name="interfaceTypes"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="interfaceTypes">The interface types to be matched against.</param>
    /// <returns>
    /// 	<c>true</c> if the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements one or more of the specified <paramref name="interfaceTypes"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="interfaceTypes"/> cannot be null.
    /// </exception>
    public static bool HasInterfaces(this IDecorator<Type> decorator, params Type[] interfaceTypes)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfNull(interfaceTypes);
        var si = decorator.Inner.IsInterface ? Arguments.Yield(decorator.Inner).Concat(decorator.Inner.GetInterfaces()).ToList() : decorator.Inner.GetInterfaces().ToList();
        foreach (var ti in interfaceTypes.Where(t => t.IsInterface))
        {
            foreach (var i in si)
            {
                if (i.IsGenericType && ti == i.GetGenericTypeDefinition()) { return true; }
                if (ti == i) { return true; }
            }
        }
        return false;
    }

    /// <summary>
    /// Determines whether the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements either <see cref="DictionaryEntry"/> or <see cref="KeyValuePair{TKey,TValue}"/>.; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool HasKeyValuePairImplementation(this IDecorator<Type> decorator)
    {
        return HasTypes(decorator, typeof(KeyValuePair<,>), typeof(DictionaryEntry));
    }

    /// <summary>
    /// Determines whether the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements either <see cref="IEqualityComparer"/> or <see cref="IEqualityComparer{T}"/>; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool HasEqualityComparerImplementation(this IDecorator<Type> decorator)
    {
        return HasInterfaces(decorator, typeof(IEqualityComparer), typeof(IEqualityComparer<>));
    }

    /// <summary>
    /// Determines whether the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements either <see cref="IComparable"/> or <see cref="IComparable{T}"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements either <see cref="IComparable"/> or <see cref="IComparable{T}"/>; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool HasComparableImplementation(this IDecorator<Type> decorator)
    {
        return HasInterfaces(decorator, typeof(IComparable), typeof(IComparable<>));
    }

    /// <summary>
    /// Determines whether the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements either <see cref="IComparer"/> or <see cref="IComparer{T}"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements either <see cref="IComparer"/> or <see cref="IComparer{T}"/>; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool HasComparerImplementation(this IDecorator<Type> decorator)
    {
        return HasInterfaces(decorator, typeof(IComparer), typeof(IComparer<>));
    }

    /// <summary>
    /// Determines whether the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements either <see cref="IEnumerable"/> or <see cref="IEnumerable{T}"/>; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool HasEnumerableImplementation(this IDecorator<Type> decorator)
    {
        return HasInterfaces(decorator, typeof(IEnumerable), typeof(IEnumerable<>));
    }

    /// <summary>
    /// Determines whether the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the underlying <see cref="Type"/> of the <paramref name="decorator"/> implements either <see cref="IDictionary"/>, <see cref="IDictionary{TKey,TValue}"/> or <see cref="IReadOnlyDictionary{TKey,TValue}"/>; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool HasDictionaryImplementation(this IDecorator<Type> decorator)
    {
        return HasInterfaces(decorator, typeof(IDictionary), typeof(IDictionary<,>), typeof(IReadOnlyDictionary<,>));
    }

    /// <summary>
    /// Determines whether the underlying <see cref="Type"/> of the <paramref name="decorator"/> is a nullable <see cref="ValueType"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>
    ///   <c>true</c> if the underlying <see cref="Type"/> of the <paramref name="decorator"/> is nullable; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsNullable(this IDecorator<Type> decorator)
    {
        Validator.ThrowIfNull(decorator);
        if (!decorator.Inner.IsValueType) { return false; }
        return Nullable.GetUnderlyingType(decorator.Inner) != null;
    }

    /// <summary>
    /// Determines whether the underlying <see cref="Type"/> of the <paramref name="decorator"/> suggest an anonymous implementation (be that in a form of a type, delegate or lambda expression).
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the underlying <see cref="Type"/> of the <paramref name="decorator"/> suggest an anonymous implementation; otherwise, <c>false</c>.</returns>
    /// <remarks>If you can avoid it, don't use this method. It is - to say the least - fragile.</remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool HasAnonymousCharacteristics(this IDecorator<Type> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return decorator.Inner.GetCustomAttribute<CompilerGeneratedAttribute>() != null && decorator.Inner.IsClass && decorator.Inner.IsSealed && decorator.Inner.BaseType == typeof(object);
    }

    /// <summary>
    /// Determines whether the underlying <see cref="Type"/> of the <paramref name="decorator"/> has a default constructor.
    /// </summary>
    /// <returns><c>true</c> if the underlying <see cref="Type"/> of the <paramref name="decorator"/> has a default constructor; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool HasDefaultConstructor(this IDecorator<Type> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return decorator.Inner.IsValueType || decorator.Inner.GetConstructor(Type.EmptyTypes) != null;
    }

    /// <summary>
    /// Gets a collection (inherited-to-self) of inherited / ancestor types from the underlying <see cref="Type"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns>An <see cref="IEnumerable{Type}"/> that contains the inherited types from the underlying <see cref="Type"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IEnumerable<Type> GetInheritedTypes(this IDecorator<Type> decorator)
    {
        Validator.ThrowIfNull(decorator);
        var pt = new Stack<Type>();
        var ct = decorator.Inner;
        while (ct != null)
        {
            pt.Push(ct);
            ct = ct.GetTypeInfo().BaseType;
        }
        return pt;
    }

    /// <summary>
    /// Gets a collection (self-to-derived) of derived / descendant types from the underlying <see cref="Type"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="assemblies">The assemblies to include in the search of derived types.</param>
    /// <returns>An <see cref="IEnumerable{Type}"/> that contains the derived types from the underlying <see cref="Type"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IEnumerable<Type> GetDerivedTypes(this IDecorator<Type> decorator, params Assembly[] assemblies)
    {
        Validator.ThrowIfNull(decorator);
        var ac = new List<Assembly>(assemblies ?? Enumerable.Empty<Assembly>());
        if (!ac.Contains(decorator.Inner.Assembly)) { ac.Add(decorator.Inner.Assembly); }
        var dt = new List<Type>();
        foreach (var a in ac)
        {
            dt.AddRange(Decorator.Enclose(a).GetTypes(typeFilter: decorator.Inner));
        }
        return dt;
    }

    /// <summary>
    /// Gets a collection (inherited-to-self-to-derived) of inherited / ancestor and derived / descendant types from the underlying <see cref="Type"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="assemblies">The assemblies to include in the search of derived types.</param>
    /// <returns>An <see cref="IEnumerable{Type}"/> that contains a sorted (base-to-derived) collection of inherited and derived types from the underlying <see cref="Type"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static IEnumerable<Type> GetHierarchyTypes(this IDecorator<Type> decorator, params Assembly[] assemblies)
    {
        var ancestor = GetInheritedTypes(decorator);
        var descendant = GetDerivedTypes(decorator, assemblies);
        return ancestor.Concat(descendant).Distinct();
    }

    /// <summary>
    /// Gets the default value from the underlying <see cref="Type"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <returns>The default value from the underlying <see cref="Type"/> of the <paramref name="decorator"/>.</returns>
    /// <remarks>Usage is primarily intended for <c>struct</c>.</remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static object GetDefaultValue(this IDecorator<Type> decorator)
    {
        if (HasDefaultConstructor(decorator) && Nullable.GetUnderlyingType(decorator.Inner) == null) { return Activator.CreateInstance(decorator.Inner); }
        return null;
    }

    /// <summary>
    /// Determines whether the underlying <see cref="Type"/> of the <paramref name="decorator"/> is considered complex in its nature.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <returns><c>true</c> if the underlying <see cref="Type"/> of the <paramref name="decorator"/> is considered complex in its nature; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static bool IsComplex(this IDecorator<Type> decorator)
    {
        Validator.ThrowIfNull(decorator);
        return IsComplex(decorator.Inner);
    }

    /// <summary>
    /// Returns a human-readable <see cref="string"/> that represents the underlying <see cref="Type"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="setup">The <see cref="TypeNameOptions"/> which may be configured.</param>
    /// <returns>A human readable <see cref="string"/> that represents the underlying <see cref="Type"/> of the <paramref name="decorator"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static string ToFriendlyName(this IDecorator<Type> decorator, Action<TypeNameOptions> setup = null)
    {
        Validator.ThrowIfNull(decorator);
        Validator.ThrowIfInvalidConfigurator(setup, out var options);
        var typeName = options.FriendlyNameStringConverter(decorator.Inner, options.FormatProvider, options.FullName);
        if (options.ExcludeGenericArguments || !decorator.Inner.GetTypeInfo().IsGenericType) { return typeName; }
        return string.Format(options.FormatProvider, "{0}<{1}>", typeName, DelimitedString.Create(decorator.Inner.GetGenericArguments(), o =>
        {
            o.Delimiter = options.FormatProvider is CultureInfo ci ? ci.TextInfo.ListSeparator : ",";
            o.StringConverter = type => options.FriendlyNameStringConverter(type, options.FormatProvider, options.FullName);
        }));
    }

    /// <summary>
    /// Determines whether the specified <paramref name="source"/> (which type must be the same as the underlying <see cref="Type"/> of the <paramref name="decorator"/>) has a circular reference.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="source">The source to check for circular reference.</param>
    /// <param name="maxDepth">The maximum depth to traverse of <paramref name="source"/>.</param>
    /// <param name="valueResolver">The function delegate that is invoked when a property can be read and is of same type as the underlying <see cref="Type"/> of the <paramref name="decorator"/>.</param>
    /// <returns><c>true</c> if the specified <paramref name="source"/> has a circular reference; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="source"/> has a different type than the underlying type of <paramref name="decorator"/>.
    /// </exception>
    public static bool HasCircularReference(this IDecorator<Type> decorator, object source, int maxDepth = 2, Func<object, PropertyInfo, object> valueResolver = null)
    {
        Validator.ThrowIfNull(source);
        Validator.ThrowIfLowerThanOrEqual(maxDepth, 0, nameof(maxDepth));
        if (source.GetType() != decorator.Inner) { throw new InvalidOperationException("The specified source has a different type than the underlying type of the extended decorator."); }
        valueResolver ??= (s, i) => Decorator.RawEnclose(s).DefaultPropertyValueResolver(i);
        var hasCircularReference = false;
        var currentDepth = 0;
        var stack = new Stack<object>();
        stack.Push(source);
        while (stack.Count != 0 && currentDepth <= maxDepth)
        {
            var current = stack.Pop();
            foreach (var property in decorator.Inner.GetProperties())
            {
                if (property.CanRead && property.PropertyType == decorator.Inner)
                {
                    var propertyValue = valueResolver(current, property);
                    if (propertyValue != null)
                    {
                        stack.Push(propertyValue);
                        hasCircularReference = currentDepth == maxDepth;
                    }
                }
            }
            currentDepth++;
        }
        return hasCircularReference;
    }

    /// <summary>
    /// Conduct a search for <paramref name="memberName"/> using the specified <paramref name="setup"/> on the underlying <see cref="Type"/> of the <paramref name="decorator"/>.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="memberName">The name of the member on the underlying <see cref="Type"/> of the <paramref name="decorator"/>.</param>
    /// <param name="setup">The <see cref="MethodBaseOptions"/> which may be configured.</param>
    /// <returns>A <see cref="MethodBase"/> object representing the method that matches the specified requirements, if found on the underlying <see cref="Type"/> of the <paramref name="decorator"/>; otherwise, <c>null</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null -or-
    /// <paramref name="memberName"/> cannot be null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="memberName"/> cannot be empty or consist only of white-space characters.
    /// </exception>
    public static MethodBase MatchMember(this IDecorator<Type> decorator, string memberName, Action<MethodBaseOptions> setup = null)
    {
        Validator.ThrowIfNullOrWhitespace(memberName);
        var options = Patterns.Configure(setup);
        var methods = decorator.Inner.GetMethods(options.Flags).Where(info => info.Name.Equals(memberName, options.Comparison)).ToList();
        var matchedMethod = Parse(methods, options.Types);
        if (matchedMethod != null) { return matchedMethod; }
        throw new AmbiguousMatchException(FormattableString.Invariant($"Ambiguous matching in method resolution. Found {methods.Count} matching the member name of {memberName}. Consider specifying the signature of the member."));
    }

    private static MethodInfo Parse(IReadOnlyList<MethodInfo> methods, Type[] types)
    {
        if (methods.Count == 0) { return null; }
        if (methods.Count == 1) { return methods[0]; }
        if (methods.Count > 1 && types == null) { return null; }
        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            if (parameters.Length == types.Length)
            {
                var match = true;
                for (var i = 0; i < parameters.Length; i++)
                {
                    match &= parameters[i].ParameterType == types[i];
                }
                if (match) { return method; }
            }
        }
        return null;
    }

    private static bool IsComplex(params Type[] types)
    {
        var result = true;
        foreach (var source in types)
        {
            if (!ComplexValueTypeLookup.TryGetValue(source.AssemblyQualifiedName ?? $"{source.Name}|{source.GUID:N}", out var isPrimitive))
            {
                var sourceInfo = source.GetTypeInfo();
                if (sourceInfo.IsGenericType)
                {
                    var generics = source.GetGenericArguments().ToList();
                    if (sourceInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsComplex(generics[0]);
                    }
                    return IsComplex(generics.ToArray());
                }
                isPrimitive = sourceInfo.IsPrimitive;
                isPrimitive |= sourceInfo.IsEnum;
                isPrimitive |= sourceInfo.IsValueType && IsSimple(source);
                isPrimitive |= source == typeof(string);
                isPrimitive |= source == typeof(decimal);
                if (!ComplexValueTypeLookup.ContainsKey(sourceInfo.AssemblyQualifiedName ?? $"{source.Name}|{source.GUID:N}"))
                {
                    ComplexValueTypeLookup.TryAdd(source.AssemblyQualifiedName ?? $"{source.Name}|{source.GUID:N}", isPrimitive);
                }
            }
            result &= isPrimitive;
        }
        return !result;
    }

    private static bool IsSimple(Type type)
    {
        var simple = type.IsPrimitive;
        if (!simple)
        {
            var constructors = type.GetConstructors(new MemberReflection(true, true));
            var propertyNames = type.GetProperties(new MemberReflection(true, true)).Where(p => p.CanRead && !p.IsSpecialName).Select(p => p.Name).ToList();
            foreach (var constructor in constructors)
            {
                var arguments = constructor.GetParameters().Select(p => p.Name).ToList();
                var match = arguments.Intersect(propertyNames, StringComparer.OrdinalIgnoreCase).ToList();
                if (arguments.Count == match.Count)
                {
                    return true;
                }
            }

            var staticMethods = type.GetMethods(new MemberReflection(excludeInheritancePath: true)).Where(info => info.ReturnType == type && info.IsStatic).ToList();
            foreach (var staticMethod in staticMethods)
            {
                var parameters = staticMethod.GetParameters().Select(p => p.Name).ToList();
                var match = parameters.Intersect(propertyNames, StringComparer.OrdinalIgnoreCase).ToList();
                if (parameters.Count == match.Count)
                {
                    return true;
                }
            }
        }
        return simple;
    }
}
