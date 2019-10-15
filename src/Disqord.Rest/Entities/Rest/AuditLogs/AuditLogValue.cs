using Disqord.Models;

namespace Disqord.Rest.AuditLogs
{
    public sealed class AuditLogValue<T>
    {
        public Optional<T> OldValue { get; }

        public Optional<T> NewValue { get; }

        public bool WasModified => OldValue.HasValue || NewValue.HasValue;

        internal AuditLogValue(Optional<T> oldValue, Optional<T> newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        internal AuditLogValue(AuditLogChangeModel model)
        {
            OldValue = model.OldValue.HasValue
                ? (T) model.OldValue.Value
                : Optional<T>.Empty;

            NewValue = model.NewValue.HasValue
                ? (T) model.NewValue.Value
                : Optional<T>.Empty;
        }

        public override string ToString()
            => $"{OldValue} | {NewValue}";
    }
}
