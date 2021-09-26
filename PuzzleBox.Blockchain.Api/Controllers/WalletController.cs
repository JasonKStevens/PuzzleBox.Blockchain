using Microsoft.AspNetCore.Mvc;
using PuzzleBox.Blockchain.Abstraction;
using System.Threading.Tasks;

namespace PuzzleBox.Blockchain.Api.Controllers
{
    [Route("wallet")]
    public class WalletController : Controller
    {
        private readonly INode<Transaction> _node;

        public WalletController(INode<Transaction> node)
        {
            _node = node;
        }

        [Route("mine")]
        public async Task<IActionResult> MineAsync()
        {
            var transaction = new Transaction
            {
                FromAddress = "Alice",
                ToAddress = "Bob",
                Amount = 12.34m
            };

            var blockchain = await _node.MineAsync(transaction);

            return Ok(blockchain);
        }
    }
}
