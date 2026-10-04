namespace Disqord.Tests.Fakes;

internal sealed class ManualTimeProvider : TimeProvider
{
    /// <summary>
    ///     Gets a task that completes once at least one timer is scheduled.
    /// </summary>
    public Task TimerScheduled
    {
        get
        {
            lock (_lock)
            {
                return _timerScheduled.Task;
            }
        }
    }

    /// <summary>
    ///     Gets the time left until the earliest scheduled timer fires, or <see langword="null"/> if none is scheduled.
    /// </summary>
    public TimeSpan? TimeUntilNextTimer
    {
        get
        {
            lock (_lock)
            {
                return _timers.Count == 0
                    ? null
                    : _timers.Min(static timer => timer.DueAt) - _elapsed;
            }
        }
    }

    private readonly object _lock = new();
    private readonly List<ManualTimer> _timers = [];
    private readonly List<(int ScheduledTimerCount, TaskCompletionSource Source)> _scheduledTimerCountWaiters = [];
    private DateTimeOffset _utcNow = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private TimeSpan _elapsed;
    private int _scheduledTimerCount;
    private TaskCompletionSource _timerScheduled = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public override DateTimeOffset GetUtcNow()
    {
        lock (_lock)
        {
            return _utcNow;
        }
    }

    public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
        var timer = new ManualTimer(this, callback, state);
        timer.Change(dueTime, period);
        return timer;
    }

    /// <summary>
    ///     Returns a task that completes once the total amount of scheduled timers reaches the specified count.
    /// </summary>
    public Task WaitForScheduledTimerCountAsync(int scheduledTimerCount)
    {
        lock (_lock)
        {
            if (_scheduledTimerCount >= scheduledTimerCount)
            {
                return Task.CompletedTask;
            }

            var source = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            _scheduledTimerCountWaiters.Add((scheduledTimerCount, source));
            return source.Task;
        }
    }

    /// <summary>
    ///     Moves the clock forward and fires every timer that became due.
    /// </summary>
    public void Advance(TimeSpan duration)
    {
        var dueTimers = new List<ManualTimer>();
        lock (_lock)
        {
            _utcNow += duration;
            _elapsed += duration;
            foreach (var timer in _timers)
            {
                if (timer.DueAt <= _elapsed)
                {
                    dueTimers.Add(timer);
                }
            }

            foreach (var dueTimer in dueTimers)
            {
                _timers.Remove(dueTimer);
            }

            ResetTimerScheduledIfEmpty();
        }

        foreach (var dueTimer in dueTimers)
        {
            dueTimer.Fire();
        }
    }

    private void Schedule(ManualTimer timer, TimeSpan dueTime)
    {
        lock (_lock)
        {
            _timers.Remove(timer);
            if (dueTime == Timeout.InfiniteTimeSpan)
            {
                ResetTimerScheduledIfEmpty();
                return;
            }

            timer.DueAt = _elapsed + dueTime;
            _timers.Add(timer);
            _timerScheduled.TrySetResult();

            _scheduledTimerCount++;
            for (var index = _scheduledTimerCountWaiters.Count - 1; index >= 0; index--)
            {
                var waiter = _scheduledTimerCountWaiters[index];
                if (_scheduledTimerCount >= waiter.ScheduledTimerCount)
                {
                    waiter.Source.TrySetResult();
                    _scheduledTimerCountWaiters.RemoveAt(index);
                }
            }
        }
    }

    private void Unschedule(ManualTimer timer)
    {
        lock (_lock)
        {
            _timers.Remove(timer);
            ResetTimerScheduledIfEmpty();
        }
    }

    private void ResetTimerScheduledIfEmpty()
    {
        if (_timers.Count == 0 && _timerScheduled.Task.IsCompleted)
        {
            _timerScheduled = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        }
    }

    private sealed class ManualTimer(ManualTimeProvider timeProvider, TimerCallback callback, object? state) : ITimer
    {
        public TimeSpan DueAt { get; set; }

        public bool Change(TimeSpan dueTime, TimeSpan period)
        {
            timeProvider.Schedule(this, dueTime);
            return true;
        }

        public void Fire()
        {
            callback(state);
        }

        public void Dispose()
        {
            timeProvider.Unschedule(this);
        }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
