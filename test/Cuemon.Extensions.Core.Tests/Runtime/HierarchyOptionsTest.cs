using System;
using System.Collections.Generic;
using System.Reflection;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Extensions.Runtime;
public class HierarchyOptionsTest : Test
{
    public HierarchyOptionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Ctor_ShouldInitializeDefaultsAndDelegates_WhenCreated()
    {
        var sut = new HierarchyOptions();
        var nameProperty = typeof(HierarchyOptionsModel).GetProperty(nameof(HierarchyOptionsModel.Name));
        var countProperty = typeof(List<int>).GetProperty(nameof(List<int>.Count));
        var capacityProperty = typeof(List<int>).GetProperty(nameof(List<int>.Capacity));

        Assert.Equal(10, sut.MaxDepth);
        Assert.Equal(2, sut.MaxCircularCalls);
        Assert.NotNull(sut.ReflectionRules);
        Assert.NotNull(sut.SkipPropertyType);
        Assert.NotNull(sut.SkipProperty);
        Assert.NotNull(sut.HasCircularReference);
        Assert.NotNull(sut.ValueResolver);
        Assert.True(sut.SkipPropertyType(typeof(string)));
        Assert.False(sut.SkipPropertyType(typeof(HierarchyOptionsModel)));
        Assert.True(sut.SkipProperty(countProperty));
        Assert.False(sut.SkipProperty(capacityProperty));
        Assert.Equal("alpha", sut.ValueResolver(new HierarchyOptionsModel { Name = "alpha" }, nameProperty));
    }

    [Fact]
    public void Properties_ShouldValidateAssignedValues_WhenConfigured()
    {
        var sut = new HierarchyOptions();
        var reflectionRules = new MemberReflection(excludePrivate: true);
        Func<Type, bool> skipPropertyType = type => type == typeof(HierarchyOptionsModel);
        Func<PropertyInfo, bool> skipProperty = property => property.Name == nameof(HierarchyOptionsModel.Name);
        Func<object, bool> hasCircularReference = _ => false;
        Func<object, PropertyInfo, object> valueResolver = (_, property) => property.Name;

        Assert.Throws<ArgumentOutOfRangeException>(() => sut.MaxDepth = -1);
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.MaxCircularCalls = -1);
        Assert.Throws<ArgumentNullException>(() => sut.ReflectionRules = null);
        Assert.Throws<ArgumentNullException>(() => sut.SkipPropertyType = null);
        Assert.Throws<ArgumentNullException>(() => sut.SkipProperty = null);
        Assert.Throws<ArgumentNullException>(() => sut.HasCircularReference = null);
        Assert.Throws<ArgumentNullException>(() => sut.ValueResolver = null);

        sut.MaxDepth = 3;
        sut.MaxCircularCalls = 4;
        sut.ReflectionRules = reflectionRules;
        sut.SkipPropertyType = skipPropertyType;
        sut.SkipProperty = skipProperty;
        sut.HasCircularReference = hasCircularReference;
        sut.ValueResolver = valueResolver;

        Assert.Equal(3, sut.MaxDepth);
        Assert.Equal(4, sut.MaxCircularCalls);
        Assert.Same(reflectionRules, sut.ReflectionRules);
        Assert.Same(skipPropertyType, sut.SkipPropertyType);
        Assert.Same(skipProperty, sut.SkipProperty);
        Assert.Same(hasCircularReference, sut.HasCircularReference);
        Assert.Same(valueResolver, sut.ValueResolver);
    }

    private sealed class HierarchyOptionsModel
    {
        public string Name { get; set; }
    }
}
