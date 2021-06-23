using System;
using System.Collections;
using System.Collections.Generic;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace EduMaze {
    class MazeNode : IDrawable {

        Nd state;
        Tuple <int, int> location;
        private RectangleShape body;
        private RectangleShape bodyTop;
        private RectangleShape bodyRight;
        private RectangleShape bodyBottom;
        private RectangleShape bodyLeft;
        private LinkedList<RectangleShape> black;
        private LinkedList<RectangleShape> white;
        private Texture questionTexture;

        public Nd State {
            get => state;
            set {
                state |= value;
            }
        }

        public Boolean Visited {
            get => (state & Nd.MAZE_VISITED) != 0;
        }

        public Tuple <int, int> Location {
            get => location;
        }

        public MazeNode (int x, int y, ref Texture questionTexture) {

            this.questionTexture = questionTexture;
            state = 0;
            location = new Tuple <int, int> (x, y);

            black = new LinkedList<RectangleShape>();
            white = new LinkedList<RectangleShape>();

            body = new RectangleShape (new Vector2f (16.0f, 16.0f));
            body.Position = new Vector2f ((float)location.Item1 * 20.0f + 2.0f, (float)location.Item2 * 20.0f + 2.0f);

            bodyTop = new RectangleShape (new Vector2f (20.0f, 2.0f));
            bodyTop.Position = new Vector2f ((float)location.Item1 * 20.0f, (float)location.Item2 * 20.0f);

            bodyRight = new RectangleShape (new Vector2f (2.0f, 20.0f));
            bodyRight.Position = new Vector2f ((float)location.Item1 * 20.0f + 18.0f, (float)location.Item2 * 20.0f);

            bodyBottom = new RectangleShape (new Vector2f (20.0f, 2.0f));
            bodyBottom.Position = new Vector2f ((float)location.Item1 * 20.0f, (float)location.Item2 * 20.0f + 18.0f);

            bodyLeft = new RectangleShape (new Vector2f (2.0f, 20.0f));
            bodyLeft.Position = new Vector2f ((float)location.Item1 * 20.0f, (float)location.Item2 * 20.0f);
        }

        public void SetBorders () {

            if ((State & Nd.MAZE_UP) > 0) {
                bodyTop.FillColor = Color.White;
                white.AddLast (bodyTop);
            }
            else { 
                bodyTop.FillColor = Color.Black;
                black.AddLast (bodyTop);
            }

            if ((State & Nd.MAZE_RIGHT) > 0) {
                bodyRight.FillColor = Color.White;
                white.AddLast (bodyRight);
            }
            else {
                bodyRight.FillColor = Color.Black;
                black.AddLast (bodyRight);
            }
                
            if ((State & Nd.MAZE_DOWN) > 0) {
                bodyBottom.FillColor = Color.White;
                white.AddLast (bodyBottom);
            }
            else { 
                bodyBottom.FillColor = Color.Black;
                black.AddLast (bodyBottom);
            }

            if ((State & Nd.MAZE_LEFT) > 0) {
                bodyLeft.FillColor = Color.White;
                white.AddLast (bodyLeft);
            }
            else {
                bodyLeft.FillColor = Color.Black;
                black.AddLast (bodyLeft);
            }

            if ((State & Nd.MAZE_QUESTION) > 0) body.Texture = questionTexture;
            else body.FillColor = Color.White;
        }

        public void Draw (ref RenderWindow window) {
            
            window.Draw (body);

            foreach (var body in black) {
                window.Draw (body);
            }
        }

        public void UnQuestion() {
            state = (Nd)((int)state & 0xFFFFFFDF);

            body = new RectangleShape (new Vector2f (16.0f, 16.0f));
            body.Position = new Vector2f ((float)location.Item1 * 20.0f + 2.0f, (float)location.Item2 * 20.0f + 2.0f);
            body.FillColor = Color.White;
        }

    }
    class Maze : IDrawable {
        // Maze generator alghorithm!
        private MazeNode[,] nodes;
        private Tuple <int, int> position;
        private Stack <MazeNode> dfsStack;
        private Texture questionTexture;
        private int width;
        private int height;
        public event EventHandler QuestionEntered;
        public event EventHandler FinishEntered;

        protected virtual void SendQuestionEnteredSignal (EventArgs e) {
            QuestionEntered?.Invoke(this, e);
        }

        protected virtual void SendFinishEnteredSignal (EventArgs e) {
            FinishEntered?.Invoke(this, e);
        }

        public Maze (int width, int height) {

            this.width = width;
            this.height = height;

            nodes = new MazeNode [height, width];
            dfsStack = new Stack <MazeNode> ();
            questionTexture = new Texture ("img/question.png");

            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    nodes [i, j] = new MazeNode (j, i, ref questionTexture);
                }
            }

            position = new Tuple <int, int> (0, 0);

            dfsStack.Push (nodes [0, 0]);
            nodes[0, 0].State = Nd.MAZE_VISITED;
            Dfs(30);

            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    nodes [i, j].SetBorders ();
                }
            }
        }

        public ref MazeNode GetNode(){
            return ref nodes [position.Item2, position.Item1];
        }

        public Tuple <int, int> Position {
            get => position;
            set {
                if (value.Item1 >= 0 && value.Item1 < this.width && value.Item2 >= 0 && value.Item2 < this.height)
                    position = value;
            }
        }

        public Nd State {
            get => nodes [position.Item2, position.Item1].State;
        }

        public Boolean Question {
            get => (State & Nd.MAZE_QUESTION) > 0;
            set {
                if (value) nodes [position.Item2, position.Item1].State = Nd.MAZE_QUESTION;
                else nodes [position.Item2, position.Item1].UnQuestion();
            }
        }

        public void Go (Nd direction) {

            if (((int)State & (int)direction) > 0) {
                switch (direction) {
                    case Nd.MAZE_UP: 
                        position = new Tuple <int, int> (position.Item1, position.Item2 - 1);
                        break;
                    case Nd.MAZE_RIGHT:
                        position = new Tuple <int, int> (position.Item1 + 1, position.Item2);
                        break;
                    case Nd.MAZE_DOWN:
                        position = new Tuple <int, int> (position.Item1, position.Item2 + 1);
                        break;
                    case Nd.MAZE_LEFT:
                        position = new Tuple <int, int> (position.Item1 - 1, position.Item2);
                        break;
                }
            }
            
            if (Position.Item1 == 39 && Position.Item2 == 19) SendFinishEnteredSignal (null);
            else if ((State & Nd.MAZE_QUESTION) > 0) SendQuestionEnteredSignal (null);
        }

        public void Draw (ref RenderWindow window) {
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    nodes [i, j].Draw (ref window);
                }
            }
        }

        private Nd ReversedNd (Nd temp) {
            switch (temp) {
                case Nd.MAZE_UP: return Nd.MAZE_DOWN;
                case Nd.MAZE_RIGHT: return Nd.MAZE_LEFT;
                case Nd.MAZE_DOWN: return Nd.MAZE_UP;
                case Nd.MAZE_LEFT: return Nd.MAZE_RIGHT;
                default: return temp;
            }
        }

        private void Swap (ref Tuple <MazeNode, Nd> o1, ref Tuple <MazeNode, Nd> o2) {
            var temp = o1;
            o1 = o2;
            o2 = temp;
        }

        private void Shuffle (Tuple <MazeNode, Nd> [] array, int n) {
            Random random = new Random();

            while (n > 1) {
                n--;
                int i = random.Next (n + 1);
                Swap (ref array[i], ref array[n]);
            }
        }

        private void Dfs (int q) {

            MazeNode temp = dfsStack.Peek();
            dfsStack.Pop ();

            if (q-- <= 0) {
                temp.State = Nd.MAZE_QUESTION;
                q = 30;
            }

            Tuple <MazeNode, Nd>[] shuffle = new Tuple <MazeNode, Nd> [4];

            int n = 0;

            if (temp.Location.Item2 - 1 >= 0)
                shuffle[n++] = new Tuple<MazeNode, Nd> (nodes[temp.Location.Item2 - 1, temp.Location.Item1], Nd.MAZE_UP);
            if (temp.Location.Item1 + 1 < this.width)
                shuffle[n++] = new Tuple<MazeNode, Nd> (nodes[temp.Location.Item2, temp.Location.Item1 + 1], Nd.MAZE_RIGHT);
            if (temp.Location.Item2 + 1 < this.height)
                shuffle[n++] = new Tuple<MazeNode, Nd> (nodes[temp.Location.Item2 + 1, temp.Location.Item1], Nd.MAZE_DOWN);
            if (temp.Location.Item1 - 1 >= 0)
                shuffle[n++] = new Tuple<MazeNode, Nd> (nodes[temp.Location.Item2, temp.Location.Item1 - 1], Nd.MAZE_LEFT);
                
            Shuffle (shuffle, n);

            for (int i = 0; i < n; i++) {

                var element = shuffle[i];

                if (!element.Item1.Visited) {
                    temp.State = element.Item2;
                    element.Item1.State = ReversedNd (element.Item2) | Nd.MAZE_VISITED;
                    dfsStack.Push (element.Item1);
                    Dfs(q);
                }
            }
        }
    }
}