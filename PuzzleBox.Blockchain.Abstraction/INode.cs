using System;
using System.Threading.Tasks;

namespace PuzzleBox.Blockchain.Abstraction
{
    public interface INode<TData>
    {
        IBlockchain<TData> GetBlockchain();
        Task<IBlockchain<TData>> MineAsync(TData data);
        Task BroadcastChainAsync();
        void ReplaceChain(IBlockchain<TData> inbound);

        Task StartAsync();
        Task StopAsync();

        void AddPeer(Uri peer);
        void RemovePeer(Uri peer);
    }
}
