using CocosSharp;
using System;

namespace TripleTriad.Shared.GameLogic.Cards
{
	public class RedCard : CardBase
	{
		public RedCard (int id, string name, Elements element, int up, 
			int right, int down, int left) 
			: base(id, name, element, up, right, down, left)
		{
			ImageName = "images/r"+name;
			CardSprite = new CCSprite(ImageName);
			Team = Teams.RED;
		}

		public override CardBase ChangeTeam ()
		{
			return new BlueCard(Id, Name, Element, Up, Right, Down, Left);
		}

		#region ICloneable implementation

		public override object Clone ()
		{
			return new RedCard(Id, Name, Element, Up, Right, Down, Left);
		}

		#endregion
	}
}

