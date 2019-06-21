using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2.Config;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;

namespace SCPSwap
{
	[PluginDetails(
		author = "Spooky",
		name = "SCPSwap",
		description = "",
		id = "xyz.wizardlywonders.SCPSwap",
		version = "1.0.1",
		SmodMajor = 3,
		SmodMinor = 4,
		SmodRevision = 1
	)]
	public class SCPSwap : Plugin
	{
		// TODO: Fix swapping while teleporting as 106
		public List<SwapRequest> pendingSwaps = new List<SwapRequest>();
		public ConcurrentDictionary<string, int> playerSwapCounts = new ConcurrentDictionary<string, int>();

		internal AcceptSwap AcceptSwapCommand;
		internal RequestSwap RequestSwapCommand;
		internal SCPList SCPListCommand;

		public bool inSwapPeriod = true;

		public override void OnDisable() { }
		public override void OnEnable() { }

		public override void Register()
		{
			// Register config
			this.AddConfig(new ConfigSetting("scpswap_enable", true, true, "Whether SCPSwap should be enabled on server start."));
			this.AddConfig(new ConfigSetting("scpswap_timeperiod", 60, true, "Amount of time in seconds after round start that player can swap SCPs."));
			this.AddConfig(new ConfigSetting("scpswap_minhealth", 98, true, "Minimum health percentage required to be able to swap SCPs."));
			this.AddConfig(new ConfigSetting("scpswap_preservehealth", true, true, "Whether to preserve health percentage on a swap."));
			this.AddConfig(new ConfigSetting("scpswap_maxswaps", 1, true, "How many swaps can be done per player, per round."));
			this.AddConfig(new ConfigSetting("scpswap_disallowed_scps", new string[] { }, true, "SCP list that cannot be swapped to or from."));

			// Register events
			this.AddEventHandlers(new MiscEventHandler(this));
			this.AddEventHandlers(new CommandEvent(this));

			// Register commands
			this.AddCommand("scpswapdisable", new SCPSwapDisableCommand(this));
			this.AcceptSwapCommand = new AcceptSwap(this);
			this.RequestSwapCommand = new RequestSwap(this);
			this.SCPListCommand = new SCPList(this);
		}
	}

	public struct SwapRequest
	{
		public Player requester;
		public Player target;

		public SwapRequest(Player r, Player t)
		{
			this.requester = r;
			this.target = t;
		}
	}
}
