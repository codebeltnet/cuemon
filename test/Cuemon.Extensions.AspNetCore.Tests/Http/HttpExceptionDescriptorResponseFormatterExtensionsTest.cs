using System;
using System.Collections.Generic;
using System.Linq;
using Cuemon.AspNetCore.Diagnostics;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Cuemon.Extensions.AspNetCore.Http;
public class HttpExceptionDescriptorResponseFormatterExtensionsTest : Test
{
    public HttpExceptionDescriptorResponseFormatterExtensionsTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void SelectExceptionDescriptorHandlers_ShouldThrowArgumentNullException_WhenFormattersIsNull()
    {
        Assert.Throws<ArgumentNullException>("formatters", () =>
        {
            HttpExceptionDescriptorResponseFormatterExtensions.SelectExceptionDescriptorHandlers(null).ToList();
        });
    }

    [Fact]
    public void SelectExceptionDescriptorHandlers_ShouldReturnEmptySequence_WhenFormattersIsEmpty()
    {
        var sut = new List<IHttpExceptionDescriptorResponseFormatter>();

        var result = sut.SelectExceptionDescriptorHandlers().ToList();

        Assert.Empty(result);
    }
}
