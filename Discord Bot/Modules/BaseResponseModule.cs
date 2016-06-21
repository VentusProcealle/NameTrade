using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordSharp.Commands;
using Newtonsoft.Json.Linq;

namespace DiscordProject.Modules
{
	class ResponseModule
	{
		public JArray responses;

		public ResponseModule(JArray botResponses)
		{
			this.responses = botResponses;
		}

		public void Install(ref JObject config)
		{
			if(config["responses"] != null) {
				config["responses"] = new JArray(config["responses"].Union(responses));
			}
		}
	}
}
