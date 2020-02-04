using LyokoAPI.Events;
using LyokoAPI.Exceptions;
using LyokoPluginLoader.Commands.Exceptions;
using CommandException = LyokoAPI.Exceptions.CommandException;

namespace LyokoPluginLoader.Commands
{
    internal class Enable : LyokoAPI.Commands.Command
    {
        public override string Usage { get; } = "api.plugins.enable.all";
        public override string Name { get; } = "enable";
        protected override void DoCommand(string[] args)
        {
            CheckLength(1,1);
            if (!(args[0].Equals("all")))
            {
                throw new CommandException(this,"invalid argument");
            }

            if (LoaderInfo.DevMode || !LoaderInfo.StoryModeEnabled)
            {
                PluginLoader.Loader.EnableAll();
                CommandOutputEvent.Call("plugins.enable","All plugins enabled.");
            }
            else
            {
                Output("Story mode is enabled!");
            }
            
        }
    }
}