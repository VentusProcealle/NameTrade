using DiscordSharp.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace DiscordProject.Modules
{
	public class DankMemesModule : IModule
	{
		Timer cooldown = new Timer(10000);

		private Dictionary<string, string> copypastas = new Dictionary<string, string>();

		public DankMemesModule(DiscordBot bot) : base(bot)
		{
			Name = "dank_memes";
			Description = "";

			foreach(FileInfo f in new DirectoryInfo("copypastas").GetFiles("*.txt")) {
				copypastas.Add(f.Name.Remove(f.Name.Length - 4, 4), File.ReadAllText(f.FullName));
			}

			AddAlias("gorilla warfare", "navy seals");
			AddAlias("navy seal", "navy seals");

			cooldown.Elapsed += (sender, args) => { cooldown.Enabled = false; };
		}

		private void Cooldown_Elapsed(object sender, ElapsedEventArgs e)
		{
			throw new NotImplementedException();
		}

		public override void Install(CommandsManager manager)
		{
			manager.AddCommand(new CommandStub("copypasta", "", "", PermissionType.User, 1, cmd =>
			{
				if (cooldown.Enabled) {
					return;
				}

				if (cmd.Args.Count < 1) {
					cmd.Channel.SendMessage(copypastas.ElementAt(Program.rand.Next(0, copypastas.Count)).Value);
					cooldown.Enabled = true;
				} else if (cmd.Args.Count > 0 && copypastas.ContainsKey(cmd.Args[0].ToLower().Trim())) {
					cmd.Channel.SendMessage(copypastas[cmd.Args[0]]);
					cooldown.Enabled = true;
				} else {
					cmd.Channel.SendMessage("Error: \"" + cmd.Args[0] + "\" is not a known copypasta.");
				}

			}), this);
		}

		private void AddAlias(string alias, string key)
		{
			if (copypastas.ContainsKey(key))
				copypastas.Add(alias, copypastas[key]);
		}
	}
}
