using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RTSMiner.Units
{
	public class BlueHQ : Unit
	{
		Game1 myGame;

		Texture2D overlayTexture;
		Color overlayColor;

		public BlueHQ(Texture2D texture, Vector2 position, Texture2D overlayTexture, Color overlayColor, Game1 myGame)
			: base(position, myGame)
		{

			currentBehaviors = Helpers.RTSHelper.UnitBehaviors.Idle;
			UnitType = Helpers.RTSHelper.UnitTypes.BlueHQ;

			this.myGame = myGame;

			this.overlayColor = overlayColor;
			this.overlayTexture = overlayTexture;

			UnitName = "Blue HQ";
			MainHP = HPMax = 100;

			AddAnimations(texture);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			spriteBatch.Draw(overlayTexture, new Vector2(position.X, position.Y), overlayColor);
		}

		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE", texture, new Point(texture.Width, texture.Height), new Point(0, 0), new Point(0, 0), 1600, false);
			SetAnimation("IDLE");

			base.AddAnimations(texture);
		}
	}
}
