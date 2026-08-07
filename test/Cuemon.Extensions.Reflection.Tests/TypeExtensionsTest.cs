using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection.Assets;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Extensions.Reflection;
public class TypeExtensionsTest : Test
{
    public TypeExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetDerivedTypes_ShouldHaveSelfToDerivedTypes()
    {
        var msType = typeof(Stream);
        var selfToDerived = msType.GetDerivedTypes();
        TestOutput.WriteLines(selfToDerived.Where(t => t.IsPublic));

        Assert.DoesNotContain(selfToDerived, t => t == typeof(object));
        Assert.DoesNotContain(selfToDerived, t => t == typeof(MarshalByRefObject));
        Assert.Contains(selfToDerived, t => t == typeof(Stream));
        Assert.Contains(selfToDerived, t => t == typeof(FileStream));
        Assert.Contains(selfToDerived, t => t == typeof(MemoryStream));
        Assert.Contains(selfToDerived, t => t == typeof(UnmanagedMemoryStream));
    }

    [Fact]
    public void GetInheritedTypes_ShouldHaveInheritedToSelfTypes()
    {
        var msType = typeof(Stream);
        var inheritedToSelf = msType.GetInheritedTypes();
        TestOutput.WriteLines(inheritedToSelf.Where(t => t.IsPublic));

        Assert.Contains(inheritedToSelf, t => t == typeof(object));
        Assert.Contains(inheritedToSelf, t => t == typeof(MarshalByRefObject));
        Assert.Contains(inheritedToSelf, t => t == typeof(Stream));
        Assert.DoesNotContain(inheritedToSelf, t => t == typeof(FileStream));
        Assert.DoesNotContain(inheritedToSelf, t => t == typeof(MemoryStream));
        Assert.DoesNotContain(inheritedToSelf, t => t == typeof(UnmanagedMemoryStream));
    }

    [Fact]
    public void GetHierarchyTypes_ShouldHaveInheritedToSelfToDerivedTypes()
    {
        var msType = typeof(Stream);
        var hierarchy = msType.GetHierarchyTypes();
        TestOutput.WriteLines(hierarchy.Where(t => t.IsPublic));

        Assert.Contains(hierarchy, t => t == typeof(object));
        Assert.Contains(hierarchy, t => t == typeof(MarshalByRefObject));
        Assert.Contains(hierarchy, t => t == typeof(Stream));
        Assert.Contains(hierarchy, t => t == typeof(FileStream));
        Assert.Contains(hierarchy, t => t == typeof(MemoryStream));
        Assert.Contains(hierarchy, t => t == typeof(UnmanagedMemoryStream));
    }

    [Fact]
    public void GetAllProperties_ShouldThrowArgumentNullException()
    {
        Type type = null;
        var sut = Assert.Throws<ArgumentNullException>(() => type.GetAllProperties());

        TestOutput.WriteLine(sut.ToString());

        Assert.Equal("source", sut.ParamName);
    }

    [Fact]
    public void GetAllProperties_ShouldIncludeFullInheritanceChainOfProperties()
    {
        var tae = new TypeArgumentOutOfRangeException("typeParameterName", 42, "message");
        var taeType = tae.GetType();
        var members = taeType.GetAllProperties();
        var expected = 13;

#if NET48_OR_GREATER
        expected = 14;
#endif

        TestOutput.WriteLines(members);

        Assert.Equal(expected, members.Count());
    }

    [Fact]
    public void GetAllEvents_ShouldThrowArgumentNullException()
    {
        Type type = null;
        var sut = Assert.Throws<ArgumentNullException>(() => type.GetAllEvents());

        TestOutput.WriteLine(sut.ToString());

        Assert.Equal("source", sut.ParamName);
    }

    [Fact]
    public void GetAllEvents_ShouldIncludeFullInheritanceChainOfEvents()
    {
        var tae = new TypeArgumentOutOfRangeException("typeParameterName", 42, "message");
        var taeType = tae.GetType();
        var members = taeType.GetAllEvents();
        var expected = 1;

        TestOutput.WriteLines(members);

        Assert.Equal(expected, members.Count());
    }

    [Fact]
    public void GetAllMethods_ShouldThrowArgumentNullException()
    {
        Type type = null;
        var sut = Assert.Throws<ArgumentNullException>(() => type.GetAllMethods());

        TestOutput.WriteLine(sut.ToString());

        Assert.Equal("source", sut.ParamName);
    }

