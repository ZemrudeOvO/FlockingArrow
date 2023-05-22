using Raylib_cs;

namespace FlockingArrow
{
	public class Flock
	{
		List<Boid> boids;

		public Flock()
		{
			boids = new List<Boid>();
		}

		public void run()
		{ 
			foreach (Boid b in boids)
			{
				b.run(boids);
			}
		}

		public void addBoid(Boid b)
		{
			boids.Add(b);
		}
	}
}

