using Smod2;
using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Collections.Generic;
using System;

namespace SCPSwap
{
	class MiscEventHandler : IEventHandlerWaitingForPlayers
	{
		private readonly SCPSwap plugin;

		public MiscEventHandler(SCPSwap plugin) => this.plugin = plugin;

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			if (!this.plugin.GetConfigBool("scpswap_enable")) this.plugin.pluginManager.DisablePlugin(plugin);
		}
	}
}
