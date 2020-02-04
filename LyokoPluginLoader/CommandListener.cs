using LyokoPluginLoader.Commands;

namespace LyokoPluginLoader
{
    public class CommandListener : LyokoAPI.Commands.CommandListener
    {
        protected override string Prefix { get; } = "plugins";

        public CommandListener()
        {
            AddCommand(new Enable());
            AddCommand(new Disable());
            AddCommand(new Pluginlist());
            AddCommand(new Help());
        }
    }
}