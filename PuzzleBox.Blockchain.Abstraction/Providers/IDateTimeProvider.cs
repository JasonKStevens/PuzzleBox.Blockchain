using System;

namespace PuzzleBox.Blockchain.Abstraction.Providers
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
