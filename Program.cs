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

                    IList<QuestionAnswerPair> questions = ReadQuestions(args[1]).Result;
                    Console.WriteLine($"Loaded {questions.Count} questions.");

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

    }

}
