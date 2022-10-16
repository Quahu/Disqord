using System.Collections.Generic;
using Disqord.Gateway;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord.Extensions.Interactivity.Menus;

public sealed class SelectionEventArgs : ViewComponentEventArgs
{
    public override ISelectionComponentInteraction Interaction => (base.Interaction as ISelectionComponentInteraction)!;

    public SelectionViewComponent Selection { get; }

    public IReadOnlyList<LocalSelectionComponentOption> SelectedOptions
    {
        get
        {
            if (_selectedOptions != null)
                return _selectedOptions;

            var selectedOptions = new List<LocalSelectionComponentOption>();
            var selectedValues = Interaction.SelectedValues;
            var selectedValueCount = selectedValues.Count;
            for (var i = 0; i < selectedValueCount; i++)
            {
                var selectedValue = selectedValues[i];
                var options = Selection.Options;
                var optionCount = options.Count;
                for (var j = 0; j < optionCount; j++)
                {
                    var option = options[j];
                    if (selectedValue != option.Value)
                        continue;

                    selectedOptions.Add(option);
                }
            }

            return _selectedOptions = selectedOptions;
        }
    }
    private IReadOnlyList<LocalSelectionComponentOption>? _selectedOptions;

    public IReadOnlyList<ISnowflakeEntity> SelectedEntities
    {
        get
        {
            if (_selectedEntities != null)
                return _selectedEntities;

            if (Interaction is not IEntitySelectionComponentInteraction entitySelectionInteraction)
                return ReadOnlyList<ISnowflakeEntity>.Empty;

            var selectedValues = entitySelectionInteraction.SelectedValues;
            var selectedValueCount = selectedValues.Count;
            var selectedEntities = entitySelectionInteraction.ComponentType switch
            {
                SelectionComponentType.User => new IUser[selectedValueCount],
                SelectionComponentType.Role => new IRole[selectedValueCount],
                SelectionComponentType.Mentionable => new ISnowflakeEntity[selectedValueCount],
                SelectionComponentType.Channel => new IInteractionChannel[selectedValueCount]
            };

            for (var i = 0; i < selectedValueCount; i++)
            {
                var entityId = selectedValues[i];
                var entities = entitySelectionInteraction.Entities;
                var entity = entitySelectionInteraction.ComponentType switch
                {
                    SelectionComponentType.User => entities.Users[entityId],
                    SelectionComponentType.Role => entities.Roles[entityId],
                    SelectionComponentType.Mentionable => entities.Users.GetValueOrDefault(entityId)
                        ?? entities.Roles.GetValueOrDefault(entityId)
                        ?? entities.Channels.GetValueOrDefault(entityId) as ISnowflakeEntity,
                    SelectionComponentType.Channel => entities.Channels[entityId],
                    _ => Throw.ArgumentOutOfRangeException<ISnowflakeEntity>(nameof(entitySelectionInteraction.ComponentType), entitySelectionInteraction.ComponentType, "Unsupported entity selection type.")
                };

                if (entity != null)
                    selectedEntities[i] = entity;
            }

            return _selectedEntities = selectedEntities;
        }
    }
    private IReadOnlyList<ISnowflakeEntity>? _selectedEntities;

    public SelectionEventArgs(SelectionViewComponent selection, InteractionReceivedEventArgs e)
        : base(e)
    {
        Selection = selection;
    }
}
