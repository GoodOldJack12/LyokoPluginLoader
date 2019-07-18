using LyokoAPI.Events;
using LyokoPluginLoader.Commands.Exceptions;

namespace LyokoPluginLoader.Commands
{
    internal class Enable : Command
    {
        public override string Usage { get; } = "api.plugins.enable.all";
        public override string Name { get; } = "enable";
        protected override bool DoCommand(string[] args)
        {
            CheckArgs(args,1,1);
            if (!(args[0].Equals("all")))
            {
                throw new CommandException(this);
            }
            PluginLoader.Loader.EnableAll();
            CommandOutputEvent.Call("plugins.enable","All plugins enabled.");
            return true;
        }
    }
}