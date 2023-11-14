using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace Bemagoló {

    static class Program {

        static Locale locale;
        public static Locale ActiveLocale => locale;


        static Program() {
            System.Globalization.CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;

            SetLocale(culture.TwoLetterISOLanguageName);
        }

        [MemberNotNull(nameof(locale))]
        static void SetLocale(string isoId) {
            locale = isoId switch {
                "hu" => new Locales.Magyar(),
                _ => new Locales.English()
            };
        }


        static void ConvertWallOfText(string srcPath) {
            using(System.IO.TextReader reader = new System.IO.StreamReader(System.IO.File.OpenRead(srcPath))) {
                foreach(QuestionAnswerPair q in BibliaismeretMediaWikiParser.ParseText(reader)) {
                    Console.WriteLine(q.ToString());
                }
            }
        }

        static async Task<List<QuestionAnswerPair>> ReadQuestions(string path) {

            var list = new List<QuestionAnswerPair>();
            using(System.IO.TextReader reader = new System.IO.StreamReader(System.IO.File.OpenRead(path))) {
                await foreach(var qap in QuestionAnswerPair.ParseLinesAsync(reader)) list.Add(qap);

            }

            return list;
        }


        public delegate bool TryParseDelegate<T>(string str, [NotNullWhen(true)] out T? parsed);

        public static T PromptChecked<T>(TryParseDelegate<T> parser) {
            T? parsed;
            while(!parser(Console.ReadLine()!, out parsed));
            return parsed;
        }


        public static void Main( string[] args ) {

            var argList = new List<string>(args);

            // HACK-y argument implementation (TODO DitoDisco.CommandLineArguments amikor kész)
            while(argList.Count > 0) {
                string arg = argList[0];
                if(!arg.StartsWith("--")) break;
                argList.RemoveAt(0);

                switch(arg) {
                    case "--lang":
                    case "--nyelv":
                        if(argList.Count == 0) throw new Exception("--lang requires an option following it.");
                        SetLocale(argList[0]);
                        argList.RemoveAt(0);
                        break;
                }
            }

            string command = "";
            if(argList.Count > 0) command = argList[0];


            switch(command) {
                case "study":
                    if(argList.Count < 2) {
                        Console.WriteLine($"{locale.Usage}: study QUESTIONFILE\n\n{locale.StudyHelp}");
                        return;
                    }

                    Task<List<QuestionAnswerPair>> loadTask = ReadQuestions(argList[1]);
                    int d = 0;
                    Console.CursorVisible = false;
                    while(!loadTask.IsCompleted) {
                        const string DECOR = "|/-\\";
                        Console.Write(DECOR[d]);
                        try {
                            Console.CursorLeft--;
                        } catch(Exception) {}
                        d++;
                        d %= DECOR.Length;
                        System.Threading.Thread.Sleep(50);
                    }
                    Console.CursorVisible = true;

                    List<QuestionAnswerPair> questions;
                    try {
                        questions = loadTask.Result;
                    } catch(AggregateException) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{locale.InvalidQuestionFile}");
                        Console.ResetColor();
                        throw;
                    }

                    Console.WriteLine(locale.LoadFinished(questions.Count));

                    Study(questions);

                    break;

                case "convert":
                    if(argList.Count != 2) {
                        Console.WriteLine($"{locale.Usage}: convert SOURCE\n\n{locale.ConvertHelp}");
                        return;
                    }

                    ConvertWallOfText(argList[1]);
                    break;

                default:
                    Console.WriteLine($"{locale.WhatDoYouWant}:\n\nstudy: {locale.StudyBrief}\nconvert: {locale.ConvertBrief}");
                    break;
            }

        }


        static void Study(List<QuestionAnswerPair> qaps) {

            var randAsker = new RandomQuestionAsker();
            randAsker.qaps = qaps;

            void setup_asker(QuestionAsker asker) {
                if(asker is IConcreteAsker concreteAsker) {
                    Console.WriteLine($"\n{concreteAsker.Title}\n{concreteAsker.Description}\n\n{locale.AskQuestionWeightOrDisable}");

                    double weight = PromptChecked<double>(double.TryParse);
                    if(weight == 0) return;

                    randAsker.AddAsker(asker, Math.Abs(weight));
                } else {
                    throw new ArgumentException("This is not a concrete asker.", nameof(asker));
                }
            }

            Console.WriteLine($"\n\n--- {locale.SetupTitle} ---\n\n");

            setup_asker(new SelectMatchingAnswerQuestion.Asker());
            setup_asker(new FillBlankQuestion.Asker());

            if(randAsker.AskerCount == 0) {
                Console.WriteLine(locale.NoQuestionTypesEnabled);
                return;
            }


            const string QUIT_CMD = "!quit";

            Console.WriteLine($"{locale.Starting(QUIT_CMD)}\n\n");

            int correctAnswers = 0;
            int incorrectAnswers = 0;
            while(true) {
                Question qu = randAsker.AskQuestion(Random.Shared);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(qu.GetAskString());

                string? inp = Console.ReadLine();
                if(inp == null || inp.Trim() == QUIT_CMD) break;

                if(qu.IsAnswerGood(inp)) {
                    Console.WriteLine(locale.CorrectAnswer);
                    correctAnswers++;
                } else {
                    Console.WriteLine(locale.IncorrectAnswer);
                    Console.WriteLine();
                    Console.WriteLine(locale.ExampleCorrectAnswer(qu.GetCorrectAnswer()));
                    Console.WriteLine(locale.PressAnyKeyToContinue);
                    Console.ReadKey(intercept: true);
                    incorrectAnswers++;
                }

            }

            int total = correctAnswers + incorrectAnswers;
            Console.WriteLine($"\n--- {locale.SummaryTitle} ---");
            if(total > 0) {
                Console.WriteLine(locale.CorrectIncorrectTotals(correctAnswers, incorrectAnswers));
                Console.WriteLine(locale.TotalAndPercentage(total, (int)Math.Round(correctAnswers * 100 / (double)total)));
            } else {
                Console.WriteLine(locale.NoQuestionsAnswered);
            }

        }

    }

}
