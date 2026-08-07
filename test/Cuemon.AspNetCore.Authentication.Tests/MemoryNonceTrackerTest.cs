using System;
using System.Collections.Concurrent;
using System.Reflection;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.AspNetCore.Authentication;
public class MemoryNonceTrackerTest : Test
{
    public MemoryNonceTrackerTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void MemoryNonceTracker_ShouldAddGetAndRemoveEntries()
    {
        using (var sut = new MemoryNonceTracker())
        {
            Assert.True(sut.TryAddEntry("nonce-1", 7));
            Assert.False(sut.TryAddEntry("nonce-1", 8));
            Assert.True(sut.TryGetEntry("nonce-1", out var entry));
            Assert.Equal(7, entry.Count);
            Assert.True(entry.Created <= DateTime.UtcNow);
            Assert.True(sut.TryRemoveEntry("nonce-1"));
            Assert.False(sut.TryRemoveEntry("nonce-1"));
            Assert.False(sut.TryGetEntry("nonce-1", out _));
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void MemoryNonceTracker_ShouldRejectInvalidNonces(string nonce)
    {
        using (var sut = new MemoryNonceTracker())
        {
            Assert.ThrowsAny<ArgumentException>(() => sut.TryAddEntry(nonce, 1));
            Assert.ThrowsAny<ArgumentException>(() => sut.TryGetEntry(nonce, out _));
            Assert.ThrowsAny<ArgumentException>(() => sut.TryRemoveEntry(nonce));
        }
    }

    [Fact]
    public void MemoryNonceTracker_ShouldRemoveStaleEntries_WhenCleanupRuns()
    {
        using (var sut = new MemoryNonceTracker())
        {
            var entriesField = typeof(MemoryNonceTracker).GetField("_entries", BindingFlags.Instance | BindingFlags.NonPublic);
            var cleanupMethod = typeof(MemoryNonceTracker).GetMethod("OnAutomatedSweepCleanup", BindingFlags.Instance | BindingFlags.NonPublic);
            var entries = Assert.IsType<ConcurrentDictionary<string, NonceTrackerEntry>>(entriesField.GetValue(sut));

            entries["stale"] = new NonceTrackerEntry(1, DateTime.UtcNow.AddMinutes(-6));
            entries["fresh"] = new NonceTrackerEntry(2, DateTime.UtcNow);

            cleanupMethod.Invoke(sut, null);

            Assert.False(sut.TryGetEntry("stale", out _));
            Assert.True(sut.TryGetEntry("fresh", out var fresh));
            Assert.Equal(2, fresh.Count);
        }
    }
}
