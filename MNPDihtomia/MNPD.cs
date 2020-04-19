using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MultiDimensionOptimization.MDO;

namespace MultiDimensionOptimization.MNPDihtomia
{

    public enum RuleD
    {
        FIFO,
        LIFO,
        MINQ
    }
    public static class MNPD
    {
        static DihtomiaRectangle GetRectangle(LinkedList<DihtomiaRectangle> list,RuleD rule)
        {
            switch (rule)
            {
                case RuleD.FIFO:
                    return list.Dequeue();
                case RuleD.LIFO:
                    return list.Pop();
                case RuleD.MINQ:
                    return list.GetWithMinQ();
                default:
                    return list.Dequeue();
            }
        }
        public static (double FunctionMinimum, int counter, int optimal) Solve(OptimizingFunction function, LipzitsFunction lipzits, double precision, double epsilon, List<double> lowerBound, List<double> upperBound, RuleD rule,GraphicSettings settings =null)
        {
            double lipConst = lipzits(epsilon);
            LinkedList<DihtomiaRectangle> rectangles = new LinkedList<DihtomiaRectangle>();
            DihtomiaRectangle first = new DihtomiaRectangle(lowerBound, upperBound,settings);
            first.EvalQ(function, lipConst);
            rectangles.AddFirst(first);
            int counter = 1;
            int optimal = counter;
            double currentMin = Math.Min(function(lowerBound),function(first.center));
            if(first.Q >= currentMin - epsilon)
            {
                return (currentMin, counter, optimal);
            }
            while(rectangles.Count != 0)
            {
                var current = GetRectangle(rectangles, rule);
                var newRects = current.SplitByMaxSide(settings);
                newRects.first.EvalQ(function, lipConst);
                newRects.second.EvalQ(function, lipConst);
                double funcInFirst = function(newRects.first.center);
                double funcInSecond = function(newRects.second.center);
                if(currentMin < Math.Min(funcInFirst,funcInSecond))
                {
                    if(newRects.first.Q <= (currentMin - epsilon) && newRects.first.GetMaxSide().max >= precision)
                    {
                        rectangles.AddFirst(newRects.first);
                    }
                    if (newRects.second.Q <= (currentMin - epsilon) && newRects.second.GetMaxSide().max >= precision)
                    {
                        rectangles.AddFirst(newRects.second);
                    }
                }
                else
                {
                    if(funcInFirst <= funcInSecond)
                    {
                        currentMin = funcInFirst;
                        rectangles.AddFirst(newRects.first);
                    }
                    else
                    {
                        currentMin = funcInSecond;
                        rectangles.AddFirst(newRects.second);
                    }
                    rectangles.RemoveWorse(currentMin,epsilon);
                    optimal = counter;
                }
                counter++;
            }
            GC.Collect();
            return (currentMin, counter,optimal);
        }
    }

    public static class LinkedListExtension
    {
        
        internal static DihtomiaRectangle GetWithMinQ(this LinkedList<DihtomiaRectangle> list)
        {
            var minQ = list.OrderByDescending(item => item.Q).Last();
            list.Remove(minQ);
            return minQ;
        }
        internal static void RemoveWorse(this LinkedList<DihtomiaRectangle> list, double curMin, double epsilon)
        {
            var node = list.First;
            while (node != null)
            {
                var nextNode = node.Next;
                if (node.Value.Q >= curMin - epsilon)
                {
                    list.Remove(node);
                }
                node = nextNode;
            }
        }
    }
}
