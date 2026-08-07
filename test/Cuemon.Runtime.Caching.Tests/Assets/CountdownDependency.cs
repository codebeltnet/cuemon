using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Cuemon.Runtime.Caching.Assets;
public class CountdownDependency : Dependency, IDisposable
{
    private Timer _handler;
    private TimeSpan _timer;
    private Stopwatch _sw = Stopwatch.StartNew();

    public CountdownDependency(TimeSpan timer) : base(_ => new List<IWatcher>(), true)
    {
        _timer = timer;
    }

    private void OnCountdown()
    {
        _timer -= TimeSpan.FromSeconds(1);
        if (_timer < TimeSpan.Zero)
        {
            _timer = TimeSpan.Zero;
            _handler?.Dispose();
            _handler = null;
        }
    }

    public override bool HasChanged => _timer == TimeSpan.Zero;

    public override Task StartAsync()
    {
        _handler = TimerFactory.CreateNonCapturingTimer(state => ((CountdownDependency)state).OnCountdown(), this, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _handler?.Dispose();
    }
}
