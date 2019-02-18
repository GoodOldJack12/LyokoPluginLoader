using System;

namespace LyokoPluginLoader.Commands.Exceptions
{
    public abstract class AbstractCommandException : Exception

    {
    public abstract void ResolveToLog();
    public abstract void ResolveToUser();
    }
}