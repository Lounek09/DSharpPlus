using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;

namespace DSharpPlus.Lavalink;

public static class DiscordClientExtensions
{
    /// <summary>
    /// Creates a new Lavalink client with specified settings.
    /// </summary>
    /// <param name="client">Discord client to create Lavalink instance for.</param>
    /// <returns>Lavalink client instance.</returns>
    [Obsolete("DSharpPlus.Lavalink is deprecated for removal.", true)]
    public static LavalinkExtension UseLavalink(this DiscordClient client)
    {
        if (client.GetExtension<LavalinkExtension>() != null)
        {
            throw new InvalidOperationException("Lavalink is already enabled for that client.");
        }

        if (!client.Intents.HasIntent(DiscordIntents.GuildVoiceStates))
        {
            client.Logger.LogCritical(LavalinkEvents.Intents, "The Lavalink extension is registered but the guild voice states intent is not enabled. It is highly recommended to enable it.");
        }

        LavalinkExtension lava = new();
        client.AddExtension(lava);
        return lava;
    }

    /// <summary>
    /// Gets the active instance of the Lavalink client for the DiscordClient.
    /// </summary>
    /// <param name="client">Discord client to get Lavalink instance for.</param>
    /// <returns>Lavalink client instance.</returns>
    [Obsolete("DSharpPlus.Lavalink is deprecated for removal.", true)]
    public static LavalinkExtension GetLavalink(this DiscordClient client)
        => client.GetExtension<LavalinkExtension>();

    /// <summary>
    /// Connects to this voice channel using Lavalink.
    /// </summary>
    /// <param name="channel">Channel to connect to.</param>
    /// <param name="node">Lavalink node to connect through.</param>
    /// <returns>If successful, the Lavalink client.</returns>
    [Obsolete("DSharpPlus.Lavalink is deprecated for removal.", true)]
    public static Task ConnectAsync(this DiscordChannel channel, LavalinkNodeConnection node)
    {
        if (channel == null)
        {
            throw new NullReferenceException();
        }

        if (channel.Guild == null)
        {
            throw new InvalidOperationException("Lavalink can only be used with guild channels.");
        }

        if (channel.Type is not DiscordChannelType.Voice and not DiscordChannelType.Stage)
        {
            throw new InvalidOperationException("You can only connect to voice and stage channels.");
        }

        if (channel.Discord is not DiscordClient discord || discord == null)
        {
            throw new NullReferenceException();
        }

        LavalinkExtension lava = discord.GetLavalink();
        return lava == null
            ? throw new InvalidOperationException("Lavalink is not initialized for this Discord client.")
            : node.ConnectAsync(channel);
    }
}
