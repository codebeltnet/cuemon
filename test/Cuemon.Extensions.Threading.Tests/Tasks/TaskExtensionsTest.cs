using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;

namespace Cuemon.Extensions.Threading.Tasks
{
    public class TaskExtensionsTest : Test
    {
        public TaskExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ContinueWithCapturedContext_ShouldHaveTaskAwaiterSetToTrue()
        {
            var task = new Task(() =>
            {
            });

            var sut = task.ContinueWithCapturedContext();
            var configuredTaskAwaiter = sut.GetType().GetField("m_configuredTaskAwaiter", MemberReflection.Everything).GetValue(sut).As<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter>();
            var continueOnCapturedContext = false;
#if NET10_0_OR_GREATER
            continueOnCapturedContext = configuredTaskAwaiter.GetType().GetField("m_options", MemberReflection.Everything)?.GetValue(configuredTaskAwaiter)?.As<ConfigureAwaitOptions>() == ConfigureAwaitOptions.ContinueOnCapturedContext;
#else
            continueOnCapturedContext = configuredTaskAwaiter.GetType().GetField("m_continueOnCapturedContext", MemberReflection.Everything).GetValue(configuredTaskAwaiter).As<bool>();
#endif

            Assert.IsNotType<ConfiguredTaskAwaitable>(task);
            Assert.IsType<ConfiguredTaskAwaitable>(sut);
            Assert.IsType<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter>(configuredTaskAwaiter);
            Assert.True(continueOnCapturedContext);
        }

        [Fact]
        public void ContinueWithSuppressedContext_ShouldHaveTaskAwaiterSetToFalse()
        {
            var task = new Task(() =>
            {
            });

            var sut = task.ContinueWithSuppressedContext();
            var configuredTaskAwaiter = sut.GetType().GetField("m_configuredTaskAwaiter", MemberReflection.Everything).GetValue(sut).As<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter>();
            var continueOnCapturedContext = true;
#if NET10_0_OR_GREATER
            continueOnCapturedContext = configuredTaskAwaiter.GetType().GetField("m_options", MemberReflection.Everything)?.GetValue(configuredTaskAwaiter)?.As<ConfigureAwaitOptions>() == ConfigureAwaitOptions.ContinueOnCapturedContext;
#else
            continueOnCapturedContext = configuredTaskAwaiter.GetType().GetField("m_continueOnCapturedContext", MemberReflection.Everything).GetValue(configuredTaskAwaiter).As<bool>();
#endif

            Assert.IsNotType<ConfiguredTaskAwaitable>(task);
            Assert.IsType<ConfiguredTaskAwaitable>(sut);
            Assert.IsType<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter>(configuredTaskAwaiter);
            Assert.False(continueOnCapturedContext);
        }

        [Fact]
        public void ContinueWithCapturedContextOfResult_ShouldHaveTaskAwaiterSetToTrue()
        {
            var sut = new Task<int>(() => 0);

            var awaitableTask = sut.ContinueWithCapturedContext();
            var configuredTaskAwaiter = awaitableTask.GetType().GetField("m_configuredTaskAwaiter", MemberReflection.Everything).GetValue(awaitableTask).As<ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter>();
            var continueOnCapturedContext = false;
#if NET10_0_OR_GREATER
            continueOnCapturedContext = configuredTaskAwaiter.GetType().GetField("m_options", MemberReflection.Everything)?.GetValue(configuredTaskAwaiter)?.As<ConfigureAwaitOptions>() == ConfigureAwaitOptions.ContinueOnCapturedContext;
#else
            continueOnCapturedContext = configuredTaskAwaiter.GetType().GetField("m_continueOnCapturedContext", MemberReflection.Everything).GetValue(configuredTaskAwaiter).As<bool>();
#endif

            Assert.IsNotType<ConfiguredTaskAwaitable>(sut);
            Assert.IsType<ConfiguredTaskAwaitable<int>>(awaitableTask);
            Assert.IsType<ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter>(configuredTaskAwaiter);
            Assert.True(continueOnCapturedContext);
        }

        [Fact]
        public void ContinueWithSuppressedContextOfResult_ShouldHaveTaskAwaiterSetToFalse()
        {
            var task = new Task<int>(() => 0);

            var sut = task.ContinueWithSuppressedContext();
            var configuredTaskAwaiter = sut.GetType().GetField("m_configuredTaskAwaiter", MemberReflection.Everything).GetValue(sut).As<ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter>();
            var continueOnCapturedContext = true;
#if NET10_0_OR_GREATER
            continueOnCapturedContext = configuredTaskAwaiter.GetType().GetField("m_options", MemberReflection.Everything)?.GetValue(configuredTaskAwaiter)?.As<ConfigureAwaitOptions>() == ConfigureAwaitOptions.ContinueOnCapturedContext;
#else
            continueOnCapturedContext = configuredTaskAwaiter.GetType().GetField("m_continueOnCapturedContext", MemberReflection.Everything).GetValue(configuredTaskAwaiter).As<bool>();
#endif

            Assert.IsNotType<ConfiguredTaskAwaitable>(task);
            Assert.IsType<ConfiguredTaskAwaitable<int>>(sut);
            Assert.IsType<ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter>(configuredTaskAwaiter);
            Assert.False(continueOnCapturedContext);
        }
    }
}