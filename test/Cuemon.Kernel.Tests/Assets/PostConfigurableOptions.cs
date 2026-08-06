using System;
using Cuemon.Configuration;

namespace Cuemon.Assets;
public class PostConfigurableOptions : IPostConfigurableParameterObject, IValidatableParameterObject
{
    public PostConfigurableOptions()
    {
    }

    public Guid Id { get; set; }

    public virtual void PostConfigureOptions()
    {
        Id = Guid.NewGuid();
    }

    public void ValidateOptions()
    {
        Validator.ThrowIfInvalidState(Id == Guid.Empty);
    }
}
