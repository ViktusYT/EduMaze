using System;
using System.Collections;

namespace EduMaze {

    public enum Nd {
        MAZE_UP = 0x1,
        MAZE_RIGHT = 0x2,
        MAZE_DOWN = 0x4,
        MAZE_LEFT = 0x8,
        MAZE_VISITED = 0x10
    }
    class Maze {
        // Maze generator alghorithm!
        private class MazeNode {

            Nd state;
            Tuple <int, int> location;

            public Nd State {
                get => state;
                set {
                    state = state | value;
                }
            }

            public Boolean Visited {
                get => ((int)state >> 4) == 1;
            }

            public Tuple <int, int> Location {
                get => location;
            }

            public MazeNode (int x, int y) {
                state = 0;
                location = new Tuple<int, int> (x, y);
            }
        }

        private MazeNode[,] Nodes;

        private Tuple <int, int> position;

        private Stack dfsStack;

        private int width;
        private int height;

        public Maze (int width, int height) {

            this.width = width;
            this.height = height;

            Nodes = new MazeNode [height, width];
            dfsStack = new Stack();

            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    Nodes [i, j] = new MazeNode (j, i);
                }
            }

            dfsStack.Push(Nodes[0, 0]);
            Nodes[0, 0].State = Nd.MAZE_VISITED;
            position = new Tuple<int, int>(0, 0);


            dfs();
        }

        public Tuple <int, int> Position {
            get => position;
            set {
                if (value.Item1 >= 0 && value.Item1 < this.width && value.Item2 >= 0 && value.Item2 < this.height)
                    position = value;
            }
        }

        public Nd State {
            get => Nodes[position.Item2, position.Item1].State;
        }

        public void primitivePrint() {
            for (int i = 0; i < this.height; i++) {
                for (int j = 0; j < this.width; j++) {
                    Console.WriteLine("x: " + j.ToString() + ", y: " + i.ToString() + ", Wartość: " + (int)Nodes[i, j].State);
                }
            }
        }

        public void go (int direction) {
            if ((((int)State >> direction) & 1) == 1) {
                switch (direction) {
                    case 0: 
                        position = new Tuple <int, int> (position.Item1, position.Item2 - 1);
                        break;
                    case 1:
                        position = new Tuple <int, int> (position.Item1 + 1, position.Item2);
                        break;
                    case 2:
                        position = new Tuple <int, int> (position.Item1, position.Item2 + 1);
                        break;
                    case 3:
                        position = new Tuple <int, int> (position.Item1 - 1, position.Item2);
                        break;
                }
            }
        }

        private void swap (ref Tuple <MazeNode, Nd> o1, ref Tuple <MazeNode, Nd> o2) {
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
                int i = random.Next(n + 1);
                swap (ref array[i], ref array[n]);
            }
        }

        private Nd reversedND (Nd temp) {
            switch (temp) {
                case Nd.MAZE_UP: return Nd.MAZE_DOWN;
                case Nd.MAZE_RIGHT: return Nd.MAZE_LEFT;
                case Nd.MAZE_DOWN: return Nd.MAZE_UP;
                case Nd.MAZE_LEFT: return Nd.MAZE_RIGHT;
                default: return temp;
            }
        }

        private void dfs() {
            MazeNode temp = (MazeNode) dfsStack.Peek();

            Tuple <MazeNode, Nd>[] shuffle = new Tuple <MazeNode, Nd> [4];

            int n = 0;

            if (temp.Location.Item2 - 1 >= 0)
                shuffle[n++] = new Tuple<MazeNode, Nd> (Nodes[temp.Location.Item2 - 1, temp.Location.Item1], Nd.MAZE_UP);
            if (temp.Location.Item1 + 1 < this.width)
                shuffle[n++] = new Tuple<MazeNode, Nd> (Nodes[temp.Location.Item2, temp.Location.Item1 + 1], Nd.MAZE_RIGHT);
            if (temp.Location.Item2 + 1 < this.height)
                shuffle[n++] = new Tuple<MazeNode, Nd> (Nodes[temp.Location.Item2 + 1, temp.Location.Item1], Nd.MAZE_DOWN);
            if (temp.Location.Item1 - 1 >= 0)
                shuffle[n++] = new Tuple<MazeNode, Nd> (Nodes[temp.Location.Item2, temp.Location.Item1 - 1], Nd.MAZE_LEFT);
            
            Shuffle(shuffle, n);

            for (int i = 0; i < n; i++) {
                var element = shuffle[i];
                if (!element.Item1.Visited) {
                    temp.State = element.Item2;
                    element.Item1.State = reversedND (element.Item2) | Nd.MAZE_VISITED;
                    dfsStack.Push(element.Item1);
                    dfs();
                }
            }

            dfsStack.Pop();
        }
    }

    class Game {

        private Maze theMaze;
        private Boolean exitSignal;

        public Boolean Exit {
            get => exitSignal;
        }

        public Game (ref Maze maze) {
            theMaze = maze;
            exitSignal = false;
        }

        public void update() {

        }

        public void render () {
            Console.WriteLine("Jesteś w punkcie: " + theMaze.Position);

            int state = (int)theMaze.State;
            Console.WriteLine("Dostępne ścieżki: ");
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

            if (direction <= 4 && direction >= 0 && ((state >> (direction - 1)) & 1) == 1) theMaze.go(direction - 1);
        }
    }

    class Program {
        static void Main(string[] args) {

            Maze labirynt = new Maze(40, 20);

            Console.WriteLine("Hello World!");

            Game thegame = new Game (ref labirynt);

            while (!thegame.Exit) {
                thegame.update();
                thegame.render();
            }
        }
    }
}
