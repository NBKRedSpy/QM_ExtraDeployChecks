using MGSC;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    public class ModConfig
    {
        public bool CheckEmptyInventory { get; set; } = true;
        public bool CheckEmptyBackpack { get; set; } = true;
        public bool CheckExtraReloads { get; set; } = true;
        public bool CheckPartiallyLoadedWeapons { get; set; } = true;
        public bool CheckArmorSlotNotFilled { get; set; } = true;

        /// <summary>
        /// If true, will always cause the confirmation dialog to be shown, even if there are no errors.
        /// </summary>
        public bool DebugDialog { get; set; } = false;


        public static ModConfig LoadConfig(string configPath)
        {
            ModConfig config;

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };


            if (File.Exists(configPath))
            {
                try
                {
                    string sourceJson = File.ReadAllText(configPath);

                    config = JsonConvert.DeserializeObject<ModConfig>(sourceJson, serializerSettings);

                    //Add any new elements that have been added since the last mod version the user had.
                    string upgradeConfig = JsonConvert.SerializeObject(config, serializerSettings);

                    if (upgradeConfig != sourceJson)
                    {
                        Debug.Log("Updating config with missing elements");
                        //re-write
                        File.WriteAllText(configPath, upgradeConfig);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error parsing configuration.  Ignoring config file and using defaults");
                    Debug.LogException(ex);

                    //Not overwriting in case the user just made a typo.
                    config = new ModConfig();
                }
            }
            else
            {
                config = new ModConfig();

                string json = JsonConvert.SerializeObject(config, serializerSettings);
                File.WriteAllText(configPath, json);

            }

            return config;
        }
    }
}
