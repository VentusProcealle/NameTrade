using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DiscordSharp;
using DiscordSharp.Objects;
using DiscordSharp.Events;
using DiscordSharp.Commands;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace DiscordProject
{
	// Basic abstract Discord bot with commands.
	public abstract class DiscordBot
	{
		public JObject config;
		public DiscordClient client;
		public CommandsManager commandsManager;

		public string commandPrefix;

		protected void Initialize(JObject cfg)
		{
			Log("Initializing...");

			config = cfg;

			client = new DiscordClient((string)config["token"]);
			//client.RequestAllUsersOnStartup = true;

			commandPrefix = (string)config["command_prefix"];

			SetupConfig();

			commandsManager = new CommandsManager(client);
			commandsManager.AddPermission((string)config["owner"], PermissionType.Owner);

			client.MessageReceived += _MessageReceived;

			/*
				How Modules Are Loaded: 
					This finds every type that extends IModules and installs it.
					TODO: Config file lists modules
					TODO: Modules could be changed to static classes now
			*/

			var modules = from m in Assembly.GetExecutingAssembly().GetTypes()
						  where m.BaseType == typeof(IModule)
						  select m;

			foreach (Type m in modules) {
				commandsManager.Install((IModule)m.GetConstructor(new Type[] { typeof(DiscordBot) }).Invoke(new object[] { this }));
			}


			SetupEvents();

			Log("Connecting...");

			if (client.SendLoginRequest() != null)
				client.Connect();

			Start();
		}

		// Passes commands to manager when recieved.
		private void _MessageReceived(object Sender, DiscordMessageEventArgs e)
		{
			if (e.Author == client.Me) {
				if (e.MessageText.Length > 128) {
					Log($"Sent: {e.MessageText.Substring(0, 128)}...");
				} else {
					Log($"Sent: {e.MessageText}");
				}
				return;
			}

			if (e.MessageText.StartsWith(commandPrefix)) {
				string rawCommand = e.MessageText.Substring(commandPrefix.Length);
				Log($"Received Command {rawCommand}");

				try {
					commandsManager.ExecuteOnMessageCommand(rawCommand, e.Channel, e.Author);
				}
				catch (Exception ex) {
					Log(ex.Message);
					return;
				}
			}
		}

		public abstract void SetupConfig();	// Handles Config
		public abstract void SetupEvents();	// Handles Event Callbacks
		public abstract void Start();       // Handles Post-Connection

		protected static void Log(string message)
		{
			Program.Log(message);
		}

	}
}
