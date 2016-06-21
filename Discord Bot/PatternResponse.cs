using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordProject
{
	class PatternResponse
	{
		public string pattern;
		public string response;

		public PatternResponse(string pattern, string response)
		{
			this.pattern = pattern;
			this.response = response;
		}
	}
}
