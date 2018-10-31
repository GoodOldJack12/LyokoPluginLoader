using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using LyokoAPI.Plugin;
using LyokoPluginLoader.Events;

namespace LyokoPluginLoader
{
    //TODO: onGameStart(), DisableAll(), EnableAll()
  public class PluginLoader
  {
      private DirectoryInfo pluginDirectory;
      public List<LyokoAPIPlugin> Plugins;
      public PluginLoader(string path)
      {
          if (GetOrCreateDirectory(path, out pluginDirectory))
          {
              LoadPlugins();
              RegisterListeners();
          }
      }




      private bool GetOrCreateDirectory(string path, out DirectoryInfo directory)
      {
          if (File.Exists(path))
          {
              Console.WriteLine("The given plugin directory is a file!");
              directory = null;
              return false;
          }else if (Directory.Exists(path))
          {
              directory = new DirectoryInfo(path);
              return true;
          }
          else
          {
              directory =  Directory.CreateDirectory(path);
              return true;
          }
      }

      private void LoadPlugins()
      {
          string[] pluginFiles = Directory.GetFiles(pluginDirectory.FullName);
          List<LyokoAPIPlugin> UnloadedPlugins = new List<LyokoAPIPlugin>();
          UnloadedPlugins = (
              // From each file in the files.
              from file in pluginFiles
              // Load the assembly.
              let asm = Assembly.LoadFile(file)
              // For every type in the assembly that is visible outside of
              // the assembly.
              from type in asm.GetExportedTypes()
              // Where the type implements the interface.
              where typeof(LyokoAPIPlugin).IsAssignableFrom(type)
              // Create the instance
              select (LyokoAPIPlugin) Activator.CreateInstance(type)
          ).ToList();
          Plugins = new List<LyokoAPIPlugin>();
          foreach (var unloadedPlugin in UnloadedPlugins)
          {
              bool loaded = unloadedPlugin.OnEnable();
              if (loaded)
              {
                  Plugins.Add(unloadedPlugin);
              }
              else
              {
                  Console.WriteLine("The plugin: {0} by {1} couldn't be loaded!",unloadedPlugin.GetName(),unloadedPlugin.GetAuthor());
              }
          }
      }

      public void DisableAll()
      {
          Plugins.ForEach(plugin=>plugin.OnDisable());
      }

      public void EnableAll()
      {
          Plugins.ForEach(plugin => plugin.OnEnable());
      }

      private void RegisterListeners()
      {
        GameStartEvent.Subscribe(OnGameStart);
        GameEndEvent.Subscribe(OnGameEnd);
      }
      


      private void OnGameStart(bool story = false)
      {
          Plugins.ForEach(plugin => plugin.OnGameStart(story));
      }

      private void OnGameEnd(bool failed)
      {
          Plugins.ForEach(plugin => plugin.OnGameEnd(failed));
      }

  }
}