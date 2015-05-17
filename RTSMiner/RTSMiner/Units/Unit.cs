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
using RTSMiner.Managers;
using RTSMiner.Other;
using RTSMiner.Helpers;
using RTSMiner.Resources;
using VoidEngine.VGUI;
using VoidEngine.VGame;
using VoidEngine.Helpers;

namespace RTSMiner.Units
{
	public class Unit : Sprite
	{
		Game1 myGame;

		bool isOverlay;
		Texture2D overlayTexture = null;
		Color overlayColor;

		Vector2 BuildingLocation = new Vector2();
		Vector2 TempBuildingLocation = new Vector2();

		protected static Texture2D tempTexture = null;

		protected MouseState mouseState, previousMouseState;

		bool okToPlace = false;

		#region Variables
		#region Enums
		public RTSHelper.ResourceTypes currentMaterial
		{
			get;
			set;
		}
		public RTSHelper.UnitBehaviors currentBehaviors
		{
			get;
			set;
		}
		public RTSHelper.UnitTypes UnitType
		{
			get;
			set;
		}
		#endregion
		#region Stats
		public bool IsDead
		{
			get;
			set;
		}
		public string UnitName
		{
			get;
			set;
		}
		public int MainHP
		{
			get;
			set;
		}
		public int HPMax
		{
			get;
			set;
		}
		public bool LockUnit
		{
			get;
			set;
		}
		public Vector2 Direction;
		public float Speed;
		public Rectangle BoundingCollisions;
		#endregion
		#region Mining Stuff
		public int MaterialsCount
		{
			get;
			set;
		}
		public int MaterialsMax
		{
			get;
			set;
		}
		public int HarvestTime
		{
			get;
			set;
		}
		public int BuildTime
		{
			get;
			set;
		}
		#endregion
		#region Other Stuff
		//protected UnitProgress progressBar;
		//protected UnitProgress unitHPBar;
		#endregion
		#endregion

		public Unit(Vector2 position, Game1 myGame)
			: base(position, Color.White, tempTexture)
		{
			this.myGame = myGame;
		}

		public Unit(Vector2 position, Texture2D overlayTexture, Color overlayColor, Game1 myGame)
			: base(position, Color.White, tempTexture)
		{
			this.overlayTexture = overlayTexture;
			this.overlayColor = overlayColor;
			isOverlay = true;

			this.myGame = myGame;
		}

