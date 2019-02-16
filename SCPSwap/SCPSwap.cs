using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2.Config;
using System.Collections.Generic;
using System;

namespace SCPSwap
{
	[PluginDetails(
		author = "Spooky",
		name = "SCPSwap",
		description = "",
		id = "xyz.wizardlywonders.SCPSwap",
		version = "1.2.0",
		SmodMajor = 3,
		SmodMinor = 2,
		SmodRevision = 2
	)]
	public class SCPSwap : Plugin
	{
		public List<SwapRequest> pendingSwaps = new List<SwapRequest>();

		internal AcceptSwap AcceptSwapCommand;
		internal RequestSwap RequestSwapCommand;

		public override void OnDisable()
		{
			this.Info("SCPSwap has been disabled.");
		}

		public override void OnEnable()
		{
			this.Info("SCPSwap has loaded successfully.");
		}

		public override void Register()
		{
			// Register config
			this.AddConfig(new ConfigSetting("scpswap_enable", true, SettingType.BOOL, true, "Whether SCPSwap should be enabled on server start."));

			// Register events
			this.AddEventHandlers(new MiscEventHandler(this));
			this.AddEventHandlers(new CommandEvent(this));

			// Register commands
			this.AddCommand("scpswapdisable", new SCPSwapDisableCommand(this));
			this.AcceptSwapCommand = new AcceptSwap(this);
			this.RequestSwapCommand = new RequestSwap(this);
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
