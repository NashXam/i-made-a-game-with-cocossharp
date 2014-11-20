using CocosSharp;

namespace TripleTriad.Shared.GameLogic.Cards
{
	public class BlueCard : CardBase
	{
		public BlueCard (int id, string name, Elements element, int up, 
			int right, int down, int left) 
			: base(id, name, element, up, right, down, left)
		{
			ImageName = "images/b"+name;
			CardSprite = new CCSprite(ImageName);
			Team = Teams.BLUE;
		}

		public override CardBase ChangeTeam ()
		{
			return new RedCard(Id, Name, Element, Up, Right, Down, Left);
		}

		#region ICloneable implementation

		public override object Clone ()
		{
			return new BlueCard(Id, Name, Element, Up, Right, Down, Left);
		}

		#endregion
	}
}

