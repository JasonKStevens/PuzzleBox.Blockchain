using PuzzleBox.Blockchain.Abstraction;
using System;

namespace PuzzleBox.Blockchain
{
    public class Block<TData> : IBlock<TData>
    {
        public DateTime Timestamp { get; private set; }
        public string LastHash { get; private set; }
        public string Hash { get; private set; }
        public TData Data { get; private set; }
        public int Nonce { get; private set; }

        public Block(DateTime timestamp, string hash) : this(timestamp, null, hash, default, 0)
        {
        }

        public Block(DateTime timestamp, string lastHash, string hash, TData data, int nonce)
        {
            Timestamp = timestamp;
            LastHash = lastHash;
            Hash = hash;
            Data = data;
            Nonce = nonce;
        }
    }
}
