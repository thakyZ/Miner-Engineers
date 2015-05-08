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

namespace VoidEngine.Particles
{
	public static class ParticleSystem
	{
		public static void CreateParticles(Vector2 pos, Texture2D tex, Random rng, List<Particle> particleList, int redmin, int redmax, int greenmin, int greenmax, int bluemin, int bluemax, int minparticles, int maxparticles, int minangle, int maxangle, int minlifespan, int maxlifespan, int minspeed, int maxspeed, int alphamin, int alphamax)
		{
			Color washcolor = new Color(rng.Next(redmin, redmax), rng.Next(greenmin, greenmax), rng.Next(bluemin, bluemax), rng.Next(alphamin, alphamax));
			for (int i = 0; i < rng.Next(minparticles, maxparticles); i++)
			{
				particleList.Add(new Particle(pos, tex, rng.Next(minlifespan, maxlifespan), rng.Next(minspeed, maxspeed), washcolor, (float)(rng.NextDouble() * (maxangle - minangle) + minangle)));
			}
		}
	}
}
