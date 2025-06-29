using System;
using System.Threading;
using System.Threading.Tasks;

namespace PinionCore.Utility
{
    public class PowerRegulator
    {
        private readonly FPSCounter _FPS;

        private readonly int _LowPower;

        private long _Busy;

        private long _SpinCount;

        private long _WorkCount;

        public int FPS
        {
            get { return _FPS.Value; }
        }

        public double Power => _GetSample();

        private double _GetSample()
        {
            var count = _WorkCount + _SpinCount;
            if (count == 0)
                return 0;

            var power = _WorkCount / (double)count;
            return power;
        }

        public PowerRegulator(int low_power) : this()
        {
            _LowPower = low_power;
        }

        public PowerRegulator()
        {

            _SpinCount = 0;
            _WorkCount = 0;
            _Busy = 0;

            _FPS = new FPSCounter();
        }

        public async Task Operate(long busy)
        {
            await Operate(busy, new CancellationTokenSource());
        }
        public async Task Operate(long busy,CancellationTokenSource source)
        {

            _FPS.Update();

            if (_Busy <= busy && _FPS.Value >= _LowPower)
            {
                _SpinCount++;

                var ms = (int)TimeSpan.FromTicks(busy - _Busy).TotalMilliseconds;
                await System.Threading.Tasks.Task.Delay(ms, source.Token);
            }
            else
            {
                _WorkCount++;
            }
            _Busy = busy;
        }
    }
}
