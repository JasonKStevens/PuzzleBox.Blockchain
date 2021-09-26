using PuzzleBox.Blockchain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleBox.Blockchain
{
    public abstract class Node<TData> : INode<TData>
    {
        private readonly IP2PClientFactory<TData> _clientFactory;
        private readonly INodeSettings _settings;
        protected readonly IBlockchain<TData> _blockchain;
        protected readonly IList<IP2PClient<TData>> _clients = new List<IP2PClient<TData>>();

        public Node(
            IBlockchain<TData> blockchain,
            IP2PClientFactory<TData> clientFactory,
            INodeSettings settings)
        {
            _blockchain = blockchain;
            _clientFactory = clientFactory;
            _settings = settings;

            AddInitialPeers(settings.Peers);
        }

        public IBlockchain<TData> GetBlockchain()
        {
            return _blockchain;
        }

        public Task<IBlockchain<TData>> MineAsync(TData block)
        {
            _blockchain.AddBlock(block);
            return Task.FromResult(_blockchain);
        }

        public Task BroadcastChainAsync()
        {
            var tasks = _clients
                .Select(c => c.SendChainAsync(_blockchain));
            return Task.WhenAll(tasks);
        }

        public void ReplaceChain(IBlockchain<TData> inbound)
        {
            _blockchain.ReplaceChain(inbound);
        }

        public abstract Task StartAsync();
        public abstract Task StopAsync();

        private void AddInitialPeers(IReadOnlyCollection<Uri> peers)
        {
            foreach (var peer in peers)
            {
                AddPeer(peer);
            }
        }

        public void AddPeer(Uri peer)
        {
            var existing = _clients.FirstOrDefault(c => AreEqual(c.Uri, peer));
            if (existing != null)
                return;

            var client = _clientFactory.Create(peer);
            _clients.Add(client);
        }

        public void RemovePeer(Uri peer)
        {
            var existing = _clients.FirstOrDefault(c => AreEqual(c.Uri, peer));
            if (existing == null)
                return;

            _clients.Remove(existing);
        }

        public bool AreEqual(Uri uri1, Uri uri2)
        {
            var areEqual = Uri.Compare(uri1, uri2,
                UriComponents.Host | UriComponents.PathAndQuery,
                UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0;
            return areEqual;
        }
    }
}
