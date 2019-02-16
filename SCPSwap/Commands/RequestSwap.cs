using Smod2;
using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Collections.Generic;
using System.Linq;
using System;


namespace SCPSwap
{
	class RequestSwap : IGameConsoleCommand
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

		public RequestSwap(SCPSwap plugin) => this.plugin = plugin;

		public string GetCommandDescription()
		{
			throw new NotImplementedException();
		}

		public string GetUsage()
		{
			throw new NotImplementedException();
		}

		public void OnCall(PlayerCallCommandEvent ev, string[] args)
		{
			List<Player> curPlayers = this.plugin.Server.GetPlayers();

			// -- Parse SCP integer argument & ensure it's a valid SCP number
			if (!Int32.TryParse(args[0], out int num))
			{
				ev.ReturnMessage = "Invalid argument.";
				return;
			}
			else
			{
				int[] validSCPs = new int[] { 49, 79, 96, 106, 173, 939 };
				if (!validSCPs.Contains(num))
				{
					ev.ReturnMessage = "The specified SCP does not exist.";
					return;
				}
			}

			// -- Attempt to find SCPs already with this role
			List<Player> targets = new List<Player>();
			foreach (Player p in curPlayers)
			{
				if (p.SteamId == ev.Player.SteamId) continue; // can't swap with yourself silly

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
				List<string> output = new List<string>();
				foreach (Player target in targets)
				{
					target.PersonalBroadcast(
						10,
						"<color=#eee><color=#eeee99>"
							+ ev.Player.Name +
						"</color> (<color=#ff9999>"
							+ ev.Player.TeamRole.Name +
						"</color>) wants to swap SCPs with you."
							+ "\n" +
						"Type <color=#eeeeaa>.SCPSWAP</color> in <color=#bbaaff>GameConsole</color> to accept.</color>",
						false
					);
					this.plugin.pendingSwaps.Add(new SwapRequest(ev.Player, target));
					output.Add("Sent a swap request to: " + target.Name);
				}

				ev.ReturnMessage = string.Join("\n", output.ToArray());
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
