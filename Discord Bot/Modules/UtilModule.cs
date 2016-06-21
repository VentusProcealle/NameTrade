using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using DiscordSharp.Commands;
using DiscordSharp;
using System.Data;
using NCalc;
using System.Security.Cryptography;
using System.IO;
using DiscordSharp.Objects;

namespace DiscordProject.Modules
{
	class UtilModule : IModule
	{
		public UtilModule(DiscordBot bot) : base(bot)
		{
			Name = "util";
		}

		public override void Install(CommandsManager manager)
		{
			manager.AddCommand(new CommandStub(
				"math", 
				"", 
				"<expression>", 
				PermissionType.User, 
				1, 
			cmd => {
				Expression e = new Expression(cmd.Args[0]);
				try {
					cmd.Channel.SendMessage("Result: " + e.Evaluate().ToString());
				} catch (Exception ex) {
					cmd.Channel.SendMessage("Error: " + ex.Message);
				}

			}), this);

			manager.AddCommand(new CommandStub(
				name: "aes128.encrypt",
				description: "",
				helpTag: "<text> <key>",
				minPerm: PermissionType.User,
				argCount: 2,
			action: cmd => {
				cmd.Channel.SendMessage("Encrypted: " + AES128_Encrypt(cmd.Args[0], cmd.Args[1]));
			}), this);

			manager.AddCommand(new CommandStub(
				name: "aes128.decrypt",
				description: "",
				helpTag: "<text> <key>",
				minPerm: PermissionType.User,
				argCount: 2,
			action: cmd => {
				cmd.Channel.SendMessage("Decrypted: " + AES128_Decrypt(cmd.Args[0], cmd.Args[1]));
			}), this);

			manager.AddCommand(new CommandStub(
				name: "convert",
				description: "Can convert to and from: text, base64",
				helpTag: "<from> <to> <payload>",
				minPerm: PermissionType.User,
				argCount: 3,
			action: cmd => {
				cmd.Channel.SendMessage(ConvertInput(cmd.Args[0], cmd.Args[1], cmd.Args[2]));
			}), this);

			manager.AddCommand(new CommandStub(
				name: "pastebin",
				description: "",
				helpTag: "",
				minPerm: PermissionType.User,
				argCount: 2,
			action: cmd => {
				WebClient client = new WebClient();
				string text = client.DownloadString("http://pastebin.com/raw/" + cmd.Args[0]);
				if (cmd.Args.Count > 1) {
					cmd.Channel.SendMessage("```" + cmd.Args[1] + "\n" + text + "```");
				} else {
					cmd.Channel.SendMessage("```\n" + text + "```");
				}

			}), this);
        }

		public static string ConvertInput(string from, string to, string payload)
		{
			if (from == "text") {
				
			} else if (from == "base64") {
				payload = Encoding.Default.GetString(Convert.FromBase64String(payload));
			}

			if (to == "base64") {
				return Convert.ToBase64String(Encoding.Default.GetBytes(payload));
			}
			
			if (to == "text") {
				return payload;
			}

			return "ERROR";
		}

		public static string AES128_Encrypt(string text, string key)
		{
			byte[] clearBytes = Encoding.Unicode.GetBytes(text);
			using (Aes encryptor = Aes.Create()) {
				Rfc2898DeriveBytes pdb = new
					Rfc2898DeriveBytes(key, new byte[]
					{ 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream()) {
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)) {
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					text = Convert.ToBase64String(ms.ToArray());
				}
			}
			return text;
		}

		public static string AES128_Decrypt(string text, string key)
		{
			byte[] bytes = Convert.FromBase64String(text);
			using (Aes encryptor = Aes.Create()) {
				Rfc2898DeriveBytes pdb = new
					Rfc2898DeriveBytes(key, new byte[]
					{ 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream()) {
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)) {
						cs.Write(bytes, 0, bytes.Length);
						cs.Close();
					}
					text = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return text;
		}

	}
}
