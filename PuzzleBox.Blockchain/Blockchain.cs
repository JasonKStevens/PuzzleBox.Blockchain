using PuzzleBox.Blockchain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleBox.Blockchain
{
    public class Blockchain<TData> : IBlockchain<TData>
    {
        private readonly IMint<TData> _mint;

        private List<IBlock<TData>> _chain;
        public IReadOnlyList<IBlock<TData>> Chain => _chain.ToList();

        public Blockchain(IMint<TData> mint)
        {
            _mint = mint;
            _chain = new List<IBlock<TData>>();

            AddGenesisBlock();
        }

        private void AddGenesisBlock()
        {
            var genesisBlock = _mint.Genesis();
            _chain.Add(genesisBlock);
        }

        public void AddBlock(TData data)
        {
            var block = _mint.Mine(_chain.Last(), data);
            _chain.Add(block);
        }

        public bool IsValid()
        {
            var isValid = IsValid(_chain);
            return isValid;
        }

        private bool IsValid(IReadOnlyList<IBlock<TData>> chain)
        {
            var isChained = chain
                .Zip(chain.Skip(1), (prev, next) => (prev, next))
                .Aggregate(true, (result, pair) => result && pair.prev.Hash == pair.next.LastHash);

            var isValid = isChained && chain
                .Aggregate(true, (result, block) => result && _mint.IsValid(block));

            return isValid;
        }

        public void ReplaceChain(IBlockchain<TData> inbound)
        {
            if (inbound.Chain.Count <= _chain.Count)
            {
                // Incoming chain is not longer than current
                return;
            }

            if (!IsValid(inbound.Chain))
            {
                // Incoming chain is not valid
                return;
            }

            _chain = inbound.Chain.ToList();
        }
    }
}
