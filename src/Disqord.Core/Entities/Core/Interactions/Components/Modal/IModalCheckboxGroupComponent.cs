using System.Collections.Generic;

namespace Disqord;

public interface IModalCheckboxGroupComponent : IModalComponent, ICustomIdentifiableEntity
{
    IReadOnlyList<string> Values { get; }
}
