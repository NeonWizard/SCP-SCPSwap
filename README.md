# SCPSwap
A simple plugin for SCP:SL Smod2 servers that allows players to change which SCP they are at the start of the round. There are configurable conditions to be met to allow swapping SCPs, such as time and health limits. If a player wants to switch to an already taken SCP, a request is sent to the user to allow them to accept the trade.

## Installation
**[Smod2](https://github.com/Grover-c13/Smod2) must be installed for this to work.**

1. Grab the [latest release](https://github.com/NeonWizard/SCP-SCPSwap/releases/latest) of SCPSwap.
2. Place SCPSwap.dll in your server's `sm_plugins` folder.

## Commands
Command | Value Type | Description
:---: | :---: | ---
SCPSWAPDISABLE | | **Disables the SCPSwap plugin.** Server will need to restart to enable it again.

## GameConsole Commands
Command | Value Type | Description
:---: | :---: | ---
SCPSWAP | Int | Attempt to swap with the SCP designated under the provided Int.
SCPLIST | | List all alive SCPs (only accessible to SCPs).

## Configuration
Config Option | Value Type | Default Value | Description
:---: | :---: | :---: | ---
scpswap_enable | Bool | True | Whether SCPSwap should be enabled on server start.
scpswap_timeperiod | Int | 30 | Amount of time in seconds after round start that player can swap SCPs.
scpswap_minhealth | Int | 98 | Minimum health percentage required to be able to swap SCPs.
scpswap_preservehealth | Bool | True | Whether to preserve health percentage on a swap.
scpswap_maxswaps | Int | 1 | How many swaps can be done per player, per round.

*Note that all configs should go in your server config file, not config_remoteadmin.txt

### Place any suggestions/problems in [issues](https://github.com/NeonWizard/SCP-SCPSwap/issues)!
