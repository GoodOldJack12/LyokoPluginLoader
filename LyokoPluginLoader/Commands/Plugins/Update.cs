using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using LyokoAPI.Events;
using LyokoAPI.Plugin;
using LyokoPluginLoader.Commands.Exceptions;
using YamlDotNet.Serialization;

namespace LyokoPluginLoader.Commands
{
    internal class Update : Command
    {
        public override string Usage { get; } = "api.plugins.update";
        public override string Name { get; } = "update";
        protected override bool DoCommand(string[] args)
        {
            if (LoaderInfo.DevMode || !LoaderInfo.StoryModeEnabled)
            {
                CommandOutputEvent.Call("plugins.update", "Updating plugins!");
                
                WebClient wc = new WebClient();
                string path = Path.GetTempFileName();
                
                wc.DownloadFileAsync(new System.Uri("https://github.com/LyokoAPI/LyokoAPIDoc/blob/V2/docs/LyokoPlugin/pluginlinks.yml"), path);
                wc.DownloadFileCompleted += delegate(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
                {
                    if(e.Error == null && !e.Cancelled)
                    {
                        LyokoLogger.Log("LPL","Plugin list download completed!");
                        checkPlugins(path);
                    }
                    else
                    {
                        LyokoLogger.Log("LPL","An error has occured while downloading the plugin list!");
                    }
                };
            }

            else
            {
                Output("Story mode is enabled!");
            }
            
            return true;
        }

        private void checkPlugins(string path)
        {
            Deserializer deserializer = new Deserializer();
            Dictionary<string,string> urls = deserializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));

            foreach (LyokoAPIPlugin plugin in LoaderInfo.Instance.Plugins)
            {
                if (urls.ContainsKey(plugin.Name.ToLowerInvariant()))
                {
                    updatePlugin(plugin,urls[plugin.Name.ToLowerInvariant()]);
                }
            }
        }

        private void updatePlugin(LyokoAPIPlugin plugin, string url)
        {
            LyokoLogger.Log("LPL", "Checking plugin: " + plugin.Name);
            WebClient wc = new WebClient();
            string path = Path.GetTempFileName();
                
            wc.DownloadFileAsync(new Uri(url), path);
            wc.DownloadFileCompleted += delegate(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
            {
                if(e.Error == null && !e.Cancelled)
                {
                    LyokoLogger.Log("LPL","Plugin download completed!");
                    try
                    {
                        Assembly asm = Assembly.LoadFile(path);
                        foreach (var type in asm.GetExportedTypes())
                        {
                            if (typeof(LyokoAPIPlugin).IsAssignableFrom(type))
                            {
                                LyokoAPIPlugin newPlugin = (LyokoAPIPlugin) Activator.CreateInstance(type);
                                if (isHigherVersion(plugin, newPlugin))
                                {
                                    plugin.Disable();
                                    File.Move(path,PluginLoader.Loader.pluginDirectory.FullName + @"\" + getFileName(url));
                                    bool loaded = newPlugin.Enable();
                                    if (loaded)
                                    {
                                        PluginLoader.Loader.Plugins.Add(newPlugin);
                                        Output("The plugin " + newPlugin.Name + " has been updated and enabled!");
                                    }
                                    else
                                    {
                                        Output("The plugin " + newPlugin.Name + " has been updated but couldn't be enabled!");
                                    }
                                }
                                else
                                {
                                    Output("The plugin " + plugin.Name + " is already updated!");
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        LyokoLogger.Log("LPL", $"Assembly could not be loaded: {exception.ToString()}");
                    }
                }
                else
                {
                    LyokoLogger.Log("LPL","An error has occured while downloading the plugin!");
                }
            };
        }
        
        private string getFileName(string url)
        {
            string result = "";

            var req = WebRequest.Create(url);
            req.Method = "HEAD";
            using (WebResponse resp = req.GetResponse())
            {
                if (!string.IsNullOrEmpty(resp.Headers["Content-Disposition"]))
                {
                    result = resp.Headers["Content-Disposition"].Substring(resp.Headers["Content-Disposition"].IndexOf("filename=") + 9).Replace("\"", "");
                }
            }
            return result;
        }

        private bool isHigherVersion(LyokoAPIPlugin source, LyokoAPIPlugin target)
        {
            if (source.Version.MajorVersion <= target.Version.MajorVersion)
            {
                if (source.Version.MinorVersion <= target.Version.MinorVersion)
                {
                    if (source.Version.SubVersion <= target.Version.SubVersion)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}