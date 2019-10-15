using System.IO;

namespace Disqord.Serialization
{
    public interface ILocalAttachment
    {
        Stream Stream { get; }

        string FileName { get; }
    }
}
