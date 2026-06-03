using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace Cuemon.AspNetCore.Mvc.Filters.ModelBinding
{
    public class DisableModelBindingAttributeTest : Test
    {
        public DisableModelBindingAttributeTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenValueProviderFactoryTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new DisableModelBindingAttribute(null));
        }

        [Fact]
        public void Constructor_ShouldThrowNotSupportedException_WhenValueProviderFactoryTypeIsUnsupported()
        {
            var ex = Assert.Throws<NotSupportedException>(() => new DisableModelBindingAttribute(typeof(DisableModelBindingAttributeTest)));

            Assert.Equal("Only a type that implements the IValueProviderFactory interface is supported.", ex.Message);
        }

        [Fact]
        public async Task OnResourceExecutionAsync_ShouldRemoveMatchingValueProviderFactoryType()
        {
            var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
            var factories = new List<IValueProviderFactory>()
            {
                new FormValueProviderFactory(),
                new QueryStringValueProviderFactory(),
                new FakeValueProviderFactory()
            };
            var context = new ResourceExecutingContext(actionContext, new List<IFilterMetadata>(), factories);
            var sut = new DisableModelBindingAttribute(typeof(FakeValueProviderFactory));
            var wasNextCalled = false;

            await sut.OnResourceExecutionAsync(context, () =>
            {
                wasNextCalled = true;
                return Task.FromResult(new ResourceExecutedContext(actionContext, new List<IFilterMetadata>()));
            });

            Assert.True(wasNextCalled);
            Assert.DoesNotContain(context.ValueProviderFactories, factory => factory is FakeValueProviderFactory);
            Assert.Contains(context.ValueProviderFactories, factory => factory is FormValueProviderFactory);
            Assert.Contains(context.ValueProviderFactories, factory => factory is QueryStringValueProviderFactory);
        }

        private sealed class FakeValueProviderFactory : IValueProviderFactory
        {
            public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
            {
                return Task.CompletedTask;
            }
        }
    }
}
