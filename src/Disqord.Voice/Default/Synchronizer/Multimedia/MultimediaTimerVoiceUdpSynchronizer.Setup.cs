using System;
using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace Disqord.Voice.Default;

public partial class MultimediaTimerVoiceSynchronizer
{
    private static readonly object FirstRunLock = new();
    private static bool _isFirstRun = true;

    private static uint _minResolution;
    private static uint _maxResolution;
    private static uint _resolution;

    private static unsafe void Setup(MultimediaTimerVoiceUdpSynchronizerConfiguration configuration, ILogger logger)
    {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("The multimedia synchronizer can only be used on Windows.");

        if (!_isFirstRun)
            return;

        lock (FirstRunLock)
        {
            if (!_isFirstRun)
                return;

            _isFirstRun = false;

            var result = timeGetDevCaps(out var tc, (uint) sizeof(TIMECAPS));
            if (result != MMRESULT.MMSYSERR_NOERROR)
                throw new Win32Exception($"Failed to retrieve timer resolutions ({result}).");

            _minResolution = tc.wPeriodMin;
            _maxResolution = tc.wPeriodMax;
            _resolution = Math.Min(Math.Max(_minResolution, 1), _maxResolution);
            logger.LogDebug("Available timer resolution is {ResolutionMilliseconds}ms.", _resolution);
        }
    }
}
