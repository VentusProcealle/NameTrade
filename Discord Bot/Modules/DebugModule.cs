using DiscordSharp.Commands;
using DiscordSharp.Objects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordProject.Modules
{
	public class DebugModule : IModule
	{

		public DebugModule(DiscordBot bot) : base(bot)
		{
			Name = "debug";
			Description = "";
		}

		public override void Install(CommandsManager manager)
		{
			manager.AddCommand(new CommandStub("debug.me.id", "", "", cmd => {
				cmd.Channel.SendMessage(cmd.Author.ID);
			}));

			manager.AddCommand(new CommandStub("debug.me.roles", "", "", cmd => {
				cmd.Channel.SendMessage(FormatList(cmd.Author.Roles, x => x.Name));
			}));

			manager.AddCommand(new CommandStub("debug.modules", "", "", cmd => {
				List<IModule> modules = bot.commandsManager.Modules.Keys.ToList();
				cmd.Channel.SendMessage(FormatList(modules, x => x.Name));
			}));

			manager.AddCommand(new CommandStub("debug.commands", "", "", cmd => {
				cmd.Channel.SendMessage(FormatList(bot.commandsManager.Commands, x => x.CommandName));
			}));

			manager.AddCommand(new CommandStub("debug.servers", "", "", cmd => {
				List<DiscordServer> servers = bot.client.GetServersList();
				cmd.Channel.SendMessage(FormatList(servers, x => x.Name));
			}));

			
		}

		internal static string FormatList<T>(List<T> list, Func<T, string> toString)
		{
			string message = "`" + toString(list[0]);

			for (int i = 1; i < list.Count; i++) {
				message += ", " + toString(list[i]);
			}

			return message + "`";
		}
	}
}
