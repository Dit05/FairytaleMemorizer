using System;
using System.Collections.Generic;


namespace Bemagoló {

    public static class Program {

        static void ConvertWallOfText(string srcPath) {
            using(System.IO.TextReader reader = new System.IO.StreamReader(System.IO.File.OpenRead(srcPath))) {
                foreach(Question q in BibliaismeretMediaWikiParser.ParseText(reader)) {
                    Console.WriteLine(q.ToString());
                }
            }
        }


        public static void Main( string[] args ) {

            string command;
            if(args.Length > 0) command = args[0];
            else command = "study";


            switch(command) {
                case "study":
                    // TODO
                    break;

                case "convert":
                    if(args.Length != 2) {
                        Console.WriteLine("Usage: convert SOURCE\n\nThe input file should be the bibliaismeret wall of text docx exported as MediaWiki markup. Converted questions will be written to stdout.");
                        return;
                    }

                    ConvertWallOfText(args[1]);
                    break;
            }

        }

    }

}
