﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestUser : RestSnowflakeEntity, IUser
    {
        public Task AddFriendAsync(RestRequestOptions options = null)
            => Client.CreateRelationshipAsync(Id, options: options);

        public Task BlockAsync(RestRequestOptions options = null)
            => Client.CreateRelationshipAsync(Id, RelationshipType.Blocked, options);

        public Task UnfriendOrUnblockAsync(RestRequestOptions options = null)
            => Client.DeleteRelationshipAsync(Id, options);

        public Task<RestUserProfile> GetProfileAsync(RestRequestOptions options = null)
            => Client.GetUserProfileAsync(Id, options);

        public Task SetNoteAsync(string note, RestRequestOptions options = null)
            => Client.SetNoteAsync(Id, note, options);

        public Task<RestDmChannel> CreateDmChannelAsync(RestRequestOptions options = null)
            => Client.CreateDmChannelAsync(Id, options);

        public async Task<RestUserMessage> SendMessageAsync(string content = null, bool textToSpeech = false, Embed embed = null, RestRequestOptions options = null)
        {
            var channel = await CreateDmChannelAsync(options).ConfigureAwait(false);
            return await channel.SendMessageAsync(content, textToSpeech, embed, options).ConfigureAwait(false);
        }

        public async Task<RestUserMessage> SendMessageAsync(LocalAttachment attachment, string content = null, bool textToSpeech = false, Embed embed = null, RestRequestOptions options = null)
        {
            var channel = await CreateDmChannelAsync(options).ConfigureAwait(false);
            return await channel.SendMessageAsync(attachment, content, textToSpeech, embed, options).ConfigureAwait(false);
        }

        public async Task<RestUserMessage> SendMessageAsync(IEnumerable<LocalAttachment> attachments, string content = null, bool textToSpeech = false, Embed embed = null, RestRequestOptions options = null)
        {
            var channel = await CreateDmChannelAsync(options).ConfigureAwait(false);
            return await channel.SendMessageAsync(attachments, content, textToSpeech, embed, options).ConfigureAwait(false);
        }
    }
}
