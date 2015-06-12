using System;
using USC.GISResearchLab.Common.Geographics.Units;
using USC.GISResearchLab.Common.Geographics.Units.Linears;
using USC.GISResearchLab.Common.Geometries.Points;
using USC.GISResearchLab.Common.Utils.Math;

namespace USC.GISResearchLab.Common.Geographics.DistanceFunctions
{
    public class GreatCircleDistance
    {

        public const double EARTHRADIUSASMILES = 3959.87122552164;
        public static double LinearDistanceInMeters(Point p1, Point p2)
        {
            return LinearDistance(p1.Y, p1.X, p2.Y, p2.X, LinearUnitTypes.Meters);
        }

        public static double LinearDistance(Point p1, Point p2, Unit unit)
        {
            return LinearDistance(p1.Y, p1.X, p2.Y, p2.X, unit);
        }

        public static double LinearDistanceInMeters(double lat1, double lon1, double lat2, double lon2)
        {
            return LinearDistance(lat1, lon1, lat2, lon2, LinearUnitTypes.Meters);
        }

        public static double LinearDistance(double lat1, double lon1, double lat2, double lon2, Unit unit)
        {
            double ret = 0;
            if (unit.UnitType == UnitTypes.Linear)
            {
                LinearUnit linearUnit = (LinearUnit)unit;
                ret = LinearDistance(lat1, lon1, lat2, lon2, linearUnit.LinearUnitTypes);
            }
            else if (unit.UnitType == UnitTypes.NonLinear)
            {
                throw new Exception("Error calculating linear distance: output unit is non-linear, use DistanceInDegrees() directly instead");

            }
            else
            {
                throw new Exception("Unexpected unit type: " + unit.UnitType);
            }
            return ret;
        }

        public static double LinearDistance(double lat1, double lon1, double lat2, double lon2, LinearUnitTypes linearUnitType)
        {
            double ret = 0;
            double distInDegrees = Math.Abs(DistanceInDegrees(lat1, lon1, lat2, lon2));
            double distInMeters = distInDegrees * UnitConverter.MetersPerDD(lat1);

            double factor = UnitConverter.GetLengthConversionFactorFromMeters(linearUnitType);

            ret = distInMeters * factor;
            return ret;
        }

        public static double DistanceInDegrees(double lat1, double lon1, double lat2, double lon2)
        {
            double ret = 0;
            if (lat1 != lat2 || lon1 != lon2)
            {
                double theta = lon1 - lon2;
                double dist =
                    Math.Sin(MathUtils.DegreesToRadians(lat1)) *
                    Math.Sin(MathUtils.DegreesToRadians(lat2)) +
                    Math.Cos(MathUtils.DegreesToRadians(lat1)) *
                    Math.Cos(MathUtils.DegreesToRadians(lat2)) *
                    Math.Cos(MathUtils.DegreesToRadians(theta));
                dist = Math.Acos(dist);
                dist = MathUtils.RadiansToDegrees(dist);
                ret = Math.Abs(dist);
            }
            return ret;
        }

        // Kaveh: I put this new method here which is based on another method. Refere to 'Distance_GetDeg_OutRad' comment.
        public static double LinearDistance2(double lat1, double lon1, double lat2, double lon2, LinearUnitTypes linearUnitType)
        {
            double ret = 0;
            double distInRad = Distance_GetDeg_OutRad(lat1, lon1, lat2, lon2);
            double distInMeters = distInRad * GetRealRadiusFromRadianToMeter((lat1 + lat2) / 2);
            ret = distInMeters * UnitConverter.GetLengthConversionFactorFromMeters(linearUnitType);
            return ret;
        }

        // Kaveh: Returns earth radius based on latitude
        // ref: http://en.wikipedia.org/wiki/Geographic_coordinate_system and http://en.wikipedia.org/wiki/Earth_radius
        // Earth's average R = 6372795 Meter = 3959.87122552164 miles
        public static double GetRealRadiusFromRadianToMeter(double p)
        {
            double ret = 0;
            double a2 = 40680631590769;
            double b2 = 40408299803555.29;
            double q1 = (a2 * a2 * Math.Cos(p) * Math.Cos(p)) + (b2 * b2 * Math.Sin(p) * Math.Sin(p));
            double q2 = (a2 * Math.Cos(p) * Math.Cos(p)) + (b2 * Math.Sin(p) * Math.Sin(p));
            ret = Math.Sqrt(q1 / q2);
            return ret;
        }

        // Kaveh: another method based on other formulas mentioned here: http://en.wikipedia.org/wiki/Great-circle_distance
        // this will input in decimal degree co--ordinates and output in radinas !!
        public static double Distance_GetDeg_OutRad(double lat1, double lon1, double lat2, double lon2)
        {
            double ret = 0;
            if (lat1 != lat2 || lon1 != lon2)
            {
                double dLat = MathUtils.DegreesToRadians(lat2 - lat1); // phi
                double dLong = MathUtils.DegreesToRadians(lon2 - lon1); // lambda
                double q1 = Math.Pow(Math.Sin(dLat / 2), 2);
                double q2 = Math.Cos(MathUtils.DegreesToRadians(lat1)) * Math.Cos(MathUtils.DegreesToRadians(lat2)) * Math.Pow(Math.Sin(dLong / 2), 2);
                ret = 2 * Math.Atan2(Math.Sqrt(q1 + q2), Math.Sqrt(1 - q1 - q2));
                // ret = MathUtils.RadiansToDegrees(ret);
                ret = Math.Abs(ret);
            }
            return ret;
        }


        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        /// <param name="P1">First point.</param>
        /// <param name="P2">Second point.</param>
        /// <returns>Distance value.</returns>
        public static double DistanceBetween(Point3D P1, Point3D P2)
        {
            return GreatCircleDistance.LinearDistance(P1.Y, P1.X, P2.Y, P2.X, LinearUnitTypes.Miles);
        }
    }
}
