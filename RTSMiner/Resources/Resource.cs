using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RTSMiner.Helpers;
using System.Collections.Generic;
using VoidEngine.VGame;

namespace RTSMiner.Resources
{
	public class Resource : Sprite
	{
		public int MainHP
		{
			get;
			set;
		}
		public bool Breakable
		{
			get;
			protected set;
		}
		protected int GridSize;
		public RTSHelper.ResourceTypes ResourceType;
		protected List<Resource> ResourceList;
		public Rectangle BoundingCollisions;
		protected Point WorldSize;

		public Resource(Vector2 position, Texture2D texture, Point worldSize, List<Resource> ResourceList)
			: base(position, Color.White, texture)
		{
			this.ResourceList = ResourceList;
			this.WorldSize = worldSize;

			BoundingCollisions = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
		}

		public void UpdateConnections()
		{
			bool mapup = IsAResorceAboveMe();
			bool mapleft = IsAResorceLeftMe();
			bool mapright = IsAResorceRightMe();
			bool mapdown = IsAResorceBelowMe();


			if (!mapright && !mapleft && !mapdown && !mapup)
				SetAnimation("0");
			else if (!mapright && mapleft && !mapdown && !mapup)
				SetAnimation("1");
			else if (mapright && mapleft && !mapdown && !mapup)
				SetAnimation("2");
			else if (mapright && !mapleft && !mapdown && !mapup)
				SetAnimation("3");
			else if (!mapright && !mapleft && mapdown && !mapup)
				SetAnimation("4");
			else if (!mapright && mapleft && mapdown && !mapup)
				SetAnimation("5");
			else if (mapright && mapleft && mapdown && !mapup)
				SetAnimation("6");
			else if (mapright && !mapleft && mapdown && !mapup)
				SetAnimation("7");
			else if (!mapright && !mapleft && mapdown && mapup)
				SetAnimation("8");
			else if (!mapright && mapleft && mapdown && mapup)
				SetAnimation("9");
			else if (mapright && mapleft && mapdown && mapup)
				SetAnimation("10");
			else if (mapright && !mapleft && mapdown && mapup)
				SetAnimation("11");
			else if (!mapright && !mapleft && !mapdown && mapup)
				SetAnimation("12");
			else if (!mapright && mapleft && !mapdown && mapup)
				SetAnimation("13");
			else if (mapright && mapleft && !mapdown && mapup)
				SetAnimation("14");
			else if (mapright && !mapleft && !mapdown && mapup)
				SetAnimation("15");
		}

		protected bool IsSameResourceAboveMe()
		{
			foreach (Resource r in ResourceList)
			{
				if (r.ResourceType == ResourceType && r.Position.Y == Position.Y - GridSize && r.Position.X == Position.X)
				{
					return true;
				}
			}

			return false;
		}

		protected bool IsSameResourceLeftMe()
		{
			foreach (Resource r in ResourceList)
			{
				if (r.ResourceType == ResourceType && r.Position.X == Position.X + GridSize && r.Position.Y == Position.Y)
				{
					return true;
				}
			}

			return false;
		}

		protected bool IsSameResourceRightMe()
		{
			foreach (Resource r in ResourceList)
			{
				if (r.ResourceType == ResourceType && r.Position.X == Position.X - GridSize && r.Position.Y == Position.Y)
				{
					return true;
				}
			}

			return false;
		}

		protected bool IsSameResourceBelowMe()
		{
			foreach (Resource r in ResourceList)
			{
				if (r.ResourceType == ResourceType && r.Position.Y == Position.Y + GridSize && r.Position.X == Position.X)
				{
					return true;
				}
			}

			return false;
		}

		public bool IsAResorceAboveMe()
		{
			foreach (Resource r in ResourceList)
			{
				if (r.Position.Y == Position.Y - GridSize && r.Position.X == Position.X || Position.Y == 0)
				{
					return true;
				}
			}

			return false;
		}

		protected bool IsAResorceLeftMe()
		{
			foreach (Resource r in ResourceList)
			{
				if (r.Position.X == Position.X + GridSize && r.Position.Y == Position.Y || Position.X + GridSize == WorldSize.X)
				{
					return true;
				}
			}

			return false;
		}

		protected bool IsAResorceRightMe()
		{
			foreach (Resource r in ResourceList)
			{
				if (r.Position.X == Position.X - GridSize && r.Position.Y == Position.Y || Position.X == 0)
				{
					return true;
				}
			}

			return false;
		}

		protected bool IsAResorceBelowMe()
		{
			foreach (Resource r in ResourceList)
			{
				if (r.Position.Y == Position.Y + GridSize && r.Position.X == Position.X || Position.Y + GridSize == WorldSize.Y)
				{
					return true;
				}
			}

			return false;
		}

		protected override void AddAnimations(Texture2D texture)
		{
			AnimationSets.Add(new Sprite.AnimationSet("0", texture, new Point(GridSize, GridSize), Point.Zero, new Point(0, 0), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("1", texture, new Point(GridSize, GridSize), Point.Zero, new Point(GridSize, 0), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("2", texture, new Point(GridSize, GridSize), Point.Zero, new Point(GridSize * 2, 0), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("3", texture, new Point(GridSize, GridSize), Point.Zero, new Point(GridSize * 3, 0), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("4", texture, new Point(GridSize, GridSize), Point.Zero, new Point(0, GridSize), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("5", texture, new Point(GridSize, GridSize), Point.Zero, new Point(GridSize, GridSize), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("6", texture, new Point(GridSize, GridSize), Point.Zero, new Point(GridSize * 2, GridSize), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("7", texture, new Point(GridSize, GridSize), Point.Zero, new Point(GridSize * 3, GridSize), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("8", texture, new Point(GridSize, GridSize), Point.Zero, new Point(0, GridSize * 2), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("9", texture, new Point(GridSize, GridSize), Point.Zero, new Point(GridSize, GridSize * 2), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("10", texture, new Point(GridSize, GridSize), Point.Zero, new Point(GridSize * 2, GridSize * 2), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("11", texture, new Point(GridSize, GridSize), Point.Zero, new Point(GridSize * 3, GridSize * 2), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("12", texture, new Point(GridSize, GridSize), Point.Zero, new Point(0, GridSize * 3), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("13", texture, new Point(GridSize, GridSize), Point.Zero, new Point(GridSize, GridSize * 3), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("14", texture, new Point(GridSize, GridSize), Point.Zero, new Point(GridSize * 2, GridSize * 3), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("15", texture, new Point(GridSize, GridSize), Point.Zero, new Point(GridSize * 3, GridSize * 3), 0, false));
			SetAnimation("0");
		}
	}
}
