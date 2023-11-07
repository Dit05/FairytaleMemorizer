

namespace Bemagoló.Locales {

    class English : Locale {

        public override string WhatDoYouWant => "Specify what you want to do";

        public override string Usage => "Usage";

        public override string StudyBrief => "Answer questions.";
        public override string ConvertBrief => "Convert the bibliaismeret notes exported as MediaWiki markup into a question file.";

        public override string StudyHelp => "The question file's lines should each have a question and an answer separated by a <tab>.";
        public override string ConvertHelp => "The input file should be the bibliaismeret wall of text docx exported as MediaWiki markup. Converted questions will be written to stdout.";

        public override string LoadFinished(int loadedCount) => $"Loaded {loadedCount} questions.";
        public override string InvalidQuestionFile => "Failed to load file. Are you sure you were trying to load a question file?";


        public override string SetupTitle => "Setup";
        public override string AskQuestionWeightOrDisable => "With what weight should this question type be present? (0 to disable this type)";

        public override string NoQuestionTypesEnabled => "No question types are enabled. Exiting.";

        public override string Starting(string quitCmd) => $"Starting. Type {quitCmd} to stop.";

        public override string CorrectAnswer => "That's correct.";
        public override string IncorrectAnswer => "That's not correct.";
        public override string ExampleCorrectAnswer(string aCorrectAnswer) => $"A correct answer would've been: '{aCorrectAnswer}'.";

        public override string PressAnyKeyToContinue => "(press any key to continue)";


        public override string SummaryTitle => "Summary";
        public override string CorrectIncorrectTotals(int correctAnswers, int incorrectAnswers) => $"Correct answers: {correctAnswers}, incorrect: {incorrectAnswers}";
        public override string TotalAndPercentage(int total, int percentage) => $"You answered a total of {total} questions, with an accuracy of {percentage}%.";
        public override string NoQuestionsAnswered => "You haven't answered any questions.";


        public override string SelectMatchingQuestionTitle => "Multiple choice question";
        public override string SelectMatchingQuestionDescription => "Select the correct answer to the question.";
        public override string SelectMatchingQuestionConfigChoiceCount => "How many choices per question? Enter a positive integer.";
        public override string SelectMatchingQuestionAsk => "Which one could be its pair?";

        public override string FillBlankQuestionTitle => "Missing word question";
        public override string FillBlankQuestionDescription => "Enter a word to fill the blank.";
        public override string FillBlankQuestionAsk => "What could the missing word be?";

    }

}
