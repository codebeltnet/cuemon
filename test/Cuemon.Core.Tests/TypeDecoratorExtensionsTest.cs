using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Cuemon.Assets;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon;
public class TypeDecoratorExtensionsTest : Test
{
    public TypeDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetDerivedTypes_ShouldHaveSelfToDerivedTypes()
    {
        var msType = typeof(Stream);
        var selfToDerived = Decorator.Enclose(msType).GetDerivedTypes();
        TestOutput.WriteLine(DelimitedString.Create(selfToDerived.Where(t => t.IsPublic), o => o.Delimiter = Environment.NewLine));

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
        var inheritedToSelf = Decorator.Enclose(msType).GetInheritedTypes();
        TestOutput.WriteLine(DelimitedString.Create(inheritedToSelf.Where(t => t.IsPublic), o => o.Delimiter = Environment.NewLine));

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
        var hierarchy = Decorator.Enclose(msType).GetHierarchyTypes();
        TestOutput.WriteLine(DelimitedString.Create(hierarchy.Where(t => t.IsPublic), o => o.Delimiter = Environment.NewLine));

        Assert.Contains(hierarchy, t => t == typeof(object));
        Assert.Contains(hierarchy, t => t == typeof(MarshalByRefObject));
        Assert.Contains(hierarchy, t => t == typeof(Stream));
        Assert.Contains(hierarchy, t => t == typeof(FileStream));
        Assert.Contains(hierarchy, t => t == typeof(MemoryStream));
        Assert.Contains(hierarchy, t => t == typeof(UnmanagedMemoryStream));
    }

    [Fact]
    public void HasAnonymousCharacteristics_ShouldBeTrueForTypeDelegateAndLambda()
    {
        var at = new { Id = 1, Name = "Cuemon" };
        var am = new Action(() => { });
        var nat = new MimicAnonymousType();
        var nam = new Action(NamedMethod);
        Action al = () => { };
        Action nal = NamedMethod;

        Assert.True(Decorator.Enclose(at.GetType()).HasAnonymousCharacteristics());
        Assert.True(Decorator.Enclose(am.Target.GetType()).HasAnonymousCharacteristics());
        Assert.True(Decorator.Enclose(al.Target.GetType()).HasAnonymousCharacteristics());
        Assert.False(Decorator.Enclose(nat.GetType()).HasAnonymousCharacteristics());
        Assert.False(Decorator.Enclose(nam.Target.GetType()).HasAnonymousCharacteristics());
        Assert.False(Decorator.Enclose(nal.Target.GetType()).HasAnonymousCharacteristics());
    }

    [Fact]
    public void IsNullable_ShouldBeTrueForPrimitiveNullableValueTypes()
    {
        var valueTypes = Decorator.Enclose(typeof(ValueType)).GetDerivedTypes();
        var primitives = valueTypes.Where(t => t.IsPublic && t.IsPrimitive);

        foreach (var primitive in primitives)
        {
            Assert.False(Decorator.Enclose(primitive).IsNullable());
            Assert.True(Decorator.Enclose(typeof(Nullable<>).MakeGenericType(primitive)).IsNullable());
        }
    }

    [Fact]
    public void HasDictionaryImplementation_ShouldBeTrueForDictionaryTypes()
    {
        var ht = new Hashtable();
        var d = new Dictionary<int, string>();
        var rd = new ReadOnlyDictionary<int, string>(d);

        Assert.True(Decorator.Enclose(ht.GetType()).HasDictionaryImplementation());
        Assert.True(Decorator.Enclose(d.GetType()).HasDictionaryImplementation());
        Assert.True(Decorator.Enclose(rd.GetType()).HasDictionaryImplementation());

        Assert.False(Decorator.Enclose(new List<int>().GetType()).HasDictionaryImplementation());
        Assert.False(Decorator.Enclose(Enumerable.Empty<int>().GetType()).HasDictionaryImplementation());
        Assert.False(Decorator.Enclose(new Collection<string>().GetType()).HasDictionaryImplementation());

        Assert.True(Decorator.Enclose(typeof(IDictionary)).HasDictionaryImplementation());
        Assert.True(Decorator.Enclose(typeof(IDictionary<int, string>)).HasDictionaryImplementation());
        Assert.True(Decorator.Enclose(typeof(IReadOnlyDictionary<int, string>)).HasDictionaryImplementation());
    }

