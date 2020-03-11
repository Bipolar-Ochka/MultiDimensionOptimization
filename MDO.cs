using System;
using System.Collections.Generic;

namespace MultiDimensionOptimization
{
    public static class MDO
    {
        public delegate double OptimizingFunction(List<double> list);
        public delegate double LipzitsFunction(double epsilon);
        public delegate void RectangleHandler(double xPosition, double yPosition, double width, double height);
        public class GraphicSettings
        {
            int firstDimension;
            int secondDimension;
            RectangleHandler handler;
            public GraphicSettings(RectangleHandler handler, int firstDimension, int secondDimension)
            {
                this.handler = handler;
                this.firstDimension = firstDimension;
                this.secondDimension = secondDimension;
            }
            internal void Handle(List<double>lower, List<double> upper)
            {
                double width = Math.Abs(upper[firstDimension] - lower[firstDimension]);
                double height = Math.Abs(upper[secondDimension] - lower[secondDimension]);
                handler?.Invoke(lower[this.firstDimension], upper[this.secondDimension],width,height);
            }
        }
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
