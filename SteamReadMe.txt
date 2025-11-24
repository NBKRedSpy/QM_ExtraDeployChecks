[h1]Quasimorph Extra Deployment Checks[/h1]


Just deployed on a mission to find that the merc wasn't reloaded from the previous mission's "unload all"?

This mod includes optional loadout checks to the deployment window.

When deploying, the confirmation window will be shown if any of the optional checks below fail.

[h1]Configuration[/h1]

This mod supports MCM.  The config can be changed in the Mods menu or directly in the config file.

The configuration file will be created on the first game run and can be found at [i]%AppData%\..\LocalLow\Magnum Scriptum Ltd\Quasimorph_ModConfigs\QM_ExtraDeployChecks\config.json[/i].
[table]
[tr]
[td]Name
[/td]
[td]Description
[/td]
[td]Config Key
[/td]
[td]Default
[/td]
[/tr]
[tr]
[td]Empty Inventory
[/td]
[td]The game's default check.  Checks if the merc's inventory is completely empty.
[/td]
[td]CheckEmptyInventory
[/td]
[td]true
[/td]
[/tr]
[tr]
[td]Empty Backpack
[/td]
[td]Checks if there are no items in the backpack
[/td]
[td]CheckEmptyBackpack
[/td]
[td]true
[/td]
[/tr]
[tr]
[td]Extra Reloads
[/td]
[td]Checks if any weapon does not have a matching stack of ammo in inventory.
[/td]
[td]CheckExtraReloads
[/td]
[td]true
[/td]
[/tr]
[tr]
[td]Partially Loaded Weapons
[/td]
[td]Checks if a weapon is not fully loaded.  Ex:  12 out of 24 rounds
[/td]
[td]CheckPartiallyLoadedWeapons
[/td]
[td]true
[/td]
[/tr]
[tr]
[td]Armor Slot Empty
[/td]
[td]Checks if any armor slot is not filled.  Head, body, legs, feet
[/td]
[td]CheckArmorSlotNotFilled
[/td]
[td]true
[/td]
[/tr]
[tr]
[td]Weapon Slot Empty
[/td]
[td]Checks if any weapon slot is not filled.  Head, body, legs, feet
[/td]
[td]CheckWeaponSlotNotFilled
[/td]
[td]true
[/td]
[/tr]
[/table]

[h1]Rough Edges[/h1]

[h2]UI Overdraw[/h2]

The game's confirmation dialog does expand to the text size.
The warnings are still readable, just not pretty.

[h1]Support[/h1]

If you enjoy my mods and want to buy me a coffee, check out my [url=https://ko-fi.com/nbkredspy71915]Ko-Fi[/url] page.
Thanks!

[h1]Source Code[/h1]

Source code is available on GitHub at https://github.com/NBKRedSpy/QM_ExtraDeployChecks

[h1]Credits[/h1]
[list]
[*]Special thanks to Crynano for his excellent Mod Configuration Menu.
[/list]

[h1]Change Log[/h1]

[h2]1.5.1[/h2]
[list]
[*]Added check for empty weapon slot.
[/list]

[h2]1.5.0[/h2]
[list]
[*]Added MCM.
[/list]

[h2]1.4.0[/h2]
[list]
[*]The names of items are now localized.  See note about missing ammo above.
[/list]
