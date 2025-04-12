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

            if (__instance._mercenary.CreatureData.Inventory.Empty && __instance._showMode != 0)
            {
                return true;
            }

            if(__instance._showMode == ShowMode.Station)
            {
                return true;
            }

            StringBuilder message = new StringBuilder();

            Inventory inventory = __instance._mercenary.CreatureData.Inventory;

            //TODO:  I think the game's check already does this.  Leaving for now.
            if (Plugin.Config.CheckEmptyInventory && __instance._mercenary.CreatureData.Inventory.Empty)
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

            if (message.Length == 0)
            {
                return true;
            }

            UI.Chain<ConfirmDialogWindow>().Invoke(delegate (ConfirmDialogWindow v)
            {
                v.Configure(__instance.ConfirmStartMissionDialog, message.ToString());
            }).Show();

            return false;
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
