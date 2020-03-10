using System;
using System.Collections.Generic;

namespace MultiDimensionOptimization
{
    public static class MDO
    {
        public delegate double OptimizingFunction(List<double> list);
        public delegate double LipzitsFunction(double epsilon);
        public static T Dequeue<T>(this LinkedList<T> list)
        {
            if (list.First == null) return default;
            T first = list.First.Value;
            list.RemoveFirst();
            return first;
        }
        public static T Pop<T>(this LinkedList<T> list)
        {
            if (list.Last == null) return default;
            T last = list.Last.Value;
            list.RemoveLast();
            return last;
        }
    }
}
