using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Builders;
using DSharpPlus.CommandsNext.Converters;
using Microsoft.Extensions.Logging;

namespace DSharpPlus.CommandsNext;

/// <summary>
/// Defines various extensions specific to CommandsNext.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Enables CommandsNext module on this <see cref="DiscordClient"/>.
    /// </summary>
    /// <param name="client">Client to enable CommandsNext for.</param>
    /// <param name="cfg">CommandsNext configuration to use.</param>
    /// <returns>Created <see cref="CommandsNextExtension"/>.</returns>
    public static CommandsNextExtension UseCommandsNext(this DiscordClient client, CommandsNextConfiguration cfg)
    {
        if (client.GetExtension<CommandsNextExtension>() != null)
        {
            throw new InvalidOperationException("CommandsNext is already enabled for that client.");
        }

        if (!Utilities.HasMessageIntents(client.Intents))
        {
            client.Logger.LogCritical(CommandsNextEvents.Intents, "The CommandsNext extension is registered but there are no message intents enabled. It is highly recommended to enable them.");
        }

        if (!client.Intents.HasIntent(DiscordIntents.Guilds))
        {
            client.Logger.LogCritical(CommandsNextEvents.Intents, "The CommandsNext extension is registered but the guilds intent is not enabled. It is highly recommended to enable it.");
        }

        CommandsNextExtension cnext = new(cfg);
        client.AddExtension(cnext);
        return cnext;
    }

    /// <summary>
    /// Gets the active CommandsNext module for this client.
    /// </summary>
    /// <param name="client">Client to get CommandsNext module from.</param>
    /// <returns>The module, or null if not activated.</returns>
    public static CommandsNextExtension GetCommandsNext(this DiscordClient client)
        => client.GetExtension<CommandsNextExtension>();

    /// <summary>
    /// Registers all commands from a given assembly. The command classes need to be public to be considered for registration.
    /// </summary>
    /// <param name="extensions">Extensions to register commands on.</param>
    /// <param name="assembly">Assembly to register commands from.</param>
    public static void RegisterCommands(this IReadOnlyDictionary<int, CommandsNextExtension> extensions, Assembly assembly)
    {
        foreach (CommandsNextExtension extension in extensions.Values)
        {
            extension.RegisterCommands(assembly);
        }
    }
    /// <summary>
    /// Registers all commands from a given command class.
    /// </summary>
    /// <typeparam name="T">Class which holds commands to register.</typeparam>
    /// <param name="extensions">Extensions to register commands on.</param>
    public static void RegisterCommands<T>(this IReadOnlyDictionary<int, CommandsNextExtension> extensions) where T : BaseCommandModule
    {
        foreach (CommandsNextExtension extension in extensions.Values)
        {
            extension.RegisterCommands<T>();
        }
    }
    /// <summary>
    /// Registers all commands from a given command class.
    /// </summary>
    /// <param name="extensions">Extensions to register commands on.</param>
    /// <param name="t">Type of the class which holds commands to register.</param>
    public static void RegisterCommands(this IReadOnlyDictionary<int, CommandsNextExtension> extensions, Type t)
    {
        foreach (CommandsNextExtension extension in extensions.Values)
        {
            extension.RegisterCommands(t);
        }
    }
    /// <summary>
    /// Builds and registers all supplied commands.
    /// </summary>
    /// <param name="extensions">Extensions to register commands on.</param>
    /// <param name="cmds">Commands to build and register.</param>
    public static void RegisterCommands(this IReadOnlyDictionary<int, CommandsNextExtension> extensions, params CommandBuilder[] cmds)
    {
        foreach (CommandsNextExtension extension in extensions.Values)
        {
            extension.RegisterCommands(cmds);
        }
    }

    /// <summary>
    /// Unregisters specified commands from CommandsNext.
    /// </summary>
    /// <param name="extensions">Extensions to unregister commands on.</param>
    /// <param name="cmds">Commands to unregister.</param>
    public static void UnregisterCommands(this IReadOnlyDictionary<int, CommandsNextExtension> extensions, params Command[] cmds)
    {
        foreach (CommandsNextExtension extension in extensions.Values)
        {
            extension.UnregisterCommands(cmds);
        }
    }

    /// <summary>
    /// Registers an argument converter for specified type.
    /// </summary>
    /// <typeparam name="T">Type for which to register the converter.</typeparam>
    /// <param name="extensions">Extensions to register the converter on.</param>
    /// <param name="converter">Converter to register.</param>
    public static void RegisterConverter<T>(this IReadOnlyDictionary<int, CommandsNextExtension> extensions, IArgumentConverter<T> converter)
    {
        foreach (CommandsNextExtension extension in extensions.Values)
        {
            extension.RegisterConverter(converter);
        }
    }

    /// <summary>
    /// Unregisters an argument converter for specified type.
    /// </summary>
    /// <typeparam name="T">Type for which to unregister the converter.</typeparam>
    /// <param name="extensions">Extensions to unregister the converter on.</param>
    public static void UnregisterConverter<T>(this IReadOnlyDictionary<int, CommandsNextExtension> extensions)
    {
        foreach (CommandsNextExtension extension in extensions.Values)
        {
            extension.UnregisterConverter<T>();
        }
    }

    /// <summary>
    /// Registers a user-friendly type name.
    /// </summary>
    /// <typeparam name="T">Type to register the name for.</typeparam>
    /// <param name="extensions">Extensions to register the name on.</param>
    /// <param name="value">Name to register.</param>
    public static void RegisterUserFriendlyTypeName<T>(this IReadOnlyDictionary<int, CommandsNextExtension> extensions, string value)
    {
        foreach (CommandsNextExtension extension in extensions.Values)
        {
            extension.RegisterUserFriendlyTypeName<T>(value);
        }
    }

    /// <summary>
    /// Sets the help formatter to use with the default help command.
    /// </summary>
    /// <typeparam name="T">Type of the formatter to use.</typeparam>
    /// <param name="extensions">Extensions to set the help formatter on.</param>
    public static void SetHelpFormatter<T>(this IReadOnlyDictionary<int, CommandsNextExtension> extensions) where T : BaseHelpFormatter
    {
        foreach (CommandsNextExtension extension in extensions.Values)
        {
            extension.SetHelpFormatter<T>();
        }
    }
}
