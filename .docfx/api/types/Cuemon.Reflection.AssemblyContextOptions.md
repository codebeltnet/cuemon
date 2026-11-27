---
uid: Cuemon.Reflection.AssemblyContextOptions
example:
- *content
---

The following example demonstrates how to configure <xref:Cuemon.Reflection.AssemblyContextOptions> to control which assemblies are returned by [GetCurrentDomainAssemblies](https://docs.cuemon.net/api/dotnet/Cuemon.Reflection.AssemblyContext.html#Cuemon_Reflection_AssemblyContext_GetCurrentDomainAssemblies_System_Action_Cuemon_Reflection_AssemblyContextOptions__).

```csharp
// Program.cs
using System;
using System.Linq;
using System.Reflection;
using Cuemon.Reflection;
using MyApp.Examples;

new AssemblyContextOptionsExample().FilterAssemblies();

namespace MyApp.Examples
{
    public class AssemblyContextOptionsExample
    {
        public void FilterAssemblies()
        {
            // Configure to include only assemblies whose name contains "Cuemon"
            // and exclude the current assembly from results
            var assemblies = AssemblyContext.GetCurrentDomainAssemblies(o =>
            {
                o.AssemblyFilter = assembly =>
                    assembly.FullName.StartsWith("Cuemon", StringComparison.Ordinal);

                o.ReferencedAssemblyFilter = assemblyName =>
                    assemblyName.FullName.StartsWith("Cuemon", StringComparison.Ordinal);

                o.IncludeReferencedAssemblies = true;

                // Remove the default exclusion of Cuemon.Core
                o.ExcludedAssemblies.Clear();
            });

            foreach (var assembly in assemblies)
            {
                Console.WriteLine(assembly.GetName().Name);
            }
        }

        public void UseDefaults()
        {
            // Default options exclude System and Microsoft assemblies
            var options = new AssemblyContextOptions();
            Console.WriteLine($"IncludeReferencedAssemblies: {options.IncludeReferencedAssemblies}"); // true
            Console.WriteLine($"AssemblyFilter: {(options.AssemblyFilter != null ? "set" : "null")}");
            Console.WriteLine($"ReferencedAssemblyFilter: {(options.ReferencedAssemblyFilter != null ? "set" : "null")}");
            Console.WriteLine($"ExcludedAssemblies count: {options.ExcludedAssemblies.Count}");
        }

        public void GetCuemonAssemblies()
        {
            // Get all Cuemon assemblies in the current domain
            var cuemonAssemblies = AssemblyContext.GetCurrentDomainAssemblies(o =>
            {
                o.AssemblyFilter = a => a.FullName.StartsWith("Cuemon", StringComparison.Ordinal);
                o.ReferencedAssemblyFilter = an => an.FullName.StartsWith("Cuemon", StringComparison.Ordinal);
                o.IncludeReferencedAssemblies = true;
                o.ExcludedAssemblies.Clear();
            });

            Console.WriteLine($"Found {cuemonAssemblies.Count} Cuemon assemblies:");
            foreach (var asm in cuemonAssemblies)
            {
                Console.WriteLine($"  - {asm.GetName().Name} v{asm.GetName().Version}");
            }
        }
    }
}
```

<!-- docfx-run-actions:start -->
<div class="docfx-run-actions" data-example-id="8799955b6476bef870749b1b"><a href="~/run-actions/8799955b6476bef870749b1b/native.ps1" data-transport="native">View Native Script</a> · <a href="~/run-actions/8799955b6476bef870749b1b/docker.ps1" data-transport="docker">View Docker Script</a></div>
<!-- docfx-run-actions:end -->
