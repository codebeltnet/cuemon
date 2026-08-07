using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Collections.Generic;

namespace Cuemon.Reflection;
/// <summary>
/// Provides a generic way to rehydrate serialized objects.
/// </summary>
public class MemberParser
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MemberParser"/> class.
    /// </summary>
    /// <param name="source">The <see cref="Type"/> of the source to rehydrate.</param>
    /// <param name="memberArguments">The arguments associated with the <paramref name="source"/>.</param>
    public MemberParser(Type source, IEnumerable<MemberArgument> memberArguments)
    {
        Source = source;
        MemberArguments = memberArguments;
    }

    /// <summary>
    /// Gets the <see cref="Type"/> of the source to rehydrate.
    /// </summary>
    /// <value>The <see cref="Type"/> of the source to rehydrate.</value>
    public Type Source { get; }

    /// <summary>
    /// Gets the arguments associated with the source to rehydrate.
    /// </summary>
    /// <value>The arguments associated with the source to rehydrate.</value>
    public IEnumerable<MemberArgument> MemberArguments { get; }

    /// <summary>
    /// Gets the processed arguments associated with the source to rehydrate.
    /// </summary>
    /// <value>The arguments associated with the source to rehydrate.</value>
    /// <remarks>This can be useful for debugging why an instance is not rehydrated as expected.</remarks>
    public IEnumerable<MemberArgument> ProcessedMemberArguments { get; private set; } = new List<MemberArgument>();

    /// <summary>
    /// Creates an instance of the <see cref="Type"/> associated with this parser.
    /// </summary>
    /// <param name="predicate">A function to test each <see cref="ConstructorInfo"/> for a condition.</param>
    /// <returns>An instance equivalent to the <see cref="Type"/> associated with this parser.</returns>
    public object CreateInstance(Func<ConstructorInfo, bool> predicate = null)
    {
        if (predicate == null && Patterns.TryInvoke(() => Activator.CreateInstance(Source), out var instance))
        {
            ProcessedMemberArguments = Populate(instance, MemberArguments);
        }
        else
        {
            var processedConstructorMembers = Populate(predicate ?? (_ => true), MemberArguments, out instance);
            var processedMembers = Populate(instance, MemberArguments.Except(processedConstructorMembers, DynamicEqualityComparer.Create<MemberArgument>(member => member.Name.GetHashCode(), (m1, m2) => m1.Name.Equals(m2.Name, StringComparison.Ordinal))));
            ProcessedMemberArguments = processedConstructorMembers.Concat(processedMembers);
        }
        return instance;
    }

    private IEnumerable<MemberArgument> Populate(Func<ConstructorInfo, bool> predicate, IEnumerable<MemberArgument> members, out object instance)
    {
        var constructors = Source!.GetConstructors(MemberReflection.CreateFlags(o => o.ExcludeStatic = true)).Where(predicate).Reverse().ToList();
        var processedMembers = new List<MemberArgument>();
        var arguments = new List<object>();
        foreach (var ctor in constructors) // 1:1 match with constructor
        {
            var parameters = ctor.GetParameters();
            var matchingMembersCount = parameters.Select(info => info.Name).Intersect(members.Select(member => member.Name), StringComparer.OrdinalIgnoreCase).Count();
            if (parameters.Length == matchingMembersCount)
            {
                foreach (var parameter in parameters)
                {
                    var member = members.First(member => member.Name.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase));
                    arguments.Add(Decorator.Enclose(parameter.ParameterType).IsComplex()
                        ? member.Value
                        : Decorator.Enclose(member.Value).ChangeType(parameter.ParameterType));
                    processedMembers.Add(member);
                }
                break;
            }
        }

        if (arguments.Count == 0) // unable to locate a 1:1 match; do a partial match instead
        {
            foreach (var ctor in constructors)
            {
                var parameters = ctor.GetParameters();
                var matchingMembersCount = parameters.Select(info => info.Name).Count(parameterName => members.Any(member => parameterName.EndsWith(member.Name, StringComparison.OrdinalIgnoreCase)));
                if (parameters.Length == matchingMembersCount)
                {
                    foreach (var parameter in parameters)
                    {
                        var member = members.First(member => parameter.Name.EndsWith(member.Name, StringComparison.OrdinalIgnoreCase));
                        arguments.Add(Decorator.Enclose(parameter.ParameterType).IsComplex()
                            ? member.Value
                            : Decorator.Enclose(member.Value).ChangeType(parameter.ParameterType));
                        processedMembers.Add(member);
                    }
                    break;
                }
            }
        }

        instance = Activator.CreateInstance(Source, arguments.ToArray());
        return processedMembers;
    }

    private IEnumerable<MemberArgument> Populate(object instance, IEnumerable<MemberArgument> members)
    {
        var fields = Decorator.Enclose(Source).GetAllFields().ToList();
        var properties = Decorator.Enclose(Source).GetAllProperties().ToList();
        var processedMembers = new List<MemberArgument>();
        foreach (var member in members.OrderBy(member => member.Priority))
        {
            var property = properties.SingleOrDefault(pi => pi.Name.Equals(member.Name, StringComparison.OrdinalIgnoreCase));
            if (property != null && property.CanWrite)
            {
                property.SetValue(instance, Decorator.Enclose(property.PropertyType).IsComplex()
                    ? member.Value
                    : Decorator.Enclose(member.Value).ChangeType(property.PropertyType));
                processedMembers.Add(member);
            }
            else // fallback to potential backing field
            {
                var field = fields.SingleOrDefault(fi => fi.Name.EndsWith(member.Name, StringComparison.OrdinalIgnoreCase));
                if (field != null)
                {
                    field.SetValue(instance, Decorator.Enclose(field.FieldType).IsComplex()
                        ? member.Value
                        : Decorator.Enclose(member.Value).ChangeType(field.FieldType));
                    processedMembers.Add(member);
                }
            }
        }
        return processedMembers;
    }
}
