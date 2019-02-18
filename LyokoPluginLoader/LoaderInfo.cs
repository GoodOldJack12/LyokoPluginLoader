using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LyokoAPI.API;
using LyokoAPI.Events;
using LyokoAPI.Plugin;

namespace LyokoPluginLoader
{
    public static class LoaderInfo
    {
        public static Collection<LVersion> CompatibleLapiVersions = new Collection<LVersion>()
        {
            "2.0.0"
        };

        public static LVersion Version = "2.0.0";

        public static string GetGreeting()
        {
            DateTime now = DateTime.Now;
            int hour = now.Hour;
            string timegreeting = "Hello";

            if (hour < 6)
            {
                timegreeting = "You should be sleeping";
            }else if (hour < 12)
            {
                timegreeting = "Good morning";
            }else if (hour < 18)
            {
                timegreeting = "Good afternoon";
            }else if (hour < 2)
            {
                timegreeting = "Good evening";
            }

            string user = Environment.UserName;

            string greeting = $"{timegreeting}, {user}.\n" +
                              $"I'm LyokoPluginLoader {Version}.\n" +
                              $"Current LAPI version: {Info.Version()}\n (Compatibility: {Info.Version().GetCompatibility(Version)})";

            return greeting;
        }

        public static string PluginsList(ICollection<LyokoAPIPlugin> loadedPlugins)
        {
            IEnumerable<LyokoAPIPlugin> enabled = loadedPlugins.Where(plugin => plugin.Enabled);
            IEnumerable<LyokoAPIPlugin> disabled = loadedPlugins.Where(plugin => !plugin.Enabled);
            string result = $"{EnabledPluginsList(enabled.ToList())}\n \n{DisabledPluginsList(disabled.ToList())}";
            return result;
        }

        public static string EnabledPluginsList(ICollection<LyokoAPIPlugin> loadedPlugins)
        {
            string title = $"LIST OF ENABLED PLUGINS ({loadedPlugins.Count})\n";

            string list = "";
            foreach (var lyokoApiPlugin in loadedPlugins)
            {
                list += $"{lyokoApiPlugin.GetInfo()}\n";
            }

            return $"{title} {list}";
        }
        public static string DisabledPluginsList(ICollection<LyokoAPIPlugin> loadedPlugins)
        {
            string title = $"LIST OF DISABLED PLUGINS ({loadedPlugins.Count})\n";

            string list = "";
            foreach (var lyokoApiPlugin in loadedPlugins)
            {
                list += $"{lyokoApiPlugin.GetInfo()}\n";
            }

            return $"{title} {list}";
        }

        public static string GetInfo(this LyokoAPIPlugin plugin)
        {
            string result = $"{plugin.Name} by {plugin.Author}. Version: {plugin.Version} (LAPI compatiblity {Info.Version().GetCompatibility(plugin.Version)}";
            return result;
        }
        
    }
}