using Microsoft.Extensions.Configuration;
using PuzzleBox.Blockchain.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleBox.Blockchain.Api.Application
{
    public class NodeSettings : INodeSettings
    {
        public IReadOnlyCollection<Uri> Peers { get; init; }

        public NodeSettings(IConfiguration configuration)
        {
            Peers = configuration.GetValue<string>("peers")
                .Split(',')
                .Select(port => new Uri($"http://localhost:{port}"))
                .ToList();
        }
    }
}
