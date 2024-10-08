using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DSharpPlus.Commands.Converters;

public partial class DiscordRoleConverter : ISlashArgumentConverter<DiscordRole>, ITextArgumentConverter<DiscordRole>
{
    [GeneratedRegex(@"^<@&(\d+?)>$", RegexOptions.Compiled | RegexOptions.ECMAScript)]
    private static partial Regex getRoleRegex();

    public DiscordApplicationCommandOptionType ParameterType => DiscordApplicationCommandOptionType.Role;
    public string ReadableName => "Discord Role";
    public bool RequiresText => true;

    public Task<Optional<DiscordRole>> ConvertAsync(TextConverterContext context, MessageCreatedEventArgs eventArgs)
    {
        if (context.Guild is null)
        {
            return Task.FromResult(Optional.FromNoValue<DiscordRole>());
        }

        if (!ulong.TryParse(context.Argument, CultureInfo.InvariantCulture, out ulong roleId))
        {
            // value can be a raw channel id or a channel mention. The regex will match both.
            Match match = getRoleRegex().Match(context.Argument);
            if (!match.Success || !ulong.TryParse(match.Groups[1].ValueSpan, NumberStyles.Number, CultureInfo.InvariantCulture, out roleId))
            {
                // Attempt to find a role by name, case sensitive.
                DiscordRole? namedRole = context.Guild.Roles.Values.FirstOrDefault(role => role.Name.Equals(context.Argument, StringComparison.Ordinal));
                return Task.FromResult(namedRole is not null ? Optional.FromValue(namedRole) : Optional.FromNoValue<DiscordRole>());
            }
        }

        return context.Guild.Roles.GetValueOrDefault(roleId) is DiscordRole guildRole
            ? Task.FromResult(Optional.FromValue(guildRole))
            : Task.FromResult(Optional.FromNoValue<DiscordRole>());
    }

    public Task<Optional<DiscordRole>> ConvertAsync(InteractionConverterContext context, InteractionCreatedEventArgs eventArgs) => context.Interaction.Data.Resolved is null
        || !ulong.TryParse(context.Argument.RawValue, CultureInfo.InvariantCulture, out ulong roleId)
        || !context.Interaction.Data.Resolved.Roles.TryGetValue(roleId, out DiscordRole? role)
            ? Task.FromResult(Optional.FromNoValue<DiscordRole>())
            : Task.FromResult(Optional.FromValue(role));
}
