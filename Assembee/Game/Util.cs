using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {
    class Util {
        public static float Lerp(float firstFloat, float secondFloat, float by) {
            return firstFloat * (1 - by) + secondFloat * by;
        }


        public static void Log(string message) {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public static void Log(object value) {
            System.Diagnostics.Debug.WriteLine(value);
        }


    }
}
