using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace Bemagoló {

    public class Question {

        static string Escape(string str) {
            return str.Replace("\n", "\\n").Replace("\t", "\\t");
        }

        static string Unescape(string str) {
            return str.Replace("\\t", "\t").Replace("\\n", "\n");
        }


        public static bool TryParse(string text, [NotNullWhen(true)] out Question? parsed) {
            int tabI = text.IndexOf('\t');
            if(tabI < 0) goto fail;

            parsed = new Question();

            parsed.QuestionText = Unescape(text.Substring(startIndex: 0, length: tabI));
            parsed.AnswerText = Unescape(text.Substring(startIndex: tabI + 1));
            return true;

        fail:
            parsed = null;
            return false;
        }

        public static Question Parse(string text) {
            if(TryParse(text, out Question? parsed)) return parsed;
            else throw new ArgumentException("The text was not formatted correctly. It should be some text, a tab, then some more text, with other tabs escaped into \\t and newlines into \\n.", nameof(text));
        }


        public static IEnumerable<Question> ParseLines(System.IO.TextReader reader) {
            while(true) {
                string? line = reader.ReadLine();
                if(line == null) break;
                if(string.IsNullOrWhiteSpace(line)) continue;

                yield return Parse(line);
            }
        }


        //


        public string QuestionText { get; set; } = string.Empty;
        public string AnswerText { get; set; } = string.Empty;


        public override string ToString() => $"{Escape(QuestionText)}\t{Escape(AnswerText)}";

    }

}
