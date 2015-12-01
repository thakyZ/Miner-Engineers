using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RTSMiner.Helpers;

namespace RTSMiner.Units
{
	public class BlueHarvester : Unit
	{
		public static float UnitRadius = 10;

		public BlueHarvester(Vector2 position, Texture2D texture, Game1 myGame)
			: base(position, myGame)
		{
			UnitType = RTSHelper.UnitTypes.BlueHarvester;
			AddAnimations(texture);
			UnitName = "Harvester";

			currentBehaviors = RTSHelper.UnitBehaviors.Idle;
			MaterialsCount = 0;
			MaterialsMax = 10;
			HarvestTime = 100;
			Speed = 2;
			MainHP = HPMax = 20;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
		}

		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE", texture, new Point(20, 20), new Point(0, 0), new Point(0, 0), 1600, false);
			AddAnimation("FLYING", texture, new Point(20, 20), new Point(0, 0), new Point(20, 0), 1600, false);
			AddAnimation("DRILL", texture, new Point(20, 20), new Point(2, 0), new Point(40, 0), 1600, true);
			AddAnimation("DRIFLY", texture, new Point(20, 20), new Point(2, 0), new Point(0, 20), 1600, true);
			SetAnimation("IDLE");

			base.AddAnimations(texture);
		}
	}
}
