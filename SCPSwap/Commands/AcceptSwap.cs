using Smod2;
using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SCPSwap
{
	class AcceptSwap : IGameConsoleCommand
	{
		private readonly SCPSwap plugin;

		public AcceptSwap(SCPSwap plugin) => this.plugin = plugin;

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

			// -- Fetch any requests targetting command caller
			List<SwapRequest> applicablePending = this.plugin
				.pendingSwaps
				.Where(x => x.target.SteamId == ev.Player.SteamId)
				.ToList();

			Player target = null;
			foreach (SwapRequest sr in applicablePending)
			{
				target = curPlayers.Where(x => x.SteamId == sr.requester.SteamId).FirstOrDefault();

				// If person making the request is still on the server, go ahead
				if (target != null) break;
			}

			if (target == null)
			{
				ev.ReturnMessage = "You don't have any pending requests!";
			}
			else
			{
				Role tmp = ev.Player.TeamRole.Role;

				ev.Player.ChangeRole(target.TeamRole.Role);
				target.ChangeRole(tmp);

				ev.ReturnMessage = "You've swapped SCPs with " + target.Name + " successfully!";
			}
		}
	}
}