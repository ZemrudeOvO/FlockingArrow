using System.Numerics;
using Raylib_cs;

namespace FlockingArrow
{
	public class Boid
	{
        Vector2 position, velocity, acceleration;
        float r, maxForce, maxSpeed;

        public Boid(float x, float y)
		{
			acceleration = new Vector2(0, 0);
			Random random = new Random();
            float angle = random.NextSingle() * MathF.PI * 2;
            velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle));

            position = new Vector2(x, y);
            r = 4.0f;
            maxSpeed = 0.1f;//2.0f
            maxForce = 0.001f;//0.3f
        }
        public void run(List<Boid> boids)
        {
            flock(boids);
            update();
            borders();
            render();
            
        }

        void applyForce(Vector2 force)
        {
            acceleration += force;
        }

        void flock(List<Boid> boids)
        {
            Vector2 sep = separate(boids);
            Vector2 ali = align(boids);
            Vector2 coh = cohesion(boids);

            sep *= 1.5f;
            ali *= 1.0f;
            coh *= 1.0f;

            applyForce(sep);
            applyForce(ali);
            applyForce(coh);
	    }

        void update()
        {
            velocity += acceleration;
            velocity = Vector2.Normalize(velocity) * maxSpeed;
            position += velocity;
            acceleration *= 0;
	    }

        Vector2 seek(Vector2 target)
        {
            Vector2 desired = target - position;
            desired = Vector2.Normalize(desired);
            desired *= maxSpeed;
            Vector2 steer = desired - velocity;
            steer = Vector2.Normalize(steer) * maxForce;
            return steer;
	    }

        //TODO
        void render()
        {
            float theta = MathF.Acos(Vector2.Dot(Vector2.Normalize(velocity), Vector2.UnitX)) + MathF.PI / 2.0f;

            Rlgl.rlPushMatrix();
            Rlgl.rlTranslatef(position.X, position.Y, 0);
            Rlgl.rlRotatef(theta * 180 / MathF.PI, 0, 0, 1);
            //Raylib.DrawTriangle(new Vector2(0, -r * 2), new Vector2(-r, r * 2), new Vector2(r, r * 2), Color.WHITE);
            Raylib.DrawCircle(0, 0, r, Color.WHITE);
            Rlgl.rlPopMatrix();
	    }

        void borders()
        {
            if (position.X < -r) position.X = Raylib.GetScreenWidth() + r;
            if (position.Y < -r) position.Y = Raylib.GetScreenHeight() + r;
            if (position.X > Raylib.GetScreenWidth() + r) position.X = -r;
            if (position.Y > Raylib.GetScreenHeight() + r) position.Y = -r;
        }

        Vector2 separate(List<Boid> boids)
        {
            float desireSeparation = 25.0f;
            Vector2 steer = new Vector2(0, 0);
            int count = 0;
            foreach (Boid other in boids)
            {
                float d = Vector2.Distance(position, other.position);
                if ((d>0)&& (d<desireSeparation))
                {
                    Vector2 diff = position - other.position;
                    diff = Vector2.Normalize(diff);
                    diff /= d;
                    steer += diff;
                    count++;
		        }
	        }
            if (count > 0)
            {
                steer /= (float)count;
	        }
            if (steer.Length() > 0)
            {
                steer = Vector2.Normalize(steer);
                steer *= maxSpeed;
                steer -= velocity;
                steer = Vector2.Normalize(steer) * maxForce;
            }
            return steer;
        }

        Vector2 align(List<Boid> boids)
        {
            float neighbordist = 50.0f;
            Vector2 sum = Vector2.Zero;
            int count = 0;
            foreach (Boid other in boids)
            {
                float d = Vector2.Distance(position, other.position);
                if ((d>0)&&(d<neighbordist))
                {
                    sum += other.velocity;
                    count++;
		        }
	        }
            if (count > 0)
            {
                sum /= (float)count;
                sum = Vector2.Normalize(sum);
                sum *= maxSpeed;
                Vector2 steer = sum - velocity;
                steer = Vector2.Normalize(steer) * maxForce;
                return steer;
            }
            else
                return Vector2.Zero;
	    }

        Vector2 cohesion(List<Boid> boids)
        {
            float neighbordist = 50;
            Vector2 sum = Vector2.Zero;
            int cout = 0;
            foreach (Boid other in boids)
            {
                float d = Vector2.Distance(position, other.position);
                if ((d>0)&&(d<neighbordist))
                {
                    sum += other.position;
                    cout++;
		        }
	        }
            if (cout > 0)
            {
                sum /= cout;
                return seek(sum);
            }
            else
                return Vector2.Zero;
	    }
    }
}

