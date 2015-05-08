using System;
using System.Collections.Generic;
using System.Linq;
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

namespace RTSMiner.Managers
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class GameManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		/// <summary>
		/// The game that this manager runs off of.
		/// </summary>
		Game1 myGame;
		/// <summary>
		/// The sprite batch to draw the sprites with.
		/// </summary>
		SpriteBatch spriteBatch;

		/// <summary>
		/// Used to control the game's screen.
		/// </summary>
		public Camera camera;

		/// <summary>
		/// Used to get random real numbers.
		/// </summary>
		public Random random = new Random();

		/// <summary>
		/// Used to get the current state of the keyboard.
		/// </summary>
		public KeyboardState keyboardState, previousKeyboardState;

		/// <summary>
		/// Used to control the state of the cursor.
		/// </summary>
		public MouseState mouseState, previousMouseState;

		#region Textures
		/// <summary>
		/// The textures for the resources.
		/// </summary>
		public Texture2D stoneTile, uraniumTile, ironTile, goldTile;
		/// <summary>
		/// The textures for the boundries of the map.
		/// </summary>
		public Texture2D voidTile, spaceTile, asteroidTile;
		/// <summary>
		/// The texture of the buttons on the screen.
		/// </summary>
		public Texture2D buttonTexture, buttonWindowTexture, progressBarTexture;
		/// <summary>
		/// The texture of the particles on the screen.
		/// </summary>
		public Texture2D particalTexture;
		/// <summary>
		/// The heavyMetal background textures.
		/// </summary>
		public Texture2D backgroundHeavy;
		public Texture2D hqTexture;
		public Texture2D hqOverlayTexture;
		public Texture2D harvesterTexture;
		#endregion

		#region Tile Stuff
		/// <summary>
		/// The main map array.
		/// </summary>
		public int[,] MapArray;
		/// <summary>
		/// The list of tiles.
		/// </summary>
		public List<Tile> TileList = new List<Tile>();
		/// <summary>
		/// The list of resources in the game.
		/// </summary>
		public List<Resource> ResourceList = new List<Resource>();
		/// <summary>
		/// The list of boundries in the game.
		/// </summary>
		public List<Tile> BoundryTiles = new List<Tile>();
		#endregion

		#region Unit Stuff
		public List<Unit> UnitList = new List<Unit>();
		public int SelectedResource
		{
			get;
			set;
		}
		public int SelectedUnit
		{
			get;
			set;
		}
		public bool HarvesterPlaced
		{
			get;
			set;
		}
		public bool HQPlaced
		{
			get;
			set;
		}
		public int UraniumStored
		{
			get;
			set;
		}
		public int StoneStored
		{
			get;
			set;
		}
		public int IronStored
		{
			get;
			set;
		}
		public int GoldStored
		{
			get;
			set;
		}
		public int AsteroidsStored
		{
			get;
			set;
		}
		#endregion

		#region Level & Transition Stuff
		/// <summary>
		/// The parallax for whatever it's used for?
		/// </summary>
		Vector2 Parallax = Vector2.One;
		#endregion

		#region Partical System
		/// <summary>
		/// The list of the particles.
		/// </summary>
		List<Particle> ParticleList = new List<Particle>();
		/// <summary>
		/// Gets or sets the blood's minimum radius.
		/// </summary>
		public float PartMinRadius
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the blood's maximum radius.
		/// </summary>
		public float PartMaxRadius
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// Creates the game manager
		/// </summary>
		/// <param name="game">The game that the manager is running off of.</param>
		public GameManager(Game1 game)
			: base(game)
		{
			// Set the default game.
			myGame = game;

			// Start to Initialize the game.
			Initialize();
		}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public override void Initialize()
		{
			// TODO: Add your initialization code here

			base.Initialize();
		}

		/// <summary>
		/// Loads the game component's content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create the sprite batch.
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);
			// Load all the images.
			LoadImages();
			// Loads the map once.
			LoadMap();
			// Load the background.
			myGame.GenerateBackground();

			// Create the camera.
			camera = new Camera(GraphicsDevice.Viewport, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), 1.0f);
			camera.Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2); // Set the camera's position to Zero.

			base.LoadContent();
		}

		protected virtual void LoadImages()
		{
			voidTile = Game.Content.Load<Texture2D>(@"images\tilesets\void");
			spaceTile = Game.Content.Load<Texture2D>(@"images\tilesets\space");
			asteroidTile = Game.Content.Load<Texture2D>(@"images\tilesets\asteroids");
			stoneTile = Game.Content.Load<Texture2D>(@"images\tilesets\stone");
			uraniumTile = Game.Content.Load<Texture2D>(@"images\tilesets\uranium");
			ironTile = Game.Content.Load<Texture2D>(@"images\tilesets\iron");
			goldTile = Game.Content.Load<Texture2D>(@"images\tilesets\gold");
			backgroundHeavy = Game.Content.Load<Texture2D>(@"images\gui\backgroundHeavy");
			hqTexture = Game.Content.Load<Texture2D>(@"images\buildings\hq1");
			hqOverlayTexture = Game.Content.Load<Texture2D>(@"images\buildings\hq1-overlay");
			harvesterTexture = Game.Content.Load<Texture2D>(@"images\units\harvester1");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// Get the states of the keyboard and mouse.
			keyboardState = Keyboard.GetState();
			mouseState = Mouse.GetState();

			myGame.cursor.Update(gameTime);

			// The thickness of the border around the edges of the screen
			// to move the camera.
			int MoveRadius = 20;
			// The speed to move the camera at.
			int MoveSpeed = 5;

			#region Camera Controls
			// Move the camera up.
			if (keyboardState.IsKeyDown(Keys.Up))
			{
				camera.Position -= new Vector2(0, 5);
			}
			// Move the camera to the left.
			if (keyboardState.IsKeyDown(Keys.Left))
			{
				camera.Position -= new Vector2(5, 0);
			}
			// Reset the camera.
			if (keyboardState.IsKeyDown(Keys.NumPad5))
			{
				camera.Zoom = 1f;
				camera.RotationX = 0;
				camera.RotationY = 0;
				camera.RotationZ = 0;
				camera.Position = new Vector2(0, 0);
			}
			// Move the camera to the right.
			if (keyboardState.IsKeyDown(Keys.Right))
			{
				camera.Position += new Vector2(5, 0);
			}
			// Move the camera down.
			if (keyboardState.IsKeyDown(Keys.Down))
			{
				camera.Position += new Vector2(0, 5);
			}
			#endregion

			// Move the camera if the mouse is at a specific area.
			if (Game.IsActive)
			{
				if (mouseState.X > 0 && mouseState.X < myGame.WindowSize.X && mouseState.Y > 0 && mouseState.Y < myGame.WindowSize.Y)
				{
					// Move the camera down.
					if (mouseState.Y + MoveRadius >= myGame.WindowSize.Y)
					{
						camera.Position += new Vector2(0, MoveSpeed);
					}
					// Move the camera up.
					if (mouseState.Y <= MoveRadius)
					{
						camera.Position -= new Vector2(0, MoveSpeed);
					}
					// Move the camera to the right.
					if (mouseState.X + MoveRadius >= myGame.WindowSize.X)
					{
						camera.Position += new Vector2(MoveSpeed, 0);
					}
					// Move the camera to the left.
					if (mouseState.X <= MoveRadius)
					{
						camera.Position -= new Vector2(MoveSpeed, 0);
					}
				}
			}

			camera.Position += new Vector2(0, 0);

			if (keyboardState.IsKeyDown(Keys.Escape))
			{
				myGame.SetCurrentLevel(Game1.GameLevels.MENU);
			}

			foreach (Tile t in TileList)
			{
				// Update all tiles.
				t.Update(gameTime);
			}

			foreach (Resource r in ResourceList)
			{
				// Update all the resources.
				r.Update(gameTime);
			}

			foreach (Tile t in BoundryTiles)
			{
				// Update the boundry tiles.
				t.Update(gameTime);
			}

			foreach (Tile t in myGame.BackgroundTilesList)
			{
				// Update all the background tiles.
				t.Update(gameTime);
			}

			for (int i = 0; i < UnitList.Count; i++)
			{
				if (UnitList[i].IsDead)
				{
					UnitList.RemoveAt(i);
					i--;
				}
				else
				{
					if (UnitList[i].IsUnitClicked() && SelectedUnit != i)
					{
						SelectedUnit = i;
						SelectedResource = -1;
						//SetUnitButtons(i);
					}
					UnitList[i].Update(gameTime);
				}
			}

			// Tell if the letter 'U' is pressed so we can regenerate the starmap.
			if (keyboardState.IsKeyUp(Keys.U) && previousKeyboardState.IsKeyDown(Keys.U))
			{
				TileList.RemoveRange(0, TileList.Count);
				ResourceList.RemoveRange(0, ResourceList.Count);
				UnitList.RemoveRange(0, UnitList.Count);
				HarvesterPlaced = false;
				HQPlaced = false;
				RTSMiner.Other.MapHelper.PlacedBlueHarvesters = 3;
				RTSMiner.Other.MapHelper.PlacedBlueHQs = 1;
				LoadMap();
			}

			if (myGame.GameDebug)
			{
				// Update the debug label.
				myGame.debugLabel.Update(gameTime);

				myGame.debugStrings[0] = RTSMiner.Other.MapHelper.PlacedBlueHarvesters + " " + RTSMiner.Other.MapHelper.PlacedBlueHQs + " HA" + HarvesterPlaced + " HQ" + HQPlaced;

				if (UnitList.Count > 0)
				{
					myGame.debugStrings[1] = UnitList[0].Position.X + "," + UnitList[0].Position.Y;
				}
				if (UnitList.Count > 1)
				{
					myGame.debugStrings[2] = UnitList[1].Position.X + "," + UnitList[1].Position.Y;
				}

				myGame.debugStrings[3] = camera.Position.X + "," + camera.Position.Y;
			}

			if (myGame.isOptionsChanged > myGame.OldIsOptionsChanged)
			{
				camera.viewportSize = new Vector2(myGame.WindowSize.X, myGame.WindowSize.Y);
			}

			base.Update(gameTime);

			// Set the previous mouse and keyboard states.
			previousKeyboardState = keyboardState;
			previousMouseState = mouseState;
		}

		protected void LoadMap()
		{
			// Create the map
			MapArray = RTSMiner.Other.MapHelper.SaveMap(MapGenerator.CreateMap(100, 100, 10, 0.03f), 3);
			// Create the unit map.
			int[,] UnitArray = Maps.Level1UnitMapGen();

			// Calculate out the map.
			for (int x = 0; x < MapArray.GetLength(0); x++)
			{
				for (int y = 0; y < MapArray.GetLength(1); y++)
				{
					switch (MapArray[x, y])
					{
						case -1: // "Water"
							break;
						case 0: // "Ground" Tiles
							//TileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
							break;
						case 1: // "Cliff" Tiles
							//TileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
							ResourceList.Add(new StoneResource(new Vector2(x * 30, y * 30), stoneTile, 10, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case 2: // "Uranium" Tiles
							//TileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
							ResourceList.Add(new UraniumResource(new Vector2(x * 30, y * 30), uraniumTile, 15, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case 3: // "Iron" Tiles
							//TileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
							ResourceList.Add(new IronResource(new Vector2(x * 30, y * 30), ironTile, 15, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case 4: // "Gold" Tiles
							//TileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
							ResourceList.Add(new GoldResource(new Vector2(x * 30, y * 30), goldTile, 15, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case 5: // "Asteroid" Tiles
							//sTileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
							ResourceList.Add(new AsteroidResource(new Vector2(x * 30, y * 30), asteroidTile, 100, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case -2:
							UnitList.Add(new BlueHarvester(new Vector2(x * 30 + 5, y * 30 + 5), harvesterTexture, myGame));
							HarvesterPlaced = true;
							break;
						case -3:
							UnitList.Add(new BlueHQ(hqTexture, new Vector2(x * 30, y * 30), hqOverlayTexture, Color.Blue, myGame));
							HQPlaced = true;
							break;
					}

					TileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
				}
			}

			//UnitList.Add(new BlueHQ(hqTexture, new Vector2(100, 100), hqOverlayTexture, Color.Blue, myGame));
			//UnitList.Add(new BlueHarvester(new Vector2(50, 100), harvesterTexture, myGame));

			foreach (Resource r in ResourceList)
			{
				r.UpdateConnections();
			}
		}

		/// <summary>
		/// Draws the game component's content.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Draw(GameTime gameTime)
		{
			// Draw the back stage.
			spriteBatch.Begin();
			{
				foreach (Tile t in myGame.BackgroundTilesList)
				{
					// Draw all the background tiles.
					t.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			// Draw in the frame of the camera.
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetTransformation());
			{
				foreach (Tile t in TileList)
				{
					// Draw all the tiles.
					t.Draw(gameTime, spriteBatch);
				}

				foreach (Resource r in ResourceList)
				{
					// Draw all the resources.
					r.Draw(gameTime, spriteBatch);
				}

				foreach (Tile t in BoundryTiles)
				{
					// Draw the boundry tiles.
					t.Draw(gameTime, spriteBatch);
				}

				foreach (Unit u in UnitList)
				{
					// Draw all of the Units.
					u.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			// Draw outside the frame of the camera.
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null);
			{
				myGame.cursor.Draw(gameTime, spriteBatch);

				// Tell if the game is in debug mode.
				if (myGame.GameDebug)
				{
					// Draw the debug label.
					myGame.debugLabel.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
