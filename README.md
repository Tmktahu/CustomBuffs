# CustomBuffs - V Rising Mod

A mod for V Rising that allows you to apply custom made buffs to players.

At the moment, there is only a single custom buff in this mod made to facilitate roleplaying as a human. In the future, more custom buffs may be added.

If you are in need of a specific custom made buff that cannot be achieved through other mods, feel free to reach out to Fryke (fryke) on Discord.

## Dependancies
- Vampire Command Framework (https://github.com/decaprime/VampireCommandFramework)

## Installation
1. Download from the Releases section of the repository.
2. Copy `CustomBuffs.dll` to your VRising server's `BepInEx/plugins` folder
3. Restart server (if running)

## Commands
This mod uses Vampire Command Framework for its commands. All commands require admin, and also work with VRoles out of the box.
- `.custombuffs` or `.cbuff`: The top level parent command. You can do `.help custombuffs` to see a list of commands in-game.
- `.cbuff add <custom_buff_id> <player_name>`: Adds the specified custom buff to the target player.
- `.cbuff remove <custom_buff_id> <player_name>`: Removes the specified custom buff from the target player.
- `.cbuff list <player_name>`: Lists all custom buffs currently applied to the target player.

## Current Custom Buffs
- `human_buff`: This provides immunity to sunlight, 1000 Garlic resistance, and 1000 silver resistance.

## Credits
- Most of the code and concepts are taken directly from Bloodcraft (https://github.com/mfoltz/Bloodcraft), licensed under CC BY-NC 4.0. Huge shoutout to zfolmt (https://github.com/mfoltz) for pointing me in the right direction to figure things out.
- Shoutout to KindredCommands (https://github.com/odjit/KindredCommands) for giving me a codebase to learn from with respect to their implementation of handling sunlight immunity.