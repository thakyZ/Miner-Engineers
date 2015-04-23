using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VoidEngine.Helpers
{
	public static class RectangleHelper
	{
		public static bool TouchTopOf(this Rectangle r1, Rectangle r2)
		{
			return (r1.Bottom >= r2.Top - 1 &&
					r1.Bottom <= r2.Top + (r2.Height / 2) &&
					r1.Right >= r2.Left + (r2.Width / 5) &&
					r1.Left <= r2.Right - (r2.Width / 5));
		}
		public static bool TouchBottomOf(this Rectangle r1, Rectangle r2)
		{
			return (r1.Top <= r2.Bottom + 1 &&
					r1.Top >= r2.Bottom - (r2.Height / 2) &&
					r1.Right >= r2.Left + (r2.Width / 5) &&
					r1.Left <= r2.Right - (r2.Width / 5));
		}
		public static bool TouchLeftOf(this Rectangle r1, Rectangle r2)
		{
			return (r1.Right <= r2.Right &&
					r1.Right >= r2.Left - 1 &&
					r1.Top <= r2.Bottom - (r2.Height / 2.45f) &&
					r1.Bottom >= r2.Top + (r2.Height / 2.45f));
		}
		public static bool TouchRightOf(this Rectangle r1, Rectangle r2)
		{
			return (r1.Left >= r2.Left &&
					r1.Left <= r2.Right + 1 &&
					r1.Top <= r2.Bottom - (r2.Height / 2.45f) &&
					r1.Bottom >= r2.Top + (r2.Height / 2.45f));
		}
	}
}
