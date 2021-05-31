namespace EduMaze {

    class Program {
        static void Main(string[] args) {

            Maze maze = new Maze (40, 20);

            Game thegame = new Game (ref maze);

            while (thegame.running) {
                thegame.ProcessEvents ();
                thegame.Update ();
                thegame.Render ();
            }
        }
    }
}
