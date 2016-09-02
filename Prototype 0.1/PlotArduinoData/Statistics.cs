using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Statistics
{
    public static class Statistics
    {
        public static double standardDeviation(List<int> values)
        {
            double sd = 0.0;

            values.ForEach(x => sd = sd + Math.Pow((x - values.Average()), 2));

            sd = sd / values.Count();

            return sd;
        }

        public static double standardDeviation(List<double> values)
        {
            double sd = 0.0;

            values.ForEach(x => sd = sd + Math.Pow((x - values.Average()), 2));

            sd = sd / values.Count();

            return sd;
        }

        public static double threshold_p(List<int> values)
        {
            return values.Average() + 2 * standardDeviation(values);
        }

        public static double threshold_n(List<int> values)
        {
            return values.Average() - 2 * standardDeviation(values);
        }

        public static bool pass_threshold(double value, List<double> values)
        {
            var sd = standardDeviation(values);
            var avg = values.Average();
            var threshold = avg + 4 * sd;
            if (value > threshold)
                return true;
            else
                return false;
        }
    }
}
