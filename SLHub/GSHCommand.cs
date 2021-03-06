using Synapse.Command;

namespace SLHub
{
    //This Command is required for verified Server if you using a Hub and some invisible Server so just to be sure I add this
    [CommandInformation(
        Name = "GSH",
        Aliases = new string[] { },
        Description = "Required Command for Verified Serves",
        Permission = "",
        Platforms = new[] { Platform.ClientConsole, Platform.RemoteAdmin },
        Usage = ".gsh port"
        )]
    public class GSHCommand : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            if (context.Player.ServerRoles.GlobalBadgeType < 2) return new CommandResult
            {
                Message = "This Command is only for Global Staff",
                State = CommandResultState.NoPermission
            };

            if (context.Arguments.Count < 1 || !ushort.TryParse(context.Arguments.At(0), out var port)) return new CommandResult
            {
                Message = "The Usage of this Command in the RemoteAdmin is 'gsh port' and in the Client Console '.gsh port'.",
                State = CommandResultState.Error
            };

            context.Player.SendToServer(port);

            return new CommandResult
            {
                Message = "You are now redirected to the Server",
                State = CommandResultState.Ok
            };
        }
    }
}
