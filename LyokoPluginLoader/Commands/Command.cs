using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using LyokoPluginLoader.Commands.Exceptions;

namespace LyokoPluginLoader.Commands
{
    internal abstract class Command : ICommand

    {
        protected ICollection<ICommand> SubCommands = new List<ICommand>(); 
        public abstract string Usage { get; }
        public abstract string Name { get; }
        public bool Run(string[] arguments)
        {
            try
            {
                return DoCommand(arguments);
            }
            catch (AbstractCommandException e)
            {
                e.ResolveToUser();
                return false;
            }
        }


        protected abstract bool DoCommand(string[] args);
        public bool HasSubCommand(string name)
        {
            return SubCommands.Any(command => command.Name.Equals(name));
        }

        protected void AddSubCommand(ICommand command)
        {
            if (!HasSubCommand(command.Name))
            {
                SubCommands.Add(command);
            }
        }

        protected void CheckArgs(string[] strings,int min, int max = Int32.MaxValue)
        {
            if (strings == null)
            {
                strings = new string[] { };
            }
            if (strings.Length < min)
            {
                throw new CommandException(this, "not enough arguments!");
            }else if (strings.Length > max)
            {
                    throw new CommandException(this,"too much arguments");
            }
            
        }
        
    }
}