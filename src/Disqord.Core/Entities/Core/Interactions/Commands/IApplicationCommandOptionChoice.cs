using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disqord
{
    public interface IApplicationCommandOptionChoice : INamable
    {
        object Value { get; }
    }
}
