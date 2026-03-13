using System;
using System.Threading;

namespace Cuemon.Assets
{
    public class ManagedOnlyDisposable : Disposable
    {
        public int ManagedDisposeCount { get; private set; }

        public void DisposeCore(bool disposing)
        {
            Dispose(disposing);
        }

        protected override void OnDisposeManagedResources()
        {
            ManagedDisposeCount++;
        }
    }

    public class TrackingDisposable : Disposable
    {
        public int ManagedDisposeCount { get; private set; }

        public int UnmanagedDisposeCount { get; private set; }

        public void DisposeCore(bool disposing)
        {
            Dispose(disposing);
        }

        protected override void OnDisposeManagedResources()
        {
            ManagedDisposeCount++;
        }

        protected override void OnDisposeUnmanagedResources()
        {
            UnmanagedDisposeCount++;
        }
    }

    public sealed class BlockingDisposable : Disposable
    {
        public BlockingDisposable(ManualResetEventSlim managedStarted, ManualResetEventSlim continueDisposal)
        {
            ManagedStarted = managedStarted;
            ContinueDisposal = continueDisposal;
        }

        public ManualResetEventSlim ManagedStarted { get; }

        public ManualResetEventSlim ContinueDisposal { get; }

        public int ManagedDisposeCount { get; private set; }

        public int UnmanagedDisposeCount { get; private set; }

        protected override void OnDisposeManagedResources()
        {
            ManagedDisposeCount++;
            ManagedStarted.Set();
            ContinueDisposal.Wait(TimeSpan.FromSeconds(5));
        }

        protected override void OnDisposeUnmanagedResources()
        {
            UnmanagedDisposeCount++;
        }
    }

    public class TrackingFinalizeDisposable : FinalizeDisposable
    {
        public static int UnmanagedDisposeCount;

        public void DisposeCore(bool disposing)
        {
            Dispose(disposing);
        }

        public static void Reset()
        {
            UnmanagedDisposeCount = 0;
        }

        protected override void OnDisposeUnmanagedResources()
        {
            Interlocked.Increment(ref UnmanagedDisposeCount);
        }
    }
}
