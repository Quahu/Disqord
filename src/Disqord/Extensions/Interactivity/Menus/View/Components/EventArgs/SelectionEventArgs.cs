using System.Collections.Generic;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity.Menus;

public sealed class SelectionEventArgs : ViewComponentEventArgs
{
    public SelectionViewComponent Selection { get; }

    public IReadOnlyList<LocalSelectionComponentOption> SelectedOptions { get; }

    public SelectionEventArgs(SelectionViewComponent selection, InteractionReceivedEventArgs e)
        : base(e)
    {
        Selection = selection;
        var selectedOptions = new List<LocalSelectionComponentOption>();
        var selectedValues = Interaction.SelectedValues;
        var selectedValueCount = selectedValues.Count;
        for (var i = 0; i < selectedValueCount; i++)
        {
            var selectedValue = selectedValues[i];
            var options = selection.Options;
            var optionCount = options.Count;
            for (var j = 0; j < optionCount; j++)
            {
                var option = options[j];
                if (selectedValue != option.Value)
                    continue;

                selectedOptions.Add(option);
            }
        }

        SelectedOptions = selectedOptions;
    }
}