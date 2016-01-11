using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RTSMiner.Managers;
using RTSMiner.Other;
using System;
using System.Collections.Generic;
using VoidEngine.VGame;
using VoidEngine.VGUI;

namespace RTSMiner
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		public enum GameLevels
		{
			SPLASH,
			MENU,
			OPTIONS,
			GAME
		}

		#region Variables
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		#region Options stuff
		/// <summary>
		/// The current window Size
		/// </summary>
		public Point WindowSize = new Point(1200, 720);//new Point(800, 480);
		/// <summary>
		/// Gets or sets if vsync is active;
		/// </summary>
		public bool isVSync
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the options are confirmed.
		/// </summary>
		public bool OptionsComfirmed
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the options have been changed.
		/// </summary>
		public int isOptionsChanged
		{
			get;
			protected set;
		}
		/// <summary>
		/// Gets or sets if the options have been changed.
		/// </summary>
		public int OldIsOptionsChanged
		{
			get;
			protected set;
		}
		#endregion

		#region Other stuff
		/// <summary>
		/// The random generator for stuff.
		/// </summary>
		public Random random = new Random();
		/// <summary>
		/// The current and previous keyboard states.
		/// </summary>
		public KeyboardState keyboardState, previousKeyboardState;
		#endregion

		#region Managers
		public GameManager gameManager;
		MainMenuManager mainMenuManager;
		OptionsManager optionsManager;
		#endregion

		#region Textures
		/// <summary>
		/// The texture of the cursor.
		/// </summary>
		public Texture2D cursorTexture;
		/// <summary>
		/// The SpriteFont for debuging the game.
		/// </summary>
		public SpriteFont segoeUIMonoDebug;
		/// <summary>
		/// The SpriteFont for buttons.
		/// </summary>
		public SpriteFont segoeUIBold;
		/// <summary>
		/// The larger button terxture.
		/// </summary>
		public Texture2D button1Texture;
		/// <summary>
		/// The smaller button terxture.
		/// </summary>
		public Texture2D button2Texture;
		/// <summary>
		/// The background tiles.
		/// </summary>
		public Texture2D backgroundHeavy;
		#endregion

		#region Gui stuff
		/// <summary>
		/// Used to control the cursor.
		/// </summary>
		public Cursor cursor;
		/// <summary>
		/// The current game level or screen.
		/// </summary>
		protected GameLevels currentGameLevel;
		#endregion

		#region Tile stuff
		/// <summary>
		/// The list of tiles for the background.
		/// </summary>
		public List<Tile> BackgroundTilesList = new List<Tile>();
		protected bool isBackgroundGenerated;
		#endregion

		#region Debug Stuff
		/// <summary>
		/// The value to debug the game.
		/// </summary>
		public bool GameDebug = true;
		/// <summary>
		/// The label used for debuging.
		/// </summary>
		public Label debugLabel;
		/// <summary>
		/// The list of strings that are used for debuging.
		/// </summary>
		public string[] debugStrings = new string[10];
		#endregion
		#endregion

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			LoadImages();

			// Create the managers.
			gameManager = new GameManager(this);
			mainMenuManager = new MainMenuManager(this);
			optionsManager = new OptionsManager(this);
			// Add the managers to components.
			Components.Add(gameManager);
			Components.Add(mainMenuManager);
			Components.Add(optionsManager);
			// Disable the managers.
			gameManager.Enabled = false;
			gameManager.Visible = false;
			mainMenuManager.Enabled = false;
			mainMenuManager.Visible = false;
			optionsManager.Enabled = false;
			optionsManager.Visible = false;

			cursor = new Cursor(cursorTexture, Vector2.Zero);

			OptionsComfirmed = true;

			// Create the debug label.
			if (GameDebug)
			{
				debugLabel = new Label(new Vector2(0, 0), segoeUIMonoDebug, 1.0f, Color.White, "");
			}

			OptionsComfirmed = true;

			// TODO: use this.Content to load your game content here
			SetCurrentLevel(GameLevels.MENU);
		}

		protected void LoadImages()
		{
			// Load textures.
			cursorTexture = Content.Load<Texture2D>(@"images\cursor\cursor1");
			button1Texture = Content.Load<Texture2D>(@"images\gui\button2");
			button2Texture = Content.Load<Texture2D>(@"images\gui\button1");
			backgroundHeavy = Content.Load<Texture2D>(@"images\gui\backgroundHeavy");

			// Load fonts.
			segoeUIMonoDebug = Content.Load<SpriteFont>(@"fonts\segoeuimonodebug");
			segoeUIBold = Content.Load<SpriteFont>(@"fonts\segoeuibold");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				this.Exit();
			}

			if (OptionsComfirmed)
			{
				if (WindowSize != new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight))
				{
					graphics.PreferredBackBufferWidth = WindowSize.X;
					graphics.PreferredBackBufferHeight = WindowSize.Y;
				}

				graphics.SynchronizeWithVerticalRetrace = isVSync;

				graphics.ApplyChanges();

				if (!isBackgroundGenerated)
				{
					GenerateBackground();
				}

				OptionsComfirmed = false;

				OldIsOptionsChanged = isOptionsChanged;
				isOptionsChanged += 1;
			}

			// Tell if the game is in debug mode.
			if (GameDebug)
			{
				// Set the debug label's text.
				debugLabel.Text = debugStrings[0] + "\n" +
								  debugStrings[1] + "\n" +
								  debugStrings[2] + "\n" +
								  debugStrings[3] + "\n" +
								  debugStrings[4] + "\n" +
								  debugStrings[5] + "\n" +
								  debugStrings[6] + "\n" +
								  debugStrings[7] + "\n" +
								  debugStrings[8] + "\n" +
								  debugStrings[9] + "\n";
			}
			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			base.Draw(gameTime);
		}

		public void GenerateBackground()
		{
			Point screenTileSize = new Point((int)(WindowSize.X / 128) + 1, (int)(WindowSize.X / 128) + 1);

			for (int x = 0; x < screenTileSize.X; x++)
			{
				for (int y = 0; y < screenTileSize.Y; y++)
				{
					BackgroundTilesList.Add(new Tile(backgroundHeavy, new Vector2(x * 128, y * 128), 0, random.Next(743874982), Color.White));
				}
			}

			isBackgroundGenerated = true;
		}

		/// <summary>
		/// This sets the current scene or level that the game is at.
		/// </summary>
		/// <param name="level">The game level to change to.</param>
		public void SetCurrentLevel(GameLevels level)
		{
			if (currentGameLevel != level)
			{
				currentGameLevel = level;

				//splashScreenManager.Enabled = false;
				//splashScreenManager.Visible = false;
				mainMenuManager.Enabled = false;
				mainMenuManager.Visible = false;
				gameManager.Enabled = false;
				gameManager.Visible = false;
				optionsManager.Enabled = false;
				optionsManager.Visible = false;
				isBackgroundGenerated = false;
			}

			switch (currentGameLevel)
			{
				case GameLevels.SPLASH:
					//splashScreenManager.Enabled = true;
					//splashScreenManager.Visible = true;
					break;
				case GameLevels.MENU:
					mainMenuManager.Enabled = true;
					mainMenuManager.Visible = true;
					break;
				case GameLevels.OPTIONS:
					optionsManager.Enabled = true;
					optionsManager.Visible = true;
					break;
				case GameLevels.GAME:
					gameManager.Enabled = true;
					gameManager.Visible = true;
					break;
				default:
					break;
			}
		}
	}
}
