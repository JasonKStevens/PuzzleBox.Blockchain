using System;
using System.Threading.Tasks;

namespace PuzzleBox.Blockchain.Abstraction
{
    public interface IP2PClient<TData> : IDisposable
    {
        Uri Uri { get; }

        Task ConnectAsync();
        Task DisconnectAsync();

        Task MineAsync();
        Task SendChainAsync(IBlockchain<TData> blockchain);
    }
}
