namespace Disqord.Udp.Default;

public class DefaultUdpClientFactory : IUdpClientFactory
{
    public DefaultUdpClientFactory()
    { }

    public virtual IUdpClient CreateClient()
    {
        return new DefaultUdpClient();
    }
}
