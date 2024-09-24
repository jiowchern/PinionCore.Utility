namespace Regulus.Memorys
{
    public interface IPool
    {
        Buffer Alloc(int size);
    }
}
