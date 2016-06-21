using DiscordSharp.Commands;
using DiscordSharp.Objects;
using DiscordSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DiscordSharp.Events;

namespace DiscordProject.Modules
{
	public class CoreModule : IModule
	{
		public CoreModule(DiscordBot bot) : base(bot)
		{
			Name = "core";
			Description = "";

			bot.client.Connected += OnConnected;
		}

		public override void Install(CommandsManager manager)
		{

			manager.AddCommand(new CommandStub("help", "", "", PermissionType.None, 1, cmd => {

				if (cmd.Args.Count == 0) {
					cmd.Channel.SendMessage($"Use `{bot.commandPrefix}help <command>` for more info.");
					return;
				} 

				foreach (ICommand c in bot.commandsManager.Commands) {
					if (c.CommandName == cmd.Args[0]) {
						cmd.Channel.SendMessage("\nUsage: `" + c.CommandName + " " + c.HelpTag + "`\nDescription: " + c.Description);
						return;
					}
				}
			}));

			manager.AddCommand(new CommandStub("stop", "", "", PermissionType.Owner, 0, cmd => {

				Environment.Exit(0);

			}));

			manager.AddCommand(new CommandStub(
				"echo",
				"",
				"",
				PermissionType.None,
				1,
			cmd => {
				cmd.Channel.SendMessage(cmd.Args[0]);
			}), this);
		}

		private void OnConnected(object sender, DiscordConnectEventArgs e)
		{
			Program.Log($"Connected as {e.User.Username} (#{e.User.Discriminator})");
			bot.client.UpdateCurrentGame("Half-Life 3");
			
		}
	}
}
