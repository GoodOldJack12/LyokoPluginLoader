using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using LyokoAPI.Events;
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
              LyokoLogger.Log("LyokoPluginLoader","Go yell this at the application dev: The given plugin directory is a file!"); //this will likely never show up unless application dev subscribes to logger
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
          var unloadedTypes = (
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
              select type
          ).ToList();
          Plugins = new List<LyokoAPIPlugin>();
          #region LoadLogger
          var loggerplugin = unloadedTypes.Find(type => type.Name.Equals("LoggerPlugin"));
          if (loggerplugin != null)
          {
              try
              {
                  var logger = ((LyokoAPIPlugin) Activator.CreateInstance(loggerplugin));
                  logger.Enable();
                  if (logger.Enabled)
                  {
                      Plugins.Add(logger);
                      unloadedTypes.Remove(loggerplugin);
                  }
              }
              catch(TypeLoadException)
              {
                  //do nothing
              }
          }
          #endregion
          
          foreach (var type in unloadedTypes)
          {
              try
              {
                  UnloadedPlugins.Add((LyokoAPIPlugin) Activator.CreateInstance(type));
              }
              catch (TypeLoadException)
              {
                  LyokoLogger.Log("LyokoPluginLoader",$"An unidentified plugin ({type.Assembly.FullName}) could not be loaded! Check if Your plugin has the right API version!");
              }
          }
          
          foreach (var unloadedPlugin in UnloadedPlugins)
          {
              bool loaded = unloadedPlugin.Enable();
              if (loaded)
              {
                  Plugins.Add(unloadedPlugin);
              }
          }
      }

      public void DisableAll()
      {
          Plugins.ForEach(plugin=>plugin.Disable());
      }

      public void EnableAll()
      {
          Plugins.ForEach(plugin => plugin.Enable());
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