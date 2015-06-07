using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gal3DEngine
{
	/// <summary>
	/// Holds time information.
	/// </summary>
    public class Time
    {

		/// <summary>
		/// The time passed sinced the start of the program.
		/// </summary>
        public static double TotalTime { get; private set; }
		/// <summary>
		/// The time passed sinced the previous Update.
		/// </summary>
        public static double DeltaTime { get; private set; }

        private static DateTime prevTime;

		/// <summary>
		/// Initializes the time information.
		/// </summary>
        public static void Init()
        {
            TotalTime = DeltaTime = 0;
            prevTime = DateTime.Now;
        }

		/// <summary>
		/// Updates the time information.
		/// </summary>
        public static void Update()
        {
            DateTime curTime = DateTime.Now;
            DeltaTime = (curTime - prevTime).TotalSeconds;
            TotalTime += DeltaTime;
            prevTime = curTime;
        }

    }
}
