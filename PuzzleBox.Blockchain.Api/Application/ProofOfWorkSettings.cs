using PuzzleBox.Blockchain.ProofOfWork;

namespace PuzzleBox.Blockchain.Api.Application
{
    public class ProofOfWorkSettings : IProofOfWorkSettings
    {
        public int Difficulty { get; } = 5;
    }
}
