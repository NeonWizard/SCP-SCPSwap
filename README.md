# SCPSwap
A simple plugin for SCP:SL Smod2 servers that allows players to change which SCP they are at the start of the round. There is configurable conditions to be met to allow swapping SCPs, such as time and health limits. If a player wants to switch to an already taken SCP, a request is sent to the user to allow them to accept the trade.

## Installation
**[Smod2](https://github.com/Grover-c13/Smod2) must be installed for this to work.**

1. Grab the [latest release](https://github.com/NeonWizard/SCP-SCPSwap/releases/latest) of SCPSwap.
2. Place SCPSwap.dll in your server's `sm_plugins` folder.

## Commands
Command | Value Type | Description
:---: | :---: | ---
SCPSWAPDISABLE | | **Disables the SCPSwap plugin.** Server will need to restart to enable it again.
SCPSWAP | Int | Attempt to swap with the SCP designated under the provided Int.

## Configuration
Config Option | Value Type | Default Value | Description
:---: | :---: | :---: | ---
scpswap_enable | Bool | True | Whether SCPSwap should be enabled on server start.

*Note that all configs should go in your server config file, not config_remoteadmin.txt

### Place any suggestions/problems in [issues](https://github.com/NeonWizard/SCP-SCPSwap/issues)!
