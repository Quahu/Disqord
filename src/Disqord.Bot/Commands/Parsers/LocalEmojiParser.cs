//using System;
//using System.Threading.Tasks;
//using Qmmands;

//namespace Disqord.Bot.Parsers
//{
//    public sealed class LocalEmojiParser : TypeParser<LocalEmoji>
//    {
//        public static LocalEmojiParser Instance => _instance ?? (_instance = new LocalEmojiParser());

//        private static LocalEmojiParser _instance;

//        private LocalEmojiParser()
//        { }

//        public override ValueTask<TypeParserResult<LocalEmoji>> ParseAsync(Parameter parameter, string value, CommandContext context) 
//            => LocalEmoji.TryParse(value, out var emoji)
//                ? TypeParserResult<LocalEmoji>.Successful(emoji)
//                : TypeParserResult<LocalEmoji>.Unsuccessful("Invalid emoji format.");
//    }
//}
