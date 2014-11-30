using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine
{
    class Time
    {

        public static double TotalTime { get; private set; }
        public static double DeltaTime { get; private set; }

        private static DateTime prevTime;

        public static void Init()
        {
            TotalTime = DeltaTime = 0;
            prevTime = DateTime.Now;
        }

        public static void Update()
        {
            DateTime curTime = DateTime.Now;
            DeltaTime = (prevTime - curTime).TotalSeconds;
            TotalTime += DeltaTime;
            prevTime = curTime;
        }

    }
}
