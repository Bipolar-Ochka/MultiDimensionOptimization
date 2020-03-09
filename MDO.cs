using System;
using System.Collections.Generic;

namespace MultiDimensionOptimization
{
    public static class MDO
    {
        public delegate double OptimizingFunction(List<double> list);
        public delegate double LipzitsFunction(double epsilon);
    }
}
