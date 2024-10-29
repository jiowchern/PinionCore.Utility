namespace PinionCore.Remote
{
    public interface IProvider
    {
        void Add(IGhost entiry);

        void Remove(long id);

        IGhost Ready(long id);

        void ClearGhosts();
    }
}
