using System.Collections.Generic;
using System.Linq;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity.Menus
{
    public sealed class SelectionEventArgs : ViewComponentEventArgs
    {
        public SelectionViewComponent Selection { get; }

        public IReadOnlyList<LocalSelectionComponentOption> SelectedOptions { get; }

        public SelectionEventArgs(SelectionViewComponent selection, InteractionReceivedEventArgs e)
            : base(e)
        {
            Selection = selection;
            SelectedOptions = Interaction.SelectedValues.Select(x => Selection.Options.FirstOrDefault(y => x == y.Value))
                .Where(x => x != null) // just in case Discord messes up
                .ToArray();
        }
    }
}
