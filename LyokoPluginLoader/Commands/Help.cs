namespace LyokoPluginLoader.Commands
{
    internal class Help : Command
    {
        public override string Usage { get; } = "api.plugins.help";
        public override string Name { get; } = "help";
        protected override bool DoCommand(string[] args)
        {
            Output("Commands: [api.plugins.enable,api.plugins.disable,api.plugins.list(.disabled/.enabled]");
            return true;
        }
    }
}