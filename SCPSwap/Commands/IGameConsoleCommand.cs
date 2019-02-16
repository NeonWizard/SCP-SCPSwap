using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smod2.API;
using Smod2.Events;

namespace SCPSwap
{
	public interface IGameConsoleCommand
	{
		string GetCommandDescription();
		string GetUsage();
		void OnCall(PlayerCallCommandEvent ev, string[] args);
	}
}
