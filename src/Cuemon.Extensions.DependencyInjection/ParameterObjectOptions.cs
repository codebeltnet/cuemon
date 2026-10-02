using System;
using Cuemon.Configuration;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.DependencyInjection;

internal sealed class ParameterObjectOptions<TOptions> : IPostConfigureOptions<TOptions>, IValidateOptions<TOptions>
    where TOptions : class, IParameterObject, new()
{
    public void PostConfigure(string name, TOptions options)
    {
        if (options is IPostConfigurableParameterObject postConfigurable)
        {
            postConfigurable.PostConfigureOptions();
        }
    }

    public ValidateOptionsResult Validate(string name, TOptions options)
    {
        if (options is not IValidatableParameterObject validatable)
        {
            return ValidateOptionsResult.Skip;
        }

        try
        {
            validatable.ValidateOptions();
            return ValidateOptionsResult.Success;
        }
        catch (Exception e) when (Patterns.IsRecoverableException(e))
        {
            return ValidateOptionsResult.Fail(e.Message);
        }
    }
}
