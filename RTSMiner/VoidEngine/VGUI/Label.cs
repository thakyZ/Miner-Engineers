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

namespace VoidEngine.VGUI
{
	/// <summary>
	/// The Label class for the VoidEngine
	/// </summary>
	public class Label
	{
		public string Text;
		protected SpriteFont texture;
		public SpriteFont GetFont
		{
			get
			{
				return texture;
			}
		}
		public Vector2 Position;
		public float Scale;
		public Color Color;
		/// <summary>
		/// Creates the Label.
		/// </summary>
		/// <param name="text">The text that will be in the label.</param>
		public Label(Vector2 position, SpriteFont texture, float scale, Color color, string text)
		{
			Text = text;
			this.texture = texture;
			Position = position;
			Scale = scale;
			Color = color;
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			//Vector2 FontOrigin = texture.MeasureString(text) / 2;
			// Draw the string
			spriteBatch.DrawString(texture, Text, Position, Color, 0, new Vector2(0, 0), Scale, SpriteEffects.None, 0.5f);
		}
	}
}