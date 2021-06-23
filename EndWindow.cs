using System;

using SFML.System;
using SFML.Graphics;

namespace EduMaze {
    class EndWindow : IDrawable { 
        private RectangleShape body;
        private Font font;
        private Text text;
        public bool Render { get; set; }
        public EndWindow () {

            Render = false;
            font = new Font ("font/Roboto-Black.ttf");

            body = new RectangleShape (new Vector2f (400.0f, 200.0f));
            body.FillColor = Color.White;
            body.OutlineColor = Color.Black;
            body.OutlineThickness = 3.0f;
            body.Position = new Vector2f (200.0f, 200.0f);

            text = new Text ("Temp", font, 20);
            text.Position = body.Position + new Vector2f (4.0f, 4.0f);
            text.FillColor = Color.Black;
        }
        public void SetContent (bool win) {

            Render = false;

            if (win) text.DisplayedString = "Brawo! Wygrałxś!\n\n\nNaciśnij ESC aby zakończyć.";
            else text.DisplayedString = "Koniec gry!\n\n\nNaciśnij ESC aby zakończyć.";
        }
        
        public void Draw (ref RenderWindow window) {

            if (Render) {
                window.Draw(body);
                window.Draw(text);
            }
        }
    }
}