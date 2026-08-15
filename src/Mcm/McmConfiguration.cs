using HarmonyLib;
using ModConfigMenu;
using ModConfigMenu.Contracts;
using ModConfigMenu.Objects;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace QM_ExtraDeployChecks.Mcm
{
    internal class McmConfiguration : McmConfigurationBase
    {

        public McmConfiguration(ModConfig config) : base (config) { }

        public override void Configure()
        {
            ModConfig defaults = new ModConfig();

            ModConfigMenuAPI.RegisterModConfig("Extra Deploy Checks", new List<IConfigValue>()
            {
                CreateConfigProperty(nameof(ModConfig.CheckEmptyInventory),
                    "Checks for a completely empty inventory"),
                CreateConfigProperty(nameof(ModConfig.CheckEmptyBackpack),
                    "Check for an empty backpack."),
                CreateConfigProperty(nameof(ModConfig.CheckExtraReloads),
                    "Checks if there is at least one ammo for each weapon"),
                CreateConfigProperty(nameof(ModConfig.CheckPartiallyLoadedWeapons),
                    "Checks if a weapon has less than the maximum ammo loaded"),
                CreateConfigProperty(nameof(ModConfig.CheckArmorSlotNotFilled),
                    "Checks if one or more armor slots are empty"),
                CreateConfigProperty(nameof(ModConfig.CheckWeaponSlotNotFilled), 
                    "Checks if one or more weapon slots are empty"),
                CreateConfigProperty(nameof(ModConfig.CheckIfFoodInInventory), 
                    "Checks if there are any food items in the inventory"),
                CreateConfigProperty(nameof(ModConfig.DebugDialog),
                    "For debugging.  Always shows the 'continue' message box even with no check failures."),

            }, OnSave);
        }
         
    }
}
