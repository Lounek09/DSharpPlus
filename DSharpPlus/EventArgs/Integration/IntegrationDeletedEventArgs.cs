using DSharpPlus.Entities;

namespace DSharpPlus.EventArgs;

/// <summary>
/// Represents arguments for IntegrationDeleted
/// </summary>
public sealed class IntegrationDeletedEventArgs : DiscordEventArgs
{
    /// <summary>
    /// Gets the id of the integration.
    /// </summary>
    public ulong IntegrationId { get; internal set; }

    /// <summary>
    /// Gets the guild the integration was removed from.
    /// </summary>
    public DiscordGuild Guild { get; internal set; }

    /// <summary>
    /// Gets the id of the bot or OAuth2 application for the integration.
    /// </summary>
    public ulong? Applicationid { get; internal set; }
}
