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
                ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
                wc.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs e)
                {
                    if(e.Error == null && !e.Cancelled)
                    {
                        LyokoLogger.Log("LPL","Plugin list download completed!");
                        checkPlugins(e.Result);
                    }
                    else
                    {
                        LyokoLogger.Log("LPL","An error has occured while downloading the plugin list!\n" + e.Error.Message + "\n" + e.Error.StackTrace);
                    }
                };
                wc.DownloadStringAsync(new Uri("https://raw.githubusercontent.com/LyokoAPI/LyokoAPIDoc/V2/docs/LyokoPlugin/pluginlinks.yml"));
            }

            else
            {
                Output("Story mode is enabled!");
            }
            
            return true;
        }

        private void checkPlugins(string pluginsFile)
        {
            Deserializer deserializer = new Deserializer();
            Dictionary<string,string> urls = deserializer.Deserialize<Dictionary<string, string>>(pluginsFile);

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
            
            ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
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
                    LyokoLogger.Log("LPL","An error has occured while downloading the plugin!\n" + e.Error.Message + "\n" + e.Error.StackTrace);
                }
            };
            wc.DownloadFileAsync(new Uri(url), path);
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