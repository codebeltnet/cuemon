using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cuemon.Extensions.FileProviders;

public class PortablePhysicalFileProviderTest : Test
{
    private static readonly TimeSpan ChangeNotificationTimeout = TimeSpan.FromSeconds(15);
    private static readonly FindMatchingEntryDelegate FindMatchingEntry = CreateFindMatchingEntryDelegate();
    private static readonly ResolvePathDelegate ResolvePathWithEntries = CreateResolvePathDelegate();
    private static readonly ResolveFileInfoDelegate ResolveFileInfoSelection = CreateResolveFileInfoDelegate();
    private static readonly ResolveDirectoryContentsDelegate ResolveDirectoryContentsSelection = CreateResolveDirectoryContentsDelegate();
    private static readonly ResolveWatchTokenDelegate ResolveWatchTokenSelection = CreateResolveWatchTokenDelegate();

    public PortablePhysicalFileProviderTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Constructor_ShouldExposeRoot_WhenAbsoluteExistingRootIsProvided()
    {
        using var scope = new TemporaryFileSystemScope();
        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        Assert.Equal(expected.Root, sut.Root);
    }

    [Fact]
    public void Constructor_ShouldRejectRelativeRoot_ConsistentWithPhysicalFileProvider()
    {
        var relativeRoot = $"portable-provider-{Guid.NewGuid():N}";
        var expected = Record.Exception(() => new PhysicalFileProvider(relativeRoot));
        var actual = Record.Exception(() => new PortablePhysicalFileProvider(relativeRoot));

        AssertEquivalentException(expected, actual);
    }

    [Fact]
    public void Constructor_ShouldRejectNonExistingRoot_ConsistentWithPhysicalFileProvider()
    {
        var missingRoot = Path.Combine(Path.GetTempPath(), "cuemon", "portable-provider", Guid.NewGuid().ToString("N"));

        if (Directory.Exists(missingRoot))
        {
            Directory.Delete(missingRoot, true);
        }

        var expected = Record.Exception(() => new PhysicalFileProvider(missingRoot));
        var actual = Record.Exception(() => new PortablePhysicalFileProvider(missingRoot));

        AssertEquivalentException(expected, actual);
    }

    [Fact]
    public void UsePollingFileWatcher_ShouldDelegateGetterAndSetter()
    {
        using var scope = new TemporaryFileSystemScope();
        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        expected.UsePollingFileWatcher = true;
        sut.UsePollingFileWatcher = true;
        Assert.Equal(expected.UsePollingFileWatcher, sut.UsePollingFileWatcher);

        expected.UsePollingFileWatcher = false;
        sut.UsePollingFileWatcher = false;
        Assert.Equal(expected.UsePollingFileWatcher, sut.UsePollingFileWatcher);
    }

    [Fact]
    public void UseActivePolling_ShouldDelegateGetterAndSetter()
    {
        using var scope = new TemporaryFileSystemScope();
        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        expected.UseActivePolling = true;
        sut.UseActivePolling = true;
        Assert.Equal(expected.UseActivePolling, sut.UseActivePolling);

        expected.UseActivePolling = false;
        sut.UseActivePolling = false;
        Assert.Equal(expected.UseActivePolling, sut.UseActivePolling);
    }

    [Fact]
    public void UsePollingFileWatcher_ShouldMirrorPhysicalFileProviderBehavior_WhenWatcherIsInitialized()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("logo.svg", "one");

        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        expected.UsePollingFileWatcher = false;
        sut.UsePollingFileWatcher = false;

        _ = expected.Watch("logo.svg");
        _ = sut.Watch("logo.svg");

        var expectedException = Record.Exception(() => expected.UsePollingFileWatcher = true);
        var actualException = Record.Exception(() => sut.UsePollingFileWatcher = true);

        AssertEquivalentException(expectedException, actualException);

