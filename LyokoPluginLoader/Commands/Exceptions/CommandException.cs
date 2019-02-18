using System;
using LyokoAPI.Events;

namespace LyokoPluginLoader.Commands.Exceptions
{
    public class CommandException : AbstractCommandException
    {
        private string _errorString;
        private string _usage;
        public CommandException(ICommand command, string errormessage = "invalid syntax")
        {
            _errorString = errormessage;
            _usage = command.Usage;
        }

        public override void ResolveToLog()
        {
            LyokoLogger.Log("LyokoPluginLoader",MakeErrorMessage());
        }


        public override void ResolveToUser()
        {
            CommandOutputEvent.Call("LPL",MakeErrorMessage());
        }

        private string MakeErrorMessage()
        {
            return $"ANOMALY! ({_errorString}) Usage: {_usage}";
        }
    }
}