    [Fact]
    public void GetAllMethods_ShouldIncludeFullInheritanceChainOfMethods()
    {
        var tae = new TypeArgumentOutOfRangeException("typeParameterName", 42, "message");
        var taeType = tae.GetType();
        var methods = taeType.GetAllMethods();
        var expected = 40;

#if NET48_OR_GREATER
        expected = 49;
#endif

        TestOutput.WriteLines(methods);

        Assert.Equal(expected, methods.Count());
    }

    [Fact]
    public void GetAllFields_ShouldThrowArgumentNullException()
    {
        Type type = null;
        var sut = Assert.Throws<ArgumentNullException>(() => type.GetAllFields());

        TestOutput.WriteLine(sut.ToString());

        Assert.Equal("source", sut.ParamName);
    }

    [Fact]
    public void GetAllFields_ShouldIncludeFullInheritanceChainOfFields()
    {
        var tae = new TypeArgumentOutOfRangeException("typeParameterName", 42, "message");
        var taeType = tae.GetType();
        var members = taeType.GetAllFields();
        var actualValueName = "_actualValue";
        var expected = 17;

#if NET48_OR_GREATER
        expected = 21;
        actualValueName = "m_actualValue";
#endif
#if NET9_0_OR_GREATER
        expected = 16;
#endif

        TestOutput.WriteLines(members);

        var actualValue = members.SingleOrDefault(fi => fi.Name == actualValueName);
        actualValue.SetValue(tae, 45);

        Assert.Equal(expected, members.Count());
        Assert.Equal(45, tae.ActualValue);
    }

    [Fact]
    public void GetEmbeddedResources_ShouldReadErTextFile()
    {
        var sut1 = this.GetType().GetEmbeddedResources("Cuemon.Extensions.Reflection.Assets.ER.txt", ManifestResourceMatch.Name);
        var sut2 = this.GetType().GetEmbeddedResources("xt", ManifestResourceMatch.ContainsExtension);
        var sut3 = this.GetType().GetEmbeddedResources(".txt", ManifestResourceMatch.Extension);
        var sut4 = this.GetType().GetEmbeddedResources("ER", ManifestResourceMatch.ContainsName);
        var sut5 = this.GetType().GetEmbeddedResources("Cuemon.Extensions.Reflection.Assets.ER.xxx", ManifestResourceMatch.Name);
        var sut6 = this.GetType().GetEmbeddedResources("xx", ManifestResourceMatch.ContainsExtension);
        var sut7 = this.GetType().GetEmbeddedResources(".xxx", ManifestResourceMatch.Extension);
        var sut8 = this.GetType().GetEmbeddedResources("XX", ManifestResourceMatch.ContainsName);

        Assert.True(sut1.Count == 1, "sut1.Count == 1");
        Assert.True(sut2.Count == 1, "sut2.Count == 1");
        Assert.True(sut3.Count == 1, "sut3.Count == 1");
        Assert.True(sut4.Count == 1, "sut4.Count == 1");
        Assert.True(sut1.Values.Single().ToEncodedString().Equals("Cuemon"));
        Assert.True(sut2.Values.Single().ToEncodedString().Equals("Cuemon"));
        Assert.True(sut3.Values.Single().ToEncodedString().Equals("Cuemon"));
        Assert.True(sut4.Values.Single().ToEncodedString().Equals("Cuemon"));

        Assert.True(sut5.Count == 0, "sut1.Count == 0");
        Assert.True(sut6.Count == 0, "sut2.Count == 0");
        Assert.True(sut7.Count == 0, "sut3.Count == 0");
        Assert.True(sut8.Count == 0, "sut4.Count == 0");
    }

