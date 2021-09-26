using System.Collections.Generic;

namespace PuzzleBox.Blockchain.Abstraction
{
    public interface IBlockchain<TData>
    {
        IReadOnlyList<IBlock<TData>> Chain { get; }
        void AddBlock(TData data);
        bool IsValid();
        void ReplaceChain(IBlockchain<TData> incomingChain);
    }
}
