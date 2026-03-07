# BasicVoiceReceive
This example project records voice from users in a voice channel and uploads per-user OGG Opus files when recording stops.

It showcases the audio receive APIs and one way to subscribe to either a single user or an entire channel.

## Requirements
- Sodium  
  Disqord requires the Sodium library for voice packet encryption. You can download pre-built Sodium binaries [here](https://doc.libsodium.org/installation#pre-built-libraries). On Windows, you can ensure the library gets put into the bot's working directory correctly by using
  ```xml
  <ItemGroup>
      <None Update="libsodium.dll">
          <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      </None>
  </ItemGroup>
  ```

## Commands
- `/record [user] [channel]`  
  Joins the selected voice channel, or your current one if `channel` is omitted, and starts recording either:
  - a specific chosen user, or
  - the entire channel (default, including users who join later).
- `/stop`  
  Stops recording, disconnects, and uploads up to 10 per-user OGG Opus files in the command response.
