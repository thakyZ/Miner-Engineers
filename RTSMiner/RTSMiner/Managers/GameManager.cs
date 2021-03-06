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
using RTSMiner.GUI;
using RTSMiner.Helpers;
using RTSMiner.Other;
using RTSMiner.Particles;
using RTSMiner.Resources;

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
		/// <summary>
		/// The list of background tiles in the game;
		/// </summary>
		public List<Tile> BackgroundTiles = new List<Tile>();
		#endregion
		
		#region Player Stuff
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

			// Create the camera.
			camera = new Camera(GraphicsDevice.Viewport, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), 1.0f);
			camera.Position = Vector2.Zero; // Set the camera's position to Zero.
			
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
			if (keyboardState.IsKeyDown(Keys.Add))
			{
				camera.RotationX += 0.01f * (float)Math.PI / 180;
			}
			if (keyboardState.IsKeyDown(Keys.Subtract))
			{
				camera.RotationX -= 0.01f * (float)Math.PI / 180;
			}
			if (keyboardState.IsKeyDown(Keys.Multiply))
			{
				camera.RotationY += 0.01f * (float)Math.PI / 180;
			}
			if (keyboardState.IsKeyDown(Keys.Divide))
			{
				camera.RotationY -= 0.01f * (float)Math.PI / 180;
			}
			// Rotate the camera to the left.
			if (keyboardState.IsKeyDown(Keys.NumPad9))
			{
				camera.RotationZ += 1.00f * (float)Math.PI / 180;
			}
			// Move the camera up.
			if (keyboardState.IsKeyDown(Keys.NumPad8))
			{
				camera.Position -= new Vector2(0, 5);
			}
			// Rotate the camera to the right.
			if (keyboardState.IsKeyDown(Keys.NumPad7))
			{
				camera.RotationZ -= 1.00f * (float)Math.PI / 180;
			}
			// Move the camera to the left.
			if (keyboardState.IsKeyDown(Keys.NumPad4))
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
			if (keyboardState.IsKeyDown(Keys.NumPad6))
			{
				camera.Position += new Vector2(5, 0);
			}
			// Zoom the camera in.
			if (keyboardState.IsKeyDown(Keys.NumPad1))
			{
				camera.Zoom += 0.01f;
				camera.Position -= new Vector2(0.0f, 0.0f);
			}
			// Move the camera down.
			if (keyboardState.IsKeyDown(Keys.NumPad2))
			{
				camera.Position += new Vector2(0, 5);
			}
			// Zoom the camera out.
			if (keyboardState.IsKeyDown(Keys.NumPad3))
			{
				camera.Zoom -= 0.01f;
				camera.Position -= new Vector2(0.0f, 0.0f);
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

			foreach (Tile t in BackgroundTiles)
			{
				// Update all the background tiles.
				t.Update(gameTime);
			}

			// Tell if the letter 'U' is pressed so we can regenerate the starmap.
			if (keyboardState.IsKeyUp(Keys.U) && previousKeyboardState.IsKeyDown(Keys.U))
			{
				TileList.RemoveRange(0, TileList.Count);
				ResourceList.RemoveRange(0, ResourceList.Count);
				LoadMap();
			}

			if (myGame.GameDebug)
			{
				// Update the debug label.
				myGame.debugLabel.Update(gameTime);
			}
			
			base.Update(gameTime);
			
			// Set the previous mouse and keyboard states.
			previousKeyboardState = keyboardState;
			previousMouseState = mouseState;
		}

		protected void LoadMap()
		{
			// Create the map
			MapArray = MapGenerator.SaveMap(MapGenerator.CreateMap(100, 100, 10, 0.03f), 3);
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
							TileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
							break;
						case 1: // "Cliff" Tiles
							TileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
							ResourceList.Add(new StoneResource(new Vector2(x * 30, y * 30), stoneTile, 10, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case 2: // "Uranium" Tiles
							TileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
							ResourceList.Add(new UraniumResource(new Vector2(x * 30, y * 30), uraniumTile, 15, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case 3: // "Iron" Tiles
							TileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
							ResourceList.Add(new IronResource(new Vector2(x * 30, y * 30), ironTile, 15, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case 4: // "Gold" Tiles
							TileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
							ResourceList.Add(new GoldResource(new Vector2(x * 30, y * 30), goldTile, 15, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
						case 5: // "Asteroid" Tiles
							TileList.Add(new Tile(spaceTile, new Vector2(x * 30, y * 30), 0, random.Next(998524), Color.White));
							ResourceList.Add(new AsteroidResource(new Vector2(x * 30, y * 30), asteroidTile, 100, new Point(MapArray.GetLength(0) * 30, MapArray.GetLength(1) * 30), ResourceList));
							break;
					}
				}
			}

			foreach (Resource r in ResourceList)
			{
				r.UpdateConnections();
			}

			for (int x = 0; x < MapArray.GetLength(0) - 1; x++)
			{
				for (int y = 0; y < MapArray.GetLength(1) - 1; y++)
				{
					//BoundryTiles.Add(new Tile(voidTile, new Vector2(x * 30 + 30, 0), MapArray, -1, Color.White));
					//BoundryTiles.Add(new Tile(voidTile, new Vector2(MapArray.GetLength(0) * 30 - 30, y * 30 + 30), MapArray, -1, Color.White));
					//BoundryTiles.Add(new Tile(voidTile, new Vector2(x * 30, MapArray.GetLength(1) * 30 - 30), MapArray, -1, Color.White));
					//BoundryTiles.Add(new Tile(voidTile, new Vector2(0, y * 30), MapArray, -1, Color.White));
				}
			}

			int TileSize = (int)(backgroundHeavy.Width / 4);
			Point ScreenGridSize = new Point((int)(myGame.WindowSize.X / TileSize), (int)(myGame.WindowSize.Y / TileSize));

			for (int x = 0; x < ScreenGridSize.X + 1; x++)
			{
				for (int y = 0; y < ScreenGridSize.Y + 1; y++)
				{
					BackgroundTiles.Add(new Tile(backgroundHeavy, new Vector2(x * TileSize, y * TileSize), 0, random.Next(998524), Color.White));
				}
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
				foreach (Tile t in BackgroundTiles)
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
