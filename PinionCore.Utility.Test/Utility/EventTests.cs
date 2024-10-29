using NUnit.Framework;

namespace PinionCore.Utility.Tests
{

    public class TestInvoker : PinionCore.Utility.Invoker
    {
        public new void Invoke()
        {
            base.Invoke();
        }
    }


    public class EventTests
    {


        [NUnit.Framework.Test]
        public void InvokeTest1()
        {
            var testEvent = new TestInvoker();
            PinionCore.Utility.Notifier notifier = testEvent;
            var ok = false;
            notifier.Subscribe += () => ok = true;
            testEvent.Invoke();

            Assert.True(ok);
        }

        [NUnit.Framework.Test]
        public void InvokeTest2()
        {
            var testEvent = new TestInvoker();
            PinionCore.Utility.Notifier notifier = testEvent;
            var ok = false;
            testEvent.Invoke();
            notifier.Subscribe += () => ok = true;


            Assert.True(ok);
        }
    }
}
