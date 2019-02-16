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
				// -- Make sure both players are still SCPs
				if (target.TeamRole.Team != Smod2.API.Team.SCP || ev.Player.TeamRole.Team != Smod2.API.Team.SCP)
				{
					ev.ReturnMessage = "One of the players in the swap request is no longer an SCP.";
					return;
				}

				// -- Change roles
				if (this.plugin.GetConfigBool("scpswap_preservehealth"))
				{
					int health1 = ev.Player.GetHealth();
					int health2 = target.GetHealth();

					Role tmp = ev.Player.TeamRole.Role;

					ev.Player.ChangeRole(target.TeamRole.Role);
					target.ChangeRole(tmp);

					ev.Player.SetHealth(health2);
					target.SetHealth(health1);
				}
				else
				{
					Role tmp = ev.Player.TeamRole.Role;

					ev.Player.ChangeRole(target.TeamRole.Role);
					target.ChangeRole(tmp);
				}

				// -- Remove any pending swaps involving either players in the accepted swap
				this.plugin.pendingSwaps = this.plugin.pendingSwaps.Where(sr =>
					sr.requester.SteamId != ev.Player.SteamId &&
					sr.requester.SteamId != target.SteamId &&
					sr.target.SteamId != ev.Player.SteamId &&
					sr.target.SteamId != target.SteamId
				).ToList();

				// -- Keep track of how many times they've swapped this round
				this.plugin.playerSwapCounts.AddOrUpdate(ev.Player.SteamId, 1, (id, count) => count + 1);

				ev.ReturnMessage = "You've swapped SCPs with " + target.Name + " successfully!";
			}
		}
	}
}
