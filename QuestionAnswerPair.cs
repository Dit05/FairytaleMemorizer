using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace Bemagoló {

    public class QuestionAnswerPair {

        static string Escape(string str) {
            return str.Replace("\n", "\\n").Replace("\t", "\\t");
        }

        static string Unescape(string str) {
            return str.Replace("\\t", "\t").Replace("\\n", "\n");
        }


        public static bool TryParse(string text, [NotNullWhen(true)] out QuestionAnswerPair? parsed) {
            int tabI = text.IndexOf('\t');
            if(tabI < 0) goto fail;

            parsed = new QuestionAnswerPair();

            parsed.QuestionText = Unescape(text.Substring(startIndex: 0, length: tabI));
            parsed.AnswerText = Unescape(text.Substring(startIndex: tabI + 1));
            return true;

        fail:
            parsed = null;
            return false;
        }

        public static QuestionAnswerPair Parse(string text) {
            if(TryParse(text, out QuestionAnswerPair? parsed)) return parsed;
            else throw new ArgumentException("The text was not formatted correctly. It should be some text, a tab, then some more text, with other tabs escaped into \\t and newlines into \\n.", nameof(text));
        }


        public static async IAsyncEnumerable<QuestionAnswerPair> ParseLinesAsync(System.IO.TextReader reader) {
            while(true) {
                string? line = await reader.ReadLineAsync();
                await System.Threading.Tasks.Task.Delay(1);
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
