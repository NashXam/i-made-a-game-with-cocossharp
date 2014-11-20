using CocosSharp;
using System.Collections.Generic;
using TripleTriad.Shared.GameLogic.Cards;
using System;
using TripleTriad.Shared.GameLogic;
using System.Linq;
using TripleTriad.Shared.Layers;

namespace TripleTriad.Shared
{
	public class GameLayer : CCLayerColor
	{
		private const int HLINES = 2;
		private const int VLINES = 2;
		private const int CARD_POSITIONS = 9;
		private const float LINE_THICKNESS = 5.0f;

		private Dictionary<int, List<int>> _siblings;
		private CCDrawNode[] _cardLocations;
		private CCEventListenerTouchAllAtOnce[] _touchListeners;

		private float _cardWidth;
		private float _cardHeight;
		private CCSize _bounds;

		public BlueCard[] BlueCards;
		public RedCard[] RedCards;
		public CardBase ChosenCard;

		public Teams Team;

		public GameLayer ()
		{
			Color = CCColor3B.Gray;
			Opacity = 255;

			RedCards = new RedCard[5];
			BlueCards = new BlueCard[5];
			Team = Teams.BLUE;

			InitializeSiblings();
			RandomizeInitialCards();
			SetUpAreas();
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();
			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.ShowAll;

			//Retrieve the bounds of the mobile device
			_bounds = Window.WindowSizeInPixels;

			//Set up card Variables
			_cardWidth = (_bounds.Width/3);
			_cardHeight = (_bounds.Height/3);

			DrawAreas();
			ScheduleOnce(GoToChooseCard, 2f);
		}

		private void GoToChooseCard (float obj)
		{
			if(BlueCards.Count() == 0 || RedCards.Count() == 0)
			{
				GoToGameOver();
				return;
			}

			if(Team == Teams.BLUE)
			{
				var blueScene = ChooseCardLayer.GameScene(this, Window, BlueCards);
				var transitionScene = new CCTransitionMoveInR (.5f, blueScene);
				Window.DefaultDirector.PushScene(transitionScene);
			}
			else
			{
				var redScene = ChooseCardLayer.GameScene(this, Window, RedCards);
				var transitionScene = new CCTransitionMoveInR (.5f, redScene);
				Window.DefaultDirector.PushScene(transitionScene);
			}
		}

		private void GoToGameOver()
		{
			int redScore = 0;
			int blueScore = 0;
			foreach(var cardLoc in _cardLocations)
			{
				if(cardLoc.Children[0] != null)
				{
					var card = cardLoc.Children[0].UserObject as CardBase;
					if(card is RedCard)
					{
						redScore+=1;
					}
					else
					{
						blueScore+=1;
					}
				}
			}

			var gameOverScene = GameOverLayer.GameScene (Window, redScore, blueScore);
			var transitionToGameOver = new CCTransitionMoveInR (0.3f, gameOverScene);
			Director.ReplaceScene (transitionToGameOver);
//			Window.DefaultDirector.ReplaceScene (GameOverLayer.GameScene(Window, redScore, blueScore));
		}

		/// <summary>
		/// Create Areas where Cards will be housed on the board
		/// </summary>
		private void SetUpAreas()
		{
			_cardLocations = new CCDrawNode[CARD_POSITIONS];
			_touchListeners = new CCEventListenerTouchAllAtOnce[CARD_POSITIONS];

			for(int i = 0; i < CARD_POSITIONS; i++)
			{
				_touchListeners[i] = new CCEventListenerTouchAllAtOnce();
				_touchListeners[i].OnTouchesEnded = HandleTouchEnded;

				_cardLocations[i] = new CCDrawNode();
				_cardLocations[i].Tag = i;
				AddEventListener(_touchListeners[i], _cardLocations[i]);
				AddChild(_cardLocations[i], 1);
			}

		}
			
