using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Models
{
    internal static partial class ModelExtensions
    {
        public static AllowedMentionsModel ToModel(this LocalMentions mentions)
        {
            if (mentions == null)
                return null;

            var model = new AllowedMentionsModel
            {
                Users = mentions.UserIds.Select(x => x.RawValue).ToArray(),
                Roles = mentions.RoleIds.Select(x => x.RawValue).ToArray()
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

            return model;
        }
    }
}
