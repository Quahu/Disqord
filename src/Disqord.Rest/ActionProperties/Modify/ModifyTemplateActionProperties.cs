using Qommon;

namespace Disqord
{
    public sealed class ModifyTemplateActionProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<string> Description { internal get; set; }
    }
}
