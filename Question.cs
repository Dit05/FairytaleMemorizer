using System;
using System.Text;
using System.Collections.Generic;


namespace Bemagoló {

    public abstract class Question {

        /// <summary>
        /// Returns a string which is displayed to the user in askance before prompting for an answer.
        /// </summary>
        public abstract string GetAskString();

        /// <param name="answer">User input trimmed and normalized.</param>
        public abstract bool IsAnswerGood(string answer);

        public abstract string GetCorrectAnswer();

    }


    class SelectMatchingAnswerQuestion : Question {

        public sealed class Asker : QuestionAsker, IUserConfigurableAsker {

            public string Title => "Multiple choice question";
            public string Description => "Select the correct answer to the question.";

            public void Configure() {
                Console.WriteLine("How many choices per question? Enter a positive integer.");
                ChoiceCount = Program.PromptChecked<int>(int.TryParse);
            }


            private int choiceCount = 4;
            public int ChoiceCount {
                get => choiceCount;
                set {
                    if(value <= 0) throw new ArgumentOutOfRangeException(nameof(value), "Value must be at least 1.");
                    choiceCount = value;
                }
            }


            public override Question AskQuestion(Random rand) {
                List<QuestionAnswerPair> subList = qaps.MakeRandomSublist(Math.Min(choiceCount, qaps.Count), rand);

                return new SelectMatchingAnswerQuestion(subList);
            }

        }


        //


        private QuestionAnswerPair[] qaps;
        int correctIndex;


        public SelectMatchingAnswerQuestion(IEnumerable<QuestionAnswerPair> qaps) {
            this.qaps = new List<QuestionAnswerPair>(qaps).ToArray();

            correctIndex = Random.Shared.Next(this.qaps.Length);
        }


        public override string GetAskString() {
            var sb = new StringBuilder();

            sb.Append("Melyik lehet a párja?\n\n");
            sb.Append(qaps[correctIndex].QuestionText);
            sb.Append("\n\n");

            for(int i = 0; i < qaps.Length; i++) {
                sb.Append($"{i+1}.) {qaps[i].AnswerText}\n");
            }

            return sb.ToString();
        }

        public override bool IsAnswerGood(string answer) {
            return int.TryParse(answer, out int i) && i - 1 == correctIndex;
        }

        public override string GetCorrectAnswer() => (correctIndex + 1).ToString();

    }

    class FillBlankQuestion : Question {

        public sealed class Asker : QuestionAsker, IConcreteAsker {

            public string Title => "Missing word question";
            public string Description => "Enter a word to fill the blank.";

            public override Question AskQuestion(Random rand) {
                return new FillBlankQuestion(qaps[rand.Next(qaps.Count)]);
            }

        }


        //


        public QuestionAnswerPair qap;
        int blankStart;
        int blankEnd;


        public FillBlankQuestion(QuestionAnswerPair qap) {
            this.qap = qap;
            bool is_gap(char ch) => ch == ',' || ch == '.' || char.IsWhiteSpace(ch);

            blankStart = Random.Shared.Next((int)qap.AnswerText.Length);
            while(blankStart > 0 && is_gap(qap.AnswerText[blankStart])) blankStart--;

            if(blankStart < 0) blankStart = 0;
            if(blankStart >= qap.AnswerText.Length) blankStart = qap.AnswerText.Length - 1;

            blankEnd = blankStart;

            while(blankStart > 0 && !is_gap(qap.AnswerText[blankStart - 1])) blankStart--;
            while(blankEnd < qap.AnswerText.Length - 1 && !is_gap(qap.AnswerText[blankEnd + 1])) blankEnd++;

        }


        public override string GetAskString() {
            char[] chars = qap.AnswerText.ToCharArray();

            for(int i = blankStart; i <= blankEnd; i++) {
                chars[i] = '_';
            }

            string blankedString = new string(chars);

            return $"Mi lehet a hiányzó rész?\n\n{qap.QuestionText}\n\n{blankedString}";
        }

        public override bool IsAnswerGood(string answer) {
            return string.Equals(answer, GetCorrectAnswer().Normalize(), StringComparison.OrdinalIgnoreCase);
        }

        public override string GetCorrectAnswer() => qap.AnswerText.Substring(blankStart, blankEnd - blankStart + 1);

    }

}
