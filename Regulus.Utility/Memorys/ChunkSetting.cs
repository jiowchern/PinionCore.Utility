namespace Regulus.Memorys
{
    public class ChunkSetting
    {
        public int Size { get; }
        public int Count { get; }

        public ChunkSetting(int size, int count)
        {
            Size = size;
            Count = count;
        }
    }
}
