using System;
using System.Collections.Generic;
using System.Linq;
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
            var parameterOffset = context.RawArguments?.Count ?? 0;
            if (interaction is IComponentInteraction componentInteraction && componentInteraction.ComponentType == ComponentType.Selection)
            {
                var parameter = command.Parameters.ElementAtOrDefault(parameterOffset);
                if (parameter != null)
                {
                    var typeInformation = parameter.GetTypeInformation();
                    if (typeInformation.IsEnumerable && typeInformation.IsStringLike)
                    {
                        (context.RawArguments ??= new Dictionary<IParameter, MultiString>())[parameter] = MultiString.CreateList(out var list);
                        var selectedValues = componentInteraction.SelectedValues;
                        var selectedValueCount = selectedValues.Count;
                        for (var i = 0; i < selectedValueCount; i++)
                        {
                            list.Add(selectedValues[i].AsMemory());
                        }
                    }
                }
            }
            else if (interaction is IModalSubmitInteraction modalSubmitInteraction)
            {
                var modalComponents = modalSubmitInteraction.Components;
                var modalComponentCount = modalComponents.Count;
                var parameters = command.Parameters;
                var parameterCount = parameters.Count;
                for (var i = 0; i < modalComponentCount; i++)
                {
                    if (modalComponents[i] is not IRowComponent rowComponent)
                        continue;

                    var components = rowComponent.Components;
                    var componentCount = components.Count;
                    for (var j = 0; j < componentCount; j++)
                    {
                        var component = components[j];

                        // TODO: support selection components after they get documented
                        if (component is not ITextInputComponent textInputComponent)
                            continue;

                        for (var k = parameterOffset; k < parameterCount; k++)
                        {
                            var parameter = parameters[k];
                            if (parameter.Name != textInputComponent.CustomId)
                                continue;

                            if (!string.IsNullOrEmpty(textInputComponent.Value))
                                (context.RawArguments ??= new Dictionary<IParameter, MultiString>())[parameter] = textInputComponent.Value;

                            break;
                        }
                    }
                }
            }

            return Next.ExecuteAsync(context);
        }
    }
}
