using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RTSMiner.Other
{
	public class MapHelper
	{
		public static int PlacedBlueHarvesters = 3;
		public static int PlacedBlueFighters = 0;
		public static int PlacedBlueRepairs = 0;
		public static int PlacedBlueHQs = 1;

		public static int PlacedRedHarvesters = 0;
		public static int PlacedRedFighters = 0;
		public static int PlacedRedRepairs = 0;
		public static int PlacedRedHQs = 0;

		public static Point HarvesterSize = new Point(1, 1);
		public static Point FighterSize = new Point(1, 1);
		public static Point RepairSize = new Point(1, 1);
		public static Point HQSize = new Point(3, 3);

		/// <summary>
		/// Saves the map to a file
		/// </summary>
		/// <param name="map">The map data to use</param>
		/// <param name="path">The filepath to use</param>
		/// <param name="oceanLevel">The ocean level to use. If it is zero a simple map with " " and "X" is generated for water/land.
		/// If oceanLevel is above zero a map with heights is generated</param>
		public static int[,] SaveMap(byte[,] map, byte oceanLevel)
		{
			int[,] mapArray = new int[map.GetLength(0), map.GetLength(1)];

			//write all tiles
			for (int y = 0; y < map.GetLength(1); y++)
			{
				for (int x = 0; x < map.GetLength(0); x++)
				{
					//writes number between -2 and 1 depending on range of height
					//Deep Water = -2
					//Shallow Water = -1
					//Land = 0
					// Mountain = 1

					if (map[x, y] > oceanLevel - 1)
					{
						int LandLevelTemp = 0;

						if (map[x, y] < 15)
						{
							LandLevelTemp = 1;
						}
						else if (map[x, y] >= 15 && map[x, y] < 20)
						{
							LandLevelTemp = 2;
						}
						else if (map[x, y] >= 20 && map[x, y] < 25)
						{
							LandLevelTemp = 3;
						}
						else if (map[x, y] >= 25 && map[x, y] < 30)
						{
							LandLevelTemp = 4;
						}
						else if (map[x, y] >= 30)
						{
							LandLevelTemp = 5;
						}

						mapArray[x, y] = LandLevelTemp;
					}
					else if (map[x, y] < oceanLevel)
					{
						mapArray[x, y] = 0;
					}

					if (PlacedBlueHarvesters > 0)
					{
						Point harvesterPlacement = new Point(x + 1, y + 1);

						if (CheckAreaAround(mapArray, harvesterPlacement, HarvesterSize, 0))
						{
							mapArray[(int)harvesterPlacement.X, (int)harvesterPlacement.Y] = -2;
							PlacedBlueHarvesters--;
						}

						if (mapArray[(int)harvesterPlacement.X, (int)harvesterPlacement.Y] != -2)
						{
							PlacedBlueHarvesters += 1;
						}
					}

					if (PlacedBlueHQs > 0)
					{
						Point basePlacement = new Point(x + 1, y + 1);

						if (CheckAreaAround(mapArray, basePlacement, HQSize, 0))
						{
							mapArray[(int)basePlacement.X, (int)basePlacement.Y] = -3;
							PlacedBlueHQs--;
						}

						if (CheckAreaAround(mapArray, basePlacement, HQSize, 0) && mapArray[(int)basePlacement.X, (int)basePlacement.Y] != -2)
						{
							PlacedBlueHQs += 1;
						}
					}
				}
			}

			return mapArray;
		}

		public static bool CheckAreaAround(int[,] mapArray, Point Placement, Point Range, int CheckedSpot)
		{
			int successes = 0;

			for (int i = 0; i < Range.X; i++)
			{
				for (int j = 0; j < Range.Y; j++)
				{
					if (mapArray[(int)MathHelper.Clamp(Placement.X + i, 0, mapArray.GetLength(1) - Range.X), (int)MathHelper.Clamp(Placement.Y + j, 0, mapArray.GetLength(0) - Range.Y)] == CheckedSpot)
					{
						successes += 1;
					}
				}
			}

			return successes >= Range.X * Range.Y;
		}
	}
}
