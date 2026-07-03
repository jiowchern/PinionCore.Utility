using System;
using NUnit.Framework;

namespace PinionCore.Utility.Tests
{
    public interface IDepotTestItem
    {
    }

    public class DepotTestItem : IDepotTestItem
    {
    }

    public class DepotTests
    {
        [NUnit.Framework.Test]
        public void SupplyThroughBaseInterfaceTest()
        {
            var depot = new PinionCore.Remote.Depot<DepotTestItem>();
            PinionCore.Remote.INotifier<IDepotTestItem> notifier = depot;

            IDepotTestItem supplied = null;
            notifier.Supply += item => supplied = item;

            var instance = new DepotTestItem();
            depot.Items.Add(instance);

            NUnit.Framework.Assert.AreSame(instance, supplied);
        }

        [NUnit.Framework.Test]
        public void SupplyExistingItemsThroughBaseInterfaceTest()
        {
            var depot = new PinionCore.Remote.Depot<DepotTestItem>();
            var instance = new DepotTestItem();
            depot.Items.Add(instance);

            PinionCore.Remote.INotifier<IDepotTestItem> notifier = depot;
            IDepotTestItem supplied = null;
            notifier.Supply += item => supplied = item;

            NUnit.Framework.Assert.AreSame(instance, supplied);
        }

        [NUnit.Framework.Test]
        public void UnsupplyThroughBaseInterfaceTest()
        {
            var depot = new PinionCore.Remote.Depot<DepotTestItem>();
            PinionCore.Remote.INotifier<IDepotTestItem> notifier = depot;

            IDepotTestItem unsupplied = null;
            notifier.Unsupply += item => unsupplied = item;

            var instance = new DepotTestItem();
            depot.Items.Add(instance);
            depot.Items.Remove(instance);

            NUnit.Framework.Assert.AreSame(instance, unsupplied);
        }

        [NUnit.Framework.Test]
        public void MixedSubscriberTypesTest()
        {
            // 同一個 Depot 同時被本型別與父介面訂閱，
            // 兩種委派的執行期型別不同，list 儲存必須都能接受。
            var depot = new PinionCore.Remote.Depot<DepotTestItem>();
            PinionCore.Remote.INotifier<DepotTestItem> exactNotifier = depot;
            PinionCore.Remote.INotifier<IDepotTestItem> baseNotifier = depot;

            var exactCount = 0;
            var baseCount = 0;
            exactNotifier.Supply += item => exactCount++;
            baseNotifier.Supply += item => baseCount++;

            depot.Items.Add(new DepotTestItem());

            NUnit.Framework.Assert.AreEqual(1, exactCount);
            NUnit.Framework.Assert.AreEqual(1, baseCount);
        }

        [NUnit.Framework.Test]
        public void RemoveHandlerTest()
        {
            var depot = new PinionCore.Remote.Depot<DepotTestItem>();
            PinionCore.Remote.INotifier<IDepotTestItem> notifier = depot;

            var count = 0;
            Action<IDepotTestItem> handler = item => count++;
            notifier.Supply += handler;
            notifier.Supply -= handler;

            depot.Items.Add(new DepotTestItem());

            NUnit.Framework.Assert.AreEqual(0, count);
        }
    }
}
