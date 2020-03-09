using System;
using System.Collections.Generic;
using System.Text;
using static MultiDimensionOptimization.MDO;

namespace MultiDimensionOptimization.MNPDihtomia
{
    public static class MNPD
    {
        public static (double FunctionMinimum, int counter) Solve(OptimizingFunction function, LipzitsFunction lipzits, double precision, double epsilon, List<double> lowerBound, List<double> upperBound)
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
            //TODO: Сам метод плюс обходы
            return (currentMin, counter);
        }
    }
}
