namespace LyokoPluginLoader.Events
{
    public class GameStartEvent
    {
        internal static event Events.onGameEvent GameStartE;

        public static void Call(bool story = false)
        {
            GameStartE?.Invoke(story);
        }

        public static Events.onGameEvent Subscribe(Events.onGameEvent func)
        {
            GameStartE += func;
            return func;
        }

        public static void Unsubscrive(Events.onGameEvent func)
        {
            GameStartE -= func;
        }
    }
}