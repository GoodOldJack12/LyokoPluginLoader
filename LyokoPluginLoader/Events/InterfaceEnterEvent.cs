namespace LyokoPluginLoader.Events
{
    public class InterfaceEnterEvent
    {
        internal static event Events.onVoidEvent InterfaceEnterE;

        public static Events.onVoidEvent Subscribe(Events.onVoidEvent e)
        {
            InterfaceEnterE += e;
            return e;
        }

        public static Events.onVoidEvent Unsubscribe(Events.onVoidEvent e)
        {
            InterfaceEnterE -= e;
            return e;
        }

        private static void Call()
        {
            InterfaceEnterE?.Invoke();
        }
    }
}