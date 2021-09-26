using Grpc.Net.Client;
using PuzzleBox.Blockchain.Abstraction;
using System;
using System.Threading.Tasks;

namespace PuzzleBox.Blockchain.Api.P2PComms
{
    public class GrpcClient : IP2PClient<Transaction>
    {
        private GrpcChannel _channel;
        private P2PComms.P2PCommsClient _client;

        public Uri Uri { get; init; }

        public GrpcClient(Uri peer)
        {
            Uri = peer;
        }

        public async Task MineAsync()
        {
            var mineRequest = new MineRequest
            {
                FromAddress = "Alice",
                ToAddress = "Bob",
                Amount = "12.34"
            };

            try
            {
                var response = await _client.MineAsync(mineRequest);
            }
            catch
            {
                // TODO: When a peer goes down... retry & remove
                //       Part of a larger P2P piece
            }
        }

        public Task SendChainAsync(IBlockchain<Transaction> blockchain)
        {
            throw new NotImplementedException();
        }

        public Task ConnectAsync()
        {
            _channel = GrpcChannel.ForAddress(Uri);
            _client = new P2PComms.P2PCommsClient(_channel);
            return Task.CompletedTask;
        }

        public Task DisconnectAsync()
        {
            return _channel?.ShutdownAsync() ?? Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
