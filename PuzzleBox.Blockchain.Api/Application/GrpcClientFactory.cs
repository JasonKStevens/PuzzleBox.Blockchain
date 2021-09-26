using PuzzleBox.Blockchain.Abstraction;
using PuzzleBox.Blockchain.Api.P2PComms;
using System;

namespace PuzzleBox.Blockchain.Api.Application
{
    public class GrpcClientFactory<TData> : IP2PClientFactory<Transaction>
    {
        public IP2PClient<Transaction> Create(Uri uri) => new GrpcClient(uri);
    }
}
