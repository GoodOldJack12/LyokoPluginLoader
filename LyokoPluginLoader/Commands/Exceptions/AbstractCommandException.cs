using System;

namespace LyokoPluginLoader.Commands.Exceptions
{
    internal abstract class AbstractCommandException : Exception

    {
    public abstract void ResolveToLog();
    public abstract void ResolveToUser();
    }
}