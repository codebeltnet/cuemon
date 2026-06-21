using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions
{
    public class WrapperTest : Test
    {
        public WrapperTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenInstanceIsNull()
        {
            string instance = null;

            var exception = Assert.Throws<ArgumentNullException>(() => new Wrapper<string>(instance));

            Assert.Equal("instance", exception.ParamName);
        }

        [Fact]
        public void ParseInstance_ShouldThrowArgumentNullException_WhenWrapperIsNull()
        {
            IWrapper<string> wrapper = null;

            var exception = Assert.Throws<ArgumentNullException>(() => Wrapper.ParseInstance(wrapper));

            Assert.Equal("wrapper", exception.ParamName);
        }

        [Fact]
        public void InstanceAs_ShouldConvertWrappedValue_WhenUsingInvariantAndExplicitProviders()
        {
            var invariant = new Wrapper<string>("42");
            var providerAware = new Wrapper<int>(42);
            var culture = CultureInfo.GetCultureInfo("da-DK");

            Assert.Equal(42, invariant.InstanceAs<int>());
            Assert.Equal("42", providerAware.InstanceAs<string>(culture));
        }

        [Fact]
        public void Wrapper_ShouldExposeMetadataAndStructuredFormatting_WhenWrappingComplexValues()
        {
            var member = typeof(WrapperTestModel).GetProperty(nameof(WrapperTestModel.Name), BindingFlags.Public | BindingFlags.Instance);
            var wrapper = new Wrapper<int>(42, member);
            var pair = new Wrapper<KeyValuePair<string, object>>(new KeyValuePair<string, object>("alpha", null));
            var comparer = new Wrapper<StringComparer>(StringComparer.OrdinalIgnoreCase);

            wrapper.Data.Add("answer", true);

            Assert.True(wrapper.HasMemberReference);
            Assert.Same(member, wrapper.MemberReference);
            Assert.True(wrapper.Data.ContainsKey("answer"));
            Assert.Equal("[alpha,null]", pair.ToString());
            Assert.Contains("Comparer", comparer.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public void ParseInstance_ShouldReturnExpectedRepresentation_WhenWrappingPrimitiveAndSpecialValues()
        {
            var timestamp = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);
            var bytes = new byte[] { 1, 2, 3, 4 };
            var guid = Guid.Parse("11111111-2222-3333-4444-555555555555");
            var uri = new Uri("https://example.com/path?value=42", UriKind.Absolute);

            Assert.Equal("false", new Wrapper<bool>(false).ToString());
            Assert.Equal("42.5", new Wrapper<decimal>(42.5m).ToString());
            Assert.Equal(timestamp.ToString("O", CultureInfo.InvariantCulture), new Wrapper<DateTime>(timestamp).ToString());
            Assert.Equal("hello", new Wrapper<string>("hello").ToString());
            Assert.Equal(Convert.ToBase64String(bytes), new Wrapper<byte[]>(bytes).ToString());
            Assert.Equal(guid.ToString("D"), new Wrapper<Guid>(guid).ToString());
            Assert.Equal(typeof(Dictionary<string, int>).ToFriendlyName(), new Wrapper<Type>(typeof(Dictionary<string, int>)).ToString());
            Assert.Equal(uri.OriginalString, new Wrapper<Uri>(uri).ToString());
        }

        private sealed class WrapperTestModel
        {
            public string Name { get; set; }
        }
    }
}
