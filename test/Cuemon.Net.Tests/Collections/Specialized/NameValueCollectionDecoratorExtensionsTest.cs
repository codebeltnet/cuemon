using System;
using System.Collections.Specialized;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Net.Collections.Specialized;
public class NameValueCollectionDecoratorExtensionsTest : Test
{
    public NameValueCollectionDecoratorExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void NameValueCollectionDecoratorExtensions_ShouldFormatCollectionsWithDifferentSeparators()
    {
        var values = new NameValueCollection()
        {
            { "name", "Jane Doe" },
            { "tags", "one,two" }
        };
        IDecorator<NameValueCollection> decorator = null;

        var ampersand = Decorator.Enclose(values).ToString(FieldValueSeparator.Ampersand, true);
        var semicolon = Decorator.Enclose(values).ToString(FieldValueSeparator.Semicolon, false);

        Assert.Equal("?name=Jane+Doe&tags=one&tags=two", ampersand);
        Assert.Equal("name=Jane Doe;tags=one;tags=two;", semicolon);
        Assert.Throws<ArgumentNullException>(() => NameValueCollectionDecoratorExtensions.ToString(decorator, FieldValueSeparator.Ampersand, false));
        Assert.Throws<System.ComponentModel.InvalidEnumArgumentException>(() => Decorator.Enclose(values).ToString((FieldValueSeparator)99, false));
    }
}
