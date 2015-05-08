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
using VoidEngine.VGame;

namespace RTSMiner.Other
{
	public class Cursor : Sprite
	{
		/// <summary>
		/// The previous Mouse state.
		/// </summary>
		MouseState previousMouseState;

		/// <summary>
		/// Creates the cursor object.
		/// </summary>
		/// <param name="texture">The texture of the cursor</param>
		/// <param name="position">The position of the cursor to start with</param>
		public Cursor(Texture2D texture, Vector2 position)
			: base(position, Color.White, texture)
		{
			// Add the default animations.
			AddAnimations(texture);
		}

		/// <summary>
		/// Update the cursor.
		/// </summary>
		/// <param name="gameTime">The GameTime that the game runs off of.</summary>
		public override void Update(GameTime gameTime)
		{
			// Get the mouse state.
			MouseState currMouseState = Mouse.GetState();
			// Set the position of the mouse.
			position = new Vector2(currMouseState.X, currMouseState.Y);
			// Set the previous mouse state so we can get a new one.
			previousMouseState = currMouseState;

			base.Update(gameTime);
		}

		/// <summary>
		/// Draw the cursor.
		/// </summary>
		/// <param name="gameTime">The GameTime that the game runs off of.</param>
		/// <param name="spriteBatch">The SpriteBatch that the game uses to draw with.</param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
		}

		/// <summary>
		/// Add the animations of the cursor.
		/// </summary>
		/// <param name="texture">The texture of the cursor.</param>
		protected override void AddAnimations(Texture2D texture)
		{
			AnimationSets.Add(new AnimationSet("IDLE", texture, new Point(20, 20), new Point(0, 0), new Point(0, 0), 1600, false));
			/// Set the animation to the first one.
			SetAnimation("IDLE");
		}
	}
}
