﻿using NUnit.Framework;
using System.Collections.Generic;

namespace Regulus.Utility.Tests
{
    
    public class ChancesTableTests
    {


        [Test()]
        public void Valid()
        {
            IEnumerable<ChancesTable<int>.Item> items = new ChancesTable<int>.Item[] { new ChancesTable<int>.Item { Target = 1, Scale = 0 }, new ChancesTable<int>.Item { Target = 2, Scale = 1 }, new ChancesTable<int>.Item { Target = 3, Scale = 1 } };
            ChancesTable<int> table = new Regulus.Utility.ChancesTable<int>(items);
            int val1 = table.Get(0);
            int val2 = table.Get(1);
            int val3 = table.Get(2);

            Assert.AreEqual(1, val1);
            Assert.AreEqual(2, val2);
            Assert.AreEqual(3, val3);
        }
    }
}