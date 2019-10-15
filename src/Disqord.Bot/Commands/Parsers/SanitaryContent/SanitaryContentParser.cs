//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Qmmands;

//namespace Disqord.Bot.Parsers
//{
//    public sealed class SanitaryContentParser : TypeParser<string>
//    {
//        public static SanitaryContentParser Instance => _instance ?? (_instance = new SanitaryContentParser());

//        private static SanitaryContentParser _instance;

//        private SanitaryContentParser()
//        { }

//        public override ValueTask<TypeParserResult<string>> ParseAsync(Parameter parameter, string value, CommandContext _)
//        {
//            var context = _ as DiscordCommandContext;
//            var options = parameter.Attributes.FirstOrDefault(x => x is SanitaryContentAttribute) as SanitaryContentAttribute ?? new SanitaryContentAttribute();
//            var transformations = new Dictionary<string, string>();

//            //if (context.Guild != null)
//            //{
//            //    if (options.ConvertsChannelMentions)
//            //    {
//            //        string ResolveChannel(Snowflake id)
//            //        {
//            //            var channel = 
//            //        }
//            //    }
//            //}

//            return TypeParserResult<string>.Successful(value);
//        }
//    }
//}
