using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public sealed partial class CachedCurrentUser : CachedUser, ICurrentUser
    {
        public override string Name => SharedUser.Name;

        public override string Discriminator => SharedUser.Discriminator;

        public override string AvatarHash => SharedUser.AvatarHash;

        public override bool IsBot => SharedUser.IsBot;

        public CultureInfo Locale { get; private set; }

        public bool IsVerified { get; private set; }

        public string Email { get; private set; }

        public bool HasMfaEnabled { get; private set; }

        public string Phone { get; private set; }

        public UserFlags Flags { get; private set; }

        public bool HasNitro { get; private set; }

        public NitroType? NitroType { get; private set; }

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

        internal CachedCurrentUser(CachedSharedUser user, UserModel model, int relationshipCount, int noteCount) : base(user)
        {
            SharedUser = user;
            if (Client.TokenType != TokenType.Bot)
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
                Locale = Discord.Internal.CreateLocale(model.Locale.Value);

            if (model.Verified.HasValue)
                IsVerified = model.Verified.Value;

            if (model.Email.HasValue)
                Email = model.Email.Value;

            if (model.MfaEnabled.HasValue)
                HasMfaEnabled = model.MfaEnabled.Value;

            if (model.Phone.HasValue)
                Phone = model.Phone.Value;

            if (model.Flags.HasValue)
                Flags = model.Flags.Value;

            if (model.Premium.HasValue)
                HasNitro = model.Premium.Value;

            if (model.PremiumType.HasValue)
                NitroType = model.PremiumType.Value;

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
    }
}