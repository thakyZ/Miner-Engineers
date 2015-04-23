using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VoidEngine.Helpers
{
	/// <summary>
	/// This class doesn't always like to work, use as your own descetion.
	/// </summary>
	public class CollisionHelper
	{
		public static float Magnitude(Vector2 vector)
		{
			return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
		}

		public static Vector2 VectorNormal(Vector2 vector)
		{
			return new Vector2(-vector.Y, vector.X);
		}

		public static Vector2 UnitVector(Vector2 vector)
		{
			return new Vector2(vector.X / (float)Magnitude(vector), vector.Y / (float)Magnitude(vector));
		}

		public static float DotProduct(Vector2 unitVector, Vector2 vector)
		{
			return unitVector.X * vector.X + unitVector.Y * vector.Y;
		}

		public static Vector2 ReflectedVector(Vector2 vector, Vector2 reflectVector)
		{
			Vector2 normal = VectorNormal(reflectVector);
			float coeficient = -2 * (DotProduct(vector, normal) / (Magnitude(normal) * Magnitude(normal)));
			Vector2 r;
			r.X = vector.X + coeficient * normal.X;
			r.Y = vector.Y + coeficient * normal.Y;
			return r;
		}
	}
}