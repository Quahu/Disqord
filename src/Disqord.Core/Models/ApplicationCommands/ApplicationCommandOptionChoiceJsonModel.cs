using System;
using System.Globalization;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class ApplicationCommandOptionChoiceJsonModel : JsonModel
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("value")]
        public IJsonValue Value;

        protected override void OnValidate()
        {
            Guard.IsNotNullOrWhiteSpace(Name);
            Guard.HasSizeBetweenOrEqualTo(Name, Discord.Limits.ApplicationCommands.Options.Choices.MinNameLength, Discord.Limits.ApplicationCommands.Options.Choices.MaxNameLength);

            Guard.IsNotNull(Value);
            Guard.IsAssignableToType<IConvertible>(Value.Value, nameof(Value));

            var value = Value.Value as IConvertible;
            switch (value.GetTypeCode())
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                {
                    var integralValue = value.ToInt64(CultureInfo.InvariantCulture);
                    Guard.IsBetweenOrEqualTo(integralValue, Discord.Limits.ApplicationCommands.Options.Choices.MinIntegralValue, Discord.Limits.ApplicationCommands.Options.Choices.MaxIntegralValue);
                    break;
                }
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                {
                    var floatingPointValue = value.ToDouble(CultureInfo.InvariantCulture);
                    Guard.IsFinite(floatingPointValue);
                    break;
                }
                case TypeCode.String:
                {
                    var stringValue = value.ToString(CultureInfo.InvariantCulture);
                    Guard.HasSizeLessThanOrEqualTo(stringValue, Discord.Limits.ApplicationCommands.Options.Choices.MaxStringValueLength);
                    break;
                }
                default:
                {
                    Throw.ArgumentException("Invalid value provided. Must be an integral, floating-point, or a string value.");
                    return;
                }
            }
        }
    }
}
