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
		public string path = Path.Combine("tshock", "tweetshock.json"); //Path.Combine turns this into ./tshock/tweetshock.json

		public ConfigLayout ReadConfigFile()
		{
			if (File.Exists(path)) //If the config file exists
			{
				//Create a TextReader, read the file, then close the file
				TextReader tr = new StreamReader(path);
				string pre = tr.ReadToEnd();
				tr.Close();
				return JsonConvert.DeserializeObject<ConfigLayout>(pre); //Return back a ConfigLayout object from the JSON object
			}
			return null; //Something went wrong, I don't care what, send back null
		}

		public void WriteConfigFile()
		{
			if (!File.Exists(path)) //If the config file doesn't exist yet
			{
				///Write one
				TextWriter tw = new StreamWriter(path);
				string json = JsonConvert.SerializeObject(new ConfigLayout(), Formatting.Indented); //Write a templated ConfigLayout
				tw.Write(json);
				tw.Close();
			}
		}

		public void WriteConfigFile(ConfigLayout config)
		{
			if (File.Exists(path)) //If the config file doesn't exist yet
			{
				///Write one
				TextWriter tw = new StreamWriter(path);
				string json = JsonConvert.SerializeObject(config, Formatting.Indented); //Write a templated ConfigLayout
				tw.Write(json);
				tw.Close();
			}
		}
	}

	class ConfigLayout //Simple class that will be written to the JSON file using Newtonsoft's JsonConvert.SerializeObject method
	{
		public string AccessToken = "";
		public string AccessTokenSecret = "";
		public string ConsumerKey = "";
		public string ConsumerSecret = "";
		public bool EnableJoinHook = true;
		public bool EnableStartHook = true;
		public bool EnableLeaveHook = true;
		public string LeaveTemplate = "Player {ply} has left the server!";
		public string JoinTemplate = "Player {ply} has joined the server!";
		public string StartTemplate = "The server is now running on {ip}:{port}";
	}
}
