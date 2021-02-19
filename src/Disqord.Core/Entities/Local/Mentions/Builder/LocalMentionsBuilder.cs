﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    public sealed class LocalMentionsBuilder : ICloneable
    {
        public const int MAX_MENTION_AMOUNT = 100;

        public ParsedMention ParsedMentions { get; set; }

        public IList<Snowflake> UserIds
        {
            get => _userIds;
            set => WithUserIds(value);
        }

        public IList<Snowflake> RoleIds
        {
            get => _roleIds;
            set => WithRoleIds(value);
        }

        private readonly List<Snowflake> _userIds;
        private readonly List<Snowflake> _roleIds;

        public LocalMentionsBuilder()
        {
            _userIds = new List<Snowflake>();
            _roleIds = new List<Snowflake>();
        }

        internal LocalMentionsBuilder(LocalMentionsBuilder builder)
        {
            ParsedMentions = builder.ParsedMentions;
            _userIds = builder._userIds.ToList();
            _roleIds = builder._roleIds.ToList();
        }

        public LocalMentionsBuilder WithParsedMentions(ParsedMention parsedMentions)
        {
            ParsedMentions = parsedMentions;
            return this;
        }

        public LocalMentionsBuilder WithUserIds(params Snowflake[] userIds)
            => WithUserIds(userIds as IEnumerable<Snowflake>);

        public LocalMentionsBuilder WithUserIds(IEnumerable<Snowflake> userIds)
        {
            if (userIds == null)
                throw new ArgumentNullException(nameof(userIds));

            _userIds.Clear();
            _userIds.AddRange(userIds);
            return this;
        }

        public LocalMentionsBuilder WithRoleIds(params Snowflake[] roleIds)
            => WithRoleIds(roleIds as IEnumerable<Snowflake>);

        public LocalMentionsBuilder WithRoleIds(IEnumerable<Snowflake> roleIds)
        {
            if (roleIds == null)
                throw new ArgumentNullException(nameof(roleIds));

            _roleIds.Clear();
            _roleIds.AddRange(roleIds);
            return this;
        }

        /// <summary>
        ///     Creates a deep copy of this <see cref="LocalMentionsBuilder"/>.
        /// </summary>
        /// <returns>
        ///     A deep copy of this <see cref="LocalMentionsBuilder"/>.
        /// </returns>
        public LocalMentionsBuilder Clone()
            => new LocalMentionsBuilder(this);

        object ICloneable.Clone()
            => Clone();

        public LocalMentions Build()
        {
            if (ParsedMentions != ParsedMention.None && (ParsedMentions.HasFlag(ParsedMention.Users) && UserIds.Count != 0
                    || ParsedMentions.HasFlag(ParsedMention.Roles) && RoleIds.Count != 0))
            {
                throw new InvalidOperationException(
                    "Parsed mentions and IDs are mutually exclusive, meaning you must not set both the parsed mentions for users/roles and user/role IDs.");
            }

            if (UserIds.Count > MAX_MENTION_AMOUNT || RoleIds.Count > MAX_MENTION_AMOUNT)
                throw new InvalidOperationException($"The amount of mentions must not exceed {MAX_MENTION_AMOUNT} mentions.");

            return new LocalMentions(this);
        }
    }
}