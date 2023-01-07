using System;
using System.Runtime.InteropServices;

namespace Disqord.Voice.Default;

public unsafe partial class MultimediaTimerVoiceSynchronizer
{
    [DllImport("winmm", SetLastError = true)]
    private static extern MMRESULT timeGetDevCaps(out TIMECAPS ptc, uint cbtc);

    [DllImport("winmm", SetLastError = true)]
    private static extern MMRESULT timeBeginPeriod(uint uPeriod);

    [DllImport("winmm", SetLastError = true)]
    private static extern MMRESULT timeEndPeriod(uint uPeriod);

    [DllImport("winmm", SetLastError = true)]
    private static extern MMRESULT timeSetEvent(uint uDelay, uint uResolution,
        delegate* unmanaged[Stdcall, SuppressGCTransition]<uint, uint, IntPtr, UIntPtr, UIntPtr, void> lpTimeProc,
        IntPtr dwUser, FUEVENT fuEvent);

    [DllImport("winmm", SetLastError = true)]
    private static extern MMRESULT timeKillEvent(uint uTimerId);

    private struct TIMECAPS
    {
        public uint wPeriodMin;

        public uint wPeriodMax;
    }

    [Flags]
    private enum FUEVENT : uint
    {
        TIME_PERIODIC = 1,

        TIME_KILL_SYNCHRONOUS = 0x100
    }

    private enum MMRESULT : uint
    {
        MMSYSERR_NOERROR = 0,

        TIMEERR_NOCANDO = 97,
    }
}
