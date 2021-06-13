using System;

using SFML.System;
using SFML.Graphics;

namespace EduMaze {

    class Button : IDrawable, ISelectable {

        private RectangleShape body;
        private Text answerText;
        private String answerString;
        private Font font;
        private bool signal;
        public event EventHandler<int> Clicked;
        private int num;
        private int howManyQuestions;

        public Button (int num) {

            this.num = num;
            this.howManyQuestions = 0;
            signal = false;

            body = new RectangleShape (new Vector2f (30.0f, 30.0f));
            body.FillColor = Color.White;
            body.OutlineColor = Color.Black;
            body.OutlineThickness = 3.0f;
            
            font = new Font ("Roboto-Black.ttf");

            answerText = new Text ("Temp", font, 10);
            answerText.FillColor = Color.Black;
        }

        public void SetContent (String str, float width, Vector2f position, int howManyQuestions) {
            
            signal = false;
            this.howManyQuestions = howManyQuestions;

            body.Size = new Vector2f (width, 30.0f);
            body.Position = position;

            answerString = str;
            answerText.DisplayedString = str;
            answerText.Position = body.Position + new Vector2f (5.0f, 5.0f);
        }

        public void SetSignal (bool sig) {
            signal = sig;
        }

        public void Draw (ref RenderWindow window) {
            if (signal && (num < howManyQuestions)) {
                window.Draw(body);
                window.Draw(answerText);
            }
        }
        public void OnClick () {

            if (signal && (num < howManyQuestions))
                SendClickedSignal();
        }
        public void OnHover () {

            if (signal && (num < howManyQuestions))
                body.FillColor = Color.Green;
        }
        public void OnUnhover () {
            if (signal && (num < howManyQuestions))
                body.FillColor = Color.White;
        }

        public bool Contains (Vector2f coords) {
            return body.GetGlobalBounds().Contains(coords.X, coords.Y);
        }

        protected virtual void SendClickedSignal () {
            Clicked?.Invoke(this, num);
        }
    }
    class Question : IDrawable { 

        private RectangleShape body;
        private Font questionFont;
        private Text questionText;
        private String questionString;
        private Button [] answersButtons;
        private String [] answersString;
        private int correctAnswer;
        private int howManyQuestions;
        private bool signal;
        public event EventHandler<bool> Answered;
        protected virtual void SendAnsweredSignal (bool result) {
            Answered?.Invoke(this, result);
        }

        public void SetSignal(bool sig) {
            signal = sig;

            foreach (var i in answersButtons) i.SetSignal(sig);
        }

        public IDrawable [] GetButtons() {
            return answersButtons;
        }

        public Question () {
            
            signal = false;

            answersButtons = new Button [4];

            for (int i = 0; i < 4; i++) {
                answersButtons[i] = new Button (i);
                answersButtons[i].Clicked += ButtonHandler;
            }

            body = new RectangleShape (new Vector2f (400.0f, 200.0f));
            body.FillColor = Color.White;
            body.OutlineColor = Color.Black;
            body.OutlineThickness = 3.0f;
            body.Position = new Vector2f (200.0f, 200.0f);

            questionFont = new Font ("Roboto-Black.ttf");

            questionText = new Text ("Temp", questionFont, 24);
            questionText.Position = body.Position + new Vector2f (4.0f, 4.0f);
            questionText.FillColor = Color.Black;
        }
        public void SetContent (String question, String [] answers, int correctAnswer) {

            this.correctAnswer = correctAnswer;
            this.answersString = answers;
            this.questionString = question;

            signal = false;

            questionText.DisplayedString = "Pytanie: " + question;

            howManyQuestions = answers.GetLength(0);

            if (howManyQuestions > 4) {
                Console.WriteLine ("Too many answers to question: " + question);
                // throw
            }
            
            for (int i = 0; i < howManyQuestions; i++) {
                
                float width = (400 - (howManyQuestions + 1) * 30) / howManyQuestions;

                answersButtons[i].SetContent(answers[i], width, new Vector2f (body.Position.X + 30.0f + (float)i * (width + 30.0f), body.Position.Y + 130.0f), howManyQuestions);
            }

            for (int i = howManyQuestions; i < 4; i++)
                answersButtons[i].SetContent("", 0, new Vector2f (0.0f, 0.0f), howManyQuestions);
        }

        private void ButtonHandler (object sender, int content) {

            SendAnsweredSignal(content == correctAnswer);
        }
        
        public void Draw (ref RenderWindow window) {

            if (signal) {
                window.Draw(body);
                window.Draw(questionText);
            }
        }
    }
}