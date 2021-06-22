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
        private Question questionWindow;
        private EndWindow monit;
        private List<Layer> layers;
        private Vector2i mousePositionWindow;
        private Vector2f mousePositionView;
        private ISelectable pointed;
        private bool movementBlocked;

        private void InitVariables () {
            layers = new List<Layer>();
            pointed = null;
            movementBlocked = false;
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

        private void InitObjects (String quizFile) {

            theMaze = new Maze (40, 20);
            theMaze.QuestionEntered += QuestionEnteredHandler;
            theMaze.FinishEntered += FinishEnteredHandler;
            layers.Add (new Layer (theMaze));

            player = new Player (new Vector2f (theMaze.Position.Item1 + 3.0f, theMaze.Position.Item2 + 3.0f));
            player.ZeroHealth += GameOverHandler;
            layers.Add (new Layer (player));

            finish = new Finish (new Vector2f (39 * 20.0f + 3.0f, 19 * 20.0f + 3.0f));
            layers[1].AddObject(finish);

            questionSet = new QuestionSet (quizFile);

            questionWindow = new Question ();
            questionWindow.Answered += QuestionAnsweredHandler;
            layers.Add (new Layer (questionWindow));

            monit = new EndWindow ();
            layers[2].AddObject(monit);

            layers.Add (new Layer (questionWindow.GetButtons()));
        }

        private void ResizeEventHandler (object sender, SizeEventArgs e) {

            FloatRect visibleArea = new FloatRect (0.0f, 0.0f, e.Width, e.Height);
            gameWindow.SetView(new View (visibleArea));
        }

        private void CloseEventHandler (object sender, EventArgs e) {
            gameWindow.Close();
        }

        private void KeyPressedHandler (object sender, KeyEventArgs e) {
            switch (e.Code) {
                case Keyboard.Key.Escape: 
                                            gameWindow.Close();
                                            break;
                case Keyboard.Key.Up:
                case Keyboard.Key.W:
                                            if (!movementBlocked) theMaze.Go(Nd.MAZE_UP);
                                            break;
                case Keyboard.Key.Right:
                case Keyboard.Key.D:
                                            if (!movementBlocked) theMaze.Go(Nd.MAZE_RIGHT);
                                            break;
                case Keyboard.Key.Down:
                case Keyboard.Key.S:
                                            if (!movementBlocked) theMaze.Go(Nd.MAZE_DOWN);
                                            break;
                case Keyboard.Key.Left:
                case Keyboard.Key.A:
                                            if (!movementBlocked) theMaze.Go(Nd.MAZE_LEFT);
                                            break;

            }
        }

        private void MouseButtonReleasedHandler (object sender, MouseButtonEventArgs e) {
            switch (e.Button) {
                case Mouse.Button.Left:
                                            if (pointed != null) pointed.OnClick();
                                            break;
            }
        }

        private void QuestionEnteredHandler (object sender, EventArgs e) {
            QuestionPrototype temp = questionSet.GetQuestion();
            questionWindow.SetContent (temp.Question, temp.Answers, temp.Correct);
            questionWindow.Render = true;
        }
        
        private void FinishEnteredHandler (object sender, EventArgs e) {
            monit.SetContent(true);
            monit.Render = true;
        }
        private void GameOverHandler (object sender, EventArgs e) {
            monit.SetContent(false);
            monit.Render = true;
        }

        private void QuestionAnsweredHandler (object sender, bool result) {
            if (!result) player.takeOneLife();

            questionWindow.Render = false;
            questionWindow.UnHoverButtons();

            questionSet.NextQuestion();
            theMaze.Question = false;
        }

        private void ProcessEvents () {
            gameWindow.DispatchEvents();
        }

        private void UpdateMousePosition () {
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

        public Game (String quizFile) {
            
            InitVariables ();
            InitWindow ();
            InitObjects (quizFile);
        }

        public void Update () {

            UpdateMousePosition();
            ProcessEvents();
            player.UpdatePosition(new Vector2f (theMaze.Position.Item1 * 20.0f + 3.0f, theMaze.Position.Item2 * 20.0f + 3.0f));

            if (theMaze.Question || monit.Render) movementBlocked = true;
            else movementBlocked = false;
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