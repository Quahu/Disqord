#if DEBUG
using System.ComponentModel;

namespace Disqord
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class Debug
    {
        public static bool DumpJson;
    }
}
#endif
