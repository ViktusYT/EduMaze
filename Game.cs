using System;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace EduMaze {
    class Game {

        private Maze theMaze;
        private RenderWindow gameWindow;
        //private Event ev;

        private void initVariables () {

        }
        private void initWindow () {
            gameWindow = new RenderWindow (new VideoMode (800, 600), "EduMaze");
            gameWindow.SetFramerateLimit (60);
            gameWindow.SetVerticalSyncEnabled (true);
        }
        private void initObjects () {

        }


        public Boolean running {
            get => gameWindow.IsOpen;
        }

        public Game (ref Maze maze) {
            theMaze = maze;
            
            initVariables ();
            initWindow ();
            initObjects ();
        }

        public void update () {

        }

        public void render () {
            //Console.WriteLine ("Jesteś w punkcie: " + theMaze.Position);

            /*int state = (int) theMaze.State;
            Console.WriteLine ("Dostępne ścieżki: ");
            if ((state & 1) == 1) Console.WriteLine ("1. W górę");
            if (((state >> 1) & 1) == 1) Console.WriteLine ("2. W prawo");
            if (((state >> 2) & 1) == 1) Console.WriteLine ("3. W dół");
            if (((state >> 3) & 1) == 1) Console.WriteLine ("4. W lewo");

            int direction = 0;
            try {
                String kierunek = Console.ReadLine();
                direction = Int32.Parse(kierunek);
            } catch (System.FormatException) {
                ;
            }

            Console.WriteLine ("Odczytano: " + direction.ToString());

            if (direction <= 4 && direction >= 0 && ((state >> (direction - 1)) & 1) == 1) theMaze.go (direction - 1);*/

            gameWindow.Clear (Color.White);
            gameWindow.Display ();
        }
    }
}