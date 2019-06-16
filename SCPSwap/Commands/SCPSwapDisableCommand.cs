using Smod2.Commands;
using Smod2;
using Smod2.API;
using System.IO;


namespace SCPSwap
{
	class SCPSwapDisableCommand : ICommandHandler
	{
		private SCPSwap plugin;

		public SCPSwapDisableCommand(SCPSwap plugin)
		{
			this.plugin = plugin;
		}

		public string GetCommandDescription()
		{
			return "Disables SCPSwap";
		}

		public string GetUsage()
		{
			return "SCPSWAPDISABLE";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			plugin.Info(sender + " ran the " + GetUsage() + " command!");
			this.plugin.PluginManager.DisablePlugin(this.plugin);
			return new string[] { "SCPSwap Disabled" };
		}
	}
}
