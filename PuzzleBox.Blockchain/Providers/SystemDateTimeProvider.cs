using PuzzleBox.Blockchain.Abstraction.Providers;
using System;

namespace PuzzleBox.Blockchain.Providers
{
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
