using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static MultiDimensionOptimization.MDO;

namespace MultiDimensionOptimization.MNPModificated
{
    public enum RuleM
    {
        FIFO,
        LIFO
    }
    public static class MNPANK
    {        
        public static (double FunctionMinimum, int counter, int optimal, List<double> optimalPoint) Solve(OptimizingFunction function, LipzitsFunction lipzits, double precision, double epsilon, List<double> lowerBound, List<double> upperBound, RuleM ruleSubList, RuleM ruleMainList, GraphicSettings settings=null)
        {
            double lipVal = lipzits(epsilon);
            LinkedList<ModRectangle> rectangles = new LinkedList<ModRectangle>();
            ModRectangle first = new ModRectangle(lowerBound, upperBound,settings);
            rectangles.AddFirst(first);
            int counter = 1;
            int optimal = counter;
            double currentMin = function(lowerBound);
            double h_base = 2d * (precision / lipVal - epsilon / lipVal);
            double h = h_base;
            List<double> optimalPoint = new List<double>(lowerBound);
            while (rectangles.Count != 0)
            {
                var currentRect = rectangles.Dequeue();
                var iterPoint = currentRect.GetIterationPoint(h);
                var functionValue = function(iterPoint);
                
                if(functionValue > currentMin)
                {
                    h = h_base + (functionValue - currentMin) / lipVal;
                }
                else
                {
                    h = h_base;
                    currentMin = functionValue;
                    optimalPoint = new List<double>(iterPoint);
                    optimal = counter;
                }
                var splitted = currentRect.Split(h,ruleSubList,settings);
                rectangles.ConcatList(splitted, ruleMainList);
                counter++;
            }
            GC.Collect();
            return (currentMin, counter,optimal,optimalPoint);
        }
    }
    public static class LinkedListExtension
    { 
        internal static LinkedList<T> ConcatList<T>(this LinkedList<T> current, LinkedList<T> toAdd ,RuleM rule)
        {
            if(toAdd.Count == 0)
            {
                return current;
            }
            else
            {
                while(toAdd.Count != 0)
                {
                    if(rule == RuleM.FIFO)
                    {
                        current.AddFirst(toAdd.Pop());
                    }
                    else
                    {
                        current.AddLast(toAdd.Dequeue());
                    }
                }
            }
            return current;
        }
    }

}
