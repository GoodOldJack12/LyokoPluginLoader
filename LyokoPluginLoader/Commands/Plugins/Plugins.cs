using System;
using System.Linq;
using LyokoPluginLoader.Commands.Exceptions;

namespace LyokoPluginLoader.Commands
{
    internal class Plugins : Command
    {
        public override string Usage { get; } = "api.plugins.disable/enable";
        public override string Name { get; } = "plugins";

        public Plugins()
        {
            AddSubCommand(new Enable());
            AddSubCommand(new Disable());
        }

        protected override bool DoCommand(string[] args)
        {
           CheckArgs(args,1);
           if (HasSubCommand(args[0]))
           {
              var subcommand = SubCommands.Single(command => command.Name.Equals(args[0]));
              return subcommand.Run(args.ToList().GetRange(1, args.Length - 1).ToArray());
           }
           throw new CommandException(this,"That command was not found. Usage:"+Usage);
        }
    }
}