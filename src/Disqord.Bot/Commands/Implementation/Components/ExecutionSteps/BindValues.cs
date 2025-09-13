using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Disqord.Bot.Commands.Interaction;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Components;

public static partial class DefaultComponentExecutionSteps
{
    public class BindValues : CommandExecutionStep
    {
        protected override ValueTask<IResult> OnExecuted(ICommandContext context)
        {
            var componentContext = Guard.IsAssignableToType<IDiscordComponentCommandContext>(context);
            Guard.IsNotNull(context.Command);

            var command = context.Command;
            var interaction = componentContext.Interaction;
            var parameterOffset = context.RawArguments?.Count ?? 0; // TODO: RawArguments is a dictionary, so this is wrong.
            if (interaction is ISelectionComponentInteraction selectionInteraction)
            {
                var parameter = command.Parameters.ElementAtOrDefault(parameterOffset);
                if (parameter != null && !PopulateSelectionComponentEntityArguments(componentContext, parameter, selectionInteraction.SelectedValues, selectionInteraction.ComponentType))
                {
                    context.SetRawArgument(parameter, ToMultiString(selectionInteraction.SelectedValues));
                }
            }
            else if (interaction is IModalSubmitInteraction modalSubmitInteraction)
            {
                var parameters = command.Parameters;
                var parameterCount = parameters.Count;

                using var modalComponentsEnumerator = GetInteractableComponents(modalSubmitInteraction.Components).GetEnumerator();

                // TODO: implement custom ID matching. For now, it's just assigning positionally
                for (var parameterIndex = parameterOffset; parameterIndex < parameterCount && modalComponentsEnumerator.MoveNext(); parameterIndex++)
                {
                    var parameter = parameters[parameterIndex];
                    var modalComponent = modalComponentsEnumerator.Current;

                    BindArgumentFromModalComponent(componentContext, parameter, modalComponent);
                }
            }

            return Next.ExecuteAsync(context);
        }

        protected virtual IEnumerable<IModalComponent> GetInteractableComponents(IEnumerable<IModalComponent> modalComponents)
        {
            foreach (var modalComponent in modalComponents)
            {
                if (modalComponent is ICustomIdentifiableEntity)
                {
                    yield return modalComponent;

                    continue;
                }

                var innerModalComponents = ExtractInnerModalComponents(modalComponent);
                foreach (var innerInteractableComponent in GetInteractableComponents(innerModalComponents))
                {
                    yield return innerInteractableComponent;
                }
            }
        }

        protected virtual IEnumerable<IModalComponent> ExtractInnerModalComponents(IModalComponent modalComponent)
        {
            if (modalComponent is IModalRowComponent rowComponent)
            {
                foreach (var innerComponent in rowComponent.Components)
                {
                    yield return innerComponent;
                }
            }
            else if (modalComponent is IModalLabelComponent labelComponent)
            {
                yield return labelComponent.Component;
            }
        }

        protected virtual void BindArgumentFromModalComponent(IDiscordComponentCommandContext context, IParameter parameter, IModalComponent modalComponent)
        {
            if (modalComponent is IModalSelectionComponent selectionComponent && PopulateSelectionComponentEntityArguments(context, parameter, selectionComponent.Values, selectionComponent.Type))
            {
                return;
            }

            var rawArgument = GetRawArgumentFromModalComponent(modalComponent);
            if (rawArgument.Count > 1)
            {
                var typeInformation = parameter.GetTypeInformation();
                if (!typeInformation.IsMultiString && !typeInformation.IsEnumerable)
                {
                    Throw.InvalidOperationException($"Invalid modal multi-string argument for parameter {parameter.Name} ({typeInformation.ActualType}); must not contain multiple strings as the parameter accepts a single value.");
                }
            }

            context.SetRawArgument(parameter, rawArgument);
        }

        // TODO: this is pretty ugly
        protected static bool PopulateSelectionComponentEntityArguments(IDiscordInteractionCommandContext context, IParameter parameter, IReadOnlyList<string> selectedValues, SelectionComponentType selectionComponentType)
        {
            if (selectionComponentType is not (>= SelectionComponentType.User and <= SelectionComponentType.Channel))
            {
                return false;
            }

            var typeInformation = parameter.GetTypeInformation();
            if (!typeInformation.IsEnumerable)
            {
                return false;
            }

            var selectedValueCount = selectedValues.Count;
            if (!typeof(ISnowflakeEntity).IsAssignableFrom(typeInformation.ActualType) || context.Interaction is not IEntityInteraction entityInteraction)
            {
                return false;
            }

            var entityArray = Unsafe.As<ISnowflakeEntity[]>(Array.CreateInstance(typeInformation.ActualType, selectedValueCount));
            var entities = entityInteraction.Entities;
            for (var i = 0; i < selectedValueCount; i++)
            {
                var entityId = Snowflake.Parse(selectedValues[i]);
                var entity = selectionComponentType switch
                {
                    SelectionComponentType.User => entities.Users[entityId],
                    SelectionComponentType.Role => entities.Roles[entityId],
                    SelectionComponentType.Mentionable => entities.Users.GetValueOrDefault(entityId)
                        ?? entities.Roles.GetValueOrDefault(entityId)
                        ?? entities.Channels[entityId] as ISnowflakeEntity,
                    SelectionComponentType.Channel => entities.Channels[entityId],
                    _ => Throw.InvalidOperationException<ISnowflakeEntity>("Unsupported entity selection type.")
                };

                entityArray[i] = entity;
            }

            context.SetArgument(parameter, entityArray);

            return true;
        }

        protected virtual MultiString GetRawArgumentFromModalComponent(IModalComponent modalComponent)
        {
            switch (modalComponent)
            {
                case IModalTextInputComponent textInputComponent:
                {
                    return new MultiString(textInputComponent.Value);
                }
                case IModalSelectionComponent selectionComponent:
                {
                    return ToMultiString(selectionComponent.Values);
                }
                default:
                {
                    ThrowNotImplementedException($"{nameof(BindValues)}.{nameof(GetRawArgumentFromModalComponent)}() does not support the modal component of type: {modalComponent.Type} (ID: {modalComponent.Id}).");
                    return default;
                }
            }
        }

        [DoesNotReturn]
        private static void ThrowNotImplementedException(string message)
        {
            throw new NotImplementedException(message);
        }

        private static MultiString ToMultiString(IEnumerable<string> values)
        {
            var multiString = MultiString.CreateList(out var list);
            foreach (var value in values)
            {
                list.Add(value.AsMemory());
            }

            return multiString;
        }
    }
}
