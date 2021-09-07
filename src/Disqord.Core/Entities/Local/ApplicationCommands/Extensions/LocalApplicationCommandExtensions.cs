using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord
{
    public static class LocalApplicationCommandExtensions
    {
        public static TApplicationCommand WithOptions<TApplicationCommand>(this TApplicationCommand command, IEnumerable<LocalApplicationCommandOption> options)
            where TApplicationCommand : LocalApplicationCommand
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            command._options.Clear();
            command._options.AddRange(options);
            return command;
        }
    }
}
