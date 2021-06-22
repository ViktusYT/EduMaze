using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace EduMaze {
    class QuestionPrototype {
        public String Question { get; set; }
        public String [] Answers { get; set; }
        public int Correct { get; set; }
    }

    class QuestionSet {
        
        List <QuestionPrototype> discarded;
        List <QuestionPrototype> questions;

        private void Parse (String questionFile) {
            try {
                questions = JsonConvert.DeserializeObject <List<QuestionPrototype>> (File.ReadAllText(questionFile));
            } catch (FileNotFoundException e) {
                using var reader = new StringReader(e.ToString());
                Console.WriteLine ("Nie udało się odczytać pliku z pytaniami! Więcej informacji: " + reader.ReadLine());
                System.Environment.Exit(1);
            }
        }
        private void Shuffle(ref List <QuestionPrototype> list)
        {
            Random random = new Random ();

            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = random.Next(n + 1);  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }
        }
        private void AnsweredHandler (object sender, bool c) {
            NextQuestion();
        }

        override public String ToString () {
            String temp = "";
            foreach (var i in questions)
                temp += i.Question + ", ";

            return temp;
        }

        public QuestionSet (String questionsFile) {
            
            questions = new List<QuestionPrototype>();
            discarded = new List<QuestionPrototype>();

            Parse (questionsFile);
            Shuffle (ref questions);
        }

        public QuestionPrototype GetQuestion() {
            return questions[0];
        }

        public void NextQuestion () {

            discarded.Add(questions[0]);

            try {
                questions.RemoveAt(0);
            } catch (ArgumentOutOfRangeException e) {
                Console.WriteLine("Za mało pytań! Więcej informacji: " + e.Message);
                System.Environment.Exit(1);
            }
            
            if (questions.Count == 0) {

                Shuffle (ref discarded);
                
                foreach (var i in discarded)
                    questions.Add(i);

                discarded.Clear();
            }
        }
    }
}