        if (expectedException is null)
        {
            Assert.Equal(expected.UsePollingFileWatcher, sut.UsePollingFileWatcher);
        }
    }

    [Fact]
    public void UseActivePolling_ShouldMirrorPhysicalFileProviderBehavior_WhenWatcherIsInitialized()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("logo.svg", "one");

        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        expected.UseActivePolling = false;
        sut.UseActivePolling = false;

        _ = expected.Watch("logo.svg");
        _ = sut.Watch("logo.svg");

        var expectedException = Record.Exception(() => expected.UseActivePolling = true);
        var actualException = Record.Exception(() => sut.UseActivePolling = true);

        AssertEquivalentException(expectedException, actualException);

        if (expectedException is null)
        {
            Assert.Equal(expected.UseActivePolling, sut.UseActivePolling);
        }
    }

    [Fact]
    public void GetFileInfo_ShouldResolveSensitiveEntries_WhenExclusionFiltersIsNone()
    {
        using var scope = new TemporaryFileSystemScope();
        var providerPath = scope.CreateSensitiveFile();
        var expectedPhysicalPath = scope.GetPhysicalPath(providerPath);

        using var sut = new PortablePhysicalFileProvider(scope.RootPath, ExclusionFilters.None);

        var info = sut.GetFileInfo(providerPath.ToUpperInvariant());

        Assert.True(info.Exists);
        Assert.Equal(expectedPhysicalPath, info.PhysicalPath);
        Assert.Equal("sensitive", ReadAllText(info));
    }

    [Fact]
    public void GetFileInfo_ShouldHonorSensitiveExclusionFilters()
    {
        using var scope = new TemporaryFileSystemScope();
        var providerPath = scope.CreateSensitiveFile();

        using var expected = new PhysicalFileProvider(scope.RootPath, ExclusionFilters.Sensitive);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath, ExclusionFilters.Sensitive);

        var baseline = expected.GetFileInfo(providerPath.ToUpperInvariant());
        var info = sut.GetFileInfo(providerPath.ToUpperInvariant());

        AssertEquivalentFileInfo(baseline, info);
        Assert.False(info.Exists);
    }

    [Fact]
    public void Dispose_ShouldBeIdempotent_WhenWatcherWasInitialized()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("logo.svg", "one");

        var sut = new PortablePhysicalFileProvider(scope.RootPath);
        _ = sut.Watch("logo.svg");

        sut.Dispose();
        sut.Dispose();

        Assert.True(sut.Disposed);
    }

    [Theory]
    [InlineData("logo.svg")]
    [InlineData("Logo.svg")]
    [InlineData("LOGO.SVG")]
    [InlineData("lOgO.sVg")]
    public void GetFileInfo_ShouldResolveToSameRootFile_WhenUniqueCasingVariantIsRequested(string subpath)
    {
        using var scope = new TemporaryFileSystemScope();
        var physicalPath = scope.CreateFile("logo.svg", "root-logo");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var info = sut.GetFileInfo(subpath);

        Assert.True(info.Exists);
        Assert.Equal("logo.svg", info.Name);
        Assert.Equal(physicalPath, info.PhysicalPath);
        Assert.Equal("root-logo", ReadAllText(info));
    }

    [Theory]
    [InlineData("assets/images/logo.svg")]
    [InlineData("Assets/Images/Logo.svg")]
    [InlineData("ASSETS/IMAGES/LOGO.SVG")]
    [InlineData("aSsEtS/iMaGeS/lOgO.sVg")]
    public void GetFileInfo_ShouldResolveNestedFile_WhenDirectoryAndFileSegmentsUseDifferentCasing(string subpath)
    {
        using var scope = new TemporaryFileSystemScope();
        var physicalPath = scope.CreateFile("Assets/Images/Logo.svg", "nested-logo");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var info = sut.GetFileInfo(subpath);

        Assert.True(info.Exists);
        Assert.Equal("Logo.svg", info.Name);
        Assert.Equal(physicalPath, info.PhysicalPath);
        Assert.Equal("nested-logo", ReadAllText(info));
    }

    [Fact]
    public void GetFileInfo_ShouldUseSuccessfulFileCache_ForRepeatedLookupsAndCaseInsensitiveKeys()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("Assets/Images/Logo.svg", "cached-file");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var first = sut.GetFileInfo("assets/images/logo.svg");
        var second = sut.GetFileInfo("assets/images/logo.svg");
        var third = sut.GetFileInfo("ASSETS/IMAGES/LOGO.SVG");

        Assert.True(first.Exists);
        Assert.True(second.Exists);
        Assert.True(third.Exists);
        Assert.Equal(first.PhysicalPath, second.PhysicalPath);
        Assert.Equal(first.PhysicalPath, third.PhysicalPath);
    }

    [Fact]
    public void GetFileInfo_ShouldCanonicalizeSuccessfulFileCache_ForEquivalentSeparatorAndCasingVariations()
    {
        using var scope = new TemporaryFileSystemScope();
        var physicalPath = scope.CreateFile("Assets/Images/Logo.svg", "canonical-file");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var canonical = sut.GetFileInfo("assets/images/logo.svg");
        var mixedSeparators = sut.GetFileInfo(CreateMixedSeparatorAlias("ASSETS", "IMAGES", "LOGO.SVG"));
        var repeatedSeparators = sut.GetFileInfo("/Assets//Images///Logo.svg");

        Assert.True(canonical.Exists);
        Assert.True(mixedSeparators.Exists);
        Assert.True(repeatedSeparators.Exists);
        Assert.Equal(physicalPath, canonical.PhysicalPath);
        Assert.Equal(physicalPath, mixedSeparators.PhysicalPath);
        Assert.Equal(physicalPath, repeatedSeparators.PhysicalPath);
        Assert.Equal(1, GetSuccessfulPathCacheCount(sut, finalSegmentIsDirectory: false));
    }

    [Fact]
    public void GetFileInfo_ShouldReturnNotFound_WhenIntermediateDirectoryIsMissing()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("Assets/Logo.svg", "one");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var info = sut.GetFileInfo("missing/logo.svg");

        Assert.False(info.Exists);
    }

    [Fact]
    public void GetFileInfo_ShouldReevaluateMiss_WhenFileAppearsAfterEarlierMiss()
    {
        using var scope = new TemporaryFileSystemScope();
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var missing = sut.GetFileInfo("assets/new.svg");
        Assert.False(missing.Exists);

        var physicalPath = scope.CreateFile("Assets/New.svg", "appeared");
        var info = sut.GetFileInfo("assets/new.svg");

        Assert.True(info.Exists);
        Assert.Equal(physicalPath, info.PhysicalPath);
        Assert.Equal("appeared", ReadAllText(info));
    }

    [Fact]
    public void GetFileInfo_ShouldResolveLeadingSeparatorPath()
    {
        using var scope = new TemporaryFileSystemScope();
        var physicalPath = scope.CreateFile("Assets/Logo.svg", "leading-separator");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var info = sut.GetFileInfo("/assets/logo.svg");

        Assert.True(info.Exists);
        Assert.Equal(physicalPath, info.PhysicalPath);
    }

    [Fact]
    public void GetFileInfo_ShouldMirrorPhysicalFileProviderBehavior_ForAbsolutePath()
    {
        using var scope = new TemporaryFileSystemScope();
        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var absolutePath = Path.Combine(Path.GetTempPath(), "portable-provider-absolute", Guid.NewGuid().ToString("N"), "logo.svg");
        var baseline = expected.GetFileInfo(absolutePath);
        var info = sut.GetFileInfo(absolutePath);

        AssertEquivalentFileInfo(baseline, info);
    }

    [Fact]
    public void GetFileInfo_ShouldMirrorPhysicalFileProviderBehavior_ForAboveRootPath()
    {
        using var scope = new TemporaryFileSystemScope();
        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var baseline = expected.GetFileInfo("../logo.svg");
        var info = sut.GetFileInfo("../logo.svg");

        AssertEquivalentFileInfo(baseline, info);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetFileInfo_ShouldMirrorPhysicalFileProviderBehavior_ForNullAndEmptyInputs(string subpath)
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("logo.svg", "one");

        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var baseline = expected.GetFileInfo(subpath);
        var info = sut.GetFileInfo(subpath);

        AssertEquivalentFileInfo(baseline, info);
    }

    [Fact]
    public void GetFileInfo_ShouldMirrorPhysicalFileProviderBehavior_WhenSubpathEndsWithDirectorySeparator()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("Assets/logo.svg", "one");

        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var baseline = expected.GetFileInfo("Assets/logo.svg/");
        var info = sut.GetFileInfo("Assets/logo.svg/");

        AssertEquivalentFileInfo(baseline, info);
        Assert.Equal(0, GetSuccessfulPathCacheCount(sut, finalSegmentIsDirectory: false));
    }

    [Fact]
    public void GetFileInfo_ShouldUseOrdinalIgnoreCase_IndependentOfCurrentCulture()
    {
        using var scope = new TemporaryFileSystemScope();
        var physicalPath = scope.CreateFile("igloo.txt", "culture");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var originalCulture = CultureInfo.CurrentCulture;
        var originalUiCulture = CultureInfo.CurrentUICulture;

        try
        {
            var culture = CultureInfo.GetCultureInfo("tr-TR");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            var info = sut.GetFileInfo("IGLOO.TXT");

            Assert.True(info.Exists);
            Assert.Equal(physicalPath, info.PhysicalPath);
            Assert.Equal("culture", ReadAllText(info));
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }

    [Theory]
    [InlineData("Assets")]
    [InlineData("assets")]
    [InlineData("ASSETS")]
    [InlineData("aSsEtS")]
    public void GetDirectoryContents_ShouldResolveToSameDirectory_WhenUniqueCasingVariantIsRequested(string subpath)
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("Assets/logo.svg", "one");
        scope.CreateFile("Assets/banner.svg", "two");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var contents = sut.GetDirectoryContents(subpath);

        Assert.True(contents.Exists);
        Assert.Equal(new[] { "banner.svg", "logo.svg" }, GetOrderedNames(contents));
    }

    [Fact]
    public void GetDirectoryContents_ShouldResolveNestedDirectory_WhenSegmentsUseDifferentCasing()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("Assets/Images/logo.svg", "one");
        scope.CreateFile("Assets/Images/banner.svg", "two");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var contents = sut.GetDirectoryContents("assets/images");

        Assert.True(contents.Exists);
        Assert.Equal(new[] { "banner.svg", "logo.svg" }, GetOrderedNames(contents));
    }

    [Fact]
    public void GetDirectoryContents_ShouldUseSuccessfulDirectoryCache_ForRepeatedLookupsAndCaseInsensitiveKeys()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("Assets/Images/logo.svg", "one");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var first = sut.GetDirectoryContents("assets/images");
        var second = sut.GetDirectoryContents("assets/images");
        var third = sut.GetDirectoryContents("ASSETS/IMAGES");

        Assert.True(first.Exists);
        Assert.True(second.Exists);
        Assert.True(third.Exists);
        Assert.Equal(GetOrderedNames(first), GetOrderedNames(second));
        Assert.Equal(GetOrderedNames(first), GetOrderedNames(third));
    }

    [Fact]
    public void GetDirectoryContents_ShouldCanonicalizeSuccessfulDirectoryCache_ForEquivalentSeparatorAndCasingVariations()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("Assets/Images/logo.svg", "one");
        scope.CreateFile("Assets/Images/banner.svg", "two");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var canonical = sut.GetDirectoryContents("assets/images");
        var mixedSeparators = sut.GetDirectoryContents(CreateMixedSeparatorAlias("ASSETS", "IMAGES", trailingSeparator: true));
        var repeatedSeparators = sut.GetDirectoryContents("/Assets//Images///");

        Assert.True(canonical.Exists);
        Assert.True(mixedSeparators.Exists);
        Assert.True(repeatedSeparators.Exists);
        Assert.Equal(new[] { "banner.svg", "logo.svg" }, GetOrderedNames(canonical));
        Assert.Equal(GetOrderedNames(canonical), GetOrderedNames(mixedSeparators));
        Assert.Equal(GetOrderedNames(canonical), GetOrderedNames(repeatedSeparators));
        Assert.Equal(1, GetSuccessfulPathCacheCount(sut, finalSegmentIsDirectory: true));
    }

    [Fact]
    public void GetDirectoryContents_ShouldReturnNotFound_WhenDirectoryIsMissing()
    {
        using var scope = new TemporaryFileSystemScope();
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var contents = sut.GetDirectoryContents("missing");

        Assert.False(contents.Exists);
        Assert.Same(NotFoundDirectoryContents.Singleton, contents);
    }

    [Fact]
    public void GetDirectoryContents_ShouldReevaluateMiss_WhenDirectoryAppearsAfterEarlierMiss()
    {
        using var scope = new TemporaryFileSystemScope();
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var missing = sut.GetDirectoryContents("assets/images");
        Assert.False(missing.Exists);

        scope.CreateFile("Assets/Images/logo.svg", "appeared");
        var contents = sut.GetDirectoryContents("assets/images");

        Assert.True(contents.Exists);
        Assert.Equal(new[] { "logo.svg" }, GetOrderedNames(contents));
    }

    [Fact]
    public void GetDirectoryContents_ShouldResolveLeadingSeparatorPath()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("Assets/logo.svg", "one");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var contents = sut.GetDirectoryContents("/assets");

        Assert.True(contents.Exists);
        Assert.Equal(new[] { "logo.svg" }, GetOrderedNames(contents));
    }

    [Fact]
    public void GetDirectoryContents_ShouldMirrorPhysicalFileProviderBehavior_ForAbsolutePath()
    {
        using var scope = new TemporaryFileSystemScope();
        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var absolutePath = Path.Combine(Path.GetTempPath(), "portable-provider-absolute", Guid.NewGuid().ToString("N"), "assets");
        var baseline = expected.GetDirectoryContents(absolutePath);
        var contents = sut.GetDirectoryContents(absolutePath);

        AssertEquivalentDirectoryContents(baseline, contents);
    }

    [Fact]
    public void GetDirectoryContents_ShouldMirrorPhysicalFileProviderBehavior_ForAboveRootPath()
    {
        using var scope = new TemporaryFileSystemScope();
        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var baseline = expected.GetDirectoryContents("../assets");
        var contents = sut.GetDirectoryContents("../assets");

        AssertEquivalentDirectoryContents(baseline, contents);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("/")]
    public void GetDirectoryContents_ShouldMirrorPhysicalFileProviderBehavior_ForNullEmptyAndSeparatorOnlyInputs(string subpath)
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("Assets/logo.svg", "one");

        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var baseline = expected.GetDirectoryContents(subpath);
        var contents = sut.GetDirectoryContents(subpath);

        AssertEquivalentDirectoryContents(baseline, contents);
    }

    [Theory]
    [InlineData("logo.svg")]
    [InlineData("Logo.svg")]
    [InlineData("LOGO.SVG")]
    [InlineData("lOgO.sVg")]
    public void GetFileInfo_ShouldReturnNotFoundForFileCollision_WhenPhysicalEntriesAreCreatedInOrder(string subpath)
    {
        Assert.SkipWhen(!PortablePhysicalFileProviderTestCapabilities.SupportsDistinctCaseEntries, PortablePhysicalFileProviderTestCapabilities.DistinctCaseEntriesUnsupportedReason);

        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("logo.svg", "lower");
        scope.CreateFile("Logo.svg", "upper");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        IFileInfo info = null;
        var exception = Record.Exception(() => info = sut.GetFileInfo(subpath));

        Assert.Null(exception);
        AssertNotFoundFileInfo(subpath, info);
    }

    [Theory]
    [InlineData("logo.svg")]
    [InlineData("Logo.svg")]
    [InlineData("LOGO.SVG")]
    [InlineData("lOgO.sVg")]
    public void GetFileInfo_ShouldReturnNotFoundForFileCollision_WhenPhysicalEntriesAreCreatedInReverseOrder(string subpath)
    {
        Assert.SkipWhen(!PortablePhysicalFileProviderTestCapabilities.SupportsDistinctCaseEntries, PortablePhysicalFileProviderTestCapabilities.DistinctCaseEntriesUnsupportedReason);

        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("Logo.svg", "upper");
        scope.CreateFile("logo.svg", "lower");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        IFileInfo info = null;
        var exception = Record.Exception(() => info = sut.GetFileInfo(subpath));

        Assert.Null(exception);
        AssertNotFoundFileInfo(subpath, info);
    }

    [Fact]
    public void GetFileInfo_ShouldResolveRemainingFile_WhenCollisionEntryIsRemoved()
    {
        Assert.SkipWhen(!PortablePhysicalFileProviderTestCapabilities.SupportsDistinctCaseEntries, PortablePhysicalFileProviderTestCapabilities.DistinctCaseEntriesUnsupportedReason);

        using var scope = new TemporaryFileSystemScope();
        var remainingPath = scope.CreateFile("logo.svg", "lower");
        scope.CreateFile("Logo.svg", "upper");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        Assert.False(sut.GetFileInfo("LOGO.SVG").Exists);

        scope.DeleteFile("Logo.svg");

        var info = sut.GetFileInfo("LOGO.SVG");

        Assert.True(info.Exists);
        Assert.Equal(remainingPath, info.PhysicalPath);
        Assert.Equal("lower", ReadAllText(info));
    }

    [Fact]
    public void GetDirectoryContents_ShouldReturnNotFoundForDirectoryCollision_RegardlessOfRequestCasing()
    {
        Assert.SkipWhen(!PortablePhysicalFileProviderTestCapabilities.SupportsDistinctCaseEntries, PortablePhysicalFileProviderTestCapabilities.DistinctCaseEntriesUnsupportedReason);

        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("assets/logo.svg", "lower");
        scope.CreateFile("Assets/banner.svg", "upper");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        foreach (var subpath in new[] { "assets", "Assets", "ASSETS", "aSsEtS" })
        {
            IDirectoryContents contents = null;
            var exception = Record.Exception(() => contents = sut.GetDirectoryContents(subpath));

            Assert.Null(exception);
            Assert.False(contents.Exists);
            Assert.Same(NotFoundDirectoryContents.Singleton, contents);
        }
    }

    [Fact]
    public void GetDirectoryContents_ShouldReturnNotFoundForDirectoryCollision_WhenPhysicalEntriesAreCreatedInReverseOrder()
    {
        Assert.SkipWhen(!PortablePhysicalFileProviderTestCapabilities.SupportsDistinctCaseEntries, PortablePhysicalFileProviderTestCapabilities.DistinctCaseEntriesUnsupportedReason);

        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("Assets/banner.svg", "upper");
        scope.CreateFile("assets/logo.svg", "lower");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        foreach (var subpath in new[] { "assets", "Assets", "ASSETS", "aSsEtS" })
        {
            var contents = sut.GetDirectoryContents(subpath);

            Assert.False(contents.Exists);
            Assert.Same(NotFoundDirectoryContents.Singleton, contents);
        }
    }

    [Fact]
    public void GetFileInfo_ShouldReturnNotFound_WhenIntermediateDirectorySegmentCollides()
    {
        Assert.SkipWhen(!PortablePhysicalFileProviderTestCapabilities.SupportsDistinctCaseEntries, PortablePhysicalFileProviderTestCapabilities.DistinctCaseEntriesUnsupportedReason);

        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("assets/logo.svg", "lower");
        scope.CreateFile("Assets/banner.svg", "upper");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var info = sut.GetFileInfo("assets/logo.svg");

        Assert.False(info.Exists);
    }

    [Fact]
    public void GetDirectoryContents_ShouldResolveRemainingDirectory_WhenCollisionEntryIsRemoved()
    {
        Assert.SkipWhen(!PortablePhysicalFileProviderTestCapabilities.SupportsDistinctCaseEntries, PortablePhysicalFileProviderTestCapabilities.DistinctCaseEntriesUnsupportedReason);

        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("assets/logo.svg", "lower");
        scope.CreateFile("Assets/banner.svg", "upper");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        Assert.False(sut.GetDirectoryContents("ASSETS").Exists);

        scope.DeleteDirectory("Assets");

        var contents = sut.GetDirectoryContents("ASSETS");

        Assert.True(contents.Exists);
        Assert.Equal(new[] { "logo.svg" }, GetOrderedNames(contents));
    }

    [Fact]
    public void GetFileInfoGetDirectoryContentsAndWatch_ShouldTreatCrossKindEntriesAsCollision()
    {
        Assert.SkipWhen(!PortablePhysicalFileProviderTestCapabilities.SupportsDistinctCaseEntries, PortablePhysicalFileProviderTestCapabilities.DistinctCaseEntriesUnsupportedReason);

        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("content", "file");
        scope.CreateDirectory("Content");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        foreach (var subpath in new[] { "content", "Content", "CONTENT" })
        {
            var file = sut.GetFileInfo(subpath);
            var directory = sut.GetDirectoryContents(subpath);
            var fileWatch = sut.Watch(subpath);
            var directoryWatch = sut.Watch(subpath + "/");

            AssertNotFoundFileInfo(subpath, file);
            Assert.False(directory.Exists);
            Assert.Same(NotFoundDirectoryContents.Singleton, directory);
            Assert.Same(NullChangeToken.Singleton, fileWatch);
            Assert.Same(NullChangeToken.Singleton, directoryWatch);
        }
    }

    [Fact]
    public void GetFileInfo_ShouldReturnNotFound_WhenUniqueDirectoryIsRequestedAsFile()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateDirectory("content");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var info = sut.GetFileInfo("CONTENT");

        Assert.False(info.Exists);
    }

    [Fact]
    public void GetDirectoryContents_ShouldReturnNotFound_WhenUniqueFileIsRequestedAsDirectory()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("content", "file");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var contents = sut.GetDirectoryContents("CONTENT");

        Assert.False(contents.Exists);
        Assert.Same(NotFoundDirectoryContents.Singleton, contents);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("*.svg")]
    [InlineData("assets/*.svg")]
    public void Watch_ShouldMirrorPhysicalFileProviderBehavior_ForNullEmptyAndWildcardFilters(string filter)
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("assets/logo.svg", "one");

        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        SetPolling(expected);
        SetPolling(sut);

        var baseline = expected.Watch(filter);
        var token = sut.Watch(filter);

        AssertEquivalentChangeToken(baseline, token);
    }

    [Fact]
    public void Watch_ShouldDelegateWildcardFilterUnchanged_WhenMatchingDirectoryCollides()
    {
        Assert.SkipWhen(!PortablePhysicalFileProviderTestCapabilities.SupportsDistinctCaseEntries, PortablePhysicalFileProviderTestCapabilities.DistinctCaseEntriesUnsupportedReason);

        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("assets/logo.svg", "lower");
        scope.CreateFile("Assets/banner.svg", "upper");

        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        SetPolling(expected);
        SetPolling(sut);

        var baseline = expected.Watch("assets/*.svg");
        var token = sut.Watch("assets/*.svg");

        AssertEquivalentChangeToken(baseline, token);
        Assert.False(ReferenceEquals(NullChangeToken.Singleton, token));
    }

    [Fact]
    public async Task Watch_ShouldResolveUniqueLiteralFileFilterAndNotifyOnChange_WhenSegmentsUseDifferentCasing()
    {
        using var scope = new TemporaryFileSystemScope();
        var physicalPath = scope.CreateFile("Assets/Images/Logo.svg", "watch");

        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);
        SetPolling(expected);
        SetPolling(sut);

        var baseline = expected.Watch("Assets/Images/Logo.svg");
        var token = sut.Watch("assets/images/logo.svg");

        AssertEquivalentChangeToken(baseline, token);
        Assert.False(ReferenceEquals(NullChangeToken.Singleton, token));

        await AssertEquivalentChangeNotificationAsync(baseline, token, () => File.AppendAllText(physicalPath, Guid.NewGuid().ToString("N")));
    }

    [Fact]
    public async Task Watch_ShouldResolveLiteralDirectoryFilterWithTrailingSeparator_AndNotifyOnChange()
    {
        using var scope = new TemporaryFileSystemScope();
        var directoryPath = scope.CreateDirectory("Assets");

        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);
        SetPolling(expected);
        SetPolling(sut);

        var baseline = expected.Watch("Assets/");
        var token = sut.Watch("assets/");

        AssertEquivalentChangeToken(baseline, token);
        Assert.False(ReferenceEquals(NullChangeToken.Singleton, token));

        await AssertEquivalentChangeNotificationAsync(baseline, token, () => File.WriteAllText(Path.Combine(directoryPath, "new.txt"), Guid.NewGuid().ToString("N")));
    }

    [Fact]
    public void Watch_ShouldMirrorPhysicalFileProviderBehavior_WhenLiteralDirectoryFilterHasNoTrailingSeparator()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateDirectory("Assets");

        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        SetPolling(expected);
        SetPolling(sut);

        var baseline = expected.Watch("AsSeTs");
        var token = sut.Watch("AsSeTs");

        AssertEquivalentChangeToken(baseline, token);
    }

    [Fact]
    public void Watch_ShouldMirrorPhysicalFileProviderBehavior_WhenLiteralDirectoryFilterUsesTrailingBackslash()
    {
        using var scope = new TemporaryFileSystemScope();
        scope.CreateDirectory("Assets");

        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        SetPolling(expected);
        SetPolling(sut);

        var baseline = expected.Watch("Assets\\");
        var token = sut.Watch("Assets\\");

        AssertEquivalentChangeToken(baseline, token);
    }

    [Fact]
    public void Watch_ShouldMirrorPhysicalFileProviderBehavior_ForMissingLiteralFilter()
    {
        using var scope = new TemporaryFileSystemScope();
        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        SetPolling(expected);
        SetPolling(sut);

        var baseline = expected.Watch("missing/logo.svg");
        var token = sut.Watch("missing/logo.svg");

        AssertEquivalentChangeToken(baseline, token);
    }

    [Fact]
    public void Watch_ShouldReturnNullChangeToken_WhenLiteralFileFilterCollides()
    {
        Assert.SkipWhen(!PortablePhysicalFileProviderTestCapabilities.SupportsDistinctCaseEntries, PortablePhysicalFileProviderTestCapabilities.DistinctCaseEntriesUnsupportedReason);

        using var scope = new TemporaryFileSystemScope();
        scope.CreateFile("logo.svg", "lower");
        scope.CreateFile("Logo.svg", "upper");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var token = sut.Watch("LOGO.SVG");

        Assert.Same(NullChangeToken.Singleton, token);
    }

    [Fact]
    public void Watch_ShouldReturnNullChangeToken_WhenLiteralDirectoryFilterCollides()
    {
        Assert.SkipWhen(!PortablePhysicalFileProviderTestCapabilities.SupportsDistinctCaseEntries, PortablePhysicalFileProviderTestCapabilities.DistinctCaseEntriesUnsupportedReason);

        using var scope = new TemporaryFileSystemScope();
        scope.CreateDirectory("assets");
        scope.CreateDirectory("Assets");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var token = sut.Watch("ASSETS/");

        Assert.Same(NullChangeToken.Singleton, token);
    }

    [Fact]
    public void Watch_ShouldReturnNullChangeToken_WhenIntermediateDirectorySegmentCollides()
    {
        Assert.SkipWhen(!PortablePhysicalFileProviderTestCapabilities.SupportsDistinctCaseEntries, PortablePhysicalFileProviderTestCapabilities.DistinctCaseEntriesUnsupportedReason);

        using var scope = new TemporaryFileSystemScope();
        scope.CreateDirectory("assets");
        scope.CreateDirectory("Assets");
        scope.CreateFile("assets/logo.svg", "lower");

        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var token = sut.Watch("ASSETS/logo.svg");

        Assert.Same(NullChangeToken.Singleton, token);
    }

    [Fact]
    public void FindMatchingEntry_ShouldReturnUniqueMatch_WhenExactlyOneLogicalIdentityExists()
    {
        var result = FindMatchingEntry("LOGO.SVG", () => new[] { new StubFileInfo("logo.svg"), new StubFileInfo("banner.svg") }, out var collision);

        Assert.False(collision);
        Assert.NotNull(result);
        Assert.Equal("logo.svg", result.Name);
        Assert.False(result.IsDirectory);
    }

    [Fact]
    public void FindMatchingEntry_ShouldTreatFileAndDirectoryWithSameLogicalIdentityAsCollision()
    {
        var result = FindMatchingEntry("LOGO", () => new IFileInfo[] { new StubFileInfo("logo", isDirectory: false), new StubFileInfo("Logo", isDirectory: true) }, out var collision);

        Assert.True(collision);
        Assert.Null(result);
    }

    [Theory]
    [MemberData(nameof(SupportedLookupExceptionFactories))]
    public void FindMatchingEntry_ShouldReturnNullWithoutCollision_WhenEntriesFactoryThrowsSupportedException(Func<Exception> exceptionFactory)
    {
        var result = FindMatchingEntry("logo.svg", () => throw exceptionFactory(), out var collision);

        Assert.False(collision);
        Assert.Null(result);
    }

    [Fact]
    public void FindMatchingEntry_ShouldPropagateUnexpectedExceptions()
    {
        var exception = Assert.Throws<InvalidOperationException>(() => FindMatchingEntry("logo.svg", () => throw new InvalidOperationException("boom"), out _));

        Assert.Equal("boom", exception.Message);
    }

    [Fact]
    public void ResolvePath_ShouldReturnOriginalSubpathAndSetCollision_WhenMultipleLogicalMatchesExist()
    {
        using var scope = new TemporaryFileSystemScope();
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var resolvedPath = ResolvePathWithEntries(sut, "logo.svg", false, _ => new IFileInfo[] { new StubFileInfo("logo.svg"), new StubFileInfo("Logo.svg") }, out var collision);

        Assert.True(collision);
        Assert.Equal("logo.svg", resolvedPath);
    }

    [Fact]
    public void ResolveFileInfo_ShouldReturnNotFoundFileInfo_WhenCollisionIsTrue()
    {
        using var scope = new TemporaryFileSystemScope();
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var info = ResolveFileInfoSelection(sut, "logo.svg", "logo.svg", true);

        AssertNotFoundFileInfo("logo.svg", info);
    }

    [Fact]
    public void ResolveDirectoryContents_ShouldReturnNotFoundSingleton_WhenCollisionIsTrue()
    {
        using var scope = new TemporaryFileSystemScope();
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var contents = ResolveDirectoryContentsSelection(sut, "assets", true);

        Assert.False(contents.Exists);
        Assert.Same(NotFoundDirectoryContents.Singleton, contents);
    }

    [Fact]
    public void ResolveWatchToken_ShouldReturnNullChangeToken_WhenCollisionIsTrue()
    {
        using var scope = new TemporaryFileSystemScope();
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        var token = ResolveWatchTokenSelection(sut, "assets/logo.svg", false, true);

        Assert.Same(NullChangeToken.Singleton, token);
    }

    [Fact]
    public void ResolveWatchToken_ShouldDelegateEmptyResolvedDirectoryFilter_WhenCollisionIsFalse()
    {
        using var scope = new TemporaryFileSystemScope();
        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        SetPolling(expected);
        SetPolling(sut);

        var baseline = expected.Watch(string.Empty);
        var token = ResolveWatchTokenSelection(sut, string.Empty, true, false);

        AssertEquivalentChangeToken(baseline, token);
    }

    [Fact]
    public void ResolveWatchToken_ShouldDelegateResolvedDirectoryFilter_WhenItAlreadyEndsWithForwardSlash()
    {
        using var scope = new TemporaryFileSystemScope();
        using var expected = new PhysicalFileProvider(scope.RootPath);
        using var sut = new PortablePhysicalFileProvider(scope.RootPath);

        SetPolling(expected);
        SetPolling(sut);

        var baseline = expected.Watch("/");
        var token = ResolveWatchTokenSelection(sut, "/", true, false);

        AssertEquivalentChangeToken(baseline, token);
    }

    public static TheoryData<Func<Exception>> SupportedLookupExceptionFactories => new()
    {
        () => new ArgumentException("boom"),
        () => new DirectoryNotFoundException("boom"),
        () => new IOException("boom"),
        () => new SecurityException("boom"),
        () => new UnauthorizedAccessException("boom")
    };

    private delegate IFileInfo FindMatchingEntryDelegate(string requestedName, Func<IEnumerable<IFileInfo>> entriesFactory, out bool collision);
    private delegate string ResolvePathDelegate(PortablePhysicalFileProvider provider, string subpath, bool finalSegmentIsDirectory, Func<string, IEnumerable<IFileInfo>> entriesFactory, out bool collision);
    private delegate IFileInfo ResolveFileInfoDelegate(PortablePhysicalFileProvider provider, string subpath, string resolvedPath, bool collision);
    private delegate IDirectoryContents ResolveDirectoryContentsDelegate(PortablePhysicalFileProvider provider, string resolvedPath, bool collision);
    private delegate IChangeToken ResolveWatchTokenDelegate(PortablePhysicalFileProvider provider, string resolvedFilter, bool finalSegmentIsDirectory, bool collision);

    private static FindMatchingEntryDelegate CreateFindMatchingEntryDelegate()
    {
        var method = typeof(PortablePhysicalFileProvider).GetMethod("FindMatchingEntry", BindingFlags.NonPublic | BindingFlags.Static);

        if (method is null)
        {
            throw new InvalidOperationException("Unable to locate PortablePhysicalFileProvider.FindMatchingEntry.");
        }

        return (FindMatchingEntryDelegate)method.CreateDelegate(typeof(FindMatchingEntryDelegate));
    }

    private static ResolvePathDelegate CreateResolvePathDelegate()
    {
        var method = typeof(PortablePhysicalFileProvider).GetMethod("ResolvePath", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(string), typeof(bool), typeof(Func<string, IEnumerable<IFileInfo>>), typeof(bool).MakeByRefType() }, null);

        if (method is null)
        {
            throw new InvalidOperationException("Unable to locate PortablePhysicalFileProvider.ResolvePath.");
        }

        return (ResolvePathDelegate)method.CreateDelegate(typeof(ResolvePathDelegate));
    }

    private static ResolveFileInfoDelegate CreateResolveFileInfoDelegate()
    {
        var method = typeof(PortablePhysicalFileProvider).GetMethod("ResolveFileInfo", BindingFlags.NonPublic | BindingFlags.Instance);

        if (method is null)
        {
            throw new InvalidOperationException("Unable to locate PortablePhysicalFileProvider.ResolveFileInfo.");
        }

        return (ResolveFileInfoDelegate)method.CreateDelegate(typeof(ResolveFileInfoDelegate));
    }

    private static ResolveDirectoryContentsDelegate CreateResolveDirectoryContentsDelegate()
    {
        var method = typeof(PortablePhysicalFileProvider).GetMethod("ResolveDirectoryContents", BindingFlags.NonPublic | BindingFlags.Instance);

        if (method is null)
        {
            throw new InvalidOperationException("Unable to locate PortablePhysicalFileProvider.ResolveDirectoryContents.");
        }

        return (ResolveDirectoryContentsDelegate)method.CreateDelegate(typeof(ResolveDirectoryContentsDelegate));
    }

    private static ResolveWatchTokenDelegate CreateResolveWatchTokenDelegate()
    {
        var method = typeof(PortablePhysicalFileProvider).GetMethod("ResolveWatchToken", BindingFlags.NonPublic | BindingFlags.Instance);

        if (method is null)
        {
            throw new InvalidOperationException("Unable to locate PortablePhysicalFileProvider.ResolveWatchToken.");
        }

        return (ResolveWatchTokenDelegate)method.CreateDelegate(typeof(ResolveWatchTokenDelegate));
    }

    private static void AssertEquivalentException(Exception expected, Exception actual)
    {
        if (expected is null || actual is null)
        {
            Assert.Equal(expected is null, actual is null);
            return;
        }

        Assert.Equal(expected.GetType(), actual.GetType());
        Assert.Equal(expected.Message, actual.Message);
    }

    private static void AssertEquivalentFileInfo(IFileInfo expected, IFileInfo actual)
    {
        Assert.Equal(expected.Exists, actual.Exists);
        Assert.Equal(expected.IsDirectory, actual.IsDirectory);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.PhysicalPath, actual.PhysicalPath);

        if (!expected.Exists || !actual.Exists)
        {
            return;
        }

        Assert.Equal(expected.Length, actual.Length);
    }

    private static void AssertEquivalentDirectoryContents(IDirectoryContents expected, IDirectoryContents actual)
    {
        Assert.Equal(expected.Exists, actual.Exists);
        Assert.Equal(ReferenceEquals(NotFoundDirectoryContents.Singleton, expected), ReferenceEquals(NotFoundDirectoryContents.Singleton, actual));
        Assert.Equal(GetOrderedNames(expected), GetOrderedNames(actual));
    }

    private static void AssertEquivalentChangeToken(IChangeToken expected, IChangeToken actual)
    {
        Assert.Equal(expected.GetType(), actual.GetType());
        Assert.Equal(expected.ActiveChangeCallbacks, actual.ActiveChangeCallbacks);
        // HasChanged is a live observation and may differ between independently created polling tokens.
        Assert.Equal(ReferenceEquals(NullChangeToken.Singleton, expected), ReferenceEquals(NullChangeToken.Singleton, actual));
    }

    private static void AssertNotFoundFileInfo(string subpath, IFileInfo info)
    {
        var expected = new NotFoundFileInfo(subpath);

        Assert.False(info.Exists);
        Assert.Equal(expected.IsDirectory, info.IsDirectory);
        Assert.Equal(expected.Length, info.Length);
        Assert.Equal(expected.Name, info.Name);
        Assert.Equal(expected.PhysicalPath, info.PhysicalPath);
    }

    private static void SetPolling(PhysicalFileProvider provider)
    {
        provider.UsePollingFileWatcher = true;
        provider.UseActivePolling = true;
    }

    private static void SetPolling(PortablePhysicalFileProvider provider)
    {
        provider.UsePollingFileWatcher = true;
        provider.UseActivePolling = true;
    }

    private static async Task AssertEquivalentChangeNotificationAsync(IChangeToken expected, IChangeToken actual, Action changeAction)
    {
        var expectedChanged = WaitForChangeAsync(expected);
        var actualChanged = WaitForChangeAsync(actual);

        changeAction();

        Assert.Equal(await expectedChanged.ConfigureAwait(false), await actualChanged.ConfigureAwait(false));
    }

    private static async Task<bool> WaitForChangeAsync(IChangeToken token)
    {
        var changed = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

        using (token.RegisterChangeCallback(_ => changed.TrySetResult(null), null))
        {
            var completed = await Task.WhenAny(changed.Task, Task.Delay(ChangeNotificationTimeout)).ConfigureAwait(false);
            return ReferenceEquals(completed, changed.Task);
        }
    }

    private static string[] GetOrderedNames(IEnumerable<IFileInfo> entries)
    {
        return entries.Select(entry => entry.Name).OrderBy(name => name, StringComparer.Ordinal).ToArray();
    }

    private static int GetSuccessfulPathCacheCount(PortablePhysicalFileProvider provider, bool finalSegmentIsDirectory)
    {
        return GetSuccessfulPathCache(provider, finalSegmentIsDirectory).Count;
    }

    private static ConcurrentDictionary<string, string> GetSuccessfulPathCache(PortablePhysicalFileProvider provider, bool finalSegmentIsDirectory)
    {
        var fieldName = finalSegmentIsDirectory ? "_directories" : "_files";
        var field = typeof(PortablePhysicalFileProvider).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

        if (field?.GetValue(provider) is ConcurrentDictionary<string, string> cache)
        {
            return cache;
        }

        throw new InvalidOperationException($"Unable to locate PortablePhysicalFileProvider.{fieldName}.");
    }

    private static string CreateMixedSeparatorAlias(string firstSegment, string secondSegment, string thirdSegment = null, bool trailingSeparator = false)
    {
        var primarySeparator = Path.DirectorySeparatorChar;
        var alternateSeparator = Path.DirectorySeparatorChar == Path.AltDirectorySeparatorChar ? Path.DirectorySeparatorChar : Path.AltDirectorySeparatorChar;
        var builder = new StringBuilder();

        builder.Append(primarySeparator);
        builder.Append(alternateSeparator);
        builder.Append(firstSegment);
        builder.Append(primarySeparator);
        builder.Append(alternateSeparator);
        builder.Append(secondSegment);

        if (thirdSegment is not null)
        {
            builder.Append(alternateSeparator);
            builder.Append(primarySeparator);
            builder.Append(thirdSegment);
        }

        if (trailingSeparator)
        {
            builder.Append(primarySeparator);
            builder.Append(alternateSeparator);
        }

        return builder.ToString();
    }

    private static string ReadAllText(IFileInfo info)
    {
        using var stream = info.CreateReadStream();
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private sealed class TemporaryFileSystemScope : IDisposable
    {
        public TemporaryFileSystemScope()
        {
            RootPath = Path.Combine(Path.GetTempPath(), "cuemon", "portable-physical-file-provider", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(RootPath);
        }

        public string RootPath { get; }

        public string CreateDirectory(string providerPath)
        {
            var path = GetPhysicalPath(providerPath);
            Directory.CreateDirectory(path);
            return path;
        }

        public string CreateFile(string providerPath, string contents)
        {
            var path = GetPhysicalPath(providerPath);
            var directoryPath = Path.GetDirectoryName(path);

            if (!string.IsNullOrEmpty(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(path, contents);
            return path;
        }

        public string CreateSensitiveFile()
        {
            var providerPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Sensitive.txt" : ".Sensitive.txt";
            var physicalPath = CreateFile(providerPath, "sensitive");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                File.SetAttributes(physicalPath, File.GetAttributes(physicalPath) | FileAttributes.Hidden);
            }

            return providerPath;
        }

        public void DeleteFile(string providerPath)
        {
            var path = GetPhysicalPath(providerPath);

            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }
        }

        public void DeleteDirectory(string providerPath)
        {
            var path = GetPhysicalPath(providerPath);

            if (Directory.Exists(path))
            {
                ClearAttributes(path);
                Directory.Delete(path, true);
            }
        }

        public string GetPhysicalPath(string providerPath)
        {
            if (string.IsNullOrEmpty(providerPath))
            {
                return RootPath;
            }

            var path = RootPath;

            foreach (var segment in providerPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
            {
                path = Path.Combine(path, segment);
            }

            return path;
        }

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                ClearAttributes(RootPath);
                Directory.Delete(RootPath, true);
            }
        }

        private static void ClearAttributes(string path)
        {
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                return;
            }

            if (!Directory.Exists(path))
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }

            foreach (var directory in Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(directory, FileAttributes.Normal);
            }

            File.SetAttributes(path, FileAttributes.Normal);
        }
    }

    private sealed class StubFileInfo : IFileInfo
    {
        public StubFileInfo(string name, bool isDirectory = false)
        {
            Name = name;
            IsDirectory = isDirectory;
        }

        public bool Exists => true;

        public long Length => 0;

        public string PhysicalPath => null;

        public string Name { get; }

        public DateTimeOffset LastModified => DateTimeOffset.MinValue;

        public bool IsDirectory { get; }

        public Stream CreateReadStream()
        {
            return new MemoryStream();
        }
    }

    private static class PortablePhysicalFileProviderTestCapabilities
    {
        private static readonly Lazy<CaseDistinctCapability> CaseDistinctEntries = new(DetectCaseDistinctEntries);

        public static bool SupportsDistinctCaseEntries => CaseDistinctEntries.Value.Supported;

        public static string DistinctCaseEntriesUnsupportedReason => CaseDistinctEntries.Value.UnsupportedReason ?? "The temporary filesystem supports distinct entries whose names differ only by casing.";

        private static CaseDistinctCapability DetectCaseDistinctEntries()
        {
            var rootPath = Path.Combine(Path.GetTempPath(), "cuemon", "portable-physical-file-provider-probe", Guid.NewGuid().ToString("N"));
            var lowerPath = Path.Combine(rootPath, "probe");
            var upperPath = Path.Combine(rootPath, "PROBE");

            Directory.CreateDirectory(rootPath);

            try
            {
                using (File.Open(lowerPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                }

                try
                {
                    using (File.Open(upperPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                    {
                    }
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    return new CaseDistinctCapability(false, "The temporary filesystem does not permit two distinct entries whose names differ only by casing.");
                }

                var names = Directory.EnumerateFileSystemEntries(rootPath).Select(Path.GetFileName).Where(name => name is not null).ToArray();

                if (names.Length == 2 &&
                    names.Contains("probe", StringComparer.Ordinal) &&
                    names.Contains("PROBE", StringComparer.Ordinal))
                {
                    return new CaseDistinctCapability(true, null);
                }

                return new CaseDistinctCapability(false, "The temporary filesystem did not preserve both case-distinct probe entries.");
            }
            finally
            {
                if (File.Exists(lowerPath))
                {
                    File.SetAttributes(lowerPath, FileAttributes.Normal);
                    File.Delete(lowerPath);
                }

                if (File.Exists(upperPath))
                {
                    File.SetAttributes(upperPath, FileAttributes.Normal);
                    File.Delete(upperPath);
                }

                if (Directory.Exists(rootPath))
                {
                    Directory.Delete(rootPath, true);
                }
            }
        }

        private sealed class CaseDistinctCapability
        {
            public CaseDistinctCapability(bool supported, string unsupportedReason)
            {
                Supported = supported;
                UnsupportedReason = unsupportedReason;
            }

            public bool Supported { get; }

            public string UnsupportedReason { get; }
        }
    }
}
