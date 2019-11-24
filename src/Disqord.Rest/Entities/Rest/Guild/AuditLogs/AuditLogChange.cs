using System;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.AuditLogs
{
    public readonly struct AuditLogChange<T>
    {
        public Optional<T> OldValue { get; }

        public Optional<T> NewValue { get; }

        public bool WasChanged => OldValue.HasValue || NewValue.HasValue;

        internal AuditLogChange(Optional<T> oldValue, Optional<T> newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        internal static AuditLogChange<T> SingleConvert(AuditLogChangeModel model, IJsonSerializer serializer)
        {
            var oldValue = model.OldValue.HasValue
                ? serializer.ToObject<T>(model.OldValue.Value)
                : Optional<T>.Empty;

            var newValue = model.NewValue.HasValue
                ? serializer.ToObject<T>(model.NewValue.Value)
                : Optional<T>.Empty;

            return new AuditLogChange<T>(oldValue, newValue);
        }

        internal static AuditLogChange<T> DoubleConvert<TMiddle>(AuditLogChangeModel model, IJsonSerializer serializer, Converter<TMiddle, T> converter)
        {
            var oldValue = model.OldValue.HasValue
                ? converter(serializer.ToObject<TMiddle>(model.OldValue.Value))
                : Optional<T>.Empty;

            var newValue = model.NewValue.HasValue
                ? converter(serializer.ToObject<TMiddle>(model.NewValue.Value))
                : Optional<T>.Empty;

            return new AuditLogChange<T>(oldValue, newValue);
        }

        public override string ToString()
            => $"{OldValue} | {NewValue}";
    }
}
