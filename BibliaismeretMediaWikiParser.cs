using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Bemagoló {

    public static class BibliaismeretMediaWikiParser {

        public static IEnumerable<Question> ParseText(TextReader reader) {

            bool endOfStream = false;

            void look_for(string targetText) {
                int matchCount = 0;

                while(matchCount < targetText.Length) {
                    int read = reader.Read();
                    if(read < 0) {
                        endOfStream = true;
                        break;
                    }

                    if(targetText[matchCount] == (char)read) matchCount++;
                    else matchCount = 0;
                }
            }

            string read_until(params char[] endChars) {
                var sb = new StringBuilder();

                while(true) {
                    int read = reader.Read();
                    if(read < 0) {
                        endOfStream = true;
                        break;
                    }

                    char ch = (char)read;
                    foreach(char ender in endChars) {
                        if(ch == ender) goto fin;
                    }

                    sb.Append(ch);
                }

            fin:
                return sb.ToString();
            }


            while(!endOfStream) {
                look_for("# Kérdés: ");
                string q = read_until('\n', '<');
                look_for("Válasz: ");
                string a = read_until('\n', '<');

                yield return new Question() {
                    QuestionText = q,
                    AnswerText = a
                };
            }

        }

    }

}
