using System.Numerics;
using Raylib_cs;

namespace FlockingArrow
{
	public class Flocking
	{

		public static void Main()
		{
			Raylib.InitWindow(640, 360, "Flocking");
            Flock flock = new Flock();

            for (int i = 0;i<150;i++)
			{
				flock.addBoid(new Boid(Raylib.GetScreenWidth() / 2, Raylib.GetScreenHeight() / 2));
			}

			while (!Raylib.WindowShouldClose())
			{
				Raylib.BeginDrawing();

				Raylib.ClearBackground(new Color(0,0,0,1));

				flock.run();

				if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
					flock.addBoid(new Boid(Raylib.GetMousePosition().X, Raylib.GetMousePosition().Y));

				Raylib.EndDrawing();
			}
		}
	}
}

