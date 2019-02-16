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
				if (target.TeamRole.Team != Smod2.API.Team.SCP || ev.Player.TeamRole.Team != Smod2.API.Team.SCP)
				{
					ev.ReturnMessage = "One of the players in the swap request is no longer an SCP.";
					return;
				}

				Role tmp = ev.Player.TeamRole.Role;

				ev.Player.ChangeRole(target.TeamRole.Role);
				target.ChangeRole(tmp);

				this.plugin.pendingSwaps = this.plugin.pendingSwaps.Where(sr =>
					sr.requester.SteamId != ev.Player.SteamId &&
					sr.requester.SteamId != target.SteamId &&
					sr.target.SteamId != ev.Player.SteamId &&
					sr.target.SteamId != target.SteamId
				).ToList();

				ev.ReturnMessage = "You've swapped SCPs with " + target.Name + " successfully!";
			}
		}
	}
}
