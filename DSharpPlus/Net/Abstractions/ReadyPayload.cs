using System.Collections.Generic;
using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace DSharpPlus.Net.Abstractions;

/// <summary>
/// Represents data for websocket ready event payload.
/// </summary>
internal class ReadyPayload
{
    /// <summary>
    /// Gets the gateway version the client is connectected to.
    /// </summary>
    [JsonProperty("v")]
    public int GatewayVersion { get; private set; }

    /// <summary>
    /// Gets the current user.
    /// </summary>
    [JsonProperty("user")]
    public TransportUser CurrentUser { get; private set; }

    /// <summary>
    /// Gets the private channels available for this shard.
    /// </summary>
    [JsonProperty("private_channels")]
    public IReadOnlyList<DiscordDmChannel> DmChannels { get; private set; }

    /// <summary>
    /// Gets the guilds available for this shard.
    /// </summary>
    [JsonProperty("guilds")]
    public IReadOnlyList<DiscordGuild> Guilds { get; private set; }

    /// <summary>
    /// Gets the current session's ID.
    /// </summary>
    [JsonProperty("session_id")]
    public string SessionId { get; private set; }

    /// <summary>
    /// Gets the url which should be used for resuming the session after disconnect/reconnect
    /// </summary>
    [JsonProperty("resume_gateway_url")]
    public string ResumeGatewayUrl { get; private set; }

    /// <summary>
    /// Gets the current application sent by Discord.
    /// </summary>
    [JsonProperty("application")]
    public DiscordApplication Application { get; private set; }

    /// <summary>
    /// Gets debug data sent by Discord. This contains a list of servers to which the client is connected.
    /// </summary>
    [JsonProperty("_trace")]
    public IReadOnlyList<string> Trace { get; private set; }
}
