using System;

namespace EduMaze {

    class Program {
        static void Main(string[] args) {

            String quizFile;

            if (args.Length == 0) quizFile = "set1.json";
            else quizFile = (String) args[0];

            Game thegame = new Game (quizFile);

            while (thegame.running) {
                thegame.Update ();
                thegame.Render ();
            }
        }
    }
}
