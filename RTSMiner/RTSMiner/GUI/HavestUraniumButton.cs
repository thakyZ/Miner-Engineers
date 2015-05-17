using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VoidEngine.VGUI;
using VoidEngine.VGame;
using VoidEngine.Helpers;
using VoidEngine.VGenerator;
using VoidEngine.Particles;
using RTSMiner.GUI;
using RTSMiner.Helpers;
using RTSMiner.Other;
using RTSMiner.Resources;
using RTSMiner.Units;

namespace RTSMiner.GUI
{
	class HavestUraniumButton : Button
	{
		Unit Unit;

		public HavestUraniumButton(Vector2 Position, Texture2D texture, SpriteFont font, Unit unit)
			: base(texture, Position, font, 1.0f, Color.Black, "Harvest Wood", Color.White)
		{
			AddAnimations(texture);
			Unit = unit;
		}

		public override void Update(GameTime gameTime)
		{
			if (this.Clicked() && !Unit.LockUnit)
			{
				if (Unit.currentMaterial != RTSHelper.ResourceTypes.Uranium)
				{
					Unit.MaterialsCount = 0;
				}

				Unit.currentBehaviors = RTSHelper.UnitBehaviors.HarvestUranium;
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
		}

		protected override void AddAnimations(Texture2D texture)
		{
			base.AddAnimations(texture);
		}
	}
}
