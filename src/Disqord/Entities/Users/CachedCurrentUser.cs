using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public sealed class CachedCurrentUser : CachedUser, ICurrentUser
    {
        public override string Name => SharedUser.Name;

        public override string Discriminator => SharedUser.Discriminator;

        public override string AvatarHash => SharedUser.AvatarHash;

        public override bool IsBot => SharedUser.IsBot;

        public string Locale { get; private set; }

        public bool IsVerified { get; private set; }

        public string Email { get; private set; }

        public bool HasMfaEnabled { get; private set; }

        public IReadOnlyDictionary<Snowflake, CachedRelationship> Relationships
        {
            get
            {
                if (Client.IsBot)
                    throw new NotSupportedException("Bots cannot have relationships.");

                return _relationshipsWrapper;
            }
        }

        public IReadOnlyDictionary<Snowflake, string> Notes
        {
            get
            {
                if (Client.IsBot)
                    throw new NotSupportedException("Bots cannot set notes.");

                return _notesWrapper;
            }
        }

        public override UserStatus Status => Client.Status;

        public override Activity Activity => Activity.Create(Client.Activity);

        /// <summary>
        ///     Throws <see cref="InvalidOperationException"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override CachedRelationship Relationship => throw new InvalidOperationException();

        /// <summary>
        ///     Throws <see cref="InvalidOperationException"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override CachedDmChannel DmChannel => throw new InvalidOperationException();

        internal override CachedSharedUser SharedUser { get; }

        private readonly ConcurrentDictionary<Snowflake, CachedRelationship> _relationships;
        private readonly ReadOnlyDictionary<Snowflake, CachedRelationship> _relationshipsWrapper;
        private readonly ConcurrentDictionary<Snowflake, string> _notes;
        private readonly ReadOnlyDictionary<Snowflake, string> _notesWrapper;

        internal CachedCurrentUser(DiscordClient client, UserModel model, CachedSharedUser user, int relationshipCount, int noteCount) : base(client, user.Id)
        {
            SharedUser = user;
            if (client.TokenType != TokenType.Bot)
            {
                _relationships = Extensions.CreateConcurrentDictionary<Snowflake, CachedRelationship>(relationshipCount);
                _relationshipsWrapper = new ReadOnlyDictionary<Snowflake, CachedRelationship>(_relationships);
                _notes = Extensions.CreateConcurrentDictionary<Snowflake, string>(noteCount);
                _notesWrapper = new ReadOnlyDictionary<Snowflake, string>(_notes);
            }

            Update(model);
        }

        internal override void Update(UserModel model)
        {
            if (model.Locale.HasValue)
                Locale = model.Locale.Value;

            if (model.Verified.HasValue)
                IsVerified = model.Verified.Value.GetValueOrDefault();

            if (model.Email.HasValue)
                Email = model.Email.Value;

            if (model.MfaEnabled.HasValue)
                HasMfaEnabled = model.MfaEnabled.Value.GetValueOrDefault();

            SharedUser.Update(model);
        }

        internal bool TryAddRelationship(CachedRelationship relationship)
        {
            var result = _relationships.TryAdd(relationship.Id, relationship);
            if (result)
                relationship.User.SharedUser.References++;

            return result;
        }

        internal bool TryRemoveRelationship(Snowflake id, out CachedRelationship relationship)
        {
            var result = _relationships.TryRemove(id, out relationship);
            if (result)
                relationship.User.SharedUser.References--;

            return result;
        }

        internal void AddOrUpdateNote(Snowflake id, string note, Func<Snowflake, string, string> func)
        {
            _notes.AddOrUpdate(id, note, func);
        }

        public Task ModifyAsync(Action<ModifyCurrentUserProperties> action, RestRequestOptions options = null)
            => Client.ModifyCurrentUserAsync(action, options);
    }
}