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
		float rotationX;
		float rotationY;
		float rotationZ;
		public Vector2 viewportSize;
		Point worldSize;
		public Vector2 screenCenter;
		public Vector2 origin;

		/// <summary>
		/// Creates the camera.
		/// </summary>
		/// <param name="viewport">The viewport that the game uses.</param>
		/// <param name="worldSize">The size of the world.</param>
		/// <param name="initalZoom">The starting zoom position.</param>
		public Camera(Viewport viewport, Point worldSize, float initialZoom)
		{
			zoom = initialZoom;
			rotationX = 0.0f * (float)Math.PI / 180;
			rotationY = 0.0f * (float)Math.PI / 180;
			rotationZ = 0.0f * (float)Math.PI / 180;
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

				zoom = MathHelper.Clamp(zoom, zoomLowerLimit, zoomUpperLimit);
			}
		}

		/// <summary>
		/// Gets or sets the rotation of the camera.
		/// </summary>
		public float RotationX
		{
			get
			{
				return rotationX;
			}
			set
			{
				rotationX = value;
			}
		}

		/// <summary>
		/// Gets or sets the rotation of the camera.
		/// </summary>
		public float RotationY
		{
			get
			{
				return rotationY;
			}
			set
			{
				rotationY = value;
			}
		}

		/// <summary>
		/// Gets or sets the rotation of the camera.
		/// </summary>
		public float RotationZ
		{
			get
			{
				return rotationZ;
			}
			set
			{
				rotationZ = value;
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
				Rectangle limits = new Rectangle((int)(viewportSize.X / 2 / zoom), (int)(viewportSize.Y / 2 / zoom), (int)(worldSize.X - (viewportSize.X / zoom)), (int)(worldSize.Y - (viewportSize.Y / zoom)));
				position = value;
				position = new Vector2(MathHelper.Clamp(position.X, limits.Left, limits.Right), MathHelper.Clamp(position.Y, limits.Top, limits.Bottom));
			}
		}

		public Rectangle Bounds
		{
			get
			{
				return new Rectangle((int)(viewportSize.X / 2 / zoom), (int)(viewportSize.Y / 2 / zoom), (int)(worldSize.X - (viewportSize.X / 0.5f / zoom)), (int)(worldSize.Y - (viewportSize.Y / 0.5f / zoom)));
			}
		}

		/// <summary>
		/// Gets the matrix transformation of the camera.
		/// </summary>
		public Matrix GetTransformation()
		{
			transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) * Matrix.CreateRotationX(RotationX) * Matrix.CreateRotationY(RotationY) * Matrix.CreateRotationZ(RotationZ) * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) * Matrix.CreateTranslation(new Vector3(viewportSize.X * 0.5f, viewportSize.Y * 0.5f, 0));

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
