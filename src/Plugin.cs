using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_ExtraDeployChecks
{
    public static class Plugin
    {
        public static string ModAssemblyName => Assembly.GetExecutingAssembly().GetName().Name;

        public static string ConfigPath => Path.Combine(Application.persistentDataPath, ModAssemblyName, "config.json");
        public static string ModPersistenceFolder => Path.Combine(Application.persistentDataPath, ModAssemblyName);
        public static ModConfig Config { get; private set; }

        internal static FileSystemWatcher ConfigChangeWatcher { get; set; }

        [Hook(ModHookType.AfterConfigsLoaded)]
        public static void AfterConfig(IModContext context)
        {
            Directory.CreateDirectory(ModPersistenceFolder);

            Config = ModConfig.LoadConfig(ConfigPath);

            UnityThread.initUnityThread();
            InitConfigWatcher();
            new Harmony("NBK_RedSpy_" + ModAssemblyName).PatchAll();
        }

        private static void InitConfigWatcher()
        {
            ConfigChangeWatcher = new FileSystemWatcher(Path.GetDirectoryName(ConfigPath), Path.GetFileName(ConfigPath));

            ConfigChangeWatcher.NotifyFilter = 
                NotifyFilters.Attributes
                | NotifyFilters.CreationTime
                | NotifyFilters.DirectoryName
                | NotifyFilters.FileName
                | NotifyFilters.LastAccess
                | NotifyFilters.LastWrite
                | NotifyFilters.Security
                | NotifyFilters.Size;

            //Debug
            ConfigChangeWatcher.Changed += ConfigChangeWatcher_Changed;
            ConfigChangeWatcher.Created += ConfigChangeWatcher_Changed;
            ConfigChangeWatcher.Deleted += ConfigChangeWatcher_Changed;
            ConfigChangeWatcher.Renamed += ConfigChangeWatcher_Changed;
            ConfigChangeWatcher.Error += ConfigChangeWatcher_Error;
            ConfigChangeWatcher.EnableRaisingEvents = true;

            //Debug
            Debug.Log("Watcher inited");
        }

        private static void ConfigChangeWatcher_Error(object sender, ErrorEventArgs e)
        {
            UnityThread.executeInUpdate(() =>
            {
                Debug.Log($"Config watcher error: {e.ToString()}");
            });
        }

        private static void ConfigChangeWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            lock (ConfigChangeWatcher)
            {
                try
                {
                    ConfigChangeWatcher.EnableRaisingEvents = false;

                    UnityThread.executeInUpdate(() =>
                    {
                        Debug.Log($"Reloading changed config {ConfigPath}");
                        Config = ModConfig.LoadConfig(ConfigPath);
                    });

                }
                catch (Exception ex)
                {
                    Debug.Log($"Config reload error: {ex}");
                }            
                finally
                {
                    ConfigChangeWatcher.EnableRaisingEvents = true;
                }
            
            }
        }

    }
}
