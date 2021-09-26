using System;

namespace PuzzleBox.Blockchain.Abstraction
{
    public interface IBlock<TData>
    {
        DateTime Timestamp { get; }
        string LastHash { get; }
        string Hash { get; }
        TData Data { get; }
        int Nonce { get; }
    }
}
