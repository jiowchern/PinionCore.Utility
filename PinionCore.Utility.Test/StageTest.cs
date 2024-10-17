namespace PinionCoreLibraryTest
{
    using NSubstitute;
    public class StageTest
    {
        [NUnit.Framework.Test]
        public void MachineTest()
        {

            PinionCore.Utility.IBootable stage1 = NSubstitute.Substitute.For<PinionCore.Utility.IBootable>();
            PinionCore.Utility.IBootable stage2 = NSubstitute.Substitute.For<PinionCore.Utility.IBootable>();
            PinionCore.Utility.StageMachine machine = new PinionCore.Utility.StageMachine();

            machine.Push(stage1);
            machine.Push(stage2);

            machine.Clean();

            stage1.Received().Launch();
            stage1.Received().Shutdown();
            stage2.Received().Launch();
            stage2.Received().Shutdown();

        }
    }


}
