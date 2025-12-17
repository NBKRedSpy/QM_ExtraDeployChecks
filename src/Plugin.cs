using HarmonyLib;
using MGSC;
using QM_ExtraDeployChecks.Mcm;
using QM_ExtraDeployChecks_Bootstrap;
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
    public class Plugin : BootstrapMod
    {
        public static Logger Logger { get;} = new Logger();

        public static string ModAssemblyName { get; private set; }

        /// <summary>
        /// The full path to the config file.  Stored in the mod's persistence folder.
        /// </summary>
        public static string ConfigPath { get; private set; }

        /// <summary>
        /// This mod's persistence folder.
        /// </summary>
        public static string ModsPersistenceFolder { get; private set; }

        /// <summary>
        /// The Quasimorph_Mods folder that is parallel to the game's folder.
        /// This is a workaround for Quasimorph syncing and overwriting all files in the 
        /// Game's App Data folder.
        /// </summary>
        private static string AllModsConfigFolder { get; set; }

        public static ModConfig Config { get; private set; }

        internal static FileSystemWatcher ConfigChangeWatcher { get; set; }

        private static McmConfiguration McmConfiguration { get; set; }

        static Plugin()
        {
            ModAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            AllModsConfigFolder = Path.Combine(Application.persistentDataPath, "../Quasimorph_ModConfigs/");
            ModsPersistenceFolder = Path.Combine(AllModsConfigFolder, ModAssemblyName);
            ConfigPath = Path.Combine(ModsPersistenceFolder, "config.json");
        }

        public Plugin(HookEvents hookEvents, bool isBeta) : base(hookEvents, isBeta)
        {
            HookEvents.AfterConfigsLoaded += AfterConfig;
        }

        public static void AfterConfig(IModContext context)
        {
            Directory.CreateDirectory(AllModsConfigFolder);
            UpgradeModDirectory();
            Directory.CreateDirectory(ModsPersistenceFolder);

            Config = ModConfig.LoadConfig();

            McmConfiguration = new McmConfiguration(Config);
            McmConfiguration.TryConfigure();

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
            //Plugin.Logger.Log("Watcher inited");
        }

        /// <summary>
        /// Moves the config files from the legacy directory to the new directory.
        /// </summary>
        private static void UpgradeModDirectory()
        {
            try
            {
                string oldDirectory = Path.Combine(Application.persistentDataPath,
                    ModAssemblyName);

                if (!Directory.Exists(oldDirectory)) return;

                Plugin.Logger.LogWarning($"Moving config folder from '{oldDirectory}' to '{ModsPersistenceFolder}");
                Directory.Move(oldDirectory, ModsPersistenceFolder);
            }
            catch (Exception ex)
            {
                Plugin.Logger.Log($"Unable to move the config files.  Exception: {ex.ToString()}");
            }
        }

        #region File Watcher
        private static void ConfigChangeWatcher_Error(object sender, ErrorEventArgs e)
        {
            UnityThread.executeInUpdate(() =>
            {
                Plugin.Logger.Log($"Config watcher error: {e.ToString()}");
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
                        Plugin.Logger.Log($"Reloading changed config {ConfigPath}");
                        Config = ModConfig.LoadConfig();
                    });

                }
                catch (Exception ex)
                {
                    Plugin.Logger.Log($"Config reload error: {ex}");
                }            
                finally
                {
                    ConfigChangeWatcher.EnableRaisingEvents = true;
                }
            
            }
        }
        #endregion

    }
}
