# BasicVoice
This example project is a very simple music bot that uses slash commands to let users play music from local files in basically any audio format.

It showcases the use of the audio player and audio source types and implements a queueing system.

## Requirements
- FFmpeg  
  This example uses FFmpeg to convert input files to Opus in the Ogg format. You can download pre-built FFmpeg binaries [here](https://ffmpeg.org/download.html). You can either put the downloaded binary in the bot's working directory or add `ffmpeg` to PATH.
- Sodium  
  Disqord requires the Sodium library for voice packet encryption. You can download pre-built Sodium binaries [here](https://doc.libsodium.org/installation#pre-built-libraries). On Windows, you can ensure the library gets put into the bot's working directory correctly by using
  ```xml
  <ItemGroup>
      <None Update="libsodium.dll">
          <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      </None>
  </ItemGroup>
  ```
