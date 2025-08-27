# Quasimorph Extra Deployment Checks

![thumbnail icon](media/thumbnail.png)

Just deployed on a mission to find that the merc wasn't reloaded from the previous mission's "unload all"? 

This mod includes optional loadout checks to the deployment window.

When deploying, the confirmation window will be shown if any of the optional checks below fail.


# Configuration

This mod supports MCM.  The config can be changed in the Mods menu or directly in the config file.

The configuration file will be created on the first game run and can be found at `%AppData%\..\LocalLow\Magnum Scriptum Ltd\Quasimorph_ModConfigs\QM_ExtraDeployChecks\config.json`.

|Name|Description|Config Key| Default|
|--|--|--|--|
|Empty Inventory|The game's default check.  Checks if the merc's inventory is completely empty.|CheckEmptyInventory|true|
|Empty Backpack|Checks if there are no items in the backpack|CheckEmptyBackpack|true|
|Extra Reloads|Checks if any weapon does not have a matching stack of ammo in inventory.|CheckExtraReloads|true|
|Partially Loaded Weapons|Checks if a weapon is not fully loaded.  Ex:  12 out of 24 rounds|CheckPartiallyLoadedWeapons|true|
|Armor Slot Empty|Checks if any armor slot is not filled.  Head, body, legs, feet|CheckArmorSlotNotFilled|true|


# Rough Edges
## UI Overdraw
The game's confirmation dialog does expand to the text size.
The warnings are still readable, just not pretty.

## Ammo Type
There is not a translation for the generic ammo category.  Ex: all of the 9mm types of ammo.  It does have a translation for the "specific type of ammo".  Ex: "9mm bursting" or just "9mm".

The mod uses the "currently loaded" name.  If the weapon is empty, it uses the game's default, which is usually
the most common ammo type.  Ex: 9mm.

# Support
If you enjoy my mods and want to buy me a coffee, check out my [Ko-Fi](https://ko-fi.com/nbkredspy71915) page.
Thanks!

# Source Code
Source code is available on GitHub at https://github.com/NBKRedSpy/QM_ExtraDeployChecks

# Credits
* Special thanks to Crynano for his excellent Mod Configuration Menu. 

# Change Log

## 1.5.0 
* Added MCM.
## 1.4.0
* The names of items are now localized.  See note about missing ammo above.

