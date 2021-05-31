using System;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace EduMaze {

    class Player : IDrawable {

        private RectangleShape body;

        public Player (Vector2f position) {

            body = new RectangleShape (new Vector2f (14.0f, 14.0f));
            body.FillColor = Color.Red;
            body.Position = position;
        }

        public void UpdatePosition (Vector2f position) {
            body.Position = position;
        }

        public void Draw (ref RenderWindow window) {
            window.Draw (body);
        }

    }
}