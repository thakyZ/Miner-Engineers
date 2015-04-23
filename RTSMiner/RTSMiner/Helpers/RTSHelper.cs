using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTSMiner.Helpers
{
	public class RTSHelper
	{
		/// <summary>
		/// The player's inventory
		/// </summary>
		public struct Inventory
		{
			/// <summary>
			/// The amount of Uranium in the player's inventory.
			/// </summary>
			public int Uranium = 1000;
			/// <summary>
			/// The amount of Stone in the player's inventory.
			/// </summary>
			public int Stone = 0;
			/// <summary>
			/// The amount of Iron in the player's inventory.
			/// </summary>
			public int Iron = 0;
			/// <summary>
			/// The amount of Gold in the player's ineventory.
			/// </summary>
			public int Gold = 0;
		}
		
		/// <summary>
		/// This is the behaviors of all the units.
		/// </summary>
		public enum UnitBehaviors
		{
			IDLE,
			HARVESTURANIUM,
			HARVESTSTONE,
			HARVESTIRON,
			HARVESTGOLD,
			BUILDHARVESTER,
			BUILDHQ
		}
		/// <summary>
		/// This is the different types of units.
		/// </summary>
		public enum UnitTypes
		{
			BLUEHARVESTER,
			BLUEFIGHTER,
			BLUEREPAIR,
			BLUEHQ,
			REDHARVESTER,
			REDFIGHTER,
			REDREPAIR,
			REDHQ
		}
		/// <summary>
		/// These are the different types of resources
		/// </summary>
		public enum ResourceTypes
		{
			ASTEROID,
			URANIUM,
			STONE,
			IRON,
			GOLD
		}

		/// <summary>
		/// The time to build an harvester.
		/// </sumamry>
		public int BuildHarvesterTime = 3000;
		/// <summary>
		/// The time it takes to build a HQ.
		/// </summary>
		public int BuildHQTime = 30000;
		/// <summary>
		/// The time it takes to build a fighter.
		/// </summary>
		public int BuildFighterTime = 3500;
		/// <summary>
		/// The time it takes to build a repair ship.
		/// </summary>
		public int BuildRepairTime = 4000;

		/// <summary>
		/// To flip the X and Y axis' of an array.
		/// </summary>
		/// <param name="a">The array to flip</summary>
		/// <returns>Returns the array fliped on it's 'z'/'w' axis</returns>
		public int[,] TransposeArray(int[,] a)
		{
			int[,] b = new int[a.GetLength(1), a.GetLength(0)];

			for (int i = 0; i < a.GetLength(0); i++)
			{
				for (int j = 0; j < a.GetLength(1); j++)
				{
					b[j, i] = a[i, j];
				}
			}

			return b;
		}
	}
}
