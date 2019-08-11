using System.Collections.Generic;
using System.IO;
using System.Text;
using LyokoAPI.Events;
using LyokoAPI.Plugin;

namespace LyokoPluginLoader.DependencyLoading
{
    internal class DependencyLoader
    {
        private string DependencyDir { get; }
        private bool disabled = false;

        internal DependencyLoader(string dependencyDir)
        {
            DependencyDir = dependencyDir;
        }

        internal void LoadDependency(LyokoAPIPlugin plugin)
        {
            LyokoLogger.Log("LPL",$"Loading dependencies of {plugin.Name}"); //TODO remove
            if (disabled)
            {
                return;
            }

            foreach (var pluginDependency in plugin.Dependencies)
            {
                downloadDepend(pluginDependency);
            }
        }

        private void CopyDependencies(List<string> paths)
        {
            paths.ForEach(path => File.Move(path,Path.Combine(DependencyDir,Path.GetFileName(path))));
        }

        private void downloadDepend(Dependency dependency)
        {
            LyokoLogger.Log("LPL",$"DownLoading dependency {dependency.NugetID}"); //TODO remove

             AutomaticNugetDownloader.DownloadDLLs(dependency.NugetID, dependency.VersionString);
        }
    }
}