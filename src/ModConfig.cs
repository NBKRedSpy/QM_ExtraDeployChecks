using MGSC;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QM_ExtraDeployChecks.Mcm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QM_ExtraDeployChecks
{
    public class ModConfig : ISave
    {
        public bool CheckEmptyInventory { get; set; } = true;
        public bool CheckEmptyBackpack { get; set; } = true;
        public bool CheckExtraReloads { get; set; } = true;
        public bool CheckPartiallyLoadedWeapons { get; set; } = true;
        public bool CheckArmorSlotNotFilled { get; set; } = true;

        [JsonIgnore]
        private static JsonSerializerSettings SerializerSettings { get; } = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
        };


        [JsonIgnore]
        private static string ConfigPath { get; } = Plugin.ConfigPath;

        /// <summary>
        /// If true, will always cause the confirmation dialog to be shown, even if there are no errors.
        /// </summary>
        public bool DebugDialog { get; set; } = false;

        public static ModConfig LoadConfig()
        {
            ModConfig config;


            if (File.Exists(ConfigPath))
            {
                try
                {
                    string sourceJson = File.ReadAllText(ConfigPath);

                    config = JsonConvert.DeserializeObject<ModConfig>(sourceJson, SerializerSettings);

                    //Add any new elements that have been added since the last mod version the user had.
                    string upgradeConfig = JsonConvert.SerializeObject(config, SerializerSettings);

                    if (upgradeConfig != sourceJson)
                    {
                        Plugin.Logger.Log("Updating config with missing elements");
                        //re-write
                        File.WriteAllText(ConfigPath, upgradeConfig);
                    }
                }
                catch (Exception ex)
                {
                    Plugin.Logger.LogError(ex,"Error parsing configuration.  Ignoring config file and using defaults");

                    //Not overwriting in case the user just made a typo.
                    config = new ModConfig();
                }
            }
            else
            {
                //Use the defaults.
                config = Save(new ModConfig());
            }

            return config;
        }


        public ModConfig Save() 
        {
            return Save(this);
        }

        private static ModConfig Save(ModConfig config)
        {
            string json = JsonConvert.SerializeObject(config, SerializerSettings);
            File.WriteAllText(ConfigPath, json);
            return config;
        }

        void ISave.Save()
        {
            Save(this);
        }
    }
}
