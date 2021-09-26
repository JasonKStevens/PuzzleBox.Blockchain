using System;
using System.Collections.Generic;

namespace PuzzleBox.Blockchain.Abstraction
{
    public interface INodeSettings
    {
        IReadOnlyCollection<Uri> Peers { get; }
    }
}
