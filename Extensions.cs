using System;
using System.Collections.Generic;


namespace Bemagoló {

    static class ListExtensions {

        public static List<T> MakeRandomSublist<T>(this IList<T> original, int count, Random? rand = null) {
            if(count > original.Count) throw new ArgumentOutOfRangeException(nameof(count), "The count of the sublist cannot be higher than the size of the original list.");
            if(count == original.Count) return new List<T>(original);

            rand ??= Random.Shared;

            var visitedIndices = new HashSet<int>();
            var list = new List<T>();

            while(list.Count < count) {
                int i = rand.Next(original.Count);
                if(visitedIndices.Contains(i)) continue;

                visitedIndices.Add(i);
                list.Add(original[i]);
            }

            return list;
        }

    }

}
