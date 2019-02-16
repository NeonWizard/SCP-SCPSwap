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
			// TODO: add configuration to allow non-SCPs to run scpswap
			if (ev.Player.TeamRole.Team != Smod2.API.Team.SCP)
			{
				ev.ReturnMessage = "You are not an SCP!";
				return;
			}

			// -- Execute respective commands
			string[] cmdSplit = command.Split(' ');
			if (cmdSplit[0] == "scpswap")
			{
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
