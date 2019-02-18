namespace LyokoPluginLoader.Commands
{
    public interface ICommand
    {
        
        string Usage { get; }
        string Name { get; }
        bool Run(string[] arguments);
        bool HasSubCommand(string name);

    }
}