    [Fact]
    public void HasEnumerableImplementation_ShouldBeTrueForEnumerableTypes()
    {
        var s = "Cuemon";
        var a = new[] { "C", "u", "e", "m", "o", "n" };

        Assert.True(Decorator.Enclose(s.GetType()).HasEnumerableImplementation());
        Assert.True(Decorator.Enclose(a.GetType()).HasEnumerableImplementation());
        Assert.True(Decorator.Enclose(new ConcurrentBag<int>().GetType()).HasEnumerableImplementation());

        Assert.True(Decorator.Enclose(new List<int>().GetType()).HasEnumerableImplementation());
        Assert.True(Decorator.Enclose(Enumerable.Empty<int>().GetType()).HasEnumerableImplementation());
        Assert.True(Decorator.Enclose(new Collection<string>().GetType()).HasEnumerableImplementation());

        Assert.True(Decorator.Enclose(typeof(IEnumerable<>)).HasEnumerableImplementation());
        Assert.True(Decorator.Enclose(typeof(IEnumerable)).HasEnumerableImplementation());
    }

    [Fact]
    public void HasComparerImplementation_ShouldBeTrueForComparerTypes()
    {
        Assert.True(Decorator.Enclose(StringComparer.InvariantCulture.GetType()).HasComparerImplementation());
        Assert.True(Decorator.Enclose(Comparer.DefaultInvariant.GetType()).HasComparerImplementation());
        Assert.True(Decorator.Enclose(CaseInsensitiveComparer.DefaultInvariant.GetType()).HasComparerImplementation());

        Assert.True(Decorator.Enclose(typeof(IComparer<>)).HasComparerImplementation());
        Assert.True(Decorator.Enclose(typeof(IComparer)).HasComparerImplementation());
    }

    [Fact]
    public void HasComparableImplementation_ShouldBeTrueForComparableTypes()
    {
        Assert.True(Decorator.Enclose("Cuemon".GetType()).HasComparableImplementation());
        Assert.True(Decorator.Enclose(1.GetType()).HasComparableImplementation());
        Assert.True(Decorator.Enclose(3.14159265359.GetType()).HasComparableImplementation());
        Assert.True(Decorator.Enclose(true.GetType()).HasComparableImplementation());

        Assert.True(Decorator.Enclose(typeof(IComparable<>)).HasComparableImplementation());
        Assert.True(Decorator.Enclose(typeof(IComparable)).HasComparableImplementation());
    }

    [Fact]
    public void HasEqualityComparerImplementation_ShouldBeTrueForEqualityComparerTypes()
    {
        Assert.True(Decorator.Enclose(EqualityComparer<int>.Default.GetType()).HasEqualityComparerImplementation());
        Assert.True(Decorator.Enclose(StringComparer.OrdinalIgnoreCase.GetType()).HasEqualityComparerImplementation());
        Assert.True(Decorator.Enclose(typeof(Range<int>)).HasEqualityComparerImplementation());

        Assert.True(Decorator.Enclose(typeof(IEqualityComparer<>)).HasEqualityComparerImplementation());
        Assert.True(Decorator.Enclose(typeof(IEqualityComparer)).HasEqualityComparerImplementation());
    }

    [Fact]
    public void HasKeyValuePairImplementation_ShouldBeTrueForKeyValuePairTypes()
    {
        var h = new Hashtable
        {
            {1, "Cuemon"}
        };

        var d = new Dictionary<int, string>
        {
            {1, "Cuemon"}
        };

        Assert.True(Decorator.Enclose(d.First().GetType()).HasKeyValuePairImplementation());
        foreach (var de in h)
        {
            Assert.True(Decorator.Enclose(de.GetType()).HasKeyValuePairImplementation());
        }

        Assert.True(Decorator.Enclose(typeof(KeyValuePair<,>)).HasKeyValuePairImplementation());
        Assert.True(Decorator.Enclose(typeof(DictionaryEntry)).HasKeyValuePairImplementation());
    }

