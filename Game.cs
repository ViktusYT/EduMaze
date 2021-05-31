using System;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace EduMaze {
    class Game {
        private Maze theMaze;
        private Player player;
        private RenderWindow gameWindow;

        private void InitVariables () {

        }

        private void InitWindow () {

            gameWindow = new RenderWindow (new VideoMode (800, 600), "EduMaze");
            gameWindow.SetFramerateLimit (60);
            gameWindow.SetVerticalSyncEnabled (true);

            gameWindow.KeyPressed += new EventHandler <KeyEventArgs> (KeyPressedHandler);
            gameWindow.Closed += new EventHandler (CloseEventHandler);
            gameWindow.Resized += new EventHandler<SizeEventArgs> (ResizeEventHandler);
        }

        private void InitObjects () {
            theMaze = new Maze (40, 20);
            player = new Player (new Vector2f (theMaze.Position.Item1 + 3.0f, theMaze.Position.Item2 + 3.0f));
        }

        private void ResizeEventHandler (object sender, SizeEventArgs e) {
            //gameWindow.Get = new Vector2u (e.Width, e.Height);
        }

        private void CloseEventHandler (object sender, EventArgs e) {
            gameWindow.Close();
        }

        private void KeyPressedHandler (object sender, KeyEventArgs e) {
            switch (e.Code) {
                case Keyboard.Key.Escape: 
                                            gameWindow.Close();
                                            break;
                case Keyboard.Key.W:
                                            theMaze.Go(Nd.MAZE_UP);
                                            break;
                case Keyboard.Key.D:
                                            theMaze.Go(Nd.MAZE_RIGHT);
                                            break;
                case Keyboard.Key.S:
                                            theMaze.Go(Nd.MAZE_DOWN);
                                            break;
                case Keyboard.Key.A:
                                            theMaze.Go(Nd.MAZE_LEFT);
                                            break;

            }
        }

        public void ProcessEvents () {
            gameWindow.DispatchEvents();
        }

        public Boolean running {
            get => gameWindow.IsOpen;
        }

        public Game () {
            
            InitVariables ();
            InitWindow ();
            InitObjects ();
        }

        public void Update () {
            player.UpdatePosition(new Vector2f (theMaze.Position.Item1 * 20.0f + 3.0f, theMaze.Position.Item2 * 20.0f + 3.0f));
        }

        public void Render () {

            gameWindow.Clear (Color.White);
            theMaze.Draw (ref gameWindow);
            player.Draw (ref gameWindow);
            gameWindow.Display ();
        }
    }
}