using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RTSMiner.Other;
using RTSMiner.Resources;
using RTSMiner.Units;
using System;
using System.Collections.Generic;
using VoidEngine.Particles;
using VoidEngine.VGame;
using VoidEngine.VGenerator;
using VoidEngine.VGUI;

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
		public Camera Camera;

		/// <summary>
		/// Used to get random real numbers.
		/// </summary>
		public Random Random = new Random();

		/// <summary>
		/// Used to get the current state of the keyboard.
		/// </summary>
		public KeyboardState KeyboardState, PreviousKeyboardState;

		/// <summary>
		/// Used to control the state of the cursor.
		/// </summary>
		public MouseState MouseState, PreviousMouseState;

		#region Textures
		/// <summary>
		/// The textures for the resources.
		/// </summary>
		public Texture2D StoneTileTexture, UraniumTileTexture, IronTileTexture, GoldTileTexture;
		/// <summary>
		/// The textures for the boundries of the map.
		/// </summary>
		public Texture2D VoidTileTexture, SpaceTileTexture, AsteroidTileTexture;
		/// <summary>
		/// The texture of the buttons on the screen.
		/// </summary>
		public Texture2D ButtonTexture, ButtonWindowTexture, ProgressBarTexture;
		/// <summary>
		/// The texture of the particles on the screen.
		/// </summary>
		public Texture2D ParticalTexture;
		/// <summary>
		/// The heavyMetal background textures.
		/// </summary>
		public Texture2D BackgroundHeavyTexture;
		public Texture2D HQTexture;
		public Texture2D HQOverlayTexture;
		public Texture2D HarvesterTexture;
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
		public bool MapLoaded
		{
			get;
			set;
		}
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

		#region GUI Stuff
		public List<Button> ButtonList = new List<Button>();
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
			// Load the background.
			myGame.GenerateBackground();

			// Create the camera.
			Camera = new Camera(myGame.GraphicsDevice.Viewport, new Point(myGame.WindowSize.X, myGame.WindowSize.Y), 1.0f);

			base.LoadContent();
		}

		protected virtual void LoadImages()
		{
			VoidTileTexture = Game.Content.Load<Texture2D>(@"images\tilesets\void");
			SpaceTileTexture = Game.Content.Load<Texture2D>(@"images\tilesets\space");
			AsteroidTileTexture = Game.Content.Load<Texture2D>(@"images\tilesets\asteroids");
			StoneTileTexture = Game.Content.Load<Texture2D>(@"images\tilesets\stone");
			UraniumTileTexture = Game.Content.Load<Texture2D>(@"images\tilesets\uranium");
			IronTileTexture = Game.Content.Load<Texture2D>(@"images\tilesets\iron");
			GoldTileTexture = Game.Content.Load<Texture2D>(@"images\tilesets\gold");
			BackgroundHeavyTexture = Game.Content.Load<Texture2D>(@"images\gui\backgroundHeavy");
			HQTexture = Game.Content.Load<Texture2D>(@"images\buildings\hq1");
			HQOverlayTexture = Game.Content.Load<Texture2D>(@"images\buildings\hq1-overlay");
			HarvesterTexture = Game.Content.Load<Texture2D>(@"images\units\harvester1");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// Get the states of the keyboard and mouse.
			KeyboardState = Keyboard.GetState();
			MouseState = Mouse.GetState();

			if (KeyboardState.IsKeyDown(Keys.Escape))
			{
				myGame.SetCurrentLevel(Game1.GameLevels.MENU);
			}

			#region Update Tiles and Objects
			myGame.cursor.Update(gameTime);

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
			#endregion

			#region Camera Controls
			float MoveSpeed = 5;
			float MoveRadius = 10;

			// Move the camera up.
			if (KeyboardState.IsKeyDown(Keys.Up))
			{
				Camera.Position -= new Vector2(0, 5);
			}
			// Move the camera to the left.
			if (KeyboardState.IsKeyDown(Keys.Left))
			{
				Camera.Position -= new Vector2(5, 0);
			}
			// Reset the camera.
			if (KeyboardState.IsKeyDown(Keys.NumPad5))
			{
				Camera.Position = new Vector2(0, 0);
			}
			// Move the camera to the right.
			if (KeyboardState.IsKeyDown(Keys.Right))
			{
				Camera.Position += new Vector2(5, 0);
			}
			// Move the camera down.
			if (KeyboardState.IsKeyDown(Keys.Down))
			{
				Camera.Position += new Vector2(0, 5);
			}

			Camera.Position += new Vector2(0, 0);

			// Move the camera if the mouse is at a specific area.
			if (Game.IsActive)
			{
				if (MouseState.X > 0 && MouseState.X < myGame.WindowSize.X && MouseState.Y > 0 && MouseState.Y < myGame.WindowSize.Y)
				{
					// Move the camera down.
					if (MouseState.Y + MoveRadius >= myGame.WindowSize.Y)
					{
						Camera.Position += new Vector2(0, MoveSpeed);
					}
					// Move the camera up.
					if (MouseState.Y <= MoveRadius)
					{
						Camera.Position -= new Vector2(0, MoveSpeed);
					}
					// Move the camera to the right.
					if (MouseState.X + MoveRadius >= myGame.WindowSize.X)
					{
						Camera.Position += new Vector2(MoveSpeed, 0);
					}
					// Move the camera to the left.
					if (MouseState.X <= MoveRadius)
					{
						Camera.Position -= new Vector2(MoveSpeed, 0);
					}
				}
			}
			#endregion

			#region Map Loading Stuff
			if (!MapLoaded)
			{
				RenegerateMap();
			}

			// Tell if the letter 'U' is pressed so we can regenerate the starmap.
			if (KeyboardState.IsKeyUp(Keys.U) && PreviousKeyboardState.IsKeyDown(Keys.U))
			{
				RenegerateMap();
			}
			#endregion

			#region Debug Stuff
			if (myGame.GameDebug)
			{
				// Update the debug label.
				myGame.debugLabel.Update(gameTime);
				myGame.debugStrings[0] = "harvesterCount=" + RTSMiner.Other.MapHelper.PlacedBlueHarvesters + " HQCount=" + RTSMiner.Other.MapHelper.PlacedBlueHQs;
				myGame.debugStrings[1] = "cameraSize=(" + Camera.viewportSize.X + "," + Camera.viewportSize.Y + ")";
				myGame.debugStrings[2] = "windowSize=(" + myGame.WindowSize.X + "," + myGame.WindowSize.Y + ")";
				myGame.debugStrings[3] = "cameraPosi=(" + Camera.Position.X + "," + Camera.Position.Y + ")";
			}

			if (myGame.isOptionsChanged > myGame.OldIsOptionsChanged)
			{
				Camera.viewportSize = new Vector2(myGame.WindowSize.X, myGame.WindowSize.Y);
			}
			#endregion

			base.Update(gameTime);

			// Set the previous mouse and keyboard states.
			PreviousKeyboardState = KeyboardState;
			PreviousMouseState = MouseState;
		}

		#region Load the map
		protected void RenegerateMap()
		{
			TileList.RemoveRange(0, TileList.Count);
			ResourceList.RemoveRange(0, ResourceList.Count);
			UnitList.RemoveRange(0, UnitList.Count);
			HarvesterPlaced = false;
			HQPlaced = false;
			RTSMiner.Other.MapHelper.PlacedBlueHarvesters = 1;
			RTSMiner.Other.MapHelper.PlacedBlueHQs = 0;
			// Loads the map once.
			LoadMap();

			Camera.Size = new Point(MapArray.GetLength(1) * 30, MapArray.GetLength(0) * 30);
			Camera.screenCenter = new Vector2(myGame.GraphicsDevice.Viewport.Width, myGame.GraphicsDevice.Viewport.Height);
			Camera.origin = Camera.screenCenter / Camera.Zoom;

			foreach (Unit u in UnitList)
			{
				if (u is BlueHarvester)
				{
					Camera.Position = u.Position;
				}
			}

			Camera.Position += new Vector2(0, 0);

			MapLoaded = true;
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
						case 1: // "Cliff" Tiles
							ResourceList.Add(new StoneResource(new Vector2(x * 30, y * 30), StoneTileTexture, 10, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case 2: // "Uranium" Tiles
							ResourceList.Add(new UraniumResource(new Vector2(x * 30, y * 30), UraniumTileTexture, 15, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case 3: // "Iron" Tiles
							ResourceList.Add(new IronResource(new Vector2(x * 30, y * 30), IronTileTexture, 15, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case 4: // "Gold" Tiles
							ResourceList.Add(new GoldResource(new Vector2(x * 30, y * 30), GoldTileTexture, 15, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case 5: // "Asteroid" Tiles
							ResourceList.Add(new AsteroidResource(new Vector2(x * 30, y * 30), AsteroidTileTexture, 100, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case -2:
							UnitList.Add(new BlueHarvester(new Vector2(x * 30 + 5, y * 30 + 5), HarvesterTexture, myGame));
							HarvesterPlaced = true;
							break;
						case -3:
							UnitList.Add(new BlueHQ(HQTexture, new Vector2(x * 30, y * 30), HQOverlayTexture, new Color(0, 124, 255), myGame));
							HQPlaced = true;
							break;
					}

					TileList.Add(new Tile(SpaceTileTexture, new Vector2(x * 30, y * 30), 0, Random.Next(998524), Color.White));
				}
			}

			foreach (Resource r in ResourceList)
			{
				r.UpdateConnections();
			}
		}
		#endregion

		/// <summary>
		/// Draws the game component's content.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Draw(GameTime gameTime)
		{
			#region Draw Background
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
			#endregion

			#region Draw everything in the camera
			// Draw in the frame of the camera.
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.GetTransformation());
			{
				foreach (Tile t in TileList)
				{
					if (Camera.IsInView(t.Position, new Vector2(30, 30)))
					{
						// Draw all the tiles.
						t.Draw(gameTime, spriteBatch);
					}
				}

				foreach (Resource r in ResourceList)
				{
					if (Camera.IsInView(r.Position, new Vector2(30, 30)))
					{
						// Draw all the resources.
						r.Draw(gameTime, spriteBatch);
					}
				}

				foreach (Tile t in BoundryTiles)
				{
					if (Camera.IsInView(t.Position, new Vector2(30, 30)))
					{
						// Draw the boundry tiles.
						t.Draw(gameTime, spriteBatch);
					}
				}

				foreach (Unit u in UnitList)
				{
					if (Camera.IsInView(u.Position, new Vector2(u.CurrentAnimation.frameSize.X, u.CurrentAnimation.frameSize.Y)))
					{
						// Draw all of the Units.
						u.Draw(gameTime, spriteBatch);
					}
				}
			}
			spriteBatch.End();
			#endregion

			#region Draw Cursor and debug stuff
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
			#endregion

			base.Draw(gameTime);
		}
	}
}
