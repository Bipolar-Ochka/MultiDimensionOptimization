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

        public DihtomiaRectangle(List<double> leftCorner, List<double> rightCorner)
        {
            this.lowerCorner = leftCorner;
            this.upperCorner = rightCorner;
            center = new List<double>(leftCorner.Count);
            for (int i = 0; i < leftCorner.Count; i++)
            {
                center[i] = (leftCorner[i] + rightCorner[i]) / 2;
            }
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
            Q = func(center) - lipzitsConst * GetMaxSide().max / 2;
        }
        public (DihtomiaRectangle first, DihtomiaRectangle second) SplitByMaxSide()
        {
            int splitDimension = GetMaxSide().dimension;

            List<double> firstUp = new List<double>(upperCorner);
            firstUp[splitDimension] = (lowerCorner[splitDimension] + upperCorner[splitDimension]) / 2;
            DihtomiaRectangle first = new DihtomiaRectangle(new List<double>(lowerCorner), firstUp);

            List<double> secondLow = new List<double>(lowerCorner);
            secondLow[splitDimension] = (lowerCorner[splitDimension] + upperCorner[splitDimension]) / 2;
            DihtomiaRectangle second = new DihtomiaRectangle(secondLow,new List<double>(upperCorner));

            return (first, second);
        }
    }
}
