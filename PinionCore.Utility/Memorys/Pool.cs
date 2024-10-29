using System.Collections.Generic;
using System.Linq;

namespace PinionCore.Memorys
{
    public class Pool : IPool
    {
        public static readonly Buffer Empty = new DirectBuffer(0);
        private readonly SortedList<int, ChunkPool> _chunkPools;

        public readonly IReadOnlyCollection<Chankable> Chunks;

        public Pool(IEnumerable<ChunkSetting> chunkSettings)
        {
            _chunkPools = new SortedList<int, ChunkPool>();

            foreach (ChunkSetting setting in chunkSettings)
            {
                _chunkPools.Add(setting.Size, new ChunkPool(setting.Size, setting.Count));
            }
            Chunks = _chunkPools.Select(kvp => kvp.Value as Chankable).ToList();
        }

        public Buffer Alloc(int size)
        {

            foreach (KeyValuePair<int, ChunkPool> kvp in _chunkPools)
            {
                var bufferSize = kvp.Key;
                if (bufferSize < size)
                    continue;
                ChunkPool chunkPool = kvp.Value;
                return chunkPool.Alloc(size);
            }


            return new DirectBuffer(size);
        }
    }
}
