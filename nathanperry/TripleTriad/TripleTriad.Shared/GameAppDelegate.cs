using System;
using CocosSharp;
using TripleTriad.Shared.GameLogic.Cards;
using TripleTriad.Shared.GameLogic;

namespace TripleTriad.Shared
{
	public class GameAppDelegate : CCApplicationDelegate
	{
		public static BlueCard[] BlueCards
		{
			get;
			private set;
		}

		public static RedCard[] RedCards
		{
			get;
			private set;
		}

		public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
		{
			application.PreferMultiSampling = false;
			application.ContentRootDirectory = "Content";

			CCSize winSize = mainWindow.WindowSizeInPixels;
			mainWindow.SetDesignResolutionSize(winSize.Width, winSize.Height, CCSceneResolutionPolicy.ExactFit);

			InitializeCards();

			//Game Initializer
			GameScene scene = GameStartLayer.GameStartLayerScene(mainWindow);
			mainWindow.RunWithScene (scene);
		}

		public override void ApplicationDidEnterBackground (CCApplication application)
		{
			// stop all of the animation actions that are running.
			application.Paused = true;
		}

		public override void ApplicationWillEnterForeground (CCApplication application)
		{
			application.Paused = false;
		}

		private void InitializeCards()
		{
			BlueCards = new BlueCard[11];
			RedCards = new RedCard[11];

			InitCards(0, "geezard", Elements.NONE, 1, 4, 1, 5);
			InitCards(1, "funguar", Elements.NONE, 5, 1, 1, 3);
			InitCards(2, "bitebug", Elements.NONE, 1, 3, 3, 5);
			InitCards(3, "redbat", Elements.NONE, 6, 1, 1, 2);
			InitCards(4, "blobra", Elements.NONE, 2, 3, 1, 5);
			InitCards(5, "gayla", Elements.THUNDER, 2, 1, 4, 4);
			InitCards(6, "gesper", Elements.NONE, 1, 5, 4, 1);
			InitCards(7, "fastitocalonf", Elements.EARTH, 3, 5, 2, 1);
			InitCards(8, "bloodsoul", Elements.NONE, 2, 1, 6, 1);
			InitCards(9, "caterchipillar", Elements.NONE, 4, 2, 4, 3);
			InitCards(10, "cockatrice", Elements.THUNDER, 2, 1, 2, 6);
		}

		private void InitCards(int id, string name, Elements element, int top, int right, int bottom, int left )
		{
			BlueCards[id] = new BlueCard(id, name, element, top, right, bottom, left);
			RedCards[id] = new RedCard(id, name, element, top, right, bottom, left);
		}
	}
}