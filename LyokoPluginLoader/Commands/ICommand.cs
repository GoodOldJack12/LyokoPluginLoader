namespace LyokoPluginLoader.Commands
{
    internal interface ICommand
    {
        
        string Usage { get; }
        string Name { get; }
        bool Run(string[] arguments);
        bool HasSubCommand(string name);

    }
}