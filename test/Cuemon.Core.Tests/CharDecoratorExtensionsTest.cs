using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Xunit;

namespace Cuemon;
public class CharDecoratorExtensionsTest
{
    [Fact]
    public void ToEnumerable_ShouldConvertCharSequence_ToStringSequence()
    {
        var fixture = new Fixture();
        var cs = fixture.CreateMany<char>(500);
        var ss = Decorator.Enclose(cs).ToEnumerable();
        Assert.Equal(cs.Select(c => c.ToString()), ss);
        Assert.IsAssignableFrom<IEnumerable<string>>(ss);
    }

    [Fact]
    public void ToStringEquivalent_ShouldConvertCharSequence_ToString()
    {
        var fixture = new Fixture();
        var cs = fixture.CreateMany<char>(500);
        var ss = Decorator.Enclose(cs).ToStringEquivalent();
        Assert.Equal(string.Concat(cs), ss);
        Assert.Equal(cs.Count(), ss.Length);
    }
}
