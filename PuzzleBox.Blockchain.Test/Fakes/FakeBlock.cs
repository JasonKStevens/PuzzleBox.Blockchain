using PuzzleBox.Blockchain.Abstraction;
using System;

namespace PuzzleBox.Blockchain.Test.Fakes
{
    public class FakeBlock<TData> : IBlock<TData>
    {
        public DateTime Timestamp { get; set; }
        public string LastHash { get; set; }
        public string Hash { get; set; }
        public TData Data { get; set; }
        public int Nonce { get; set; }

        public FakeBlock(DateTime timestamp, string hash) : this(timestamp, null, hash, default, 0)
        {
        }

        public FakeBlock(DateTime timestamp, string lastHash, string hash, TData data, int nonce)
        {
            Timestamp = timestamp;
            LastHash = lastHash;
            Hash = hash;
            Data = data;
            Nonce = nonce;
        }
    }
}
