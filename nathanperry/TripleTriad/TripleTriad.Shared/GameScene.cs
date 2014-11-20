using System;
using CocosSharp;

namespace TripleTriad.Shared
{
	public class GameScene : CCScene
	{
		readonly CCLayer mainLayer;

		public GameScene(CCWindow mainWindow) : base(mainWindow)
		{
			mainLayer = new CCLayer ();
			AddChild (mainLayer);
		}
	}
}

