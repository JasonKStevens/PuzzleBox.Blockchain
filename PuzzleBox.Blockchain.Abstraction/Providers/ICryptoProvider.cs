namespace PuzzleBox.Blockchain.Abstraction.Providers
{
    public interface ICryptoProvider
    {
        string GetHash(string input);
        bool VerifyHash(string input, string hash);
    }
}
