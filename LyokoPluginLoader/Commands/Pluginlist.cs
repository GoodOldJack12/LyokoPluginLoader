using System;
using System.Linq;
using LyokoAPI.Events;
using LyokoPluginLoader.Commands.Exceptions;

namespace LyokoPluginLoader.Commands
{
    internal class Pluginlist : Command
    {
        public override string Usage { get; } = "api.loader.pluginlist(.enabled/.disabled/.all/(nothing)";
        public override string Name { get; } = "pluginlist";

        protected override bool DoCommand(string[] args)
        {
            CheckArgs(args, 0, 2);
            string response = "";
            if (args.Any())
            {
                string arg = args[0];
                if (arg == "enabled")
                {
                    response = LoaderInfo.EnabledPluginsList();
                    CommandOutputEvent.Call("PluginList",LoaderInfo.SimplePluginListEnabled());
                }else if (arg == "disabled")
                {
                    response = LoaderInfo.DisabledPluginsList();
                    CommandOutputEvent.Call("PluginList",LoaderInfo.SimplePluginlistDisabled());
                }else if (arg == "all")
                {
                    response = LoaderInfo.PluginsList();
                    CommandOutputEvent.Call("PluginList",LoaderInfo.SimplePluginList());
                }
                else
                {
                    throw new CommandException(this);
                }
            }
            else
            {
                response = LoaderInfo.PluginsList();
                CommandOutputEvent.Call("PluginList",LoaderInfo.SimplePluginList());
            }
            LyokoLogger.Log("LyokoPluginLoader",response);
            return true;
        }
    }
}
