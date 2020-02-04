using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using LyokoAPI.API;
using LyokoAPI.Events;
using LyokoAPI.Plugin;
using LyokoPluginLoader.DependencyLoading;
using LyokoPluginLoader.Events;

namespace LyokoPluginLoader
{
    //TODO: onGameStart(), DisableAll(), EnableAll()
  public class PluginLoader
  {
      private DirectoryInfo pluginDirectory;
      public List<LyokoAPIPlugin> Plugins;
      private CommandListener CommandListener = new CommandListener();
      //private DependencyLoader DependencyLoader { get; }
      internal static PluginLoader Loader { get; private set; }
      public PluginLoader(string path)
      {
          if (GetOrCreateDirectory(path, out pluginDirectory))
          {
              Info.SetConfigPath(path);
              //DependencyLoader = new DependencyLoader(Path.Combine(path, "lib"));
              LoadPlugins();
              RegisterListeners();
              LoaderInfo.SetInstance(this);
              AppDomain.CurrentDomain.ProcessExit += new EventHandler(Quit);
          }
          
          Loader = this;
      }

      public PluginLoader(string path, string pluginConfigDirectory)
      {
          if (GetOrCreateDirectory(path, out pluginDirectory))
          {
              Info.SetConfigPath(pluginConfigDirectory);
              //DependencyLoader = new DependencyLoader(Path.Combine(path, "lib"));
              LoadPlugins();
              RegisterListeners();
              LoaderInfo.SetInstance(this);
              AppDomain.CurrentDomain.ProcessExit += new EventHandler(Quit);
          }

          Loader = this;
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
          List<String> pluginFiles = new List<string>(Directory.GetFiles(pluginDirectory.FullName));
          pluginFiles.RemoveAll(name => !name.EndsWith(".dll"));
          List<LyokoAPIPlugin> UnloadedPlugins = new List<LyokoAPIPlugin>();

          var unloadedTypes = new List<Type>();
          foreach (var file in pluginFiles)
          {
              try
              {
                  Assembly asm = Assembly.LoadFile(file);
                  foreach (var type in asm.GetExportedTypes())
                  {
                      if (typeof(LyokoAPIPlugin).IsAssignableFrom(type)) unloadedTypes.Add(type);
                  }
              }catch (Exception e)
              {
               LyokoLogger.Log("LPL",$"Assembly {file} could not be loaded: {e.ToString()}" );   
              }
              
          }

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

          #region Greeting
          LyokoLogger.Log("LyokoPluginLoader",LoaderInfo.GetGreeting());
          #endregion
          
          foreach (var type in unloadedTypes)
          {
              try
              {
                  UnloadedPlugins.Add((LyokoAPIPlugin) Activator.CreateInstance(type));
              }
              catch (Exception e)
              {
                  LyokoLogger.Log("LyokoPluginLoader",$"An unidentified plugin ({type.Assembly.FullName}) could not be loaded! Check if Your plugin has the right API version! Stacktrace: \n {e.ToString()} {e.Source}");
              }
          }
          //LyokoLogger.Log("LPL",$"Enabling plugins"); 
          foreach (var unloadedPlugin in UnloadedPlugins)
          {
              bool loaded = unloadedPlugin.Enable();
              try
              {
                  //LyokoLogger.Log("LPL",$"Enabled {unloadedPlugin.Name}");
                  //DependencyLoader.LoadDependency(unloadedPlugin);
                  if (loaded)
                  {
                      Plugins.Add(unloadedPlugin);
                  }
              }
              catch (Exception e)
              {
                  LyokoLogger.Log("LyokoPluginLoader",$"An exception occured while loading dependencies of {unloadedPlugin.Name}:{e.ToString()} {e.Source}.\n Plugin was disabled and removed from the list.");
                  unloadedPlugin.Disable();
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
        InterfaceEnterEvent.Subscribe(OnInterfaceEnter);
        InterfaceExitEvent.Subscribe(OnInterfaceExit);
        CommandListener.StartListening();
      }

      private void Quit(object sender, EventArgs eventArgs)
      {
          DisableAll();
          GameEndEvent.Unsubscrive(OnGameEnd);
          GameStartEvent.Unsubscribe(OnGameStart);
          InterfaceEnterEvent.Unsubscribe(OnInterfaceEnter);
          InterfaceExitEvent.Unsubscribe(OnInterfaceExit);
          CommandListener.StopListening();
      }
      


      private void OnGameStart(bool story = false)
      {
          Plugins.ForEach(plugin => plugin.OnGameStart(story));
          LoaderInfo.GameStarted = true;
          LoaderInfo.StoryModeEnabled = story;
      }

      private void OnGameEnd(bool failed)
      {
          Plugins.ForEach(plugin => plugin.OnGameEnd(failed));
          LoaderInfo.GameStarted = false;
          LoaderInfo.StoryModeEnabled = false;
      }

      private void OnInterfaceExit()
      {
          Plugins.ForEach(plugin => plugin.OnInterfaceExit());
      }

      private void OnInterfaceEnter()
      {
          Plugins.ForEach(plugin => plugin.OnInterfaceEnter());
      }
      
  }
}