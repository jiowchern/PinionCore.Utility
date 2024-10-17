namespace PinionCore.Memorys
{
    public interface IPool
    {
        Buffer Alloc(int size);
    }
}
