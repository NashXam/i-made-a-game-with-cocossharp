using System;
using CocosSharp;

namespace TripleTriad.Shared
{
	public class GameStartLayer : CCLayerColor
	{
		public GameStartLayer () : base ()
		{
			var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesEnded = (touches, ccevent) => 
			{
				var scene = GameLayer.GameScene (Window);
				var transScene = new CCTransitionCrossFade(1f, scene);
				Window.DefaultDirector.ReplaceScene (transScene);
			};
				


			AddEventListener (touchListener, this);

			Color = CCColor3B.White;
			Opacity = 255;
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.ShowAll;

			var label = new CCLabel ("Welcome to Triple Triad", null, 50) {
				Position = VisibleBoundsWorldspace.Center,
				Color = CCColor3B.Black,
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Center,
				AnchorPoint = CCPoint.AnchorMiddle,
				Dimensions = ContentSize
			};

			AddChild (label);

		}


		public static GameScene GameStartLayerScene (CCWindow mainWindow)
		{
			var scene = new GameScene (mainWindow);
			var layer = new GameStartLayer ();

			scene.AddChild (layer);
			return scene;
		}
	}
}

