using System;
using CocosSharp;

namespace TripleTriad.Shared.GameLogic.Cards
{
	public class CardBase: ICloneable
	{
		protected readonly int Id;
		protected int Up, Right, Down, Left;
		protected readonly string Name;
		protected Elements Element;

		public string ImageName
		{
			get;
			protected set;
		}

		public CCSprite CardSprite
		{
			get;
			set;
		}

		public Teams Team {
			get;
			set;
		}

		public CardBase (int id, string name, Elements element, int up, 
			int right, int down, int left)
		{
			Id = id;
			Name = name;
			Element = element;
			Up = up;
			Right = right;
			Down = down;
			Left = left;
		}


		public static bool Compare (CardBase card, CardBase siblingCard, int i)
		{
			switch (i) 
			{
			case 0:
				return card.Up > siblingCard.Down;
			case 1:
				return card.Right > siblingCard.Left;
			case 2:
				return card.Down > siblingCard.Up;
			case 3:
				return card.Left > siblingCard.Right;
			default:
				return false;
			}
		}

		public virtual CardBase ChangeTeam(){return null;}

		#region ICloneable implementation

		public virtual object Clone ()
		{
			return null;
		}

		#endregion
	}
}

