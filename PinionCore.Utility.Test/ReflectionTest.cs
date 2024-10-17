using PinionCore.Utility.Reflection;
using System;

namespace PinionCore.Utility.Test
{

    class Test
    {
        public void Method1()
        { }
    }
    public class ReflectionTest
    {
        [NUnit.Framework.Test]
        public void GetMethod()
        {
            TypeMethodCatcher catcher = new TypeMethodCatcher((System.Linq.Expressions.Expression<Action<Test>>)(ins => ins.Method1()));
            NUnit.Framework.Assert.AreEqual("Method1", catcher.Method.Name);
        }
    }
}
