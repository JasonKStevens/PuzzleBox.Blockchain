using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PuzzleBox.Blockchain.Abstraction;
using System.Text.Json;
using System.Threading.Tasks;

namespace PuzzleBox.Blockchain.Api.P2PComms
{
    public class GrpcServer : P2PComms.P2PCommsBase
    {
        private readonly INode<Transaction> _node;

        public GrpcServer(INode<Transaction> node)
        {
            _node = node;
        }

        public override Task<BlockchainResponse> GetBlockchain(BlockchainRequest request, ServerCallContext context)
        {
            var blockchain = _node.GetBlockchain();
            var response = ToResponse(blockchain);
            return Task.FromResult(response);
        }

        public override async Task<BlockchainResponse> Mine(MineRequest request, ServerCallContext context)
        {
            var transaction = new Transaction
            {
                FromAddress = request.FromAddress,
                ToAddress = request.ToAddress,
                Amount = decimal.Parse(request.Amount)
            };

            var blockchain = await _node.MineAsync(transaction);
            var response = ToResponse(blockchain);
            return response;
        }

        private static BlockchainResponse ToResponse(IBlockchain<Transaction> blockchain)
        {
            var response = new BlockchainResponse();

            foreach (var sourceBlock in blockchain.Chain)
            {
                var targetBlock = new Block
                {
                    Timestamp = Timestamp.FromDateTime(sourceBlock.Timestamp),
                    Hash = sourceBlock.Hash,
                    LastHash = sourceBlock.LastHash,
                    Data = JsonSerializer.Serialize(sourceBlock.Data)
                };

                response.Blocks.Add(targetBlock);
            }

            return response;
        }
    }
}
