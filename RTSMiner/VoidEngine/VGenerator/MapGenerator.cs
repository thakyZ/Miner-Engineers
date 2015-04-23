using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VoidEngine.VGenerator
{
	public static class MapGenerator
	{
		static Random _rnd = new Random();

		/// <summary>
		/// Creates a new, empty map
		/// </summary>
		/// <param name="width">The number of columns in the map</param>
		/// <param name="height">The number of rows in the map</param>
		/// <param name="percentageFillAtStart">How many per cent of the map is initially filled with tiles of differing heights (random 0-255)</param>
		/// <param name="roundsOfSmoothing">How many times to smooth the map</param>
		/// <returns>A new map consisting of heightvalues for the individual tiles</returns>
		public static byte[,] CreateMap(int width, int height, int roundsOfSmoothing = 8, float percentageFillAtStart = .1f)
		{
			//create the new map
			byte[,] map = new byte[width, height];

			int allCells = width * height;  //figure out how many dots to begin with
			int amountOfFirstFillCells = (int)(allCells * percentageFillAtStart);

			//add random heights to all the wanted cells
			for (int i = 0; i < amountOfFirstFillCells; i++)
			{
				Point p = map.GetRandomCellPosition();
				map[p.X, p.Y] = (byte)_rnd.Next(255);
			}

			map = SmoothMap(map, roundsOfSmoothing);

			return map;
		}

		/// <summary>
		/// Smooths the height values of a map
		/// </summary>
		/// <param name="map">The map to smooth</param>
		/// <param name="iterations">How many times to smooth the map over</param>
		/// <returns>A smoothed copy of the map</returns>
		public static byte[,] SmoothMap(byte[,] map, int iterations = 1)
		{
			for (int i = 0; i < iterations; i++)
			{
				//get a copy of the map to fill the smoothed version of the map into
				byte[,] destinationMap = (byte[,])map.Clone();

				map.GetAllPositions().ToList().ForEach(position => SmoothTile(map, destinationMap, position.X, position.Y));

				//get reference to the smoothed map
				map = destinationMap;
			}
			return map;
		}

		/// <summary>
		/// Smooths a tile by comparing the tile to its neighbors and adding/removing from the height of the tile
		/// It is necessary to write smoothed values to a new map to avoid skewing the results by using already smoothed neighbors for calculating
		/// </summary>
		/// <param name="sourceMap">The map to look at for original values</param>
		/// <param name="destinationMap">The map to store the smoothed tile to</param>
		/// <param name="x">X coordinate of tile to smooth</param>
		/// <param name="y">Y coordinate of tile to smooth</param>
		private static void SmoothTile(byte[,] sourceMap, byte[,] destinationMap, int x, int y)
		{
			//for calculating the average value of neighbors of a tile:
			float tileSum = 0;              //the sum of the height of the neighbors
			float numberOfNeighbors = 0;    //the number of neighbors for this tile

			//get the neigbors and go through them
			foreach (var neighbor in sourceMap.GetNeighborContents(x, y))
			{
				numberOfNeighbors++;                        //increase the number of neighbors found
				tileSum += neighbor;                        //and store the new sum of heights
			}

			//calculate the average of all neighbors
			float averageForNeighbors = tileSum / numberOfNeighbors;

			//find out what the difference between this tile and the neighbor average is
			float difference = averageForNeighbors - sourceMap[x, y];
			//introduce a little randomness
			float randomPct = Math.Abs(difference * .1f) * (_rnd.Next(6) - 2);
			//use a fifth of the difference to raise/lower + the randomness
			destinationMap[x, y] = (byte)MathHelper.Clamp((sourceMap[x, y] + difference * .2f + randomPct), 0, 255);
		}


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
				}
			}

			return mapArray;
		}

		public static byte[,] LoadMapWithHeight(string path)
		{

			try
			{
				string[] lines = File.ReadAllLines(path);
				string width = lines[0];
				string height = lines[1];
				width = width.Split('=')[1];
				height = height.Split('=')[1];
				int widthVal, heightVal;

				if (!int.TryParse(width, out widthVal))
				{
					throw new Exception("Error parsing the value '" + width + "' as an int");
				}
				if (!int.TryParse(height, out heightVal))
				{
					throw new Exception("Error parsing the value '" + width + "' as an int");
				}

				byte[,] map = new byte[widthVal, heightVal];

				for (int y = 2; y < lines.Length; y++)
				{
					string[] columns = lines[y].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					for (int x = 0; x < columns.Length; x++)
					{
						map[x, y] = byte.Parse(columns[x]);
					}
				}
				return map;
			}
			catch (Exception ex)
			{
				throw new Exception("Error loading map '" + path + "'. The error was: " + ex.ToString());
			}
		}
	}
}