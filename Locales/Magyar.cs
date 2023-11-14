

namespace Bemagoló.Locales {

    class Magyar : Locale {

        public override string WhatDoYouWant => "Add meg, hogy mit akarsz csinálni";

        public override string Usage => "Használat";

        public override string StudyBrief => "Válaszolj kérdésekre.";
        public override string ConvertBrief => "Alakítsd a MediaWiki markupként exportált bibliaismeret jegyzetet egy question fájllá.";

        public override string StudyHelp => "A question fájl minden egyes sora egy <tab>ulátorral elválasztott kérdés és válasz kell hogy legyen.";
        public override string ConvertHelp => "A bemeneti fájl a MediaWiki markupként exportált bibliaismeret wall of text legyen. A kinyert kérdések a standard kimenetre mennek.";

        public override string LoadFinished(int loadedCount) => $"Be lett töltve {loadedCount} kérdés.";
        public override string InvalidQuestionFile => "Nem sikerült betölteni a fájlt. Biztosan egy question fájlt próbáltál betölteni?";


        public override string SetupTitle => "Beállítás";
        public override string AskQuestionWeightOrDisable => "Mekkora súllyal legyen jelen ez a kérdéstípus? (a 0 kikapcsolja ezt a fajtát)";

        public override string NoQuestionTypesEnabled => "Semmilyen típusú kérdés sincs bekapcsolva. Kilépés.";

        public override string Starting(string quitCmd) => $"Kezdés. A kilépéshez írd be, hogy {quitCmd}.";

        public override string CorrectAnswer => "Helyes.";
        public override string IncorrectAnswer => "Nem helyes.";
        public override string ExampleCorrectAnswer(string aCorrectAnswer) => $"Egy helyes válasz lett volna például: '{aCorrectAnswer}'.";

        public override string PressAnyKeyToContinue => "(nyomj bármilyen billentyűt a folytatáshoz)";


        public override string SummaryTitle => "Összefoglalás";
        public override string CorrectIncorrectTotals(int correctAnswers, int incorrectAnswers) => $"Helyes válaszok: {correctAnswers}, helytelen: {incorrectAnswers}";
        public override string TotalAndPercentage(int total, int percentage) => $"Összesen {total} kérdést válaszoltál meg, ez {percentage}%-os pontosság.";
        public override string NoQuestionsAnswered => "Egy kérdést sem válaszoltál meg.";


        public override string SelectMatchingQuestionTitle => "Feleletválasztós kérdés";
        public override string SelectMatchingQuestionDescription => "Válaszd ki a megfelelő választ a kérdésre.";
        public override string SelectMatchingQuestionConfigChoiceCount => "Hány lehetséges válasz legyen kérdésenként? Írd be egy pozitív egész számot.";
        public override string SelectMatchingQuestionAsk => "Melyik lehet a párja?";

        public override string FillBlankQuestionTitle => "Hiányzó szavas kérdés";
        public override string FillBlankQuestionDescription => "Írd be a hiányzó szót.";
        public override string FillBlankQuestionAsk => "Mi lehet a hiányzó szó?";

    }

}
