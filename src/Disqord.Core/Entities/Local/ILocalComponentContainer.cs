using System.Collections.Generic;
using Qommon;

namespace Disqord;

public interface ILocalComponentContainer
{
    Optional<IList<LocalComponent>> Components { get; set; }
}
