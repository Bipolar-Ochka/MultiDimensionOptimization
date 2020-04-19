using System;
using System.Collections.Generic;
using System.Text;
using static MultiDimensionOptimization.MDO;

namespace MultiDimensionOptimization.MNPModificated
{
    internal class ModRectangle
    {
        List<double> lowerCorner;
        List<double> upperCorner;
        public ModRectangle(List<double> lowerCorner, List<double> upperCorner,GraphicSettings settings = null)
        {
            this.lowerCorner = lowerCorner;
            this.upperCorner = upperCorner;
            settings?.Handle(lowerCorner, upperCorner);
        }

        public List<double> GetIterationPoint (double h)
        {
            List<double> point = new List<double>(lowerCorner.Count);
            for(int i =0; i < lowerCorner.Count; i++)
            {
                point.Insert(i, Math.Min(lowerCorner[i] + h/2.0d, upperCorner[i]));
            }
            return point;
        }
        public LinkedList<ModRectangle> Split(double h, RuleM rule, GraphicSettings settings = null)
        {
            LinkedList<ModRectangle> rect = new LinkedList<ModRectangle>();
            for(int i=0; i <lowerCorner.Count; i++)
            {
                if(upperCorner[i]-lowerCorner[i]>h)
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
                        rect.AddFirst(new ModRectangle(newLow, newUp,settings));
                    }
                    else
                    {
                        rect.AddLast(new ModRectangle(newLow, newUp,settings));
                    }
                }
            }
            return rect;
        }
    }
}
