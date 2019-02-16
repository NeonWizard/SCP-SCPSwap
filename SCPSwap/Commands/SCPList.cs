using Smod2;
using Smod2.API;
using Smod2.Events;
using Smod2.EventHandlers;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SCPSwap
{
	class SCPList : IGameConsoleCommand
	{
		private readonly SCPSwap plugin;

		public SCPList(SCPSwap plugin) => this.plugin = plugin;

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
			List<string> output = new List<string> { "Listing current SCPs..." };
			foreach (Player p in this.plugin.Server.GetPlayers())
			{
				if (p.TeamRole.Team == Smod2.API.Team.SCP)
				{
					output.Add("[" + ev.Player.TeamRole.Name + "] " + ev.Player.Name);
				}
			}

			ev.ReturnMessage = string.Join("\n", output.ToArray());
		}
	}
}
