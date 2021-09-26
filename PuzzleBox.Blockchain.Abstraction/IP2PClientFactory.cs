using System;

namespace PuzzleBox.Blockchain.Abstraction
{
    public interface IP2PClientFactory<TData>
    {
        IP2PClient<TData> Create(Uri uri);
    }
}
