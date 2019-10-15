//using System;
//using Disqord.Bot.Parsers;
//using Qmmands;

//namespace Disqord.Bot
//{
//    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
//    public sealed class SanitaryContentAttribute : OverrideTypeParserAttribute
//    {
//        public bool ConvertsChannelMentions { get; set; } = true;

//        public bool UsesNicks { get; set; } = true;

//        public bool EscapesMarkdown { get; set; } = true;

//        public SanitaryContentAttribute() : base(typeof(SanitaryContentParser))
//        { }
//    }
//}
