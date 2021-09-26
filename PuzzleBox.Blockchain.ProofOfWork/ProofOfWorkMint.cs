using PuzzleBox.Blockchain.Abstraction;
using PuzzleBox.Blockchain.Abstraction.Providers;

namespace PuzzleBox.Blockchain.ProofOfWork
{
    public class ProofOfWorkMint<TData> : Mint<TData>
    {
        private readonly IProofOfWorkSettings _settings;

        public ProofOfWorkMint(
            IProofOfWorkSettings settings,
            ICryptoProvider cryptoProvider,
            IDateTimeProvider dateTimeProvider)
            : base(cryptoProvider, dateTimeProvider)
        {
            _settings = settings;
        }

        public override IBlock<TData> Mine(IBlock<TData> lastBlock, TData data)
        {
            var timestamp = _dateTimeProvider.Now;
            var lastHash = lastBlock.Hash;

            var leadingZeros = new string('0', _settings.Difficulty);
            var nonce = -1;
            string hash;

            do
            {
                nonce++;
                hash = GetHash(timestamp, lastHash, data, nonce);
            } while (hash.Substring(0, _settings.Difficulty) != leadingZeros);

            var block = new Block<TData>(timestamp, lastHash, hash, data, nonce);
            return block;
        }
    }
}
