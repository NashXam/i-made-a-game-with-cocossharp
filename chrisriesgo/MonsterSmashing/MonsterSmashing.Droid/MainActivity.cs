using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.Xna.Framework;
using MonsterSmashing.Shared;
using CocosSharp;
using Android.Content.PM;

namespace MonsterSmashing.Droid
{
	[Activity(Label = "Monster Smashing", 
		MainLauncher = true, 
		Icon = "@drawable/icon", 
		Theme = "@android:style/Theme.NoTitleBar",
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : AndroidGameActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			var application = new CCApplication();
			application.ApplicationDelegate = new MonsterSmashingAppDelegate();
			SetContentView(application.AndroidContentView);
			application.StartGame();
		}
	}
}


