using NSubstitute;
using PinionCore.Remote;
using PinionCore.Utility;
using System;
using System.Linq;
using System.Net;
using PinionCore.Utility.CommandExtension;

namespace PinionCoreLibraryTest
{

    public enum TEST_ENUM1
    {
        A, B, C
    };
    public interface IDummy
    {
        void Method1();
        void Method2(int a, int b);

        float Method3(int a, int b);
    }
    public class CommandTest
    {
        /*[NUnit.Framework.Test]
        public void TestCommandRegisterEvent1()
        {
            IDummy dummy = NSubstitute.Substitute.For<IDummy>();
            Command command = new Command();

            command.RegisterEvent += (cmd, ret, args) =>
            {
                NUnit.Framework.Assert.AreEqual("Method3", cmd);
                NUnit.Framework.Assert.AreEqual(typeof(float), ret.Param);
                NUnit.Framework.Assert.AreEqual(typeof(int), args[0].Param);
                NUnit.Framework.Assert.AreEqual(typeof(int), args[1].Param);
            };
            command.RegisterLambda<IDummy, int, int, float>(dummy, (d, i1, i2) => d.Method3(i1, i2), (result) => { });
            command.Run("method3", new[] { "3", "9" });
        }*/
        /*[NUnit.Framework.Test]
        public void TestCommandRegisterEvent2()
        {
            IDummy dummy = NSubstitute.Substitute.For<IDummy>();
            Command command = new Command();

            command.RegisterEvent += (cmd, ret, args) =>
            {
                NUnit.Framework.Assert.AreEqual("Method2", cmd);
                NUnit.Framework.Assert.AreEqual(typeof(void), ret.Param);
                NUnit.Framework.Assert.AreEqual(typeof(int), args[0].Param);
                NUnit.Framework.Assert.AreEqual(typeof(int), args[1].Param);

                NUnit.Framework.Assert.AreEqual("return_Void", ret.Description);
            };
            command.RegisterLambda<IDummy, int, int>(dummy, (d, i1, i2) => d.Method2(i1, i2));
            command.Run("method2", new[] { "3", "9" });
        }*/

        [NUnit.Framework.Test]
        public void TestCommandRegisterEvent3()
        {
            IDummy dummy = NSubstitute.Substitute.For<IDummy>();
            Command command = new Command();
            command.RegisterEvent += (cmd, ret, args) =>
            {
                NUnit.Framework.Assert.AreEqual("m", cmd);
                NUnit.Framework.Assert.AreEqual(typeof(void), ret.Param);
                NUnit.Framework.Assert.AreEqual(typeof(int), args[0].Param);
                NUnit.Framework.Assert.AreEqual(typeof(int), args[1].Param);

                NUnit.Framework.Assert.AreEqual("", ret.Description);
                NUnit.Framework.Assert.AreEqual("l1", args[0].Description);
                NUnit.Framework.Assert.AreEqual("l2", args[1].Description);
            };


            command.Register<int, int>("m [l1 ,l2]", (l1, l2) => { dummy.Method2(l1, l2); });
        }

        [NUnit.Framework.Test]
        public void TestCommandRegisterEvent4()
        {
            IDummy dummy = NSubstitute.Substitute.For<IDummy>();
            Command command = new Command();
            command.RegisterEvent += (cmd, ret, args) =>
            {
                NUnit.Framework.Assert.AreEqual("m", cmd);
                NUnit.Framework.Assert.AreEqual(typeof(float), ret.Param);
                NUnit.Framework.Assert.AreEqual(typeof(int), args[0].Param);
                NUnit.Framework.Assert.AreEqual(typeof(int), args[1].Param);



                NUnit.Framework.Assert.AreEqual("", ret.Description);
                NUnit.Framework.Assert.AreEqual("l1", args[0].Description);
                NUnit.Framework.Assert.AreEqual("l2", args[1].Description);
            };


            command.Register<int, int, float>("m [l1 ,l2]", (l1, l2) => dummy.Method3(l1, l2), f => { });
        }


