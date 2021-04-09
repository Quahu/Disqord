using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Disqord.WebSocket.Default.Discord
{
    internal sealed partial class DiscordWebSocket
    {
        private static readonly Func<Stream, DeflateStream> _createZLibStream;

        static DiscordWebSocket()
        {
            // Workaround for .NET 5 not having ZLibStream.
            var constructor = typeof(DeflateStream).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).First(x =>
            {
                var parameters = x.GetParameters();
                return parameters[1].ParameterType == typeof(CompressionMode) && parameters[2].ParameterType == typeof(bool);
            });
            var parameter = Expression.Parameter(typeof(Stream));
            var parameters = new Expression[]
            {
                parameter,
                Expression.Constant(CompressionMode.Decompress),
                Expression.Constant(true),
                Expression.Constant(15),
                Expression.Constant((long) -1)
            };
            _createZLibStream = Expression.Lambda<Func<Stream, DeflateStream>>(Expression.New(constructor, parameters), parameter).Compile();
        }
    }
}