		public override void Update(GameTime gameTime)
		{
			position += Direction * Speed;

			switch (currentBehaviors)
			{
				case RTSHelper.UnitBehaviors.Idle:
					LockUnit = true;
					Idle();
					break;
				case RTSHelper.UnitBehaviors.HarvestUranium:
					Harvest(RTSHelper.ResourceTypes.Uranium, gameTime);
					break;
				case RTSHelper.UnitBehaviors.HarvestStone:
					Harvest(RTSHelper.ResourceTypes.Stone, gameTime);
					break;
				case RTSHelper.UnitBehaviors.HarvestIron:
					Harvest(RTSHelper.ResourceTypes.Iron, gameTime);
					break;
				case RTSHelper.UnitBehaviors.HarvestGold:
					Harvest(RTSHelper.ResourceTypes.Gold, gameTime);
					break;
				case RTSHelper.UnitBehaviors.HarvestAsteroids:
					Harvest(RTSHelper.ResourceTypes.Asteroid, gameTime);
					break;
				case RTSHelper.UnitBehaviors.BuildHarvester:
					BuildMobile(RTSHelper.UnitTypes.BlueHQ, gameTime);
					break;
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (TempBuildingLocation != new Vector2())
			{
				if (okToPlace)
				{
					spriteBatch.Draw(overlayTexture, new Rectangle((int)position.X, (int)position.Y, overlayTexture.Width, overlayTexture.Height), new Color(255, 255, 255, 192));
				}
				else
				{
					spriteBatch.Draw(overlayTexture, new Rectangle((int)position.X, (int)position.Y, overlayTexture.Width, overlayTexture.Height), new Color(255, 192, 192, 192));
				}
			}

			base.Draw(gameTime, spriteBatch);

			if (isOverlay)
			{
				spriteBatch.Draw(overlayTexture, new Rectangle((int)position.X, (int)position.Y, overlayTexture.Width, overlayTexture.Height), overlayColor);
			}
		}

		public bool IsUnitClicked()
		{
			return (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released && IsUnitHovered());
		}

		public bool IsUnitHovered()
		{
			Rectangle cursorRectangle = new Rectangle((int)myGame.cursor.Position.X, (int)myGame.cursor.Position.Y, 1, 1);

			return (cursorRectangle.Intersects(BoundingCollisions));
		}

		public virtual List<Button> GetButtons()
		{
			return new List<Button>();
		}

		public int FindClosestResource(RTSHelper.ResourceTypes material)
		{
			float shortestDistance = -1;
			int destination = -1;
			for (int i = 0; i < myGame.gameManager.ResourceList.Count; i++)
			{
				if (myGame.gameManager.ResourceList[i].ResourceType == material)
				{
					float resourceDistance = CollisionHelper.Magnitude(new Vector2(BoundingCollisions.Center.X, BoundingCollisions.Center.Y) - new Vector2(myGame.gameManager.ResourceList[i].BoundingCollisions.Center.X - myGame.gameManager.ResourceList[i].BoundingCollisions.Center.Y));
					if (shortestDistance == -1 || shortestDistance < resourceDistance)
					{
						shortestDistance = resourceDistance;
						destination = i;
					}
				}
			}
			return destination;
		}

		public int FindClosestUnit(RTSHelper.UnitTypes unit)
		{
			float shortestDistance = -1;
			int destination = -1;
			for (int i = 0; i < myGame.gameManager.UnitList.Count; i++)
			{
				if (myGame.gameManager.UnitList[i].UnitType == unit)
				{
					float resourceDistance = CollisionHelper.Magnitude(new Vector2(BoundingCollisions.Center.X, BoundingCollisions.Center.Y) - new Vector2(myGame.gameManager.UnitList[i].BoundingCollisions.Center.X - myGame.gameManager.UnitList[i].BoundingCollisions.Center.Y));
					if (shortestDistance == -1 || shortestDistance < resourceDistance)
					{
						shortestDistance = resourceDistance;
						destination = i;
					}
				}
			}
			return destination;
		}

		public void Idle()
		{
		}

		public void Harvest(RTSHelper.ResourceTypes material, GameTime gameTime)
		{
			if (currentMaterial != material)
			{
				currentMaterial = 0;
				currentMaterial = material;
			}

			if (MaterialsCount < MaterialsMax && FindClosestResource(material) != -1)
			{
				int destination = FindClosestResource(material);

				if (destination != -1)
				{
					if (BoundingCollisions.TouchLeftOf(myGame.gameManager.ResourceList[destination].BoundingCollisions) && BoundingCollisions.TouchTopOf(myGame.gameManager.ResourceList[destination].BoundingCollisions) && BoundingCollisions.TouchRightOf(myGame.gameManager.ResourceList[destination].BoundingCollisions) && BoundingCollisions.TouchBottomOf(myGame.gameManager.ResourceList[destination].BoundingCollisions))
					{
						Direction = Vector2.Zero;
						HarvestTime -= gameTime.ElapsedGameTime.Milliseconds;
						MaterialsCount++;
						myGame.gameManager.ResourceList[destination].MainHP--;

						if (myGame.gameManager.ResourceList[destination].MainHP <= 0)
						{
							myGame.gameManager.ResourceList.RemoveAt(destination);

							foreach (Resource r in myGame.gameManager.ResourceList)
							{
								if (r.ResourceType == material)
								{
									r.UpdateConnections();
								}
							}
						}
					}
				}
			}
			else if (MaterialsCount >= MaterialsMax || FindClosestResource(material) == -1)
			{
				int destination = FindClosestUnit(RTSHelper.UnitTypes.BlueHQ);
				if (destination != -1)
				{
					if (BoundingCollisions.TouchLeftOf(myGame.gameManager.ResourceList[destination].BoundingCollisions) && BoundingCollisions.TouchTopOf(myGame.gameManager.ResourceList[destination].BoundingCollisions) && BoundingCollisions.TouchRightOf(myGame.gameManager.ResourceList[destination].BoundingCollisions) && BoundingCollisions.TouchBottomOf(myGame.gameManager.ResourceList[destination].BoundingCollisions))
					{
						Direction = Vector2.Zero;

						switch (material)
						{
							case RTSHelper.ResourceTypes.Uranium:
								myGame.gameManager.UraniumStored += MaterialsCount;
								break;
							case RTSHelper.ResourceTypes.Stone:
								myGame.gameManager.StoneStored += MaterialsCount;
								break;
							case RTSHelper.ResourceTypes.Iron:
								myGame.gameManager.IronStored += MaterialsCount;
								break;
							case RTSHelper.ResourceTypes.Gold:
								myGame.gameManager.GoldStored += MaterialsCount;
								break;
							case RTSHelper.ResourceTypes.Asteroid:
								myGame.gameManager.AsteroidsStored += MaterialsCount;
								break;
						}

						MaterialsCount = 0;
					}
					else
					{
						Direction = CollisionHelper.UnitVector(new Vector2(myGame.gameManager.UnitList[destination].BoundingCollisions.Center.X, myGame.gameManager.UnitList[destination].BoundingCollisions.Center.Y) - new Vector2(BoundingCollisions.Center.X, BoundingCollisions.Center.Y));
					}
				}
			}
		}

		public void BuildMobile(RTSHelper.UnitTypes unit, GameTime gameTime)
		{
			LockUnit = true;
			BuildTime += gameTime.ElapsedGameTime.Milliseconds;
			float offsetAngle = (float)(myGame.gameManager.Random.NextDouble() * Math.PI * 2);
			Vector2 offsetVector = new Vector2((float)Math.Cos(offsetAngle), (float)Math.Sin(offsetAngle));

			switch (unit)
			{
				case RTSHelper.UnitTypes.BlueHarvester:
					if (BuildTime >= RTSHelper.BuildHarvesterTime)
					{
						BuildTime = 0;
						offsetVector = offsetVector * (BlueHarvester.UnitRadius + CurrentAnimation.frameSize.X / 2) + new Vector2(-BlueHarvester.UnitRadius, -BlueHarvester.UnitRadius);
						myGame.gameManager.UnitList.Add(new BlueHarvester(new Vector2(BoundingCollisions.Center.X, BoundingCollisions.Center.Y) + offsetVector, myGame.gameManager.HarvesterTexture, myGame));
						currentBehaviors = RTSHelper.UnitBehaviors.Idle;
					}
					//progressBar.SetValue(BuildTime);
					//progressBar.SetMaxValue(BuildTime);
					break;
			}
		}

		public void BuildBuilding(RTSHelper.UnitTypes unit, GameTime gameTime)
		{
			LockUnit = true;
			Point UnitSize = Point.Zero;
			Vector2 centerOffset = Vector2.Zero;
			switch (unit)
			{
				case RTSHelper.UnitTypes.BlueHQ:
					UnitSize = new Point(3, 3);
					centerOffset = new Vector2(45, 45);
					if (BuildTime >= RTSHelper.BuildHQTime)
					{
						myGame.gameManager.UnitList.Add(new BlueHQ(myGame.gameManager.HQTexture, BuildingLocation, myGame.gameManager.HQOverlayTexture, Color.Blue, myGame));
						BuildTime = 0;
						BuildingLocation = new Vector2();
						TempBuildingLocation = new Vector2();
						currentBehaviors = RTSHelper.UnitBehaviors.Idle;
					}
					break;
			}

			Point TempPoint = new Point((int)(myGame.cursor.Position.X / 30), (int)(myGame.cursor.Position.Y / 30));
			if (BuildingLocation == new Vector2() && currentBehaviors != RTSHelper.UnitBehaviors.Idle)
			{
				for (int i = TempPoint.X; i < TempPoint.X + UnitSize.X; i++)
				{
					for (int j = TempPoint.Y; j < TempPoint.Y + UnitSize.Y; j++)
					{
						if (myGame.cursor.Position.X > 0 && myGame.cursor.Position.X < myGame.gameManager.MapArray.GetLength(0) * 30 && myGame.cursor.Position.Y > 0 && myGame.cursor.Position.Y < myGame.gameManager.MapArray.GetLength(1) * 30)
						{
							if (myGame.gameManager.MapArray[i, j] != 0)
							{
								okToPlace = true;
							}
						}
						else
						{
							okToPlace = false;
						}
					}
				}
			}
		}
	}
}