    [Fact]
    public void HasAttribute_ShouldBeTrueForMembersHavingOneOrMoreAttributes()
    {
        var t = new ClassWithAttributes().GetType();

        Assert.True(Decorator.Enclose(t).HasAttribute(typeof(ObsoleteAttribute), typeof(DataContractAttribute), typeof(DataMemberAttribute)));
        Assert.True(Decorator.Enclose(t).HasAttribute(typeof(DataContractAttribute)));
        Assert.True(Decorator.Enclose(t).HasAttribute(typeof(DataMemberAttribute)));
        Assert.True(Decorator.Enclose(t).HasAttribute(typeof(ObsoleteAttribute)));
        Assert.False(Decorator.Enclose(t).HasAttribute(typeof(XmlRootAttribute), typeof(XmlElementAttribute)));
    }

    [Fact]
    public void IsComplex_ShouldBeFalseForNoneComplexObjectTypes()
    {
        var valueTypes = Decorator.Enclose(typeof(ValueType)).GetDerivedTypes();
        var primitives = valueTypes.Where(t => t.IsPublic && t.IsPrimitive);

        foreach (var primitive in primitives)
        {
            Assert.False(Decorator.Enclose(primitive).IsComplex());
        }

        Assert.False(Decorator.Enclose(typeof(string)).IsComplex());
        Assert.False(Decorator.Enclose(typeof(decimal)).IsComplex());
        Assert.False(Decorator.Enclose(typeof(DateTime)).IsComplex());
        Assert.False(Decorator.Enclose(typeof(Guid)).IsComplex());
        Assert.False(Decorator.Enclose(typeof(TimeSpan)).IsComplex());
        Assert.False(Decorator.Enclose(typeof(AssignmentOperator)).IsComplex());

        Assert.True(Decorator.Enclose(typeof(Stream)).IsComplex());
        Assert.True(Decorator.Enclose(typeof(XmlDocument)).IsComplex());
        Assert.True(Decorator.Enclose(typeof(Exception)).IsComplex());
    }

    [Fact]
    public void HasDefaultConstructor_ShouldBeTrueForAllValueTypesAndReferenceTypesWithEmptyConstructor()
    {
        var valueTypes = Decorator.Enclose(typeof(ValueType)).GetDerivedTypes().Where(t => t.IsValueType && t.IsPublic);
        foreach (var vt in valueTypes)
        {
            Assert.True(Decorator.Enclose(vt).HasDefaultConstructor());
        }

        var referenceTypes = Decorator.Enclose(typeof(object)).GetDerivedTypes().Where(t => !t.IsValueType && t.IsPublic && t.GetConstructor(Type.EmptyTypes) != null);
        foreach (var rt in referenceTypes)
        {
            Assert.True(Decorator.Enclose(rt).HasDefaultConstructor());
        }

        Assert.False(Decorator.Enclose(typeof(ClassWithNoDefaultCtor)).HasDefaultConstructor());
    }

