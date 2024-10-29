namespace PinionCore.Utility.Tests
{
    public class LooperTest
    {
        class LooperItem
        {

        }
        [NUnit.Framework.Test]
        public void Test()
        {

            var looper = new PinionCore.Utility.Looper<LooperItem>();

            var i1 = new LooperItem();
            var i2 = new LooperItem();

            var items = new System.Collections.Generic.List<LooperItem>();
            looper.AddItemEvent += items.Add;
            looper.RemoveItemEvent += i => items.Remove(i);
            var count = 0;
            looper.UpdateEvent += () => { count = items.Count; };
            looper.Add(i1);
            looper.Add(i2);
            looper.Update();
            NUnit.Framework.Assert.AreEqual(2, count);
            looper.Remove(i1);
            looper.Update();
            NUnit.Framework.Assert.AreEqual(1, count);



        }
    }
}
