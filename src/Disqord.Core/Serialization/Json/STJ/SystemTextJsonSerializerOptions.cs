using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disqord.Serialization.Json.STJ
{
    public class SystemTextJsonSerializerOptions : IJsonSerializerOptions
    {
        public JsonFormatting Formatting { get; set; }
    }
}
