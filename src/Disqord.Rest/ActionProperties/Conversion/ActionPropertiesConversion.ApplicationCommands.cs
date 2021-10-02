using System;
using System.Linq;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    internal static partial class ActionPropertiesConversion
    {
        public static ModifyApplicationCommandJsonRestRequestContent ToContent(this Action<ModifyApplicationCommandActionProperties> action, IJsonSerializer serializer)
        {
            Guard.IsNotNull(action);

            var properties = new ModifyApplicationCommandActionProperties();
            action(properties);

            var content = new ModifyApplicationCommandJsonRestRequestContent
            {
                Name = properties.Name,
                Description = properties.Description,
                DefaultPermission = properties.IsEnabledByDefault,
                Options = Optional.Convert(properties.Options, options => options?.Select(option => option?.ToModel(serializer)).ToArray())
            };

            return content;
        }
    }
}
