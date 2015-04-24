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

namespace VoidEngine.VGUI
{
	/// <summary>
	/// The button class for the VoidEngine
	/// </summary>
	public class Button : Sprite
	{
		/// <summary>
		/// This is the Void Engine button's state enum
		/// </summary>
		protected enum bState
		{
			HOVER,
			UP,
			DOWN,
			RELEASED
		}

		protected Label label; // This is the label that the button has overlayed on it.

		public string Text
		{
			get
			{
				return label.Text;
			}
			set
			{
				label.Text = value;
			}
		}

		protected bState buttonState = new bState(); // This is the button state variable
		protected bool mousePress = false; // This
		protected bool previousMousePress = false;
		protected Point mouseCords;
		protected Rectangle testCollision;

		/// <summary>
		/// Creates a button.
		/// When creating the button's animation set,
		/// the normal button texture has to be called "IDLE",
		/// the hover button texture has to be called "HOVER",
		/// the pressed button texture has to be called "PRESSED".
		/// </summary>
		/// <param name="position">The starting position of the button.</param>
		/// <param name="font">The font to use in the button</param>
		/// <param name="scale">The scale of the text in the button</param>
		/// <param name="fontColor">The color of the font to use in the button.</param>
		/// <param name="text">The text inside the button</param>
		/// <param name="color">The color to mask the button texture with.</param>
		/// <param name="animationSetList">The animation set list to use on the button.</param>
		public Button(Vector2 position, SpriteFont font, float scale, Color fontColor, string text, Color color, List<Sprite.AnimationSet> animationSetList)
			: base(position, color, animationSetList)
		{
			Color = color;
			AnimationSets = animationSetList;
			label = new Label(new Vector2(position.X + ((animationSetList[0].frameSize.X - font.MeasureString(text).X) / 2), position.Y + ((animationSetList[0].frameSize.Y - font.MeasureString(text).Y) / 2)), font, scale, fontColor, text);
		}

		public Button(Texture2D texture, Vector2 position, SpriteFont font, float scale, Color fontColor, string text, Color buttonColor) : base(position, buttonColor, texture)
		{
			Color = buttonColor;
			AddAnimations(texture);
			label = new Label(new Vector2(position.X + (((texture.Width / 3) - font.MeasureString(text).X) / 2), position.Y + ((texture.Height - font.MeasureString(text).Y) / 2)), font, scale, fontColor, text);
		}

		/// <summary>
		/// Returns if the button is hovered over or not.
		/// </summary>
		/// <param name="tx">The texture of the button</param>
		/// <param name="ty">The texture's y</param>
		/// <param name="frameTex">the texture's width and height in Point</param>
		/// <param name="x">the x of mouse</param>
		/// <param name="y">the y of mouse</param>
		/// <returns>Boolean</returns>
		protected Boolean hitButtonAlpha(Vector2 textureSize, Point frameSize, Point mouseCords)
		{
			testCollision = new Rectangle((int)textureSize.X, (int)textureSize.Y, frameSize.X, frameSize.Y);

			if (testCollision.Intersects(new Rectangle(mouseCords.X, mouseCords.Y, 1, 1)))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Updates the button with the mouse's cords and state
		/// </summary>
		/// <param name="gameTime">The game time that the game runs off of.</param>
		public override void Update(GameTime gameTime)
		{
			MouseState mState = Mouse.GetState();

			mouseCords = new Point(mState.X, mState.Y);
			previousMousePress = mousePress;
			mousePress = mState.LeftButton == ButtonState.Pressed;

			label.Position = new Vector2(position.X + ((CurrentAnimation.frameSize.X - label.GetFont.MeasureString(label.Text).X) / 2), position.Y + ((CurrentAnimation.frameSize.Y - label.GetFont.MeasureString(label.Text).Y) / 2));

			if (hitButtonAlpha(position, CurrentAnimation.frameSize, mouseCords))
			{
				if (mousePress)
				{
					buttonState = bState.DOWN;
					SetAnimation("PRESSED");
				}
				else if (!mousePress && previousMousePress)
				{
					if (buttonState == bState.DOWN)
					{
						buttonState = bState.RELEASED;
						SetAnimation("IDLE");
					}
				}
				else
				{
					buttonState = bState.HOVER;
					SetAnimation("HOVER");
				}
			}
			else
			{
				buttonState = bState.UP;
				SetAnimation("IDLE");
			}
		}

		/// <summary>
		/// Returns weither if the button was clicked.
		/// </summary>
		/// <returns>Boolean</returns>
		public bool Clicked()
		{
			if (buttonState == bState.RELEASED)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used to draw the button.
		/// </summary>
		/// <param name="gameTime">The game time that the game runs off of.</param>
		/// <param name="spriteBatch">The sprite batch used to draw with.</param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			label.Draw(gameTime, spriteBatch);
		}

		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE", texture, new Point(texture.Width / 3, texture.Height), new Point(1, 1), new Point(0, 0), 1600, false);
			AddAnimation("HOVER", texture, new Point(texture.Width / 3, texture.Height), new Point(1, 1), new Point(texture.Width / 3, 0), 1600, false);
			AddAnimation("PRESSED", texture, new Point(texture.Width / 3, texture.Height), new Point(1, 1), new Point((texture.Width / 3) * 2, 0), 1600, false);
			SetAnimation("IDLE");

			base.AddAnimations(texture);
		}
	}
}