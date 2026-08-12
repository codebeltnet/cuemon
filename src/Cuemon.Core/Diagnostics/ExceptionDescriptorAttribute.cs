using System;
using Cuemon.Collections.Generic;
using Cuemon.Globalization;

namespace Cuemon.Diagnostics;
/// <summary>
/// Provides information about an <see cref="Exception"/>, in a developer friendly way, optimized for open- and otherwise public application programming interfaces (API).
/// </summary>
/// <seealso cref="Attribute" />
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ExceptionDescriptorAttribute : ResourceAttribute
{
    private string _helpLink;
    private string _message;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionDescriptorAttribute"/> class.
    /// </summary>
    /// <param name="failureType">The <see cref="Type"/> of the failure to match and describe.</param>
    public ExceptionDescriptorAttribute(Type failureType)
    {
        Validator.ThrowIfNull(failureType);
        Validator.ThrowIfNotContainsType(failureType, Arguments.ToArrayOf(typeof(Exception)), "The specified type is not an Exception.");
        FailureType = failureType;
    }

    /// <summary>
    /// Gets or sets an error code that uniquely identifies the type of failure.
    /// </summary>
    /// <value>The number that identifies the type of failure.</value>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets a default message that describes the current failure.
    /// </summary>
    /// <value>The default message that explains the reason for the failure.</value>
    public string Message
    {
        get
        {
            string localizedMessage = null;
            if (ResourceType != null)
            {
                localizedMessage = GetString(MessageResourceName);
            }
            return localizedMessage ?? _message;
        }
        set => _message = value;
    }

    /// <summary>
    /// Gets or sets the resource name (property name) to use as the key for looking up a localized message string.
    /// </summary>
    /// <value>The resource name (property name) to use as the key for looking up a localized message string that describes the current failure.</value>
    public string MessageResourceName { get; set; }

    /// <summary>
    /// Gets or sets a link to the help page associated with this failure.
    /// </summary>
    /// <value>The location of an optional help page associated with this failure.</value>
    public string HelpLink
    {
        get => _helpLink;
        set
        {
            if (value != null) { Validator.ThrowIfNotUri(value); }
            _helpLink = value;
        }
    }

    /// <summary>
    /// Gets the <see cref="Type"/> of <see cref="Exception"/> to match and describe.
    /// </summary>
    /// <value>The <see cref="Type"/> of <see cref="Exception"/> to match.</value>
    public Type FailureType { get; }
}
