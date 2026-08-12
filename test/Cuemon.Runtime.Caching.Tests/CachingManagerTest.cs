using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Runtime.Caching;
public class CachingManagerTest : Test
{
    public CachingManagerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void CachingManager_Cache_ShouldGetFromSingleton()
    {
        var sut = CachingManager.Cache;

        Assert.NotNull(sut);
    }
}
