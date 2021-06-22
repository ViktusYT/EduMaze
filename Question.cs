using System;

using SFML.System;
using SFML.Graphics;

namespace EduMaze {
    class Question : IDrawable { 

        private RectangleShape body;
        private Font questionFont;
        private Text questionText;
        private String questionString;
        private Button [] answersButtons;
        private String [] answersString;
        private Text [] answersText;
        private char [] letters;
        private int correctAnswer;
        private int howManyQuestions;
        private bool render;
        public event EventHandler<bool> Answered;
        protected virtual void SendAnsweredSignal (bool result) {
            Answered?.Invoke(this, result);
        }
        public bool Render {
            get => render;
            set {
                render = value;
                foreach (var i in answersButtons) i.Render = render;
            }
        }

        public IDrawable [] GetButtons() {
            return answersButtons;
        }
        public Question () {
            render = false;

            answersButtons = new Button [4];
            answersText = new Text [4];
            letters = new char [] {'a', 'b', 'c', 'd'};

            questionFont = new Font ("Roboto-Black.ttf");

            body = new RectangleShape (new Vector2f (600.0f, 400.0f));
            body.FillColor = Color.White;
            body.OutlineColor = Color.Black;
            body.OutlineThickness = 3.0f;
            body.Position = new Vector2f (100.0f, 100.0f);

            questionText = new Text ("Temp", questionFont, 20);
            questionText.Position = body.Position + new Vector2f (4.0f, 4.0f);
            questionText.FillColor = Color.Black;

            for (int i = 0; i < 4; i++) {
                answersButtons[i] = new Button (i);
                answersButtons[i].Clicked += ButtonHandler;

                answersText[i] = new Text ("Temp", questionFont, 16);
                answersText[i].Position = body.Position + new Vector2f (40.0f, 100.0f + 50.0f * (float) i);
                answersText[i].FillColor = Color.Black;
            }
        }
        public void SetContent (String question, String [] answers, int correctAnswer) {

            this.correctAnswer = correctAnswer;
            this.answersString = answers;
            this.questionString = question;

            render = false;

            questionText.DisplayedString = WordWrap ("Pytanie: " + question, 560.0f, questionText.CharacterSize);

            while (questionText.GetGlobalBounds().Height > 100) {
                questionText.CharacterSize -= 1;
                questionText.DisplayedString = WordWrap ("Pytanie: " + question, 560.0f, questionText.CharacterSize);
            }

            howManyQuestions = answers.GetLength(0);
            if (howManyQuestions > 4) {
                Console.WriteLine ("Too many answers to question: " + question);
                // throw
            }
            
            for (int i = 0; i < howManyQuestions; i++) {
                
                float width = (600 - (howManyQuestions + 1) * 30) / howManyQuestions;

                answersText[i].DisplayedString = WordWrap (letters[i].ToString() + ") " + answers[i], 520.0f, answersText[i].CharacterSize);

                while (answersText[i].GetGlobalBounds().Height > 50) {
                    answersText[i].CharacterSize -= 1;
                    answersText[i].DisplayedString = WordWrap (letters[i].ToString() + ") " + answers[i], 520.0f, answersText[i].CharacterSize);
                }

                answersButtons[i].SetContent(letters[i].ToString().ToUpper(), width, new Vector2f (body.Position.X + 30.0f + (float)i * (width + 30.0f), body.Position.Y + 330.0f), howManyQuestions);
            }

            for (int i = howManyQuestions; i < 4; i++)
                answersButtons[i].SetContent("", 0, new Vector2f (0.0f, 0.0f), howManyQuestions);
        }

        public void UnHoverButtons () {
            foreach (var i in answersButtons) {
                i.OnUnhover();
            }
        }

        private void ButtonHandler (object sender, int content) {
            SendAnsweredSignal(content == correctAnswer);
        }

        private String WordWrap (String text, float width, uint gsize) {

            String text2 = " ";
            int lastSpace = 0; 
            float acc = 0.0f;

            for (int i = 0; i < text.Length; i++) {
                text2 += text[i].ToString();
                acc += questionFont.GetGlyph(text[i], gsize, false, 0.0f).Advance;

                if (text2[i] == ' ') 
                    lastSpace = i;
                if (acc >= width) {
                    text2 = text2.Insert (lastSpace, "\n");
                    acc = 0.0f;
                }
            }

            return text2;
        }
        
        public void Draw (ref RenderWindow window) {

            if (render) {
                window.Draw(body);
                window.Draw(questionText);
                
                for (int i = 0; i < this.howManyQuestions; i++)
                    window.Draw(answersText[i]);
            }
        }
    }
}