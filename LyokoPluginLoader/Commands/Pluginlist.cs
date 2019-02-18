using System;
using System.Linq;
using LyokoAPI.Events;
using LyokoPluginLoader.Commands.Exceptions;

namespace LyokoPluginLoader.Commands
{
    public class Pluginlist : Command
    {
        public override string Usage { get; } = "api.pluginlist(.enabled/.disabled/.all/(nothing)";
        public override string Name { get; } = "pluginlist";

        protected override bool DoCommand(string[] args)
        {
            CheckArgs(args, 0, 1);
            string response = "";
            if (args.Any())
            {
                string arg = args[0];
                if (arg == "enabled")
                {
                    response = LoaderInfo.EnabledPluginsList();
                }else if (arg == "disabled")
                {
                    response = LoaderInfo.DisabledPluginsList();
                }else if (arg == "all")
                {
                    response = LoaderInfo.PluginsList();
                }
                else
                {
                    throw new CommandException(this);
                }
            }
            else
            {
                response = LoaderInfo.PluginsList();
            }
            LyokoLogger.Log("LyokoPluginLoader",response);
            return true;
        }
    }
}