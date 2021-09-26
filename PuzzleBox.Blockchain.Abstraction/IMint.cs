namespace PuzzleBox.Blockchain.Abstraction
{
    public interface IMint<TData>
    {
        IBlock<TData> Genesis();
        IBlock<TData> Mine(IBlock<TData> lastBlock, TData data);
        bool IsValid(IBlock<TData> block);
    }
}