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
	public class MainMenuManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Game1 myGame;
		SpriteBatch spriteBatch;

		Random random = new Random();

		Button PlayButton;
		Button OptionsButton;
		Button ExitButton;

		public MainMenuManager(Game1 game)
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

			LoadImages();

			PlayButton = new Button(myGame.button1Texture, new Vector2((myGame.WindowSize.X - (myGame.button1Texture.Width / 3)) / 2, 150), myGame.segoeUIBold, 1.0f, new Color(0, 160, 58), "PLAY", Color.White);
			OptionsButton = new Button(myGame.button1Texture, new Vector2((myGame.WindowSize.X - (myGame.button1Texture.Width / 3)) / 2, 230), myGame.segoeUIBold, 1.0f, new Color(0, 160, 58), "OPTIONS", Color.White);
			ExitButton = new Button(myGame.button1Texture, new Vector2((myGame.WindowSize.X - (myGame.button1Texture.Width / 3)) / 2, 310), myGame.segoeUIBold, 1.0f, new Color(0, 160, 58), "EXIT", Color.White);

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
			// TODO: Add your update code here
			PlayButton.Update(gameTime);
			OptionsButton.Update(gameTime);
			ExitButton.Update(gameTime);
			myGame.cursor.Update(gameTime);

			if (PlayButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.GAME);
			}

			if (OptionsButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.OPTIONS);
			}

			if (ExitButton.Clicked())
			{
				myGame.Exit();
			}

			foreach (Tile t in myGame.BackgroundTilesList)
			{
				t.Update(gameTime);
			}

			if (myGame.isOptionsChanged > myGame.OldIsOptionsChanged)
			{
				PlayButton.Position = new Vector2((myGame.WindowSize.X - (myGame.button1Texture.Width / 3)) / 2, 150);
				OptionsButton.Position = new Vector2((myGame.WindowSize.X - (myGame.button1Texture.Width / 3)) / 2, 230);
				ExitButton.Position = new Vector2((myGame.WindowSize.X - (myGame.button1Texture.Width / 3)) / 2, 310);
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

				PlayButton.Draw(gameTime, spriteBatch);
				OptionsButton.Draw(gameTime, spriteBatch);
				ExitButton.Draw(gameTime, spriteBatch);
				myGame.cursor.Draw(gameTime, spriteBatch);

				if (myGame.GameDebug)
				{
					myGame.debugLabel.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
