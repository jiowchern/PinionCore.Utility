﻿using System.Diagnostics;

namespace PinionCore.Utility
{
    public class TimeCounterInternal
    {
        private readonly Stopwatch _Stopwatch;

        public TimeCounterInternal()
        {
            _Stopwatch = Stopwatch.StartNew();
        }





        public long Ticks
        {
            get
            {
                return _Stopwatch.ElapsedTicks;
            }
        }

        public long Frequency { get { return Stopwatch.Frequency; } }
    }
}
