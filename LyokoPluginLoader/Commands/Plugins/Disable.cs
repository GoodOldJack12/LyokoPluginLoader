using LyokoAPI.Events;
using LyokoPluginLoader.Commands.Exceptions;

namespace LyokoPluginLoader.Commands
{
    internal class Disable : Command
    {
        public override string Usage { get; } = "api.plugins.disable.all";
        public override string Name { get; } = "disable";
        protected override bool DoCommand(string[] args)
        {
            CheckArgs(args,1,1);
            if (!(args[0].Equals("all")))
            {
                throw new CommandException(this);
            }
            PluginLoader.Loader.DisableAll();
            CommandOutputEvent.Call("plugins.disable","All plugins disabled.");
            return true;
        }
    }
}