# BasicVoiceReceive
This example project records voice from users in a voice channel and uploads per-user OGG Opus files when recording stops.

It showcases the audio receive APIs and one way to subscribe to either a single user or an entire channel.

## Commands
- `/record [user] [channel]`  
  Joins the selected voice channel, or your current one if `channel` is omitted, and starts recording either:
  - a specific chosen user, or
  - the entire channel (default, including users who join later).
- `/stop`  
  Stops recording, disconnects, and uploads up to 10 per-user OGG Opus files in the command response.
