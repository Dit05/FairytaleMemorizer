using System;
using System.Collections.Generic;


namespace Bemagoló {

    public class RandomQuestionAsker : QuestionAsker {

        readonly List<KeyValuePair<QuestionAsker, double>> askersByWeight = new();
        double totalWeight = 0;


        public void AddAsker(QuestionAsker asker, double weight) {
            if(weight <= 0) throw new ArgumentOutOfRangeException(nameof(weight), "Weight must be positive.");

            askersByWeight.Add(new KeyValuePair<QuestionAsker, double>(asker, weight));
            totalWeight += weight;
        }

        QuestionAsker PickRandomAsker(Random? rand = null) {
            rand ??= Random.Shared;

            if(totalWeight == 0) throw new InvalidOperationException("There's nothing added yet.");

            while(true) {
                KeyValuePair<QuestionAsker, double> kvp = askersByWeight[rand.Next(askersByWeight.Count)];

                if(rand.NextDouble() + double.Epsilon < kvp.Value / totalWeight) return kvp.Key;
            }
        }


        public override Question AskQuestion() {
            QuestionAsker asker = PickRandomAsker();
            asker.qaps = this.qaps;

            return asker.AskQuestion();
        }

    }

}