        /*[NUnit.Framework.Test]
        public void TestCommandLambdaRegister()
        {
            IDummy dummy = NSubstitute.Substitute.For<IDummy>();

            Command command = new Command();

            command.RegisterLambda(dummy, (d) => d.Method1());
            command.Run("method1", new string[] { });
            dummy.Received().Method1();

            command.RegisterLambda<IDummy, int, int>(dummy, (instance, a1, a2) => instance.Method2(a1, a2));
            command.Run("method2", new[] { "1", "2" });
            dummy.Received().Method2(1, 2);

            command.RegisterLambda<IDummy, int, int, float>(dummy, (instance, a1, a2) => instance.Method3(a1, a2), (result) => { });
            command.Run("method3", new[] { "3", "4" });
            dummy.Received().Method3(3, 4);

        }*/

        [NUnit.Framework.Test]
        public void TestCommandAnalysisWithParameters1()
        {
            Command.Analysis analysis = new Command.Analysis("login [ account ,    password, result]");

            NUnit.Framework.Assert.AreEqual("login", analysis.Command);
            NUnit.Framework.Assert.AreEqual("result", analysis.Parameters[2]);
            NUnit.Framework.Assert.AreEqual("account", analysis.Parameters[0]);
            NUnit.Framework.Assert.AreEqual("password", analysis.Parameters[1]);
        }

        [NUnit.Framework.Test]
        public void TestCommandAnalysisWithParameters2()
        {
            Command.Analysis analysis = new Command.Analysis("login [ account ,    password][ result ]");

            NUnit.Framework.Assert.AreEqual("login", analysis.Command);
            NUnit.Framework.Assert.AreEqual("result", analysis.Return);
            NUnit.Framework.Assert.AreEqual("account", analysis.Parameters[0]);
            NUnit.Framework.Assert.AreEqual("password", analysis.Parameters[1]);
        }


        [NUnit.Framework.Test]
        public void TestCommandAnalysisWithParameters3()
        {
            Command.Analysis analysis = new Command.Analysis("login-0.AAA [ account ,    password, result]");

            NUnit.Framework.Assert.AreEqual("login-0.AAA", analysis.Command);
            NUnit.Framework.Assert.AreEqual("result", analysis.Parameters[2]);
            NUnit.Framework.Assert.AreEqual("account", analysis.Parameters[0]);
            NUnit.Framework.Assert.AreEqual("password", analysis.Parameters[1]);
        }

        [NUnit.Framework.Test]
        public void TestCommandAnalysisNoParameters()
        {
            Command.Analysis analysis = new Command.Analysis("login");

            NUnit.Framework.Assert.AreEqual("login", analysis.Command);
            NUnit.Framework.Assert.AreEqual(0, analysis.Parameters.Length);
        }

        [NUnit.Framework.Test]
        public void TestCommandRegister0()
        {
            ICallTester callTester = Substitute.For<ICallTester>();

            Command command = new Command();
            CommandRegister<ICallTester> cr = new CommandRegister<ICallTester>(command, caller => caller.Function1());

            cr.Register(0, callTester);
            command.Run("0Function1", new string[0]);
            callTester.Received(1).Function1();
            cr.Unregister(0);
        }

        [NUnit.Framework.Test]
        public void TestCommandRegister1()
        {
            // data
            Command command = new Command();
            CommandRegister<ICallTester, int> cr = new CommandRegister<ICallTester, int>(

                command,
                (caller, arg1) => caller.Function2(arg1));
            ICallTester callTester = Substitute.For<ICallTester>();

            // test
            cr.Register(0, callTester);
            command.Run(
                "0Function2",
                new[]
                {
                    "1"
                });

            // verify
            cr.Unregister(0);
        }

        [NUnit.Framework.Test]
        public void TestCommandRegister2()
        {
            Command command = new Command();
            CommandRegisterReturn<ICallTester, int> cr = new CommandRegisterReturn<ICallTester, int>(

                command,
                caller => caller.Function3(),
                ret => { });
            ICallTester callTester = Substitute.For<ICallTester>();
            cr.Register(0, callTester);
            command.Run(
                "0Function3",
                new string[]
                {
                });
            callTester.Received(1).Function3();
            cr.Unregister(0);
        }

