using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters.ModelBinding;
/// <summary>
/// Provides a generic way to disable <see cref="IValueProviderFactory"/> implementations used for model binding.
/// </summary>
/// <seealso cref="Attribute" />
/// <remarks>
/// This attribute was inspired by this source on GitHub: https://github.com/aspnet/Entropy/blob/rel/1.1.1/samples/Mvc.FileUpload/Filters/DisableFormValueModelBindingAttribute.cs.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class DisableModelBindingAttribute : Attribute, IAsyncResourceFilter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisableModelBindingAttribute"/> class.
    /// </summary>
    /// <param name="valueProviderFactoryType">The type that needs to be disabled on class or method level.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="valueProviderFactoryType"/> cannot be null.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// Only a type that implements the <see cref="IValueProviderFactory"/> interface is supported.
    /// </exception>
    public DisableModelBindingAttribute(Type valueProviderFactoryType)
    {
        Validator.ThrowIfNull(valueProviderFactoryType);
        if (!Decorator.Enclose(valueProviderFactoryType).HasInterfaces(typeof(IValueProviderFactory))) { throw new NotSupportedException("Only a type that implements the IValueProviderFactory interface is supported."); }
        ValueProviderFactoryType = valueProviderFactoryType;
    }

    /// <summary>
    /// Gets the type that needs to be disabled on class or method level.
    /// </summary>
    /// <value>The type that needs to be disabled on class or method level.</value>
    public Type ValueProviderFactoryType { get; }

    /// <summary>
    /// Called asynchronously before the rest of the pipeline.
    /// </summary>
    /// <param name="context">The <see cref="ResourceExecutingContext" />.</param>
    /// <param name="next">The <see cref="ResourceExecutionDelegate" />. Invoked to execute the next resource filter or the remainder
    /// of the pipeline.</param>
    /// <returns>A <see cref="Task" /> which will complete when the remainder of the pipeline completes.</returns>
    public Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        context.ValueProviderFactories.RemoveType(ValueProviderFactoryType);
        return next();
    }
}
