using System.Threading;

namespace PinionCore.Utility
{
    public class AutoPowerRegulator
    {
        private readonly TimeCounter _Counter;
        private readonly PowerRegulator _PowerRegulator;

        private long _PreviousTicks;

        public AutoPowerRegulator(PowerRegulator power_regulator)
        {

            _Counter = new TimeCounter();
            _PreviousTicks = _Counter.Ticks;
            _PowerRegulator = power_regulator;
        }

        public void Operate(CancellationTokenSource source)
        {

            var ticks = _Counter.Ticks;
            _PowerRegulator.Operate(ticks - _PreviousTicks, source).Wait();
            _PreviousTicks = ticks;
        }
    }
}
