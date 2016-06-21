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
using DiscordProject.Modules;
using System.Timers;
using System.Reflection;

namespace DiscordProject
{

	class R2D2 : DiscordBot
	{
		public List<PatternResponse> responses = new List<PatternResponse>();

		public R2D2(JObject cfg)
		{
			Initialize(cfg);
		}

		public override void SetupConfig()
		{
			// Load Regex Responses
			foreach (JObject obj in (JArray)config["responses"]) {
				responses.Add(new PatternResponse((string)obj["pattern"], (string)obj["response"]));
			}

			// ADD ANY OTHER RESPONSES HERE

		}

		public override void SetupEvents()
		{
			// CONNECT EVENTS HERE:
			client.Connected += OnConnect;
			client.MessageReceived += OnMessageReceived;
		}

		public override void Start()
		{

		}


		// EVENTS //

		private void OnConnect(object sender, DiscordConnectEventArgs e)
		{
			// Insert connection-specific variables (like user info) into patterns
			for (int i = 0; i < responses.Count; i++) {
				responses[i].pattern = Regex.Replace(responses[i].pattern, "<bottag>", $"<@{e.User.ID}>");
				//Log("Response: " + responses[i].pattern);
			}
		}

		private void OnMessageReceived(object sender, DiscordMessageEventArgs e)
		{
			if (e.Author == client.Me) {
				return;
			}

			//Log($"{e.Author.Username}: {e.MessageText}");

			// R2D2: REGEX PATTERN MATCHING
			foreach (PatternResponse r in responses) {
				Match m = Regex.Match(e.MessageText, r.pattern, RegexOptions.IgnoreCase);
				if (m.Success) {
					Log($"Recieved message from @{e.Author.Username} that matched {r.pattern} in #{e.Channel.Name}");
					e.Channel.SendMessage(r.response);
					return;
				}
			}
		}

	}
}
