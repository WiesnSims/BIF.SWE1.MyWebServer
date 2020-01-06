using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebServer.Database
{
    class TemperatureTestData
    {
        private const int minTemp = 4;
        private const int maxTemp = 41;

        public static double GetRandomTemperature(double lastTemp = -99)
        {
            double randomTemp;
            if(lastTemp == -99)
            {
                randomTemp = GetRandomDouble(minTemp, maxTemp);
            }
            else
            {
                randomTemp = lastTemp + GetRandomDouble(-0.5, 0.5);
            }
            randomTemp = Math.Truncate(100 * randomTemp) / 100;
            if (randomTemp < minTemp) randomTemp = minTemp;
            if (randomTemp > maxTemp) randomTemp = maxTemp;
            return randomTemp;
        }

        private static double GetRandomDouble(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