    [Fact]
    public void GetRuntimePropertiesExceptOf_ShouldRetrieveRuntimePropertiesExceptTheSpecifiedExclusions()
    {
        var sut1 = new CustomException();
        var sut2 = sut1.GetType().GetRuntimePropertiesExceptOf<Exception>().ToList();
        var sut3 = sut1.GetType().GetRuntimePropertiesExceptOf<AggregateException>().ToList();
        var sut4 = sut1.GetType().GetRuntimeProperties().ToList();

        foreach (var pi in sut2)
        {
            TestOutput.WriteLine(pi.Name);
        }

        TestOutput.WriteLine(" --- ");

        foreach (var pi in sut3)
        {
            TestOutput.WriteLine(pi.Name);
        }

        TestOutput.WriteLine(" --- ");

#if NET48_OR_GREATER
        Assert.True(sut2.Count == 3, "sut2.Count == 3"); // with .net framework things are even worse; microsoft - CONSISTENCY IS KEY
        Assert.True(sut3.Count == 2, "sut2.Count == 2");
        Assert.Equal(15, sut4.Count);

        foreach (var pi in sut4)
        {
            TestOutput.WriteLine(pi.Name);
        }

        Assert.Collection(sut2,
            pi => Assert.Equal("Code", pi.Name),
            pi => Assert.Equal("CodePhrase", pi.Name),
            pi => Assert.Equal("InnerExceptions", pi.Name));

        Assert.Collection(sut3,
            pi => Assert.Equal("Code", pi.Name),
            pi => Assert.Equal("CodePhrase", pi.Name));

        Assert.Collection(sut4,
            pi => Assert.Equal("Code", pi.Name),
            pi => Assert.Equal("CodePhrase", pi.Name),
            pi => Assert.Equal("InnerExceptions", pi.Name),
            pi => Assert.Equal("Message", pi.Name),
            pi => Assert.Equal("Data", pi.Name),
            pi => Assert.Equal("InnerException", pi.Name),
            pi => Assert.Equal("TargetSite", pi.Name),
            pi => Assert.Equal("StackTrace", pi.Name),
            pi => Assert.Equal("HelpLink", pi.Name),
            pi => Assert.Equal("Source", pi.Name),
            pi => Assert.Equal("IPForWatsonBuckets", pi.Name),
            pi => Assert.Equal("WatsonBuckets", pi.Name),
            pi => Assert.Equal("RemoteStackTrace", pi.Name),
            pi => Assert.Equal("HResult", pi.Name),
            pi => Assert.Equal("IsTransient", pi.Name));
#else
        Assert.True(sut2.Count == 5, "sut2.Count == 5"); // with .net 6 ms decided to add two extra internals; InnerExceptionCount and InternalInnerExceptions (yup; ugly)
        Assert.True(sut3.Count == 2, "sut2.Count == 2");
        Assert.Equal(13, sut4.Count);

        foreach (var pi in sut4)
        {
            TestOutput.WriteLine(pi.Name);
        }

        Assert.Collection(sut2,
            pi => Assert.Equal("Code", pi.Name),
            pi => Assert.Equal("CodePhrase", pi.Name),
            pi => Assert.Equal("InnerExceptions", pi.Name),
            pi => Assert.Equal("InnerExceptionCount", pi.Name),
            pi => Assert.Equal("InternalInnerExceptions", pi.Name));

        Assert.Collection(sut3,
            pi => Assert.Equal("Code", pi.Name),
            pi => Assert.Equal("CodePhrase", pi.Name));

        Assert.Collection(sut4,
            pi => Assert.Equal("Code", pi.Name),
            pi => Assert.Equal("CodePhrase", pi.Name),
            pi => Assert.Equal("InnerExceptions", pi.Name),
            pi => Assert.Equal("Message", pi.Name),
            pi => Assert.Equal("InnerExceptionCount", pi.Name),
            pi => Assert.Equal("InternalInnerExceptions", pi.Name),
            pi => Assert.Equal("TargetSite", pi.Name),
            pi => Assert.Equal("Data", pi.Name),
            pi => Assert.Equal("InnerException", pi.Name),
            pi => Assert.Equal("HelpLink", pi.Name),
            pi => Assert.Equal("Source", pi.Name),
            pi => Assert.Equal("HResult", pi.Name),
            pi => Assert.Equal("StackTrace", pi.Name));
#endif

    }

    [Fact]
    public void ToFullNameIncludingAssemblyName_ShouldWriteFullTypeNameIncludingAssemblyName()
    {
        var sut1 = this.GetType();
        var sut2 = sut1.ToFullNameIncludingAssemblyName();

        TestOutput.WriteLine(sut2);

        Assert.Equal("Cuemon.Extensions.Reflection.TypeExtensionsTest, Cuemon.Extensions.Reflection.Tests", sut2);
    }
}
