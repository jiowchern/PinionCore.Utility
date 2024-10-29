namespace PinionCore.Memorys
{
    public interface Chankable
    {
        int BufferSize { get; }
        int PageSize { get; }
        int AvailableCount { get; }
        int DefaultAllocationThreshold { get; }
    }
}