    [Fact]
    public void ToFriendlyName_ShouldProvideDefaultImplementationOfTypes()
    {
        var defaultString = Decorator.Enclose(typeof(Tuple<Guid, string, int, char, double, object>)).ToFriendlyName();
        var fullNameString = Decorator.Enclose(typeof(Tuple<Guid, string, int, char, double, object>)).ToFriendlyName(o => o.FullName = true);
        var noGenericsString = Decorator.Enclose(typeof(Tuple<Guid, string, int, char, double, object>)).ToFriendlyName(o => o.ExcludeGenericArguments = true);

        Assert.Equal("Tuple<Guid,String,Int32,Char,Double,Object>", defaultString);
        Assert.Equal("System.Tuple<System.Guid,System.String,System.Int32,System.Char,System.Double,System.Object>", fullNameString);
        Assert.Equal("Tuple", noGenericsString);
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            var seCultureInfo = Decorator.Enclose(typeof(Tuple<Guid, string, int, char, double, object>)).ToFriendlyName(o => o.FormatProvider = CultureInfo.GetCultureInfo("sv-SE")); // unix has different culture interpretation
            Assert.Equal("Tuple<Guid;String;Int32;Char;Double;Object>", seCultureInfo);
        }
    }

    [Fact]
    public void HasCircularReference_ShouldBeTrueForTypesReferencingThemself()
    {
        var cr = new ClassWithCircularReference();
        var ms = new MemoryStream();

        Assert.True(Decorator.Enclose(cr.GetType()).HasCircularReference(cr));
        Assert.Throws<InvalidOperationException>(() => Decorator.Enclose(ms.GetType()).HasCircularReference(cr));
        Assert.False(Decorator.Enclose(ms.GetType()).HasCircularReference(ms));
    }

    [Fact]
    public void GetDefaultValue_ShouldBeDefaultValueFromValueTypesAndReferenceTypesWithDefaultConstructor()
    {
        Assert.Equal(0, Decorator.Enclose(typeof(int)).GetDefaultValue());
        Assert.Equal(decimal.Zero, Decorator.Enclose(typeof(decimal)).GetDefaultValue());
        Assert.Equal(Guid.Empty, Decorator.Enclose(typeof(Guid)).GetDefaultValue());
        Assert.Equal(DateTime.MinValue, Decorator.Enclose(typeof(DateTime)).GetDefaultValue());
        Assert.Equal(TimeSpan.Zero, Decorator.Enclose(typeof(TimeSpan)).GetDefaultValue());
        Assert.Null(Decorator.Enclose(typeof(int?)).GetDefaultValue());
        Assert.Null(Decorator.Enclose(typeof(bool?)).GetDefaultValue());
        Assert.Equal(new ClassWithDefaultValue(), Decorator.Enclose(typeof(ClassWithDefaultValue)).GetDefaultValue());
    }

    [Theory, MemberData(nameof(Randomizer))]
    public void MatchMember_ShouldUseDynamicWayToResolveThisMethod(Guid id, string randomString, int randomNumber)
    {
        var mb = MethodBase.GetCurrentMethod(); //Decorator.Enclose(this.GetType()).MatchMember(flags: new MemberReflection(excludeInheritancePath: true));
        var args = mb.GetParameters();

        Assert.NotNull(mb);
        Assert.NotNull(args);

        Assert.Equal(nameof(MatchMember_ShouldUseDynamicWayToResolveThisMethod), mb.Name);
        Assert.Contains(args, pi => pi.Name == nameof(id) && pi.ParameterType == typeof(Guid));
        Assert.Contains(args, pi => pi.Name == nameof(randomString) && pi.ParameterType == typeof(string));
        Assert.Contains(args, pi => pi.Name == nameof(randomNumber) && pi.ParameterType == typeof(int));
    }

    [Fact]
    public void MatchMember_ShouldUseNormalWayToResolveAMethod()
    {
        Assert.Throws<ArgumentNullException>(() => Decorator.Enclose(typeof(ClassWithAmbiguousMethods)).MatchMember((string)null));
        Assert.Throws<ArgumentException>(() => Decorator.Enclose(typeof(ClassWithAmbiguousMethods)).MatchMember(""));
        Assert.Throws<AmbiguousMatchException>(() => Decorator.Enclose(typeof(ClassWithAmbiguousMethods)).MatchMember("MethodA"));

        var mb = Decorator.Enclose(typeof(ClassWithAmbiguousMethods)).MatchMember("MethodA", o => o.Types = new[] { typeof(Guid) });
        var args = mb.GetParameters();

        Assert.NotNull(mb);
        Assert.NotNull(args);

        Assert.Equal(nameof(ClassWithAmbiguousMethods.MethodA), mb.Name);
        Assert.Contains(args, pi => pi.Name == "id" && pi.ParameterType == typeof(Guid));
    }

    public static IEnumerable<object[]> Randomizer()
    {
        yield return new object[] { Guid.NewGuid(), Generate.RandomString(10), Generate.RandomNumber() };
    }

    private void NamedMethod()
    {
    }
}
