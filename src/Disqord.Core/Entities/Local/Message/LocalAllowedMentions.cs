using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord
{
    /// <summary>
    ///     Represents the allowed mentions in a Discord message.
    /// </summary>
    public class LocalAllowedMentions : ILocalConstruct
    {
        public const int MaxMentionAmount = 100;

        /// <summary>
        ///     Get an instance in which all mentions are ignored.
        /// </summary>
        public static LocalAllowedMentions None => new LocalAllowedMentions()
            .WithParsedMentions(ParsedMention.None);

        /// <summary>
        ///     Gets an instance in which all <c>@everyone</c> mentions are ignored.
        /// </summary>
        public static LocalAllowedMentions ExceptEveryone => new LocalAllowedMentions()
            .WithParsedMentions(ParsedMention.Users | ParsedMention.Roles);

        /// <summary>
        ///     Gets an instance in which the author of the replied to message is not mentioned.
        /// </summary>
        public static LocalAllowedMentions ExceptRepliedUser => new LocalAllowedMentions()
            .WithParsedMentions(ParsedMention.All)
            .WithMentionRepliedUser(false);

        /// <summary>
        ///     Gets or sets the mention types Discord will parse from the message's content.
        /// </summary>
        public ParsedMention ParsedMentions { get; set; }

        /// <summary>
        ///     Gets or sets the IDs of the users that can be mentioned.
        /// </summary>
        public IList<Snowflake> UserIds
        {
            get => _userIds;
            set => WithUserIds(value);
        }
        private readonly List<Snowflake> _userIds;

        /// <summary>
        ///     Gets or sets the IDs of the roles that can be mentioned.
        /// </summary>
        public IList<Snowflake> RoleIds
        {
            get => _roleIds;
            set => WithRoleIds(value);
        }
        private readonly List<Snowflake> _roleIds;

        /// <summary>
        ///     Gets or sets whether the author of the replied to message is going to be mentioned.
        /// </summary>
        public bool? MentionRepliedUser { get; set; }

        public LocalAllowedMentions()
        {
            _userIds = new List<Snowflake>();
            _roleIds = new List<Snowflake>();
        }

        private LocalAllowedMentions(LocalAllowedMentions other)
        {
            ParsedMentions = other.ParsedMentions;
            _userIds = other._userIds.ToList();
            _roleIds = other._roleIds.ToList();
            MentionRepliedUser = other.MentionRepliedUser;
        }

        public LocalAllowedMentions WithParsedMentions(ParsedMention parsedMentions)
        {
            ParsedMentions = parsedMentions;
            return this;
        }

        public LocalAllowedMentions WithUserIds(params Snowflake[] userIds)
            => WithUserIds(userIds as IEnumerable<Snowflake>);

        public LocalAllowedMentions WithUserIds(IEnumerable<Snowflake> userIds)
        {
            if (userIds == null)
                throw new ArgumentNullException(nameof(userIds));

            _userIds.Clear();
            _userIds.AddRange(userIds);
            return this;
        }

        public LocalAllowedMentions WithRoleIds(params Snowflake[] roleIds)
            => WithRoleIds(roleIds as IEnumerable<Snowflake>);

        public LocalAllowedMentions WithRoleIds(IEnumerable<Snowflake> roleIds)
        {
            if (roleIds == null)
                throw new ArgumentNullException(nameof(roleIds));

            _roleIds.Clear();
            _roleIds.AddRange(roleIds);
            return this;
        }

        public LocalAllowedMentions WithMentionRepliedUser(bool? mentionRepliedUser = true)
        {
            MentionRepliedUser = mentionRepliedUser;
            return this;
        }

        public LocalAllowedMentions Clone()
            => new(this);

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (ParsedMentions != ParsedMention.None && (ParsedMentions.HasFlag(ParsedMention.Users) && UserIds.Count != 0
                || ParsedMentions.HasFlag(ParsedMention.Roles) && RoleIds.Count != 0))
                throw new InvalidOperationException("Parsed mentions and IDs are mutually exclusive, meaning you must not set both the parsed mentions for users/roles and user/role IDs.");

            if (UserIds.Count > MaxMentionAmount || RoleIds.Count > MaxMentionAmount)
                throw new InvalidOperationException($"The amount of mentions must not exceed {MaxMentionAmount} mentions.");
        }
    }
}
