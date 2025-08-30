using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
                if (parameter != null)
                {
                    var typeInformation = parameter.GetTypeInformation();
                    if (typeInformation.IsEnumerable)
                    {
                        var useRawArguments = true;
                        if (interaction is IEntitySelectionComponentInteraction entitySelectionInteraction)
                        {
                            var isSnowflake = typeInformation.ActualType == typeof(Snowflake);
                            var isEntity = typeof(ISnowflakeEntity).IsAssignableFrom(typeInformation.ActualType);
                            if (isSnowflake || isEntity)
                            {
                                var selectedValues = entitySelectionInteraction.SelectedValues;
                                var selectedValueCount = selectedValues.Count;
                                object argument;
                                if (isSnowflake)
                                {
                                    var array = new Snowflake[selectedValueCount];
                                    for (var i = 0; i < selectedValueCount; i++)
                                    {
                                        array[i] = selectedValues[i];
                                    }

                                    argument = array;
                                }
                                else
                                {
                                    var array = Unsafe.As<ISnowflakeEntity[]>(Array.CreateInstance(typeInformation.ActualType, selectedValueCount));
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
                                            _ => Throw.InvalidOperationException<ISnowflakeEntity>("Unsupported entity selection type.")
                                        };

                                        if (entity != null)
                                            array[i] = entity;
                                    }

                                    argument = array;
                                }

                                (context.Arguments ??= new Dictionary<IParameter, object?>())[parameter] = argument;
                                useRawArguments = false;
                            }
                        }

                        if (useRawArguments)
                        {
                            (context.RawArguments ??= new Dictionary<IParameter, MultiString>())[parameter] = MultiString.CreateList(out var list);
                            var selectedValues = selectionInteraction.SelectedValues;
                            var selectedValueCount = selectedValues.Count;
                            for (var i = 0; i < selectedValueCount; i++)
                            {
                                list.Add(selectedValues[i].AsMemory());
                            }
                        }
                    }
                }
            }
            else if (interaction is IModalSubmitInteraction modalSubmitInteraction)
            {
                var parameters = command.Parameters;
                var parameterCount = parameters.Count;

                using var modalComponentsEnumerator = ExtractInnerModalComponents(modalSubmitInteraction.Components).GetEnumerator();

                // TODO: implement custom ID matching? For now, just assign positionally
                for (var parameterIndex = parameterOffset; parameterIndex < parameterCount && modalComponentsEnumerator.MoveNext(); parameterIndex++)
                {
                    var parameter = parameters[parameterIndex];
                    var modalComponent = modalComponentsEnumerator.Current;
                    var rawArgument = GetRawArgumentFromModalComponent(modalComponent);
                    if (rawArgument.Count > 1)
                    {
                        var typeInformation = parameter.GetTypeInformation();
                        if (!typeInformation.IsMultiString && !typeInformation.IsEnumerable)
                        {
                            Throw.InvalidOperationException($"Invalid modal multi-string argument for parameter {parameter.Name} ({typeInformation.ActualType}); must not contain multiple strings as the parameter accepts a single value.");
                        }
                    }

                    (context.RawArguments ??= new Dictionary<IParameter, MultiString>())[parameter] = rawArgument;
                }
            }

            return Next.ExecuteAsync(context);
        }

        private IEnumerable<IModalComponent> ExtractInnerModalComponents(IReadOnlyList<IModalComponent> modalComponents)
        {
            var modalComponentCount = modalComponents.Count;
            for (var i = 0; i < modalComponentCount; i++)
            {
                var modalComponent = modalComponents[i];
                foreach (var innerModalComponent in ExtractInnerModalComponents(modalComponent))
                {
                    yield return innerModalComponent;
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
            else
            {
                ThrowNotImplementedException($"{nameof(BindValues)}.{nameof(ExtractInnerModalComponents)}() does not support the modal component of type: {modalComponent.Type} (ID: {modalComponent.Id}).");
            }
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
                    var rawArgument = MultiString.CreateList(out var list);
                    foreach (var value in selectionComponent.Values)
                    {
                        list.Add(value.AsMemory());
                    }

                    return rawArgument;
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
    }
}
