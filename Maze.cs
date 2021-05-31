using System;
using System.Collections;
using System.Collections.Generic;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace EduMaze {
    class Maze : IDrawable {
        // Maze generator alghorithm!
        private class MazeNode : IDrawable {

            Nd state;
            Tuple <int, int> location;

            private RectangleShape bodyTop;
            private RectangleShape bodyRight;
            private RectangleShape bodyBottom;
            private RectangleShape bodyLeft;

            private LinkedList<RectangleShape> black;
            private LinkedList<RectangleShape> white;

            public Nd State {
                get => state;
                set {
                    state = state | value;
                }
            }

            public Boolean Visited {
                get => ((int) state >> 4) == 1;
            }

            public Tuple <int, int> Location {
                get => location;
            }

            public MazeNode (int x, int y) {
                state = 0;
                location = new Tuple <int, int> (x, y);

                black = new LinkedList<RectangleShape>();
                white = new LinkedList<RectangleShape>();

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
            }

            public void Draw (ref RenderWindow window) {
                
                foreach (var body in black) {
                    window.Draw (body);
                }
            }

        }

        private MazeNode[,] nodes;
        private Tuple <int, int> position;
        private Stack dfsStack;

        private int width;
        private int height;

        public Maze (int width, int height) {

            this.width = width;
            this.height = height;

            nodes = new MazeNode [height, width];
            dfsStack = new Stack ();

            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    nodes [i, j] = new MazeNode (j, i);
                }
            }

            dfsStack.Push (nodes [0, 0]);
            nodes[0, 0].State = Nd.MAZE_VISITED;
            position = new Tuple <int, int> (0, 0);

            Dfs();

            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    nodes [i, j].SetBorders ();
                }
            }
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

        public void PrimitivePrint () {
            for (int i = 0; i < this.height; i++) {
                for (int j = 0; j < this.width; j++) {
                    Console.WriteLine ("x: " + j.ToString() + ", y: " + i.ToString() + ", Wartość: " + (int)nodes[i, j].State);
                }
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
        }

        public void Draw (ref RenderWindow window) {
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    nodes [i, j].Draw (ref window);
                }
            }
        }

        private void Swap (ref Tuple <MazeNode, Nd> o1, ref Tuple <MazeNode, Nd> o2) {
            var temp = o1;
            o1 = o2;
            o2 = temp;
        }

        private void Shuffle(Tuple <MazeNode, Nd> [] array, int n)
        {
            Random random = new Random();

            while (n > 1)
            {
                n--;
                int i = random.Next (n + 1);
                Swap (ref array[i], ref array[n]);
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

        private void Dfs() {
            MazeNode temp = (MazeNode) dfsStack.Peek();

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
                    Dfs ();
                }
            }

            dfsStack.Pop ();
        }
    }
}