using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RTSMiner.Helpers;
using System.Collections.Generic;

namespace RTSMiner.Resources
{
	public class IronResource : Resource
	{
		public IronResource(Vector2 position, Texture2D texture, int hp, Point worldSize, List<Resource> ResourceList)
			: base(position, texture, worldSize, ResourceList)
		{
			GridSize = 30;
			AddAnimations(texture);
			ResourceType = RTSHelper.ResourceTypes.Iron;
			MainHP = hp;
			Breakable = true;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}
