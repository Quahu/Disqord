using System;
using System.Collections.Generic;
using System.Linq;
using Qommon;

namespace Disqord.Models
{
    public static partial class LocalEntityExtensions
    {
        public static AllowedMentionsJsonModel ToModel(this LocalAllowedMentions mentions)
        {
            if (mentions == null)
                return null;

            var model = new AllowedMentionsJsonModel
            {
                Users = mentions.UserIds.ToArray(),
                Roles = mentions.RoleIds.ToArray()
            };

            if (mentions.ParsedMentions == ParsedMention.None)
            {
                model.Parse = Array.Empty<string>();
            }
            else
            {
                var parse = new List<string>(3);
                if (mentions.ParsedMentions.HasFlag(ParsedMention.Everyone))
                    parse.Add("everyone");

                if (mentions.ParsedMentions.HasFlag(ParsedMention.Users))
                    parse.Add("users");

                if (mentions.ParsedMentions.HasFlag(ParsedMention.Roles))
                    parse.Add("roles");

                model.Parse = parse;
            }

            model.RepliedUser = Optional.FromNullable(mentions.MentionRepliedUser);

            return model;
        }
    }
}
