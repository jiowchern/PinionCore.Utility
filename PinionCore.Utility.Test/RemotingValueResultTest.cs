using System.Timers;
using PinionCore.Remote;

namespace PinionCoreLibraryTest
{

    public class RemotingValueResultTest
    {
        [NUnit.Framework.Test, NUnit.Framework.Timeout(5000)]


        public void TestRemotingValueResult()
        {
            var val = new Value<bool>();
            var timer = new Timer(1);
            timer.Start();
            timer.Elapsed += (object sender, ElapsedEventArgs e) => { val.SetValue(true); };

            val.Result();
        }


    }
}
