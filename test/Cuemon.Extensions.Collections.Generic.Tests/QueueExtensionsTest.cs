#if NETSTANDARD2_0_OR_GREATER
using System.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.Collections.Generic;

public class QueueExtensionsTest : Test
{
    public QueueExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void TryPeek_ShouldReturnTrueAndFirstElement_WhenQueueIsNotEmpty()
    {
        var sut = new Queue<int>(new[] { 10, 20, 30 });

        var result = sut.TryPeek(out var value);

        Assert.True(result);
        Assert.Equal(10, value);
        Assert.Equal(3, sut.Count);
    }

    [Fact]
    public void TryPeek_ShouldReturnFalseAndDefaultValue_WhenQueueIsEmpty()
    {
        var sut = new Queue<int>();

        var result = sut.TryPeek(out var value);

        Assert.False(result);
        Assert.Equal(default, value);
        Assert.Empty(sut);
    }
}
#endif
