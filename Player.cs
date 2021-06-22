using System;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace EduMaze {

    class Player : IDrawable {

        private RectangleShape body;
        private Texture texture;
        private Text healthText;
        private Font font;
        private int health;
        public event EventHandler ZeroHealth;
        
        public int Health {
            get => health;
        }

        public Player (Vector2f position) {

            body = new RectangleShape (new Vector2f (14.0f, 14.0f));
            texture = new Texture ("player.png");
            
            body.Texture = texture;
            body.Position = position;

            font = new Font ("Roboto-Black.ttf");

            health = 3;

            healthText = new Text ("Pozostało żyć: " + health.ToString(), font, 30);
            healthText.Position = new Vector2f (15.0f, 420.0f);
            healthText.FillColor = Color.Black;
        }

        public void takeOneLife () {
            health--;
            healthText.DisplayedString = "Pozostało żyć: " + health.ToString();

            if (health == 0) SendZeroHealthSignal (null);
        }

        public void UpdatePosition (Vector2f position) {
            body.Position = position;
        }

        public void Draw (ref RenderWindow window) {
            window.Draw (body);
            window.Draw (healthText);
        }
        protected virtual void SendZeroHealthSignal (EventArgs e) {
            ZeroHealth?.Invoke(this, e);
        }
    }

    class Finish : IDrawable {
        private RectangleShape body;
        private Texture texture;

        public Finish (Vector2f position) {

            body = new RectangleShape (new Vector2f (14.0f, 14.0f));
            texture = new Texture ("finish.png");

            body.Texture = texture;
            body.Position = position;
        }

        public void Draw (ref RenderWindow window) {
            window.Draw (body);
        }
    }
}