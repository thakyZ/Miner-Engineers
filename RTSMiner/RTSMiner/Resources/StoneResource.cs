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
using VoidEngine.VGame;
using RTSMiner.Helpers;

namespace RTSMiner.Resources
{
	public class StoneResource : Resource
	{
		public StoneResource(Vector2 position, Texture2D texture, int hp, List<Resource> ResourceList)
			: base(position, texture, ResourceList)
		{
			GridSize = 30;
			AddAnimations(texture);
			ResourceType = RTSHelper.ResourceTypes.STONE;
			MainHP = hp;
			Breakable = true;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}
