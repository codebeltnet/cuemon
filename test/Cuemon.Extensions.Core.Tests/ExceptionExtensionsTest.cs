using System;
using System.Linq;
using System.Reflection;
using Cuemon.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions;
public class ExceptionExtensionsTest : Test
{
    public ExceptionExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Flatten_ShouldFlattenAnyInnerExceptions()
    {
        var sut1 = new InvalidOperationException("First", new AmbiguousMatchException("Second", new OutOfMemoryException("Third", new AggregateException(Arguments.Yield(new AccessViolationException())))));
        var sut2 = sut1.Flatten();

        TestOutput.WriteLines(sut2);

        Assert.Collection(sut2,
            e => Assert.IsType<AmbiguousMatchException>(e),
            e => Assert.IsType<OutOfMemoryException>(e),
            e => Assert.IsType<AggregateException>(e),
            e => Assert.IsType<AccessViolationException>(e));
    }
}
