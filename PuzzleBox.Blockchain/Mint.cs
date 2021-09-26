using PuzzleBox.Blockchain.Abstraction;
using PuzzleBox.Blockchain.Abstraction.Providers;
using System;

namespace PuzzleBox.Blockchain
{
    public class Mint<TData> : IMint<TData>
    {
        protected readonly ICryptoProvider _cryptoProvider;
        protected readonly IDateTimeProvider _dateTimeProvider;

        public Mint(ICryptoProvider cryptoProvider, IDateTimeProvider dateTimeProvider)
        {
            _cryptoProvider = cryptoProvider;
            _dateTimeProvider = dateTimeProvider;
        }

        public IBlock<TData> Genesis()
        {
            var timestamp = _dateTimeProvider.Now;
            var hash = GetGenesisHash(timestamp);
            return new Block<TData>(timestamp, hash);
        }

        public virtual IBlock<TData> Mine(IBlock<TData> lastBlock, TData data)
        {
            var timestamp = _dateTimeProvider.Now;
            var lastHash = lastBlock.Hash;
            var hash = GetHash(timestamp, lastHash, data, 0);

            var block = new Block<TData>(timestamp, lastHash, hash, data, 0);
            return block;
        }

        public bool IsValid(IBlock<TData> block)
        {
            var hash = block.LastHash == null ?
                GetGenesisHash(block.Timestamp) :
                GetHash(block.Timestamp, block.Hash, block.Data, block.Nonce);

            var isValid = hash == block.Hash;
            return isValid;
        }

        private string GetGenesisHash(DateTime timestamp)
        {
            var hash = _cryptoProvider.GetHash($"{timestamp:O}");
            return hash;
        }

        protected string GetHash(DateTime timestamp, string lastHash, TData data, int nonce)
        {
            var hash = _cryptoProvider.GetHash($"{timestamp:O}_{lastHash}_{data}_{nonce}");
            return hash;
        }
    }
}
