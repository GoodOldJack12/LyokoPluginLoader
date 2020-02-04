using LyokoAPI.Events;
using LyokoAPI.Exceptions;

namespace LyokoPluginLoader.Commands
{
    internal class Disable : LyokoAPI.Commands.Command
    {
        public override string Usage { get; } = "api.plugins.disable.all";
        public override string Name { get; } = "disable";
        protected override void DoCommand(string[] args)
        {
            CheckLength(1,1);
            if (!(args[0].Equals("all")))
            {
                throw new CommandException(this,"invalid argument");
            }
            if (LoaderInfo.DevMode || !LoaderInfo.StoryModeEnabled)
            {
                PluginLoader.Loader.DisableAll();
                CommandOutputEvent.Call("plugins.disable","All plugins disabled.");
            }else
            {
                Output("Story mode is enabled!");
            }
            
        }
    }
}