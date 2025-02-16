﻿using System;

namespace EduMaze {

    class Program {
        static void Main(string[] args) {

            String quizFile;

            if (args.Length == 0) quizFile = "examples/set1.json";
            else quizFile = (String) args[0];

            Game thegame = new Game (quizFile);

            while (thegame.Running) {
                thegame.Update ();
                thegame.Render ();
            }
        }
    }
}
