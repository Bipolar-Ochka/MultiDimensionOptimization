﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MultiDimensionOptimization.MDO;

namespace MultiDimensionOptimization.MNPDihtomia
{

    public enum Rule
    {
        FIFO,
        LIFO,
        MINQ
    }
    public static class MNPD
    {
        static DihtomiaRectangle GetRectangle(LinkedList<DihtomiaRectangle> list,Rule rule)
        {
            switch (rule)
            {
                case Rule.FIFO:
                    return list.Dequeue();
                case Rule.LIFO:
                    return list.Pop();
                case Rule.MINQ:
                    return list.GetWithMinQ();
                default:
                    return list.Dequeue();
            }
        }
        public static (double FunctionMinimum, int counter) Solve(OptimizingFunction function, LipzitsFunction lipzits, double precision, double epsilon, List<double> lowerBound, List<double> upperBound, Rule rule)
        {
            double lipConst = lipzits(epsilon);
            LinkedList<DihtomiaRectangle> rectangles = new LinkedList<DihtomiaRectangle>();
            DihtomiaRectangle first = new DihtomiaRectangle(lowerBound, upperBound);
            first.EvalQ(function, lipConst);
            rectangles.AddFirst(first);
            int counter = 1;
            double currentMin = Math.Min(function(lowerBound),function(first.center));
            if(first.Q >= currentMin - epsilon)
            {
                return (currentMin, counter);
            }
            while(rectangles.Count != 0)
            {
                var current = GetRectangle(rectangles, rule);
                var newRects = current.SplitByMaxSide();
                newRects.first.EvalQ(function, lipConst);
                newRects.second.EvalQ(function, lipConst);
                double funcInFirst = function(newRects.first.center);
                double funcInSecond = function(newRects.second.center);
                if(currentMin < Math.Min(funcInFirst,funcInSecond))
                {
                    if(newRects.first.Q < (currentMin - epsilon) && newRects.first.GetMaxSide().max >= precision)
                    {
                        rectangles.AddFirst(newRects.first);
                    }
                    if (newRects.second.Q < (currentMin - epsilon) && newRects.second.GetMaxSide().max >= precision)
                    {
                        rectangles.AddFirst(newRects.second);
                    }
                }
                else
                {
                    if(funcInFirst < funcInSecond)
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
                }
                counter++;
            }
            return (currentMin, counter);
        }
    }

    public static class LinkedListExtension
    {
        public static T Dequeue<T>(this LinkedList<T> list)
        {
            if (list.First == null) return default;
            T first = list.First.Value;
            list.RemoveFirst();
            return first;
        }
        public  static T Pop<T>(this LinkedList<T> list)
        {
            if (list.Last == null) return default;
            T last = list.Last.Value;
            list.RemoveLast();
            return last;
        }
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
