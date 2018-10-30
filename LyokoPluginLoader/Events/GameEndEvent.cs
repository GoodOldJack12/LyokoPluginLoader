namespace LyokoPluginLoader.Events
{
    public class GameEndEvent
    {
        internal static event Events.onGameEvent GameEndE;

        public static void Call(bool failed)
        {
            GameEndE?.Invoke(failed);
        }

        public static Events.onGameEvent Subscribe(Events.onGameEvent func)
        {
            GameEndE += func;
            return func;
        }

        public static void Unsubscrive(Events.onGameEvent func)
        {
            GameEndE -= func;
        }
    }
}