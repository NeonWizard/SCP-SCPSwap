using Smod2;
using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace SCPSwap
{
	class CommandEvent : IEventHandlerCallCommand
	{
		private readonly SCPSwap plugin;

		public CommandEvent(SCPSwap plugin)
		{
			this.plugin = plugin;
		}

		public void OnCallCommand(PlayerCallCommandEvent ev)
		{
			// -- Check for SCPSWAP/SCPLIST command
			string command = ev.Command.ToLower();
			if (!command.StartsWith("scpswap") && !command.StartsWith("scplist")) return;

			// -- Validate player is an SCP
			if (ev.Player.TeamRole.Team != Smod2.API.Team.SCP)
			{
				ev.ReturnMessage = "You are not an SCP!";
				return;
			}
			// -- Zombies can't swap either :^)
			else if (ev.Player.TeamRole.Role == Role.SCP_049_2)
			{
				ev.ReturnMessage = "nice try lol";
				return;
			}

			// -- Execute respective commands
			string[] cmdSplit = command.Split(' ');
			if (cmdSplit[0] == "scpswap")
			{
				// -- Check within swap period and required health
				int timePeriod = this.plugin.GetConfigInt("scpswap_timeperiod");
				int minHealth = this.plugin.GetConfigInt("scpswap_minhealth");

				PlayerStats playerInfo = ((GameObject)ev.Player.GetGameObject()).GetComponent<PlayerStats>();

				if (!this.plugin.inSwapPeriod)
				{
					ev.ReturnMessage = "You can only swap within the first " + timePeriod + " seconds of the round.";
					return;
				}
				else if (((float)playerInfo.health / playerInfo.maxHP * 100) < minHealth)
				{
					ev.ReturnMessage = "Your health is too low! You must be above " + minHealth + "% HP to swap SCPs.";
					return;
				}

				// -- Run accept/request commands
				if (cmdSplit.Count() == 1) this.plugin.AcceptSwapCommand.OnCall(ev, new string[] { }); // Handle 0-arg call to accept trade
				else if (cmdSplit.Count() == 2) this.plugin.RequestSwapCommand.OnCall(ev, new string[] { cmdSplit[1] }); // Handle swapping SCPs
				else ev.ReturnMessage = "Invalid argument amount.";
			}
			else if (cmdSplit[0] == "scplist")
			{
				if (cmdSplit.Count() == 1) this.plugin.SCPListCommand.OnCall(ev, new string[] { }); // List SCPs
				else ev.ReturnMessage = "Invalid argument amount";
			}
		}
	}
}
