using System;

namespace Cuemon.Assets;
public class FailPostConfigurableOptions : PostConfigurableOptions
{
    public FailPostConfigurableOptions()
    {
    }

    public Guid Id { get; set; }

    public override void PostConfigureOptions()
    {
    }
}
