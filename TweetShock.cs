using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using TShockAPI;
using Terraria;
using Twitterizer;

namespace TweetShock
{
	[APIVersion(1,11)]
	public class TweetShock : TerrariaPlugin
	{
		public TweetShock(Main game) : base(game)
		{
			Order = 100;
		}

		private OAuthTokens tokens = new OAuthTokens();
		private ConfigFile config = new ConfigFile();
		private ConfigLayout configFile = new ConfigLayout();
		public override Version Version
		{
			get { return new Version(1, 0); }
		}

		public override string Author
		{
			get { return "nicatronTg"; }
		}

		public override string Name
		{
			get { return "TweetShock"; }
		}

		public override string Description
		{
			get { return "Because if it doesn't run Twitter, we don't care."; }
		}

		public override void Initialize()
		{
			config.WriteConfigFile();
			configFile = config.ReadConfigFile();
			Console.WriteLine("Welcome to TweetShock. Initial configuration read OK. Verify that you've set all app keys before continuing.");

			tokens.AccessToken = configFile.AccessToken;
			tokens.AccessTokenSecret = configFile.AccessTokenSecret;
			tokens.ConsumerKey = configFile.ConsumerKey;
			tokens.ConsumerSecret = configFile.ConsumerSecret;

			Hooks.ServerHooks.Connect += TweetJoin;
			Hooks.GameHooks.PostInitialize += GameHooks_PostInitialize;
		}

		void GameHooks_PostInitialize()
		{
			WebClient wc = new WebClient();
			string ip = wc.DownloadString("http://whatismyip.org/");

			SendTweet("Terraria up @ " + ip + ":" + Netplay.serverPort);
		}

		void TweetJoin(int id, HandledEventArgs args)
		{
			// Spammy, don't do this, please.
			// SendTweet("Player " + Main.player[id].name + " joined the server.");
			// Console.WriteLine("Player: " + Main.player[id].name);
		}

		bool SendTweet(string msg)
		{
			TwitterResponse<TwitterStatus> tw = TwitterStatus.Update(tokens, msg);
			if (tw.Result == RequestResult.Success)
			{
				Console.WriteLine("Twitter: Tweet posted successfully.");
				return true;
			}
			else
			{
				Console.WriteLine("Twitter: Something went wrong: ");
				Console.WriteLine(tw.ErrorMessage);
				return false;
			}
		}
	}
}
