using System;
using System.Collections.Generic;
using System.Reflection;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;

namespace Cuemon.Resilience
{
    internal abstract class Transient<TOptions> where TOptions : TransientOperationOptions, new()
    {
#if NET10_0_OR_GREATER
        protected readonly System.Threading.Lock _lock = new();
#else
        protected readonly object _lock = new();
#endif

        protected Transient(MethodInfo delegateInfo, object[] runtimeArguments, Action<TOptions> setup)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            DelegateInfo = delegateInfo;
            RuntimeArguments = runtimeArguments;
            Options = options;
        }

        protected MethodInfo DelegateInfo { get; }

        protected object[] RuntimeArguments { get; }

        protected TOptions Options { get; }

        protected DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        protected TimeSpan Latency { get; set; } = TimeSpan.Zero;

        protected TimeSpan LastWaitTime { get; set; } = TimeSpan.Zero;

        protected TimeSpan TotalWaitTime { get; set; } = TimeSpan.Zero;

        protected bool IsTransientFault { get; set; }

        protected bool ThrowExceptions { get; set; }

        protected IList<Exception> AggregatedExceptions { get; } = new List<Exception>();

        protected void ResilientTry()
        {
            if (Latency > Options.MaximumAllowedLatency) { throw new LatencyException(FormattableString.Invariant($"The latency of the operation exceeded the allowed maximum value of {Options.MaximumAllowedLatency.TotalSeconds} seconds. Actual latency was: {Latency.TotalSeconds} seconds.")); }
        }

        protected void ResilientThrower()
        {
            if (ThrowExceptions) { throw new AggregateException(AggregatedExceptions); }
        }

        protected void ResilientFinally<TResult>(TResult result)
        {
            if (ThrowExceptions)
            {
                var disposable = result as IDisposable;
                disposable?.Dispose();
            }
        }

        protected void ResilientCatchInnerCatch(int attempts)
        {
            ThrowExceptions = true;
            if (IsTransientFault)
            {
                var runtimeArguments = RuntimeArguments;
                if (Options is AsyncTransientOperationOptions asyncOptions) { runtimeArguments = Arguments.Concat(RuntimeArguments, Arguments.ToArray(asyncOptions.CancellationToken)); } // we need to match the signature of async methods on TransientOperation
                var evidence = new TransientFaultEvidence(attempts, LastWaitTime, TotalWaitTime, Latency, new MethodDescriptor(DelegateInfo).AppendRuntimeArguments(runtimeArguments));
                var transientException = new TransientFaultException("The amount of retry attempts has been reached.", evidence);
                lock (_lock) { AggregatedExceptions.Insert(0, transientException); }
                TransientOperation.FaultCallback?.Invoke(evidence);
            }
        }
    }
}
