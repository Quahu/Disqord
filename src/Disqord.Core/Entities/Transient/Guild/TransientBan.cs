using Disqord.Models;

namespace Disqord;

public class TransientBan : TransientClientEntity<BanJsonModel>, IBan
{
    public Snowflake GuildId { get; }

    public IUser User => _user ??= new TransientUser(Client, Model.User);

    private TransientUser? _user;

    public string? Reason => Model.Reason;

    public TransientBan(IClient client, Snowflake guildId, BanJsonModel model)
        : base(client, model)
    {
        GuildId = guildId;
    }

    public override string ToString()
    {
        return this.GetString();
    }
}
