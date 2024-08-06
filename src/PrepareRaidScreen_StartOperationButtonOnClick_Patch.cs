using HarmonyLib;
using MGSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;
using static MGSC.PrepareRaidScreen;

namespace QM_ExtraDeployChecks
{
    [HarmonyPatch(typeof(PrepareRaidScreen), nameof(PrepareRaidScreen.StartOperationButtonOnClick))]
    public static class PrepareRaidScreen_StartOperationButtonOnClick_Patch
    {


        public static bool Prefix(PrepareRaidScreen __instance, CommonButton obj)
        {

            //WARNING - this is a copy of the StartOperationButtonOnClick function, shown below
            ////Original Game Code
            //private void StartOperationButtonOnClick(CommonButton obj)
            //{
            //    if (!_isStartingOperation && !SharedUi.ConfirmDialogWindow.IsViewActive)
            //    {
            //        if (_mercenary.Inventory.Empty && _showMode != 0)
            //        {
            //            SharedUi.ConfirmDialogWindow.Show(ConfirmStartMissionDialog, "ui.dialog.no_items_raidstart");
            //        }
            //        else
            //        {
            //            StartCoroutine(StartOperation());
            //        }
            //    }
            //}


            if (!__instance._isStartingOperation && !SharedUi.ConfirmDialogWindow.IsViewActive)
            {
                StringBuilder message = new StringBuilder();

                Inventory inventory = __instance._mercenary.Inventory;

                if (__instance._showMode != 0)
                {

                    if (Plugin.Config.CheckEmptyInventory && __instance._mercenary.Inventory.Empty)
                    {
                        message.AppendLine(Localization.Get("ui.dialog.no_items_raidstart"));
                    }

                    if (Plugin.Config.CheckEmptyBackpack && !CheckEmptyBackpack(inventory))
                    {
                        message.AppendLine("Backpack is empty");
                    }

                    if (Plugin.Config.CheckExtraReloads)
                    {
                        var missingAmmoReloads = MissingAmmoReloads(inventory);

                        if (missingAmmoReloads.Count != 0)
                        {
                            message.AppendLine($"No reloads for: {String.Join(",", missingAmmoReloads)}");
                        }
                    }


                    if (Plugin.Config.CheckPartiallyLoadedWeapons)
                    {
                        var partiallyLoaded = PartiallyLoadedWeapons(inventory);

                        if (partiallyLoaded.Count != 0)
                        {
                            message.AppendLine($"Weapons not fully loaded: {String.Join(",", partiallyLoaded)}");
                        }

                    }

                    if (Plugin.Config.CheckArmorSlotNotFilled && IsMissingArmor(inventory))
                    {
                        message.AppendLine($"One or more armor slots are empty");
                    }

                }

                if (Plugin.Config.DebugDialog)
                {
                    message.AppendLine("Debug");
                }

                if (message.Length != 0)
                {
                    SharedUi.ConfirmDialogWindow.Show(__instance.ConfirmStartMissionDialog, message.ToString());
                }
                else
                {
                    __instance.StartCoroutine(__instance.StartOperation());
                }

                return false;
            }

            return true;
        }

        private static bool IsMissingArmor(Inventory inventory)
        {
            return
                inventory.HelmetSlot.Empty ||
                inventory.ArmorSlot.Empty ||
                inventory.BootsSlot.Empty ||
                inventory.LeggingsSlot.Empty;
        }

        private static bool CheckEmptyBackpack(Inventory inventory)
        {
            return inventory.BackpackStore.Items.Count != 0;
        }

        /// <summary>
        /// Checks for any weapons that do not have an extra reload in inventory.
        /// </summary>
        /// <param name="inventory">The merc's inventory</param>
        /// <param name="ammoWeapons"></param>
        /// <returns>The ammo types that are needed, but do not have an extra reload.
        /// <exception cref="NotImplementedException"></exception>
        private static List<string> MissingAmmoReloads(Inventory inventory)
        {

            List<BasePickupItem> weapons = GetAmmoWeapons(inventory);

            //Distinct ammo types needed for guns.
            var requiredAmmoTypes = weapons
                .Select(x => x.Comp<WeaponComponent>()?.RequiredAmmoType)
                .Where(x => x != null)
                .ToHashSet();

            //Get the ammo types in invenotry
            var availableAmmoTypes = inventory.AllContainers
                .SelectMany(x => x.Items)
                .Select(x => x.Record<AmmoRecord>()?.AmmoType)
                .ToHashSet();

            var missingAmmo = requiredAmmoTypes
                .Where(x => !availableAmmoTypes.Contains(x))
                .ToList();

            return missingAmmo;
        }

        /// <summary>
        /// Returns the Ids of any weapons that do not have a full mag.
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns>The weapon ids that are partially loaded.</returns>
        private static List<string> PartiallyLoadedWeapons(Inventory inventory)
        {
            var weapons = GetAmmoWeapons(inventory);

            var result = weapons
                .Select(x => new { Weapon = x, Component = x.Comp<WeaponComponent>() })
                .Where(x => x.Weapon != null && x.Component.NeedReload)
                .Select(x => x.Weapon.Id)
                .ToList();

            return result;
        }

        /// <summary>
        /// Returns the list of weapons that require ammo
        /// </summary>
        /// <param name="inventory"></param>
        private static List<BasePickupItem> GetAmmoWeapons(Inventory inventory)
        {
            List<BasePickupItem> items = inventory.AllContainers.SelectMany(x => x.Items)
                .Where(x => {
                    WeaponComponent weapon;
                    return ((weapon = x.Comp<WeaponComponent>()) != null
                        && weapon.RequireAmmo == true);
                })
                .ToList();

            return items;
        }







    }
}
