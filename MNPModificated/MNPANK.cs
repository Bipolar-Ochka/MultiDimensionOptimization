﻿using System;
using System.Collections.Generic;
using System.Text;
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
        public static (double FunctionMinimum, int counter, int optimal) Solve(OptimizingFunction function, LipzitsFunction lipzits, double precision, double epsilon, List<double> lowerBound, List<double> upperBound, RuleM ruleSubList, RuleM ruleMainList, GraphicSettings settings=null)
        {
            double lipVal = lipzits(epsilon);
            LinkedList<ModRectangle> rectangles = new LinkedList<ModRectangle>();
            ModRectangle first = new ModRectangle(lowerBound, upperBound,settings);
            rectangles.AddFirst(first);
            int counter = 1;
            int optimal = counter;
            double currentMin = function(lowerBound);
            double h = 2 * (precision - epsilon) / lipVal;
            while (rectangles.Count != 0)
            {
                var currentRect = rectangles.Dequeue();
                var iterPoint = currentRect.GetIterationPoint(h);
                var functionValue = function(iterPoint);
                if(functionValue < currentMin)
                {
                    currentMin = functionValue;
                    optimal = counter;
                }
                else
                {
                    h += (functionValue - currentMin) / lipVal;
                }
                var splitted = currentRect.Split(h,ruleSubList,settings);
                rectangles.ConcatList(splitted, ruleMainList);
                counter++;
            }
            return (currentMin, counter,optimal);
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
