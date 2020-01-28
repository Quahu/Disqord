using System;
using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Logging;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestConnection : RestConnectedAccount
    {
        public bool IsRevoked { get; private set; }

        public IReadOnlyList<RestIntegration> Integrations { get; private set; }

        public bool HasFriendSync { get; private set; }

        public bool ShowsActivity { get; private set; }

        public ConnectionVisibility Visibility { get; private set; }

        internal RestConnection(RestDiscordClient client, ConnectionModel model) : base(client, model)
        {
            IsRevoked = model.Revoked;
            try
            {
                Integrations = model.Integrations.ToReadOnlyList(
                    this, (x, @this) => new RestIntegration(@this.Client, x));
            }
            catch (Exception ex)
            {
                Integrations = ReadOnlyList<RestIntegration>.Empty;
                client.Log(LogMessageSeverity.Error, $"Failed to create integrations for connection {this}.", ex);
            }
            HasFriendSync = model.FriendSync;
            ShowsActivity = model.ShowActivity;
            Visibility = model.Visibility;
        }
    }
}
