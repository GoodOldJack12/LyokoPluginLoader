using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LyokoAPI.API;
using LyokoAPI.API.Compatibility;
using LyokoAPI.Events;
using LyokoAPI.Plugin;

namespace LyokoPluginLoader
{
    public static class LoaderInfo
    {
        public static PluginLoader Instance { get; private set; }
        public static bool StoryModeEnabled { get; internal set; }
        public static bool GameStarted { get; internal set; }

        public static bool DevMode { get; set; }= false;

        public static Collection<LVersion> CompatibleLapiVersions = new Collection<LVersion>()
        {
            "2.0.0"
        };

        public static LVersion Version = "2.0.0";

        public static void SetInstance(PluginLoader loader)
        {
            if (Instance == null)
            {
                Instance = loader;
            }
        }

        public static string GetGreeting()
        {
            DateTime now = DateTime.Now;
            int hour = now.Hour;
            string timegreeting = "Hello";

            if (hour < 6)
            {
                timegreeting = "You should be sleeping";
            }
            else if (hour < 12)
            {
                timegreeting = "Good morning";
            }
            else if (hour < 18)
            {
                timegreeting = "Good afternoon";
            }
            else if (hour < 2)
            {
                timegreeting = "Good evening";
            }

            string user = Environment.UserName;

            string greeting = $"{timegreeting}, {user}.\n" +
                              $"I'm LyokoPluginLoader {Version}.\n" +
                              $"Current LAPI version: {Info.Version()}\n (Compatibility: {CompatibleLapiVersions.Max(version => version.GetCompatibility(Info.Version()))})";

            return greeting;
        }

        public static string PluginsList()
        {
            string result = $"{EnabledPluginsList()} \n{DisabledPluginsList()}";
            return result;
        }

        public static string SimplePluginList(IEnumerable<LyokoAPIPlugin> plugins = null)
        {
            if (plugins == null){plugins = Instance.Plugins;}

            var lyokoApiPlugins = plugins.ToList();
            string result = $"Plugins({lyokoApiPlugins.Count}): ";
            lyokoApiPlugins.ForEach(plugin => { result += $"{plugin.GetBasicInfo()} "; });
           
            return result;
        }

        public static string SimplePluginListEnabled()
        {
            return SimplePluginList(Instance.Plugins.Where(plugin => plugin.Enabled));
        }

        public static string SimplePluginlistDisabled()
        {
            return SimplePluginList(Instance.Plugins.Where(plugin => !plugin.Enabled));
        }
        public static string EnabledPluginsList()
        {
            var loadedPlugins = Instance.Plugins.Where(plugin => plugin.Enabled).ToList();
            string title = $"\nLIST OF ENABLED PLUGINS ({loadedPlugins.Count()})";

            string list = "";
            foreach (var lyokoApiPlugin in loadedPlugins)
            {
                list += $"\n{lyokoApiPlugin.GetInfo()}";
            }

            return $"{title} {list}";
        }

        public static string DisabledPluginsList()
        {
            var loadedPlugins = Instance.Plugins.Where(plugin => !plugin.Enabled).ToList();
            string title = $"\nLIST OF DISABLED PLUGINS ({loadedPlugins.Count()})";

            string list = "";
            foreach (var lyokoApiPlugin in loadedPlugins)
            {
                list += $"\n{lyokoApiPlugin.GetInfo()}";
            }

            return $"{title} {list}";
        }

        public static string GetInfo(this LyokoAPIPlugin plugin)
        {
            string result =
                $"{plugin.Name} by {plugin.Author}. Version: {plugin.Version} (LAPI compatibility {plugin.GetHighestCompatibility().ToString()})";
            return result;
        }

        public static string GetBasicInfo(this LyokoAPIPlugin plugin)
        {
            var enabledstring = plugin.Enabled ? "E" : "D";
            string result =
                $"{plugin.Name} ({enabledstring})";
            return result;
        }

        public static CompatibilityLevel GetHighestCompatibility(this LyokoAPIPlugin plugin, LVersion compare)
        {
            return plugin.CompatibleLAPIVersions.Max(version => version.GetCompatibility(compare));
        }

        public static CompatibilityLevel GetHighestCompatibility(this LyokoAPIPlugin plugin)
        {
           return plugin.GetHighestCompatibility(Info.Version());
        }
    }
}