using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace Bemagoló {

    public static class Program {

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

            string command = "";
            if(args.Length > 0) command = args[0];


            switch(command) {
                case "study":
                    if(args.Length < 2) {
                        Console.WriteLine("Usage: study QUESTIONFILE\n\nThe question file's lines should each have a question and an answer separated by a <tab>.");
                        return;
                    }

                    Task<List<QuestionAnswerPair>> loadTask = ReadQuestions(args[1]);
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
                    List<QuestionAnswerPair> questions = loadTask.Result;

                    Console.WriteLine($"Loaded {questions.Count} questions.");

                    Study(questions);

                    break;

                case "convert":
                    if(args.Length != 2) {
                        Console.WriteLine("Usage: convert SOURCE\n\nThe input file should be the bibliaismeret wall of text docx exported as MediaWiki markup. Converted questions will be written to stdout.");
                        return;
                    }

                    ConvertWallOfText(args[1]);
                    break;

                default:
                    Console.WriteLine("Specify what you want to do:\n\nstudy: Answer questions.\nconvert: Convert the notes exported as MediaWiki markup into a question file.");
                    break;
            }

        }


        static void Study(List<QuestionAnswerPair> qaps) {

            var randAsker = new RandomQuestionAsker();
            randAsker.qaps = qaps;

            void setup_asker(QuestionAsker asker) {
                if(asker is IConcreteAsker concreteAsker) {
                    Console.WriteLine($"\n{concreteAsker.Title}\n{concreteAsker.Description}\n\nWith what weight should this question type be present? (0 to disable this type)");

                    double weight = PromptChecked<double>(double.TryParse);
                    if(weight == 0) return;

                    randAsker.AddAsker(asker, Math.Abs(weight));
                } else {
                    throw new ArgumentException("This is not a concrete asker.", nameof(asker));
                }
            }

            Console.WriteLine("\n\n--- Setup ---\n\n");

            setup_asker(new SelectMatchingAnswerQuestion.Asker());
            setup_asker(new FillBlankQuestion.Asker());

            if(randAsker.AskerCount == 0) {
                Console.WriteLine("No question types are enabled. Exiting.");
                return;
            }

            Console.WriteLine("Starting. Type !quit to stop.\n\n");

            int correctAnswers = 0;
            int incorrectAnswers = 0;
            while(true) {
                Question qu = randAsker.AskQuestion(Random.Shared);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(qu.GetAskString());

                string? inp = Console.ReadLine();
                if(inp == null || inp.Trim() == "!quit") break;

                if(qu.IsAnswerGood(inp)) {
                    Console.WriteLine("That's correct.");
                    correctAnswers++;
                } else {
                    Console.WriteLine($"That's not correct.\nA correct answer would've been: '{qu.GetCorrectAnswer()}'.");
                    Console.WriteLine("(press any key to continue)");
                    Console.ReadKey(intercept: true);
                    incorrectAnswers++;
                }

            }

            int total = correctAnswers + incorrectAnswers;
            Console.WriteLine("\n--- Summary ---");
            if(total > 0) {
                Console.WriteLine($"Correct answers: {correctAnswers}, incorrect: {incorrectAnswers}");
                Console.WriteLine($"You answered a total of {total} questions, with an accuracy of {(int)Math.Round(correctAnswers * 100 / (double)total)}%.");
            } else {
                Console.WriteLine("You haven't answered any questions.");
            }

        }

    }

}
