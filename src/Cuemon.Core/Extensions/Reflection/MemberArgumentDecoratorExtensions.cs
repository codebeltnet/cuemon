using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Cuemon.Reflection;
/// <summary>
/// Extension methods for the <see cref="MemberArgument"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
/// </summary>
/// <seealso cref="IDecorator{T}"/>
/// <seealso cref="Decorator{T}"/>
public static class MemberArgumentDecoratorExtensions
{
    /// <summary>
    /// Converts the underlying <see cref="Stack{T}"/> of the <paramref name="decorator"/> that has one or more sequences of <see cref="MemberArgument"/> into an <see cref="Exception"/>.
    /// This API supports the product infrastructure and is not intended to be used directly from your code.
    /// </summary>
    /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
    /// <param name="parseAsXml">When <c>true</c>, the <see cref="Exception.Message"/> is parsed using <c>\n</c> as newline; otherwise <see cref="Environment.NewLine"/> is used. Reason for this design is explained here: https://www.w3.org/TR/REC-xml/#sec-line-ends</param>
    /// <returns>An instance of an <see cref="Exception"/> if the conversion was successful; <c>null</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="decorator"/> cannot be null.
    /// </exception>
    public static Exception CreateException(this IDecorator<Stack<IList<MemberArgument>>> decorator, bool parseAsXml = false)
    {
        Validator.ThrowIfNull(decorator);
        Exception instance = null;
        var stackOfMembers = decorator.Inner;
        while (stackOfMembers.Count > 0)
        {
            var memberArguments = stackOfMembers.Pop();
            var desiredType = memberArguments.Single(ma => ma.Name.Equals("type", StringComparison.OrdinalIgnoreCase)).Value as Type;
            if (Decorator.Enclose(desiredType).HasTypes(typeof(ArgumentException)))
            {
                var message = memberArguments.SingleOrDefault(ma => ma.Name.Equals("message", StringComparison.OrdinalIgnoreCase));
                if (message?.Value is string messageValue) // This hack will only work for MS provided default resource-strings ..it saddens me how Microsoft designed both ArgumentException and ArgumentOutOfRangeException completely disregarding Framework Design Guidelines when to use method over property: The operation returns a different result each time it is called, even if the parameters don’t change. For example, the Guid.NewGuid method returns a different value each time it is called.
                {
                    var argParamNameIndexOf = "'{0}'";
                    var resName = "System.Private.CoreLib.Strings";
#if NETSTANDARD2_0_OR_GREATER
                    argParamNameIndexOf = "{0}";
                    resName = "mscorlib";
#endif
                    var rm = new ResourceManager(resName, typeof(ArgumentException).Assembly);
                    var argParamName = rm.GetString("Arg_ParamName_Name");
                    argParamName = argParamName.Remove(argParamName.IndexOf(argParamNameIndexOf, StringComparison.Ordinal));
                    int indexOfMicrosoftParamName;
#if NETSTANDARD2_0_OR_GREATER
                    indexOfMicrosoftParamName = messageValue.LastIndexOf(string.Format(CultureInfo.InvariantCulture, "{0}", parseAsXml ? "\n" : Environment.NewLine) + argParamName, StringComparison.Ordinal);
#else
                    indexOfMicrosoftParamName = messageValue.LastIndexOf(" " + argParamName, StringComparison.Ordinal);
#endif
                    if (indexOfMicrosoftParamName > 0) { message.Value = messageValue.Remove(indexOfMicrosoftParamName); }
                }
            }

            var innerException = memberArguments.SingleOrDefault(ma => ma.Name.Equals(nameof(Exception.InnerException), StringComparison.OrdinalIgnoreCase));
            innerException?.Value = instance;

            var parser = new MemberParser(desiredType, memberArguments);

            instance = parser.CreateInstance() as Exception;
        }

        return instance;
    }
}
