using System;
using CocosSharp;

namespace TripleTriad.Shared.Layers
{
	public class GameOverLayer : CCLayerColor
	{
		private int _redScore;
		private int _blueScore;


		private string _redString;
		private string _blueString;
		private string _winningString;

		public GameOverLayer (int redScore, int blueScore)
		{
			Color = new CCColor3B (CCColor4B.Black);
			Opacity = 255;

			_redScore = redScore+1;
			_blueScore = blueScore;

			if(_redScore > _blueScore)
				_winningString = "RED TEAM WINS!";
			else if(_blueScore > _redScore)
				_winningString = "BLUE TEAM WINS!";
			else
				_winningString = "IT'S A TIE!";

			_redString = String.Format("Red Team: {0}", _redScore);
			_blueString = String.Format("Blue Team: {0}", _blueScore);

			var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesEnded = HandleTouchEnded;
			AddEventListener (touchListener, this);

		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.ShowAll;

			var scoreLabel = new CCLabel (_winningString, null, 100) {
				Position = new CCPoint (VisibleBoundsWorldspace.Size.Center.X, VisibleBoundsWorldspace.Size.Center.Y + 400),
				Color = new CCColor3B (CCColor4B.Yellow),
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Center,
				AnchorPoint = CCPoint.AnchorMiddle,
				Dimensions = ContentSize
			};

			AddChild (scoreLabel);

			var redLabel = new CCLabel (_redString, null, 75) {
				Position = new CCPoint (VisibleBoundsWorldspace.Size.Center.X, VisibleBoundsWorldspace.Size.Center.Y + 200),
				Color = new CCColor3B (CCColor4B.Red),
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Center,
				AnchorPoint = CCPoint.AnchorMiddle,
				Dimensions = ContentSize
			};

			AddChild (redLabel);

			var blueLabel = new CCLabel (_blueString, null, 75) {
				Position = new CCPoint (VisibleBoundsWorldspace.Size.Center.X, VisibleBoundsWorldspace.Size.Center.Y + 100),
				Color = new CCColor3B (CCColor4B.Blue),
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Center,
				AnchorPoint = CCPoint.AnchorMiddle,
				Dimensions = ContentSize
			};

			AddChild (blueLabel);

			var playAgainLabel = new CCLabel ("Tap to Play Again", null, 50) {
				Position = VisibleBoundsWorldspace.Size.Center,
				Color = new CCColor3B (CCColor4B.Green),
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Center,
				AnchorPoint = CCPoint.AnchorMiddle,
				Dimensions = ContentSize
			};

			AddChild (playAgainLabel);

		}

		public static CCScene GameScene (CCWindow window, int redScore, int blueScore)
		{
			var scene = new CCScene (window);
			var layer = new GameOverLayer (redScore, blueScore);

			scene.AddChild (layer);

			return scene;
		}

		void HandleTouchEnded (System.Collections.Generic.List<CCTouch> arg1, CCEvent arg2)
		{
			Window.DefaultDirector.ReplaceScene (GameLayer.GameScene (Window));
		}
	}
}