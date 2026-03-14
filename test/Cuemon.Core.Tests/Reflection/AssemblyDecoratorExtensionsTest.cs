using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Cuemon.Assets;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions;
using Xunit;

namespace Cuemon.Reflection
{
    public class AssemblyDecoratorExtensionsTest : Test
    {
        public AssemblyDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void IsDebugBuild_ShouldBeTrueForDebugOrFalseForRelease()
        {
#if RELEASE
            Assert.False(Decorator.Enclose(this.GetType().Assembly).IsDebugBuild());
#else
            Assert.True(Decorator.Enclose(this.GetType().Assembly).IsDebugBuild());
#endif
        }

        [Fact]
        public void GetTypes_ShouldReturnAllTypesFromCuemonCore()
        {
            var a = typeof(SystemSnapshots).Assembly;

            var allTypes = Decorator.Enclose(a).GetTypes();
            var disposableTypes = Decorator.Enclose(a).GetTypes(typeFilter: typeof(Disposable));
            var configurationTypes = Decorator.Enclose(a).GetTypes($"{nameof(Cuemon)}.{nameof(Configuration)}");

            var allTypesCount = Decorator.Enclose(allTypes).Inner.Count();
            var disposableTypesCount = Decorator.Enclose(disposableTypes).Inner.Count();
            var configurationTypesCount = Decorator.Enclose(configurationTypes).Inner.Count();

            TestOutput.WriteLine(disposableTypes.ToDelimitedString());
            TestOutput.WriteLine(configurationTypes.ToDelimitedString());

            Assert.InRange(allTypesCount, 250, 300); // range because of tooling on CI adding dynamic types and high range of refactoring
            Assert.Equal(3, disposableTypesCount);
            Assert.Equal(2, configurationTypesCount);
        }

        [Fact]
        public void GetAssemblyVersion_ShouldReturnAssemblyVersion()
        {
            var a = typeof(Disposable).Assembly;
            var v = Decorator.Enclose(a).GetAssemblyVersion();

            TestOutput.WriteLine(v.ToString());

            Assert.Equal("11.0.0.0", v.ToString());
            Assert.False(v.HasAlphanumericVersion);
            Assert.False(v.IsSemanticVersion());
        }

        [Fact]
        public void GetFileVersion_ShouldReturnFileVersion()
        {
            var a = typeof(Disposable).Assembly;
            var v = Decorator.Enclose(a).GetFileVersion();
            var fva = a.GetCustomAttribute<AssemblyFileVersionAttribute>();

            TestOutput.WriteLine(v.ToString());

            Assert.False(v.IsSemanticVersion());
            Assert.StartsWith(fva.Version, v.ToString());
        }

        [Fact]
        public void GetProductVersion_ShouldReturnProductVersion()
        {
            var a = typeof(Disposable).Assembly;
            var v = Decorator.Enclose(a).GetProductVersion();
            var iva = a.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            TestOutput.WriteLine(v.ToString());

            Assert.True(v.IsSemanticVersion());
            Assert.True(v.HasAlphanumericVersion);
            Assert.Equal(iva.InformationalVersion, v.Value);
        }

        [Fact]
        public void GetManifestResources_ShouldRetrieveCultureInfoSpecificCultures()
        {
            var a = typeof(ClassBase).Assembly;
            var erbn = Decorator.Enclose(a).GetManifestResources($"{nameof(Cuemon)}.{nameof(Assets)}.CultureInfo.SpecificCultures.dsv");
            var erbnv = erbn.Single().Value;
            var erbce = Decorator.Enclose(a).GetManifestResources(".d", ManifestResourceMatch.ContainsExtension);
            var erbcev = erbn.Single().Value;
            var erbcn = Decorator.Enclose(a).GetManifestResources("SpecificCultures", ManifestResourceMatch.ContainsName);
            var erbcnv = erbcn.Single().Value;
            var erbe = Decorator.Enclose(a).GetManifestResources(".dsv", ManifestResourceMatch.Extension);
            var erbev = erbe.Single().Value;

            Assert.Throws<ArgumentNullException>(() => Decorator.Enclose(a).GetManifestResources(null));
            Assert.Throws<ArgumentException>(() => Decorator.Enclose(a).GetManifestResources(""));
            Assert.Throws<InvalidEnumArgumentException>(() => Decorator.Enclose(a).GetManifestResources("SpecificCultures", (ManifestResourceMatch)22));

            Assert.NotNull(erbn);
            Assert.True(erbn.Count == 1);
            Assert.NotNull(erbnv);
            Assert.True(erbnv.Length > 0);

            Assert.NotNull(erbce);
            Assert.True(erbce.Count == 1);
            Assert.NotNull(erbcev);
            Assert.True(erbcev.Length > 0);

            Assert.NotNull(erbcn);
            Assert.True(erbcn.Count == 1);
            Assert.NotNull(erbcnv);
            Assert.True(erbcnv.Length > 0);

            Assert.NotNull(erbe);
            Assert.True(erbe.Count == 1);
            Assert.NotNull(erbev);
            Assert.True(erbev.Length > 0);
        }
    }
}
