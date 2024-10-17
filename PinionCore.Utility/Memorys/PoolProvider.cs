using System;

namespace PinionCore.Memorys
{
    public static class PoolProvider
    {
        public readonly static Pool DirectShared = Direct();

        private static Pool Direct()
        {
            return new PinionCore.Memorys.Pool(
                new PinionCore.Memorys.ChunkSetting[0]);
        }

        public readonly static Pool Shared = Default();
        public static Pool Default()
        {
            return new PinionCore.Memorys.Pool(
                   new PinionCore.Memorys.ChunkSetting[] {
                        //new PinionCore.Memorys.ChunkSetting(1, 1024),
                        //new PinionCore.Memorys.ChunkSetting(2, 1024),
                        new PinionCore.Memorys.ChunkSetting(4, 1024 * 32),
                        new PinionCore.Memorys.ChunkSetting(8, 1024 * 32),
                        new PinionCore.Memorys.ChunkSetting(16, 1024 * 32),
                        new PinionCore.Memorys.ChunkSetting(32, 1024 * 32),
                        new PinionCore.Memorys.ChunkSetting(64, 1024 * 8),
                        new PinionCore.Memorys.ChunkSetting(128, 1024* 8),
                        new PinionCore.Memorys.ChunkSetting(256, 1024* 8 ),
                        new PinionCore.Memorys.ChunkSetting(512, 1024* 8),
                        new PinionCore.Memorys.ChunkSetting(1024, 1024 * 1),
                        new PinionCore.Memorys.ChunkSetting(2048, 1024 * 1)}
               );
        }
    }
}
