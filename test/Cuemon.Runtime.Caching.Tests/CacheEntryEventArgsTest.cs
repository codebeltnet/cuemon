using System;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Runtime;
using Xunit;

namespace Cuemon.Runtime.Caching
{
    public class CacheEntryEventArgsTest : Test
    {
        public CacheEntryEventArgsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Expired_ShouldRaiseCacheEntryEventArgs_WhenDependencyChanges()
        {
            var cache = new SlimMemoryCache();
            var dependency = new DependencyStub();
            var sut = new CacheEntry("key", "value");
            object sender = null;
            CacheEntryEventArgs eventArgs = null;

            sut.Expired += (s, e) =>
            {
                sender = s;
                eventArgs = e;
            };

            cache.Add(sut, new CacheInvalidation(new[] { dependency }));
            dependency.SignalChanged();

            Assert.Same(sut, sender);
            Assert.NotNull(eventArgs);
        }

        private sealed class DependencyStub : IDependency
        {
            public event EventHandler<DependencyEventArgs> DependencyChanged;

            public DateTime? UtcLastModified { get; private set; }

            public bool HasChanged { get; private set; }

            public void Start()
            {
            }

            public Task StartAsync()
            {
                return Task.CompletedTask;
            }

            public void SignalChanged()
            {
                UtcLastModified = DateTime.UtcNow;
                HasChanged = true;
                DependencyChanged?.Invoke(this, new DependencyEventArgs(UtcLastModified.Value));
            }
        }
    }
}
