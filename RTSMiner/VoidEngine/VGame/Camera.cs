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

namespace VoidEngine.VGame
{
	public class Camera
	{
		const float zoomUpperLimit = 1.5f;
		const float zoomLowerLimit = 0.5f;

		float zoom;
		Matrix transform;
		Vector2 position;
		float rotation;
		public Vector2 viewportSize;
		Point worldSize;
		Vector2 screenCenter;
		Vector2 origin;

		/// <summary>
		/// Creates the camera.
		/// </summary>
		/// <param name="viewport">The viewport that the game uses.</param>
		/// <param name="worldSize">The size of the world.</param>
		/// <param name="initalZoom">The starting zoom position.</param>
		public Camera(Viewport viewport, Point worldSize, float initialZoom)
		{
			zoom = initialZoom;
			rotation = 0.0f;
			position = Vector2.Zero;
			viewportSize = new Vector2(viewport.Width, viewport.Height);
			this.worldSize = worldSize;
			screenCenter = new Vector2(viewport.Width / 2, viewport.Height / 2);
			origin = screenCenter / zoom;
		}

		/// <summary>
		/// Gets or sets the zoom of the camera.
		/// </summary>
		public float Zoom
		{
			get
			{
				return zoom;
			}
			set
			{
				zoom = value;

				if (zoom < zoomLowerLimit)
				{
					zoom = zoomLowerLimit;
				}
				if (zoom > zoomUpperLimit)
				{
					zoom = zoomUpperLimit;
				}
			}
		}

		/// <summary>
		/// Gets or sets the rotation of the camera.
		/// </summary>
		public float Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				rotation = value;
			}
		}

		/// <summary>
		/// Gets or sets the world size.
		/// <summary>
		public Point Size
		{
			get
			{
				return worldSize;
			}
			set
			{
				worldSize.X = value.X;
				worldSize.Y = value.Y;
			}
		}

		/// <summary>
		/// Moves the camera by an amount.
		/// </summary>
		public void Move(Vector2 amount)
		{
			position += amount;
		}

		/// <summary>
		/// Gets or sets the cameras position.
		/// </summary>
		public Vector2 Position
		{
			get
			{
				return position;
			}
			set
			{
				Rectangle limits = new Rectangle((int)(0), (int)(0), (int)(worldSize.X - viewportSize.X * 1.5f), (int)(worldSize.Y - viewportSize.Y * 1.5f));
				position = value;
				position = new Vector2(MathHelper.Clamp(position.X, limits.X, limits.Right), MathHelper.Clamp(position.Y, limits.Y, limits.Bottom));
			}
		}

		/// <summary>
		/// Gets the matrix transformation of the camera.
		/// </summary>
		public Matrix GetTransformation()
		{
			transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)); //* Matrix.CreateTranslation(new Vector3(viewportSize.X, viewportSize.Y, 0));

			return transform;
		}

		/// <summary>
		/// Determines whether the target is in view given the specified position.
		/// This can be used to increase performance by not drawing objects
		/// directly in the viewport
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="texture">The texture.</param>
		/// <returns>
		///     <c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsInView(Vector2 position, Vector2 textureSize)
		{
			// If the object is not within the horizontal bounds of the screen

			if ((position.X + textureSize.X) < (this.position.X - origin.X) || (position.X) > (this.position.X + origin.X))
			{
				return false;
			}

			// If the object is not within the vertical bounds of the screen
			if ((position.Y + textureSize.Y) < (this.position.Y - origin.Y) || (position.Y) > (this.position.Y + origin.Y))
			{
				return false;
			}

			// In View
			return true;
		}

	}
}