		private void HandleTouchEnded (List<CCTouch> touches, CCEvent touchEvent)
		{
			if(!touchEvent.CurrentTarget.BoundingBoxTransformedToParent.ContainsPoint(touches[0].Location))
				return;

			PauseListeners(true);

			var cardSprite = ChosenCard.CardSprite = new CCSprite(ChosenCard.ImageName);

			cardSprite.ContentSize = new CCSize( _cardWidth, _cardHeight);
			cardSprite.Scale = .1f;
			cardSprite.Position = cardSprite.BoundingBox.UpperRight;
			cardSprite.UserObject = ChosenCard;

			touchEvent.CurrentTarget.AddChild(cardSprite, 1);

			var animateCardIn = new CCScaleTo (.5f, 1.0f);
			cardSprite.RunAction (animateCardIn);


				
			touchEvent.StopPropogation();
			RemoveEventListener(_touchListeners[touchEvent.CurrentTarget.Tag]);
			FlipCards(touchEvent.CurrentTarget);
			ChangeTeams();

			ScheduleOnce(GoToChooseCard, 4f);
		}

		private void ChangeTeams ()
		{
			Team = Team == Teams.RED ? Teams.BLUE : Teams.RED;
		}

		/// <summary>
		/// Draw the Positions Where the Cards will be housed on the board
		/// </summary>
		private void DrawAreas()
		{
			var x = 0f;
			var y = 0f;

			for(int i = 0; i < CARD_POSITIONS; i++)
			{
				_cardLocations[i].Position = new CCPoint(x, y);
				_cardLocations[i].ContentSize = new CCSize(_cardWidth, _cardHeight);
				_cardLocations[i].DrawRect(_cardLocations[0].BoundingBox, CCColor4B.Red, 5f, CCColor4B.Black);

				if((i+1) % 3 == 0)
				{
					x = 0;
					y += _cardHeight;
				}
				else
				{
					x += _cardWidth;
				}
			}
		}

		public void RandomizeInitialCards ()
		{
			var rng = new Random();
			int value;
			for(int i = 0; i < 5; i++)
			{
				value = rng.Next(0, 11);
				BlueCards[i] = GameAppDelegate.BlueCards[value].Clone() as BlueCard;
				value = rng.Next(0, 11);
				RedCards[i] = GameAppDelegate.RedCards[value].Clone() as RedCard;
			}
		}
			
		public static CCScene GameScene (CCWindow mainWindow)
		{
			var scene = new CCScene (mainWindow);
			var layer = new GameLayer ();

			scene.AddChild (layer);

			return scene;
		}
			
		public void FlipCards (CCNode currentTarget)
		{
			var currentSiblings = _siblings[currentTarget.Tag];
			CheckSiblings(currentTarget, currentSiblings);
		}

		//up, right, down, left
		private void InitializeSiblings ()
		{
			_siblings = new Dictionary<int, List<int>>();
			_siblings[0] = new List<int>(){3, 1, -1, -1};
			_siblings[1] = new List<int>(){4, 2, -1, 0};
			_siblings[2] = new List<int>(){5, -1, -1, 1};
			_siblings[3] = new List<int>(){6, 4, 0, -1};
			_siblings[4] = new List<int>(){7, 5, 1, 3};
			_siblings[5] = new List<int>(){8, -1, 2, 4};
			_siblings[6] = new List<int>(){-1, 7, 3, -1};
			_siblings[7] = new List<int>(){-1, 8, 4, 6};
			_siblings[8] = new List<int>(){-1, -1, 5, 7};
		}

		private void CheckSiblings (CCNode currentTarget, List<int> currentSiblings)
		{
			var parent = currentTarget.Parent;
			var card = currentTarget.Children[0].UserObject as CardBase;
			foreach(var i in currentSiblings)
			{
				if(i == -1)
					continue;

				var sibling = parent.GetChildByTag(i);

				if(sibling.ChildrenCount == 1)
				{
					var siblingCardSprite = sibling.Children[0] as CCSprite;
					var siblingCard = siblingCardSprite.UserObject as CardBase;

					if(card.GetType() == siblingCard.GetType())
						continue;

					if(CardBase.Compare(card, siblingCard, currentSiblings.IndexOf(i)))
					{
						var action =  new CCOrbitCamera(1, 1, 0, 0, 360, 0, 0);
						siblingCardSprite.RunAction(action);
						var nSiblingCard = siblingCard.ChangeTeam();
						CCTexture2D tex = CCTextureCache.SharedTextureCache.AddImage(nSiblingCard.ImageName);
						siblingCardSprite.Texture = tex;
						siblingCardSprite.UserObject = nSiblingCard;
						CheckSiblings(sibling, _siblings[sibling.Tag]);
					}
				}
			}
		}
	}
}

