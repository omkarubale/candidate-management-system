using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OU.Utility.Extensions
{
    public static partial class Extensions
    {
        public static double GetMean(this List<double> values)
        {
            return values.Count == 0 ? 0 : values.GetMean(0, values.Count);
        }

        public static double GetMean(this List<double> values, int start, int end)
        {
            double s = 0;

            for (int i = start; i < end; i++)
            {
                s += values[i];
            }

            return s / (end - start);
        }

        public static double GetVariance(this List<double> values)
        {
            return values.GetVariance(values.GetMean(), 0, values.Count);
        }

        public static double GetVariance(this List<double> values, double mean)
        {
            return values.GetVariance(mean, 0, values.Count);
        }

        public static double GetVariance(this List<double> values, double mean, int start, int end)
        {
            double variance = 0;

            for (int i = start; i < end; i++)
            {
                variance += Math.Pow((values[i] - mean), 2);
            }

            int n = end - start;
            if (start > 0) n -= 1;

            return variance / (n);
        }

        public static double GetStandardDeviation(this List<double> values)
        {
            return values.Count == 0 ? 0 : values.GetStandardDeviation(0, values.Count);
        }

        public static double GetStandardDeviation(this List<double> values, int start, int end)
        {
            double mean = values.GetMean(start, end);
            double variance = values.GetVariance(mean, start, end);

            return Math.Sqrt(variance);
        }

        public static double GetPercentile(this List<double> sortedValues, double currentValue)
        {
            if (sortedValues.Count() == 0) // Data array is empty
                return 0;

            int lowerCount = 0;
            int sameCount = 0;
            int n = sortedValues.Count();
            for (int i = 0; i < sortedValues.Count(); i++)
            {
                if (sortedValues[i] < currentValue)
                {
                    lowerCount++;
                }
                else if (sortedValues[i] == currentValue)
                {
                    sameCount++;
                }
                else
                {
                    break;
                }
            }

            if (sameCount == 0) // Provided value do not exists in dataset
                return 0;

            return (lowerCount + 0.5 * sameCount) / n * 100;
        }
    }
}