        [NUnit.Framework.Test]
        public void TestCommandRegister3()
        {
            Command command = new Command();
            int result = 0;
            CommandRegisterReturn<ICallTester, int, byte, float, int> cr = new CommandRegisterReturn<ICallTester, int, byte, float, int>(

                command,
                (caller, arg1, arg2, arg3) => caller.Function4(arg1, arg2, arg3),
                ret => { result = ret; });
            ICallTester callTester = Substitute.For<ICallTester>();
            callTester.Function4(Arg.Any<int>(), Arg.Any<byte>(), Arg.Any<float>()).Returns(1);
            cr.Register(0, callTester);
            command.Run(
                "0Function4",
                new[]
                {
                    "1",
                    "2",
                    "3"
                });
            callTester.Received(1).Function4(Arg.Any<int>(), Arg.Any<byte>(), Arg.Any<float>());
            cr.Unregister(0);
            NUnit.Framework.Assert.AreEqual(1, result);
        }


        [NUnit.Framework.Test]
        public void TestCommandRegister4()
        {
            Command command = new Command();
            int result = 0;
            CommandRegisterReturn<ICallTester, int> cr = new CommandRegisterReturn<ICallTester, int>(

                command,
                (caller) => caller.Function5,
                ret => { result = ret; });
            ICallTester callTester = Substitute.For<ICallTester>();
            callTester.Function5.Returns(1);
            cr.Register(0, callTester);
            command.Run(
                "0Function5",
                new string[0]);

            cr.Unregister(0);

            NUnit.Framework.Assert.AreEqual(1, result);
        }
        [NUnit.Framework.Test]
        public void TestCommandIpEndPointEnum()
        {
            object outVal;
            Command.TryConversion("127.0.0.1:12345", out outVal, typeof(System.Net.IPEndPoint));
            IPEndPoint ip = outVal as System.Net.IPEndPoint;
            NUnit.Framework.Assert.AreEqual(ip.Address, IPAddress.Parse("127.0.0.1"));
            NUnit.Framework.Assert.AreEqual(ip.Port, 12345);
        }

        [NUnit.Framework.Test]
        public void TestCommandGuid()
        {
            Guid id = Guid.NewGuid();
            object outVal;
            Command.TryConversion(id.ToString(), out outVal, typeof(Guid));
            Guid id2 = (Guid)outVal;
            NUnit.Framework.Assert.AreEqual(id.ToString(), id2.ToString());
        }
        [NUnit.Framework.Test]
        public void TestCommandCnvEnum()
        {
            object outVal;
            Command.TryConversion("A", out outVal, typeof(TEST_ENUM1));

            NUnit.Framework.Assert.AreEqual(TEST_ENUM1.A, outVal);

            Action<TEST_ENUM1> action = _CallEnum;

            action.Invoke((TEST_ENUM1)outVal);


        }

        private void _CallEnum(TEST_ENUM1 obj)
        {

        }
        [NUnit.Framework.Test]
        public void UnanalyzableCommand()
        {
            var handler = new UnanalyzableCommandHandler();
            
            Command command = new Command();
            command.Register("Unanalyzable.Command.TestRun.Run", handler.Run );
            command.Run("Unanalyzable.Command.TestRun.Run" , new string[0]);
            NUnit.Framework.Assert.AreEqual(true , handler.Called );

        }
        [NUnit.Framework.Test]
        public void CommandReturnTest()
        {
            Command command = new Command();
            command.Register<int,int,int>("add" , _AddTest  );
            var rets = command.Run("add" , new[] { "1", "1" });
            var val = (int)rets.First();
            NUnit.Framework.Assert.AreEqual(2 , val);
        }

        private int _AddTest(int arg1, int arg2)
        {
            return arg1 + arg2;
        }
    }

}
