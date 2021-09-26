using Microsoft.Extensions.Hosting;
using PuzzleBox.Blockchain.Abstraction;
using System.Threading;
using System.Threading.Tasks;

namespace PuzzleBox.Blockchain.Api.Services
{
    public class NodeService : IHostedService
    {
        private readonly INode<Transaction> _node;

        public NodeService(INode<Transaction> node)
        {
            _node = node;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _node.StartAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _node.StopAsync();
        }
    }
}
