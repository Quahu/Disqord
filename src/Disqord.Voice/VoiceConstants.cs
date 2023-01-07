using System;

namespace Disqord.Voice;

public static class VoiceConstants
{
    public static ReadOnlyMemory<byte> SilencePacket => new byte[] { 0xF8, 0xFF, 0xFE };

    public const int DurationMilliseconds = 20;

    public const int SamplingRate = 48000;

    public const int RtpHeaderSize = 12;

    public const int AudioSize = 960;
}
