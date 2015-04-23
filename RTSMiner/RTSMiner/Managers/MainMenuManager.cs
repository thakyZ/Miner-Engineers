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

		Texture2D buttonTexture;
		Texture2D backgroundHeavy;
		SpriteFont segoeUIBold;

		Button PlayButton;

		Point screenTileSize;

		List<Tile> TilesList = new List<Tile>();

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

			PlayButton = new Button(buttonTexture, new Vector2((myGame.WindowSize.X - buttonTexture.Width) / 2, 150), segoeUIBold, 1.0f, new Color(0, 160, 58), "PLAY", Color.White);

			screenTileSize = new Point((int)(myGame.WindowSize.X / 128) + 1, (int)(myGame.WindowSize.X / 128) + 1);

			for (int x = 0; x < screenTileSize.X; x++)
			{
				for (int y = 0; y < screenTileSize.Y; y++)
				{
					TilesList.Add(new Tile(backgroundHeavy, new Vector2(x * 128, y * 128), 0, random.Next(743874982), Color.White));
				}
			}

			base.LoadContent();
		}

		protected void LoadImages()
		{
			buttonTexture = Game.Content.Load<Texture2D>(@"images\gui\button2");
			backgroundHeavy = Game.Content.Load<Texture2D>(@"images\gui\backgroundHeavy");
			segoeUIBold = Game.Content.Load<SpriteFont>(@"fonts\segoeuibold");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// TODO: Add your update code here
			PlayButton.Update(gameTime);
			myGame.cursor.Update(gameTime);

			if (PlayButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.GAME);
			}

			foreach (Tile t in TilesList)
			{
				t.Update(gameTime);
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			{
				foreach (Tile t in TilesList)
				{
					t.Draw(gameTime, spriteBatch);
				}

				PlayButton.Draw(gameTime, spriteBatch);
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
