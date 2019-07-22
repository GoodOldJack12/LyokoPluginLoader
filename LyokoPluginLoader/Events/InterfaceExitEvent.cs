namespace LyokoPluginLoader.Events
{
    public class InterfaceExitEvent
    {
        internal static event Events.onVoidEvent InterfaceExitE;

        public static Events.onVoidEvent Subscribe(Events.onVoidEvent e)
        {
            InterfaceExitE += e;
            return e;
        }

        public static Events.onVoidEvent Unsubscribe(Events.onVoidEvent e)
        {
            InterfaceExitE -= e;
            return e;
        }
        
        
    }
}