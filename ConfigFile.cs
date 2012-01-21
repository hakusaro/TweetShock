using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TweetShock
{
	class ConfigFile
	{
		public string path = Path.Combine("tshock", "tweetshock.json");

		public ConfigLayout ReadConfigFile()
		{
			if (File.Exists(path))
			{
				TextReader tr = new StreamReader(path);
				string pre = tr.ReadToEnd();
				tr.Close();
				return JsonConvert.DeserializeObject<ConfigLayout>(pre);
			}
			return null;
		}

		public void WriteConfigFile()
		{
			if (!File.Exists(path))
			{
				TextWriter tw = new StreamWriter(path);
				string json = JsonConvert.SerializeObject(new ConfigLayout());
				tw.Write(json);
				tw.Close();
			}
		}
	}

	class ConfigLayout
	{
		public string AccessToken = "";
		public string AccessTokenSecret = "";
		public string ConsumerKey = "";
		public string ConsumerSecret = "";
	}
}
