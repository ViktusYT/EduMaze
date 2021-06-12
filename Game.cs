using System;
using System.Collections.Generic;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace EduMaze {

    class Layer {
        private List <IDrawable> objects;

        public Layer () {
            objects = new List<IDrawable>();
        }

        public Layer (IDrawable obj) {
            objects = new List<IDrawable>();
            AddObject (obj);
        }
        public Layer (IDrawable [] obj) {
            objects = new List<IDrawable>();
            AddObject (obj);
        }

        public void AddObject (IDrawable obj) {
            objects.Add(obj);
        }
        public void AddObject (IDrawable [] obj) {
            foreach (var i in obj)
                objects.Add(i);
        }

        public void Draw (ref RenderWindow window) {
            foreach (var i in objects) 
                i.Draw(ref window);
        }

        public ISelectable GiveSelectable(Vector2f mousePositionView) {
            ISelectable temp = null;

            foreach (var i in objects) {
                temp = i as ISelectable;

                if (temp != null && temp.Contains(mousePositionView)) return temp;
            }

            return null;
        }
    }

    class Game {
        private Maze theMaze;
        private Player player;
        private Finish finish;
        private RenderWindow gameWindow;
        private QuestionSet questionSet;
        private List<Layer> layers;
        private Vector2i mousePositionWindow;
        private Vector2f mousePositionView;
        private ISelectable pointed;
        private bool inQuestion;

        private void InitVariables () {
            layers = new List<Layer>();
            pointed = null;
            inQuestion = false;
        }

        private void InitWindow () {

            gameWindow = new RenderWindow (new VideoMode (800, 600), "EduMaze");
            gameWindow.SetFramerateLimit (60);
            gameWindow.SetVerticalSyncEnabled (true);

            gameWindow.KeyPressed += new EventHandler <KeyEventArgs> (KeyPressedHandler);
            gameWindow.Closed += new EventHandler (CloseEventHandler);
            gameWindow.Resized += new EventHandler<SizeEventArgs> (ResizeEventHandler);
            gameWindow.MouseButtonReleased += new EventHandler<MouseButtonEventArgs> (MouseButtonReleasedHandler);
        }

        private void InitObjects () {

            theMaze = new Maze (40, 20);
            layers.Add (new Layer (theMaze));
            player = new Player (new Vector2f (theMaze.Position.Item1 + 3.0f, theMaze.Position.Item2 + 3.0f));
            finish = new Finish (new Vector2f (39 * 20.0f + 3.0f, 19 * 20.0f + 3.0f));
            layers.Add (new Layer (player));
            layers[1].AddObject(finish);

            questionSet = new QuestionSet ("set1.json");

            layers.Add(new Layer (questionSet.GetQuestion()));
            layers.Add(new Layer (questionSet.GetQuestion().GetButtons()));
        }

        private void ResizeEventHandler (object sender, SizeEventArgs e) {
            //gameWindow.Get = new Vector2u (e.Width, e.Height);
        }

        private void CloseEventHandler (object sender, EventArgs e) {
            gameWindow.Close();
        }

        private void KeyPressedHandler (object sender, KeyEventArgs e) {
            if (!inQuestion) {
                switch (e.Code) {
                    case Keyboard.Key.Escape: 
                                                gameWindow.Close();
                                                break;
                    case Keyboard.Key.Up:
                    case Keyboard.Key.W:
                                                theMaze.Go(Nd.MAZE_UP);
                                                break;
                    case Keyboard.Key.Right:
                    case Keyboard.Key.D:
                                                theMaze.Go(Nd.MAZE_RIGHT);
                                                break;
                    case Keyboard.Key.Down:
                    case Keyboard.Key.S:
                                                theMaze.Go(Nd.MAZE_DOWN);
                                                break;
                    case Keyboard.Key.Left:
                    case Keyboard.Key.A:
                                                theMaze.Go(Nd.MAZE_LEFT);
                                                break;

                }
            }
        }

        private void MouseButtonReleasedHandler (object sender, MouseButtonEventArgs e) {
            switch (e.Button) {
                case Mouse.Button.Left:
                                            if (pointed != null) pointed.OnClick();
                                            break;
            }
        }

        public void ProcessEvents () {
            gameWindow.DispatchEvents();
        }

        private void updateMousePosition () {
            mousePositionWindow = Mouse.GetPosition(gameWindow);
            mousePositionView = gameWindow.MapPixelToCoords(mousePositionWindow);

            ISelectable temp = GiveSelectable();

            if (temp == null && pointed != null) {
                pointed.OnUnhover();
                pointed = null;
            }
            else if (temp != null && pointed == null) {
                pointed = temp;
                pointed.OnHover();
            }
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
            updateMousePosition();
            ProcessEvents();
            player.UpdatePosition(new Vector2f (theMaze.Position.Item1 * 20.0f + 3.0f, theMaze.Position.Item2 * 20.0f + 3.0f));

            if (theMaze.Question) {
                inQuestion = true;
                questionSet.QuestionSignal();
                questionSet.CheckQuestion(ref theMaze.GetNode());
            }
            else {
                inQuestion = false;
            }
        }

        private ISelectable GiveSelectable() {
            
            ISelectable t = null;
            int len = layers.Count;
            for (int i = len - 1; i >= 0; i--) {
                t = layers[i].GiveSelectable(mousePositionView);  
                if (t != null) return t; 
            }

            return null;
        }

        public void Render () {
            gameWindow.Clear (Color.White);

            foreach (var i in layers)
                i.Draw(ref gameWindow);

            gameWindow.Display ();
        }
    }
}