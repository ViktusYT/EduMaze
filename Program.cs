namespace EduMaze {

    class Program {
        static void Main(string[] args) {

            Game thegame = new Game ();

            while (thegame.running) {
                thegame.Update ();
                thegame.Render ();
            }
        }
    }
}
