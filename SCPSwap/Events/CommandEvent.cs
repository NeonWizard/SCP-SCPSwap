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

		private readonly Dictionary<int, Role> SCPIDMap = new Dictionary<int, Role>
		{
			{ 49, Role.SCP_049 },
			{ 79, Role.SCP_079 },
			{ 96, Role.SCP_096 },
			{ 106, Role.SCP_106 },
			{ 173, Role.SCP_173 }
		};

		public CommandEvent(SCPSwap plugin)
		{
			this.plugin = plugin;
		}

		public void OnCallCommand(PlayerCallCommandEvent ev)
		{
			// -- Check for SWAPSCP command
			string command = ev.Command.ToLower();
			if (!command.StartsWith("swapscp")) return;

			// -- Validate player is an SCP
			// TODO: add configuration to allow non-SCPs to run scpswap
			if (ev.Player.TeamRole.Team != Smod2.API.Team.SCP)
			{
				ev.ReturnMessage = "You are not an SCP!";
				return;
			}

			// -- Validate argument count
			string[] cmdSplit = command.Split(' ');
			if (cmdSplit.Count() != 2)
			{
				ev.ReturnMessage = "Invalid number of arguments.";
				return;
			}

			// -- Parse SCP integer argument & ensure it's a valid SCP number
			if (!Int32.TryParse(ev.Command.ToLower().Split(' ')[1], out int num))
			{
				ev.ReturnMessage = "Invalid argument.";
				return;
			}
			else
			{
				int[] validSCPs = new int[] { 49, 79, 96, 106, 173, 939 };
				if (!validSCPs.Contains(num)) {
					ev.ReturnMessage = "The specified SCP does not exist.";
					return;
				}
			}

			// -- Attempt to find SCPs already with this role
			List<Player> targets = new List<Player>();
			foreach (Player p in this.plugin.Server.GetPlayers())
			{
				// -- Special case for different 939s
				if (num == 939 && (p.TeamRole.Role == Role.SCP_939_53 || p.TeamRole.Role == Role.SCP_939_89))
				{
					targets.Add(p);
				}
				// -- Someone has this role, send a swap request
				else if (this.SCPIDMap[num] == p.TeamRole.Role)
				{
					targets.Add(p);
				}
			}

			// -- Send swap request to target(s)
			if (targets.Count() > 0)
			{
				foreach (Player target in targets)
				{
					target.PersonalBroadcast(7, ev.Player.Name + " (" + ev.Player.TeamRole.Name + ") wants to swap SCPs with you.", false);
				}
			}
			// -- Otherwise, just swap right to it :)
			else
			{
				ev.ReturnMessage = "Swapped SCPs successfully! :)";
				ev.Player.ChangeRole(this.SCPIDMap[num]);
			}
		}
	}
}
