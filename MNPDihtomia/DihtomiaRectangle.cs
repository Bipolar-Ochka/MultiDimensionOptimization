using System;
using System.Collections.Generic;
using System.Text;
using MultiDimensionOptimization;
using static MultiDimensionOptimization.MDO;

namespace MultiDimensionOptimization.MNPDihtomia
{
    internal class DihtomiaRectangle
    {
        List<double> lowerCorner;
        List<double> upperCorner;
        public List<double> center;
        public double Q;

        public DihtomiaRectangle(List<double> leftCorner, List<double> rightCorner,GraphicSettings settings =null)
        {
            this.lowerCorner = leftCorner;
            this.upperCorner = rightCorner;
            center = new List<double>(leftCorner.Count);
            for (int i = 0; i < leftCorner.Count; i++)
            {
                center.Insert(i, (leftCorner[i] / 2 + rightCorner[i] / 2));
            }
            settings?.Handle(lowerCorner, upperCorner);
        }
        public (double max, int dimension) GetMaxSide()
        {
            double max = 0;
            int dimension = 0;
            for(int i = 0; i < lowerCorner.Count; i++)
            {
                double side = Math.Abs(upperCorner[i] - lowerCorner[i]);
                if(side > max)
                {
                    max = side;
                    dimension = i;
                }
            }
            return (max,dimension);
        }
        
        public void EvalQ(OptimizingFunction func, double lipzitsConst)
        {
            Q = func(center) - lipzitsConst * (GetMaxSide().max/2d);
        }
        public (DihtomiaRectangle first, DihtomiaRectangle second) SplitByMaxSide(GraphicSettings settings =null)
        {
            int splitDimension = GetMaxSide().dimension;

            List<double> firstUp = new List<double>(upperCorner);
            firstUp[splitDimension] = (lowerCorner[splitDimension] / 2 + upperCorner[splitDimension] / 2);
            DihtomiaRectangle first = new DihtomiaRectangle(new List<double>(lowerCorner), firstUp,settings);

            List<double> secondLow = new List<double>(lowerCorner);
            secondLow[splitDimension] = (lowerCorner[splitDimension] / 2 + upperCorner[splitDimension] / 2);
            DihtomiaRectangle second = new DihtomiaRectangle(secondLow,new List<double>(upperCorner),settings);

            return (first, second);
        }
    }
}
