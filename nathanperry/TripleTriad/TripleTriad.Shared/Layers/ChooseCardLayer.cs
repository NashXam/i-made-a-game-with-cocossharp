using System;
using CocosSharp;
using TripleTriad.Shared.GameLogic.Cards;
using System.Linq;
using System.Collections.Generic;

namespace TripleTriad.Shared.Layers
{
	public class ChooseCardLayer : CCLayerColor
	{
		private GameLayer _fromLayer;
		private CardBase[] _cards;
		private CCSize _bounds;
		private float _cardWidth, _cardHeight;
		private CCEventListenerTouchAllAtOnce[] _touchListeners;
		private bool _popped;

		public ChooseCardLayer (GameLayer sender, CardBase[] cards)
		{
			Color = CCColor3B.Gray;
			Opacity = 255;

			_fromLayer = sender;
			_cards = cards;
			_touchListeners = new CCEventListenerTouchAllAtOnce[_cards.Count()];
			_popped = false;
		}
			
		protected override void AddedToScene ()
		{
			base.AddedToScene ();
			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.ShowAll;

			_bounds = Window.WindowSizeInPixels;
			_cardWidth = (_bounds.Width/3);
			_cardHeight = (_bounds.Height/3);

			var label = AddLabel();
			AddChild(label);

			AddCards();
		}

		private CCLabel AddLabel()
		{
			return new CCLabel ("Choose a Card.", null, 50) {
				Position = new CCPoint(VisibleBoundsWorldspace.MidX, VisibleBoundsWorldspace.MaxY),
				Color = CCColor3B.Black,
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Top,
				AnchorPoint = CCPoint.AnchorMiddleTop,
				Dimensions = ContentSize
			};
		}

		private void AddCards ()
		{
			var x = _cardWidth/2f;
			var y = VisibleBoundsWorldspace.Center.Y;
			for(int i = 0; i < _cards.Count(); i++)
			{
				var card = _cards[i];

				if(i == 3)
				{ 
					x = _cardWidth/2f;
					y = _cardHeight/2f;
				}

				var sprite = card.CardSprite;
				sprite.ContentSize = new CCSize(_cardWidth, _cardHeight);
				sprite.Position = new CCPoint(x, y);
				sprite.UserObject = card;
				sprite.Tag = i;


				_touchListeners[i] = new CCEventListenerTouchAllAtOnce();
				_touchListeners[i].OnTouchesEnded = HandleTouchEnded;
				AddEventListener(_touchListeners[i], sprite);

				AddChild(sprite);

				x += _cardWidth;
			}
		}

		private void HandleTouchEnded (List<CCTouch> touches, CCEvent touchEvent)
		{
			if(!touchEvent.CurrentTarget.BoundingBoxTransformedToParent.ContainsPoint(touches[0].Location))
				return;

			var cardSprite = touchEvent.CurrentTarget as CCSprite;
			var card = cardSprite.UserObject as CardBase;

			_fromLayer.ChosenCard = card.Clone() as CardBase;
			_cards = _cards.Where(val => val != card).ToArray();

			if(_fromLayer.ChosenCard is BlueCard)
				_fromLayer.BlueCards = _fromLayer.BlueCards.Where(val => val != (BlueCard)card).ToArray();
			else
				_fromLayer.RedCards = _fromLayer.RedCards.Where(val => val != (RedCard)card).ToArray();

			_touchListeners[touchEvent.CurrentTarget.Tag].IsEnabled = false;
			if(!_popped)
			{
				_popped = true;
				Window.DefaultDirector.PopScene ();
				ScheduleOnce(HandlePopped, .5f );

			}

		}

		private void HandlePopped (float obj)
		{
			_fromLayer.ResumeListeners(true);
		}

		public static CCScene GameScene (GameLayer sender, CCWindow window, CardBase[] cards)
		{
			var scene = new CCScene (window);
			var layer = new ChooseCardLayer (sender, cards);

			scene.AddChild (layer);

			return scene;
		}
	}
}

