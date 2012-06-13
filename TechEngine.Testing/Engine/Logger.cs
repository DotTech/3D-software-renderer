using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TechEngine.Engine
{
    public static class Logger
    {
        private static Dictionary<string, object> logValues = new Dictionary<string, object>();

        /// <summary>
        /// Create or update a parameter that should be displayed in the debug log
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Value(string key, object value)
        {
            if (!logValues.Keys.Any(x => x == key))
            {
                logValues.Add(key, value);
            }
            else
            {
                logValues[key] = value;
            }
        }

        public static string Report()
        {
            string log = String.Empty;

            foreach (string key in logValues.Keys)
            {
                log += String.Format("{0}: {1}\r\n", key, logValues[key]);
            }

            return log;
        }
    }
}
