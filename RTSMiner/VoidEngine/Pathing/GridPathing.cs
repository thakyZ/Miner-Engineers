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

namespace VoidEngine.Pathing
{
	public static class GridPathing
	{
		public static Vector2 aStar(Point source, Point target, List<string> map)
		{
			int mapWidth = map[0].Length;
			int mapHeight = map.Count;
			List<Vector3> pathGrid = new List<Vector3>();
			bool[,] checkedPath = new bool[mapWidth, mapHeight];
			pathGrid.Add(new Vector3(target.X, target.Y, 0));
			bool pathFound = false;
			int i = 0;

			while (i < pathGrid.Count)
			{

				if (pathGrid[i].Y > 0)
				{
					if (pathGrid[i].Y - 1 == source.Y && pathGrid[i].X == source.X)
					{
						checkedPath[(int)pathGrid[i].X, (int)pathGrid[i].Y - 1] = true;
						pathFound = true;
						return new Vector2(0, 1);
					}
					else if (map[(int)pathGrid[i].Y - 1][(int)pathGrid[i].X] == '.' && !checkedPath[(int)pathGrid[i].X, (int)pathGrid[i].Y - 1])
					{
						pathGrid.Add(new Vector3(pathGrid[i].X, pathGrid[i].Y - 1, pathGrid[i].Z));
						checkedPath[(int)pathGrid[i].X, (int)pathGrid[i].Y - 1] = true;
					}
				}


				if (pathGrid[i].Y < mapHeight - 1 && !pathFound)
				{
					if (pathGrid[i].Y + 1 == source.Y && pathGrid[i].X == source.X)
					{
						checkedPath[(int)pathGrid[i].X, (int)pathGrid[i].Y + 1] = true;
						pathFound = true;
						return new Vector2(0, -1);
					}
					else if (map[(int)pathGrid[i].Y + 1][(int)pathGrid[i].X] == '.' && !checkedPath[(int)pathGrid[i].X, (int)pathGrid[i].Y + 1])
					{
						pathGrid.Add(new Vector3(pathGrid[i].X, pathGrid[i].Y + 1, pathGrid[i].Z));
						checkedPath[(int)pathGrid[i].X, (int)pathGrid[i].Y + 1] = true;
					}
				}


				if (pathGrid[i].X > 0 && !pathFound)
				{
					if (pathGrid[i].Y == source.Y && pathGrid[i].X - 1 == source.X)
					{
						checkedPath[(int)pathGrid[i].X - 1, (int)pathGrid[i].Y] = true;
						pathFound = true;
						return new Vector2(1, 0);
					}
					else if (map[(int)pathGrid[i].Y][(int)pathGrid[i].X - 1] == '.' && !checkedPath[(int)pathGrid[i].X - 1, (int)pathGrid[i].Y])
					{
						pathGrid.Add(new Vector3(pathGrid[i].X - 1, pathGrid[i].Y, pathGrid[i].Z));
						checkedPath[(int)pathGrid[i].X - 1, (int)pathGrid[i].Y] = true;
					}
				}



				if (pathGrid[i].X < mapWidth - 1 && !pathFound)
				{
					if (pathGrid[i].Y == source.Y && pathGrid[i].X + 1 == source.X)
					{
						checkedPath[(int)pathGrid[i].X + 1, (int)pathGrid[i].Y] = true;
						pathFound = true;
						return new Vector2(-1, 0);
					}
					else if (map[(int)pathGrid[i].Y][(int)pathGrid[i].X + 1] == '.' && !checkedPath[(int)pathGrid[i].X + 1, (int)pathGrid[i].Y])
					{
						pathGrid.Add(new Vector3(pathGrid[i].X + 1, pathGrid[i].Y, pathGrid[i].Z));
						checkedPath[(int)pathGrid[i].X + 1, (int)pathGrid[i].Y] = true;
					}
				}

				i++;
			}

			return Vector2.Zero;
		}
	}
}