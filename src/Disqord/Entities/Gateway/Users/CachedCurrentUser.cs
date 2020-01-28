using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Disqord.Collections;
using Disqord.Models;

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

                return _relationships.ReadOnly();
            }
        }

        public IReadOnlyDictionary<Snowflake, string> Notes
        {
            get
            {
                if (Client.IsBot)
                    throw new NotSupportedException("Bots cannot set notes.");

                return _notes.ReadOnly();
            }
        }

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

        internal readonly LockedDictionary<Snowflake, CachedRelationship> _relationships;
        internal readonly LockedDictionary<Snowflake, string> _notes;

        internal CachedCurrentUser(CachedSharedUser user, UserModel model, int relationshipCount, int noteCount) : base(user)
        {
            SharedUser = user;

            if (!Client.IsBot)
            {
                _relationships = new LockedDictionary<Snowflake, CachedRelationship>(relationshipCount);
                _notes = new LockedDictionary<Snowflake, string>(noteCount);
            }

            Update(model);
        }

        internal void Update(RelationshipModel[] models)
        {
            for (var i = 0; i < models.Length; i++)
            {
                var relationshipModel = models[i];
                _relationships.AddOrUpdate(relationshipModel.Id, _ => new CachedRelationship(Client, relationshipModel), (_, old) =>
                {
                    old.Update(relationshipModel);
                    return old;
                });
            }

            if (models.Length != _relationships.Count)
            {
                foreach (var key in _relationships.Keys)
                {
                    var found = false;
                    for (var i = 0; i < models.Length; i++)
                    {
                        if (key == models[i].Id)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        _relationships.TryRemove(key, out _);
                }
            }
        }

        internal void Update(Dictionary<ulong, string> notes)
        {
            _notes.Clear();
            foreach (var noteKvp in notes)
                _notes.AddOrUpdate(noteKvp.Key, noteKvp.Value, (_, __) => noteKvp.Value);
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

        public string GetNote(Snowflake userId)
            => Notes.GetValueOrDefault(userId);

        public CachedRelationship GetRelationship(Snowflake userId)
            => Relationships.GetValueOrDefault(userId);
    }
}