/*
 * Created as an example of a plugin.
 * 
 * Author: nicatronTg
 * 
 * Difficulty: Advanced
 */

/* This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2, as published by Sam Hocevar. See
 * http://sam.zoy.org/wtfpl/COPYING for more details. */

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
	[APIVersion(1,11)] //Runs on API version 1.11
	public class TweetShock : TerrariaPlugin
	{
		public TweetShock(Main game) : base(game)
		{
			Order = 100; //Start after TShock, order 0
		}

		private OAuthTokens tokens = new OAuthTokens(); //Create some OAuth tokens to talk to Twitter
		private ConfigFile config = new ConfigFile(); //Create a config file reader
		private ConfigLayout configFile = new ConfigLayout(); //Create a stub class for holding the configuration options

		public override Version Version
		{
			get { return new Version(1, 0); } //Version 1.0
		}

		public override string Author
		{
			get { return "nicatronTg"; } //Made by nicatronTg
		}

		public override string Name
		{
			get { return "TweetShock"; } //Corny name
		}

		public override string Description
		{
			get { return "Because if it doesn't run Twitter, we don't care."; } //Do we ever even use the description of a plugin?
		}

		public override void Initialize() //First method TShock runs when the plugin is loaded
		{
			config.WriteConfigFile(); //Write the initial config file first, in the event that it doesn't exist.
			configFile = config.ReadConfigFile(); //Read the configuration file and store it in our previously created stub class
			Console.WriteLine("Welcome to TweetShock. Initial configuration read OK. Verify that you've set all app keys before continuing."); //Output some helper text

			/*
			 * Map each token to each config option
			 */
			tokens.AccessToken = configFile.AccessToken;
			tokens.AccessTokenSecret = configFile.AccessTokenSecret;
			tokens.ConsumerKey = configFile.ConsumerKey;
			tokens.ConsumerSecret = configFile.ConsumerSecret;

			Hooks.ServerHooks.Connect += TweetJoin; //Hook when a player joins
			Hooks.GameHooks.PostInitialize += GameHooks_PostInitialize; //Hook when the map loads, directly after you see the console
		}

		void GameHooks_PostInitialize()
		{
			if (!configFile.EnableStartHook)
				return;
			WebClient wc = new WebClient(); //Create a web request
			string ip = wc.DownloadString("http://whatismyip.org/"); //Download the server's public IP from whatismyip.org using the web client.

			SendTweet("Terraria up @ " + ip + ":" + Netplay.serverPort); //Call the SendTweet method with our message.
		}

		void TweetJoin(int id, HandledEventArgs args)
		{
			// Spammy, don't do this, please.
			// Sends a tweet on a player join event
			SendTweet("Player " + Main.player[id].name + " joined the server.");
			Console.WriteLine("Player: " + Main.player[id].name);
		}

		bool SendTweet(string msg)
		{
			TwitterResponse<TwitterStatus> tw = TwitterStatus.Update(tokens, msg); //Create a new TwitterResponse by calling the Update function from TwitterStatus, part of Twitterizer
			if (tw.Result == RequestResult.Success) //If it turns out that Twitter posted the tweet...
			{
				Console.WriteLine("Twitter: Tweet posted successfully."); //Tell the user that it posted good
				return true; //Return back true
			}
			else
			{
				Console.WriteLine("Twitter: Something went wrong: "); //Someone botched the API keys or something
				Console.WriteLine(tw.ErrorMessage); //Send back the error message
				return false; //Return back false
			}
		}
	}
}
