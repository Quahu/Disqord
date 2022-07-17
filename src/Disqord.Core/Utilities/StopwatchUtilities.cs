using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Disqord;

/// <summary>
///     Defines <see cref="Stopwatch"/> utilities.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class StopwatchUtilities
{
    private const long TicksPerMillisecond = 10000;
    private const long TicksPerSecond = TicksPerMillisecond * 1000;
    private static readonly double TickFrequency = (double) TicksPerSecond / Stopwatch.Frequency;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long GetTimestamp()
    {
        return Stopwatch.GetTimestamp();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan GetElapsedTime(long startTimestamp, long endTimestamp)
    {
        return TimeSpan.FromTicks((long) ((endTimestamp - startTimestamp) * TickFrequency));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan GetElapsedTime(long startTimestamp)
    {
        return GetElapsedTime(startTimestamp, Stopwatch.GetTimestamp());
    }
}
