using System;

using SFML.System;
using SFML.Graphics;

namespace EduMaze {
    class Button : IDrawable, ISelectable {

        private RectangleShape body;
        private Text answerText;
        private String answerString;
        private Font font;
        public event EventHandler<int> Clicked;
        private int num;
        private int howManyQuestions;
        private int correct;

        public Button (int num) {

            this.correct = 0;
            this.num = num;
            this.howManyQuestions = 0;
        
            body = new RectangleShape (new Vector2f (30.0f, 30.0f));
            body.FillColor = Color.White;
            body.OutlineColor = Color.Black;
            body.OutlineThickness = 3.0f;
            
            font = new Font ("font/Roboto-Black.ttf");

            answerText = new Text ("Temp", font, 10);
            answerText.FillColor = Color.Black;

            Render = false;
        }

        public void SetContent (String str, float width, Vector2f position, int howManyQuestions, int correct) {
            
            this.correct = correct;
            Render = false;
            this.howManyQuestions = howManyQuestions;

            body.Size = new Vector2f (width, 30.0f);
            body.Position = position;

            answerString = str;
            answerText.DisplayedString = str;
            answerText.Position = body.Position + new Vector2f (5.0f, 5.0f);
        }

        public bool Render { get; set; }

        public void Draw (ref RenderWindow window) {
            if (Render && (num < howManyQuestions)) {
                window.Draw(body);
                window.Draw(answerText);
            }
        }
        public void OnClick () {
            if (Render && (num < howManyQuestions)) {
                SendClickedSignal();
            }
        }
        public void OnHover () {
            if (Render && (num < howManyQuestions))
                body.FillColor = new Color (222, 210, 209);
        }
        public void OnUnhover () {
            body.FillColor = Color.White;
        }

        public bool Contains (Vector2f coords) {
            return body.GetGlobalBounds().Contains(coords.X, coords.Y);
        }

        protected virtual void SendClickedSignal () {
            Clicked?.Invoke(this, num);
        }
    }
}