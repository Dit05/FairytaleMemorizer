

namespace Bemagoló {

    public abstract class Locale {

        public abstract string WhatDoYouWant { get; }

        public abstract string Usage { get; }

        public abstract string StudyBrief { get; }
        public abstract string ConvertBrief { get; }

        public abstract string StudyHelp { get; }
        public abstract string ConvertHelp { get; }

        public abstract string LoadFinished(int loadedCount);
        public abstract string InvalidQuestionFile { get; }


        public abstract string SetupTitle { get; }
        public abstract string AskQuestionWeightOrDisable { get; }

        public abstract string NoQuestionTypesEnabled { get; }

        public abstract string Starting { get; }

        public abstract string CorrectAnswer { get; }
        public abstract string IncorrectAnswer { get; }
        public abstract string ExampleCorrectAnswer(string aCorrectAnswer);

        public abstract string PressAnyKeyToContinue { get; }


        public abstract string SummaryTitle { get; }
        public abstract string CorrectIncorrectTotals(int correctAnswers, int incorrectAnswers);
        public abstract string TotalAndPercentage(int total, int percentage);
        public abstract string NoQuestionsAnswered { get; }


        public abstract string SelectMatchingQuestionTitle { get; }
        public abstract string SelectMatchingQuestionDescription { get; }
        public abstract string SelectMatchingQuestionConfigChoiceCount { get; }
        public abstract string SelectMatchingQuestionAsk { get; }

        public abstract string FillBlankQuestionTitle { get; }
        public abstract string FillBlankQuestionDescription { get; }
        public abstract string FillBlankQuestionAsk { get; }

    }

}
