using PinionCore.Collection;

namespace PinionCoreLibraryTest
{
    internal class EnqueueHelper
    {
        private readonly int i;

        private readonly Queue<int> ints;

        public EnqueueHelper(Queue<int> ints, int i)
        {
            // TODO: Complete member initialization
            this.ints = ints;
            this.i = i;
        }

        internal void Run()
        {
            ints.Enqueue(i);
        }
    }
}
