using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechEngine.Engine
{
    public static class Extensions
    {
        /// <summary>
        /// Converts current value from radians to degrees
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static double ToDegrees(this double radians)
        {
            return radians * (180 / Math.PI);
        }

        /// <summary>
        /// Covnerts current value from degrees to radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static double ToRadians(this double degrees)
        {
            if (degrees == 90)
            {
                return Math.PI / 2;
            }

            if (degrees == 270)
            {
                return 3 * Math.PI / 2;
            }

            return degrees * (Math.PI / 180);
        }
    }
}
