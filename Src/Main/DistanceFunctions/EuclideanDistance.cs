using System;

namespace USC.GISResearchLab.Common.Core.Geographics.DistanceFunctions
{
    public class EuclideanDistance
    {

        public static double CalculateEuclideanDistance(double fromX, double fromY, double toX, double toY)
        {
            return Math.Sqrt(Math.Pow(fromX - toX, 2) + Math.Pow(fromY - toY, 2));
        }
    }
}
