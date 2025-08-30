using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a Discord modal component.
///     I.e., a component received when a modal is submitted.
/// </summary>
public interface IModalComponent : IBaseComponent, IJsonUpdatable<ModalBaseComponentJsonModel>;
