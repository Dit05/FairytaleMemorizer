using System;
using System.Collections.Generic;


namespace Bemagoló {

    public class RandomQuestionAsker : QuestionAsker {

        readonly List<KeyValuePair<QuestionAsker, double>> askersByWeight = new();
        double totalWeight = 0;

        public int AskerCount => askersByWeight.Count;


        public void AddAsker(QuestionAsker asker, double weight) {
            if(weight <= 0) throw new ArgumentOutOfRangeException(nameof(weight), "Weight must be positive.");

            askersByWeight.Add(new KeyValuePair<QuestionAsker, double>(asker, weight));
            totalWeight += weight;
        }

        QuestionAsker PickRandomAsker(Random? rand = null) {
            rand ??= Random.Shared;

            if(totalWeight == 0) throw new InvalidOperationException("There's nothing added yet.");

            int safety = 1_000_000;
            while(--safety > 0) {
                KeyValuePair<QuestionAsker, double> kvp = askersByWeight[rand.Next(askersByWeight.Count)];

                if(rand.NextDouble() + double.Epsilon < kvp.Value / totalWeight) return kvp.Key;
            }

            throw new Exception("Couldn't pick a random question within a reasonable number of attempts.");
        }


        public override Question AskQuestion(Random rand) {
            QuestionAsker asker = PickRandomAsker(rand);
            asker.qaps = this.qaps;

            return asker.AskQuestion(rand);
        }

    }

}
