
/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Ionic.Zip;
using LyokoAPI.Events;*/

namespace LyokoPluginLoader.DependencyLoading
{
    //kill me
    public class ManualNugetDownloader
    {
        /*private static string DownloadPath = Path.Combine(Path.GetTempPath(),"LAPIDependencies");
        public static List<string> DownloadDLLs(string packageId, string versionString = "", bool clean = true)
        {
            List<string> dllList = new List<string>();
            Directory.CreateDirectory(DownloadPath);
            if (clean) // delete all files before running
            {
                var files = Directory.GetFiles(DownloadPath, "*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
            using (var client = new WebClient())
            {
                LyokoLogger.Log("LPL",$"Downloadeding file: {Path.Combine(DownloadPath,packageId)+".nupkg"}"); //TODO remove
                client.DownloadFile($"https://www.nuget.org/api/v2/package/{packageId}/{versionString}", Path.Combine(DownloadPath,packageId)+".nupkg");
                LyokoLogger.Log("LPL",$"Downloaded file: {Path.Combine(DownloadPath,packageId)+".nupkg"}"); //TODO remove
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (ZipFile zip = ZipFile.Read(Path.Combine(DownloadPath,packageId)+".nupkg"))
            {
                zip.ExtractAll(DownloadPath);
            }

            var allfiles = Directory.GetFiles(DownloadPath, "*.*", SearchOption.AllDirectories);
            dllList.AddRange(allfiles.Where(s => s.EndsWith(".dll")));
            
            var dependencies = new Dictionary<string,string>();
            foreach (var nuspec in allfiles.Where(s => s.EndsWith(".nuspec")))
            {
                var nuspecdepends = getDependencies(nuspec);
                foreach (var depend in nuspecdepends)
                {
                    dependencies.Add(depend.Key,depend.Value);
                }
            }
            foreach (var file in allfiles.Where(s => !s.EndsWith(".dll")))
            {
                File.Delete(file);
            }
            foreach (var keyValuePair in dependencies)
            {
                dllList.AddRange(DownloadDLLs(keyValuePair.Key,keyValuePair.Value,false));
            }

            return dllList;
        }
        
        
        private static Dictionary<string, string> getDependencies(string nuspecpath)
        {
            Dictionary<string,string> Dependencies = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(nuspecpath);
            XmlNode xmlNode = doc.DocumentElement.GetAttributeNode("/package/metadata/dependencies//group[1]");
            foreach (XmlElement o in xmlNode)
            {
                if (o.Name.Equals("dependency"))
                {
                    LyokoLogger.Log("LPL",$"Downloading dependency: {o.GetAttribute("Id")}");
                    Dependencies.Add(o.GetAttribute("Id"),o.GetAttribute("version"));
                }
            }

            return Dependencies;
        }
    }*/
    }
}