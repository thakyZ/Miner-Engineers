using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VoidEngine.VGame
{
	public class Tile : Sprite
	{
		protected int gridSize;

		int[,] mapArray;
		int tilenum;

		public bool GeneratedRandom;
		public bool isRandom;
		public int RandomNumber;

		Random random;

		public Tile(Texture2D texture, Vector2 position, int tilenum, int randomSeed, Color color) : base(position, color, texture)
		{
			AnimationSets = new List<AnimationSet>();
			gridSize = texture.Width / 4;
			random = new Random(randomSeed);
			isRandom = true;
			AddAnimations(texture);
			this.tilenum = tilenum;
		}

		public Tile(Texture2D texture, Vector2 position, int[,] mapArray, int tilenum, Color color)
			: base(position, color, texture)
		{
			AnimationSets = new List<AnimationSet>();
			gridSize = texture.Width / 4;
			this.mapArray = mapArray;
			this.tilenum = tilenum;
			UpdateConnections();
			AddAnimations(texture);
		}

		public void UpdateConnections()
		{
			Point gridPos = new Point((int)position.X / gridSize, (int)position.Y / gridSize);
			bool mapup, mapleft, mapright, mapdown;
			mapdown = mapup = mapleft = mapdown = mapright = false;
			if (gridPos.Y > 0)
			{
				if (mapArray[gridPos.X, gridPos.Y - 1] >= tilenum)
				{
					mapup = true;
				}
			}
			if (gridPos.Y == 0)
			{
				mapup = true;
			}
			if (gridPos.Y < mapArray.GetLength(1) - 1)
			{
				if (mapArray[gridPos.X, gridPos.Y + 1] >= tilenum)
				{
					mapdown = true;
				}
			}
			if (gridPos.Y == mapArray.GetLength(1) - 1)
			{
				mapdown = true;
			}
			if (gridPos.X > 0)
			{
				if (mapArray[gridPos.X - 1, gridPos.Y] >= tilenum)
				{
					mapright = true;
				}
			}
			if (gridPos.X == 0)
			{
				mapright = true;
			}
			if (gridPos.X < mapArray.GetLength(0) - 1)
			{
				if (mapArray[gridPos.X + 1, gridPos.Y] >= tilenum)
				{
					mapleft = true;
				}
			}
			if (gridPos.X == mapArray.GetLength(0) - 1)
			{
				mapleft = true;
			}

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

		public override void Update(GameTime gameTime)
		{
			if (isRandom && !GeneratedRandom)
			{
				RandomNumber = random.Next(0, 256);
				RandomNumber = (RandomNumber - 0) / (16 - 0);

				SetAnimation(RandomNumber.ToString());
				GeneratedRandom = true;
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
		}

		protected override void AddAnimations(Texture2D texture)
		{
			AnimationSets.Add(new Sprite.AnimationSet("0", texture,  new Point(gridSize, gridSize), Point.Zero, new Point(0,            0), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("1", texture,  new Point(gridSize, gridSize), Point.Zero, new Point(gridSize,     0), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("2", texture,  new Point(gridSize, gridSize), Point.Zero, new Point(gridSize * 2, 0), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("3", texture,  new Point(gridSize, gridSize), Point.Zero, new Point(gridSize * 3, 0), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("4", texture,  new Point(gridSize, gridSize), Point.Zero, new Point(0,            gridSize), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("5", texture,  new Point(gridSize, gridSize), Point.Zero, new Point(gridSize,     gridSize), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("6", texture,  new Point(gridSize, gridSize), Point.Zero, new Point(gridSize * 2, gridSize), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("7", texture,  new Point(gridSize, gridSize), Point.Zero, new Point(gridSize * 3, gridSize), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("8", texture,  new Point(gridSize, gridSize), Point.Zero, new Point(0,            gridSize * 2), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("9", texture,  new Point(gridSize, gridSize), Point.Zero, new Point(gridSize,     gridSize * 2), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("10", texture, new Point(gridSize, gridSize), Point.Zero, new Point(gridSize * 2, gridSize * 2), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("11", texture, new Point(gridSize, gridSize), Point.Zero, new Point(gridSize * 3, gridSize * 2), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("12", texture, new Point(gridSize, gridSize), Point.Zero, new Point(0,            gridSize * 3), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("13", texture, new Point(gridSize, gridSize), Point.Zero, new Point(gridSize,     gridSize * 3), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("14", texture, new Point(gridSize, gridSize), Point.Zero, new Point(gridSize * 2, gridSize * 3), 0, false));
			AnimationSets.Add(new Sprite.AnimationSet("15", texture, new Point(gridSize, gridSize), Point.Zero, new Point(gridSize * 3, gridSize * 3), 0, false));
			SetAnimation("0");
		}
	}
}
