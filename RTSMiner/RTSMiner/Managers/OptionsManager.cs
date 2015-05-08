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
using VoidEngine.VGame;
using VoidEngine.VGUI;


namespace RTSMiner.Managers
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class OptionsManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Game1 myGame;
		SpriteBatch spriteBatch;

		Button ApplyButton;
		Button CancelButton;

		Button ToggleVsync;
		Button DecreaseWindowSize;
		Button IncreaseWindowSize;

		Label WindowSizeLabel;

		Point windowSize = new Point();

		Point[] windowSizeOptions = new Point[3];

		int CurrentWindowSizeOption;

		bool Vsync;

		public OptionsManager(Game1 game)
			: base(game)
		{
			myGame = game;

			// TODO: Construct any child components here
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

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

			windowSize = myGame.WindowSize;

			myGame.GenerateBackground();

			ApplyButton = new Button(myGame.button1Texture, new Vector2(((myGame.WindowSize.X - (myGame.button1Texture.Width / 3) - 200) / 2), myGame.WindowSize.Y - 80), myGame.segoeUIBold, 1.0f, new Color(0, 160, 56), "APPLY", Color.White);
			CancelButton = new Button(myGame.button1Texture, new Vector2(((myGame.WindowSize.X - (myGame.button1Texture.Width / 3) + 200) / 2), myGame.WindowSize.Y - 80), myGame.segoeUIBold, 1.0f, new Color(0, 160, 56), "CANCEL", Color.White);
			ToggleVsync = new Button(myGame.button1Texture, new Vector2(((myGame.WindowSize.X - (myGame.button1Texture.Width / 3) - 250) / 2), ((myGame.WindowSize.Y - myGame.button1Texture.Height - 100) / 2)), myGame.segoeUIBold, 1.0f, new Color(0, 160, 56), "VSync: False", Color.White);
			IncreaseWindowSize = new Button(myGame.button2Texture, new Vector2(((myGame.WindowSize.X - (myGame.button2Texture.Width / 3) + 200) / 2), ((myGame.WindowSize.Y - myGame.button1Texture.Height - 100) / 2)), myGame.segoeUIBold, 1.0f, new Color(0, 160, 56), "+", Color.White);
			DecreaseWindowSize = new Button(myGame.button2Texture, new Vector2(((myGame.WindowSize.X - (myGame.button2Texture.Width / 3) + 400) / 2), ((myGame.WindowSize.Y - myGame.button1Texture.Height - 100) / 2)), myGame.segoeUIBold, 1.0f, new Color(0, 160, 56), "-", Color.White);
			WindowSizeLabel = new Label(new Vector2(((myGame.WindowSize.X - (myGame.button2Texture.Width / 3)) / 2) + 290, 280), myGame.segoeUIBold, 1.0f, new Color(0, 160, 56), windowSize.X.ToString() + "x" + windowSize.Y.ToString());

			windowSizeOptions[0] = new Point(800, 480);
			windowSizeOptions[1] = new Point(1200, 720);
			windowSizeOptions[2] = new Point(1600, 960);

			base.LoadContent();
		}

		protected void LoadImages()
		{

		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			ApplyButton.Update(gameTime);
			CancelButton.Update(gameTime);
			ToggleVsync.Update(gameTime);
			IncreaseWindowSize.Update(gameTime);
			DecreaseWindowSize.Update(gameTime);
			myGame.cursor.Update(gameTime);

			if (ApplyButton.Clicked())
			{
				myGame.isVSync = Vsync;
				myGame.WindowSize = windowSize;
				myGame.OptionsComfirmed = true;
				myGame.SetCurrentLevel(Game1.GameLevels.MENU);
			}

			if (CancelButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.MENU);
			}

			if (ToggleVsync.Clicked())
			{
				Vsync = !Vsync;
				ToggleVsync.Text = "VSync: " + Vsync;
			}

			if (IncreaseWindowSize.Clicked())
			{
				CurrentWindowSizeOption += 1;
				if (CurrentWindowSizeOption > windowSizeOptions.Length - 1)
				{
					CurrentWindowSizeOption = 0;
				}

				windowSize = windowSizeOptions[CurrentWindowSizeOption];

				WindowSizeLabel.Text = windowSize.X + "x" + windowSize.Y;
			}

			if (DecreaseWindowSize.Clicked())
			{
				CurrentWindowSizeOption -= 1;
				if (CurrentWindowSizeOption < 0)
				{
					CurrentWindowSizeOption = windowSizeOptions.Length - 1;
				}

				windowSize = windowSizeOptions[CurrentWindowSizeOption];

				WindowSizeLabel.Text = windowSize.X + "x" + windowSize.Y;
			}

			foreach (Tile t in myGame.BackgroundTilesList)
			{
				t.Update(gameTime);
			}

			if (myGame.isOptionsChanged > myGame.OldIsOptionsChanged)
			{
				ApplyButton.Position = new Vector2(((myGame.WindowSize.X - (myGame.button1Texture.Width / 3) - 200) / 2), myGame.WindowSize.Y - 80);
				CancelButton.Position = new Vector2(((myGame.WindowSize.X - (myGame.button1Texture.Width / 3) + 200) / 2), myGame.WindowSize.Y - 80);
				ToggleVsync.Position = new Vector2(((myGame.WindowSize.X - (myGame.button1Texture.Width / 3) - 250) / 2), ((myGame.WindowSize.Y - myGame.button1Texture.Height - 100) / 2));
				IncreaseWindowSize.Position = new Vector2(((myGame.WindowSize.X - (myGame.button2Texture.Width / 3) + 200) / 2), ((myGame.WindowSize.Y - myGame.button1Texture.Height - 100) / 2));
				DecreaseWindowSize.Position = new Vector2(((myGame.WindowSize.X - (myGame.button2Texture.Width / 3) + 500) / 2), ((myGame.WindowSize.Y - myGame.button1Texture.Height - 100) / 2));
				WindowSizeLabel.Position = new Vector2(((myGame.WindowSize.X - (myGame.button2Texture.Width / 3) + 290) / 2), ((myGame.WindowSize.Y - myGame.button1Texture.Height - 100) / 2));
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			{
				foreach (Tile t in myGame.BackgroundTilesList)
				{
					t.Draw(gameTime, spriteBatch);
				}

				ApplyButton.Draw(gameTime, spriteBatch);
				CancelButton.Draw(gameTime, spriteBatch);
				ToggleVsync.Draw(gameTime, spriteBatch);
				IncreaseWindowSize.Draw(gameTime, spriteBatch);
				DecreaseWindowSize.Draw(gameTime, spriteBatch);
				WindowSizeLabel.Draw(gameTime, spriteBatch);
				myGame.cursor.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
