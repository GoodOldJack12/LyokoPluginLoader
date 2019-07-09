using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LyokoAPI.Events;
using LyokoPluginLoader.Commands;

namespace LyokoPluginLoader
{
    internal class CommandListener
    {
        private static ICollection<ICommand> _commands = new List<ICommand>();
        private static bool _listening;
        
        static CommandListener()
        {
            _commands.Add(new Pluginlist());
        }
        
        public static void StartListening()
        {
            if (!_listening)
            {
                CommandInputEvent.Subscribe(OnCommand);
                _listening = true;
            }
        }

        public static void StopListening()
        {
            if (_listening)
            {
                CommandInputEvent.Unsubscribe(OnCommand);
                _listening = false;
            }
        }
        
        private static bool FindCommand(string[] commandargs)
        {
            string name = commandargs[0];
            if (commandargs.Length > 1)
            {
                commandargs = commandargs.ToList().GetRange(1, commandargs.Length - 1).ToArray();
            }
            else
            {
                commandargs = new string[] {};
            }
            foreach (var command in _commands)
            {
                if (command.Name.Equals(name))
                {
                    command.Run(commandargs);
                    return true;
                }
            }

            return false;
        }
        
        
        

        private static void OnCommand(string command)
        {
            string[] commandargs = command.Split('.');
            if (commandargs[0] != "loader")
            {
                return;
            }
            commandargs = commandargs.ToList().GetRange(1, commandargs.Length - 1).ToArray();
            if (!FindCommand(commandargs))
            {
                CommandOutputEvent.Call("LPL",$"Command {command} not found!");
            }
        }
        
    }
}