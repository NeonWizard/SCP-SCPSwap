using Smod2;
using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SCPSwap
{
	class MiscEventHandler : IEventHandlerWaitingForPlayers, IEventHandlerRoundStart, IEventHandlerFixedUpdate
	{
		private readonly SCPSwap plugin;

		private float pTime = 0;
		private int timePeriod;

		public MiscEventHandler(SCPSwap plugin)
		{
			this.plugin = plugin;
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			if (!this.plugin.GetConfigBool("scpswap_enable")) this.plugin.pluginManager.DisablePlugin(plugin);

			this.timePeriod = this.plugin.GetConfigInt("scpswap_timeperiod");

			this.plugin.pendingSwaps.Clear();
		}

		public void OnRoundStart(RoundStartEvent ev)
		{
			// -- Reset elapsed time on round start
			pTime = 0;
			this.plugin.inSwapPeriod = true;
		}

		public void OnFixedUpdate(FixedUpdateEvent ev)
		{
			if (!this.plugin.inSwapPeriod) return;

			// -- Check if SCP swapping time period has expired
			pTime += Time.fixedDeltaTime;
			if (pTime > this.timePeriod)
			{
				this.plugin.inSwapPeriod = false;
			}
		}
	}
}
