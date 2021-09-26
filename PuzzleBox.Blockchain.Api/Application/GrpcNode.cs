using PuzzleBox.Blockchain.Abstraction;
using System.Linq;
using System.Threading.Tasks;

namespace PuzzleBox.Blockchain.Api.Application
{
    public class GrpcNode<TData> : Node<TData>
    {
        public GrpcNode(
            IBlockchain<TData> blockchain,
            IP2PClientFactory<TData> clientFactory,
            INodeSettings settings)
            : base(blockchain, clientFactory, settings)
        {
        }

        public override Task StartAsync()
        {
            var tasks = _clients.Select(c => c.ConnectAsync());
            return Task.WhenAll(tasks);
        }

        public override Task StopAsync()
        {
            if (_clients == null)
                return Task.CompletedTask;

            var tasks = _clients.Select(c => c.DisconnectAsync());
            return Task.WhenAll(tasks);
        }
    }
}
