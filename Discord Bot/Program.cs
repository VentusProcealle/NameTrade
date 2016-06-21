using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace DiscordProject
{
	// Basically a launcher.
	class Program
	{
		public static Random rand = new Random();
		public static DateTime startTime;

		static void Main(string[] args)
		{
			startTime = DateTime.Now;
			Console.Title = "Discord Bot";

			string configFile = "";
			try {
				configFile = File.ReadAllText("config.json");
			} catch (IOException e) {
				Console.WriteLine("Error: Failed to find config.json.");
				return;
			}

			/*	Escaping with Regex and JSON: (LITERALLY THE WORST)
					The replacement only has one layer of escaping, the pattern has two,
					so @"\\" as the pattern is "\" and @"\\" as the response is "\\".
			*/
			JObject config = JObject.Parse(Regex.Replace(File.ReadAllText("config.json"), @"\\", @"\\"));

			R2D2 bot = new R2D2(config);

			Console.ReadKey(true);
		}

		public static void Log(string message)
		{
			Console.WriteLine("[{0:HH:mm:ss}] " + message, DateTime.Now);
		}
	}
}
