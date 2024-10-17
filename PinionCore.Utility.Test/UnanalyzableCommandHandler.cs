using System;

namespace PinionCoreLibraryTest
{
    internal class UnanalyzableCommandHandler
    {
        public bool Called;
        public UnanalyzableCommandHandler()
        {
        }

        internal void Run()
        {
            Called = true;
        }
    }
}