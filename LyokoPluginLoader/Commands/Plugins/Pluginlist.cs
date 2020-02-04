using System;
using System.Linq;
using LyokoAPI.Events;
using LyokoAPI.Exceptions;
using LyokoPluginLoader.Commands.Exceptions;
using CommandException = LyokoAPI.Exceptions.CommandException;

namespace LyokoPluginLoader.Commands
{
    internal class Pluginlist : LyokoAPI.Commands.Command
    {
        public override string Usage { get; } = "api.plugins.list(.enabled/.disabled/.all/(nothing)";
        public override string Name { get; } = "list";

        protected override void DoCommand(string[] args)
        {
            CheckLength(0, 2);
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
                    throw new CommandException(this,"invalid argument!");
                }
            }
            else
            {
                response = LoaderInfo.PluginsList();
                CommandOutputEvent.Call("PluginList",LoaderInfo.SimplePluginList());
            }
            LyokoLogger.Log("LyokoPluginLoader",response);
        }
    }
}
