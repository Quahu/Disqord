using Disqord.Models;

namespace Disqord
{
    public class EmbedField
    {
        public string Name { get; }

        public string Value { get; }

        public bool IsInline { get; }

        public EmbedField(EmbedFieldJsonModel model)
        {
            Name = model.Name;
            Value = model.Value;
            IsInline = model.Inline.GetValueOrDefault();
        }
    }
}
