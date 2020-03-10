using System;
using System.Collections.Generic;
using System.Text;

namespace MultiDimensionOptimization.MNPModificated
{
    internal class ModRectangle
    {
        List<double> lowerCorner;
        List<double> upperCorner;
        public ModRectangle(List<double> lowerCorner, List<double> upperCorner)
        {
            this.lowerCorner = lowerCorner;
            this.upperCorner = upperCorner;
        }

        public List<double> GetIterationPoint (double h)
        {
            List<double> point = new List<double>(lowerCorner.Count);
            for(int i =0; i < lowerCorner.Count; i++)
            {
                point.Insert(i, Math.Min(lowerCorner[i] + h/2d, upperCorner[i]));
            }
            return point;
        }
        public LinkedList<ModRectangle> Split(double h, RuleM rule)
        {
            LinkedList<ModRectangle> rect = new LinkedList<ModRectangle>();
            for(int i=0; i <lowerCorner.Count; i++)
            {
                if(Math.Abs(upperCorner[i]-lowerCorner[i])>h)
                {
                    List<double> newLow = new List<double>(lowerCorner);
                    newLow[i] += h;
                    List<double> newUp = new List<double>(upperCorner);
                    for(int j=0; j < i; j++)
                    {
                        newUp[j] = Math.Min(newLow[j] + h, upperCorner[j]);
                    }
                    if(rule == RuleM.FIFO)
                    {
                        rect.AddFirst(new ModRectangle(newLow, newUp));
                    }
                    else
                    {
                        rect.AddLast(new ModRectangle(newLow, newUp));
                    }
                }
            }
            return rect;
        }
    }
}
