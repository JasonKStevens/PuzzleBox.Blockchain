using Autofac;
using Microsoft.Extensions.Configuration;
using PuzzleBox.Blockchain.Abstraction;
using PuzzleBox.Blockchain.Abstraction.Providers;
using PuzzleBox.Blockchain.ProofOfWork;
using PuzzleBox.Blockchain.Providers;

namespace PuzzleBox.Blockchain.Api.Application
{
    public class ContainerModule : Module
    {
        public string ProofAlgorithm { get; set; }
        public string CryptoProvider { get; set; }
        public string DateTimeProvider { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<GrpcNode<Transaction>>()
                .As<INode<Transaction>>()
                .SingleInstance();

            builder
                .Register(CreateNodeSettings)
                .As<INodeSettings>();

            builder
                .Register(_ => new GrpcClientFactory<Transaction>())
                .As<IP2PClientFactory<Transaction>>()
                .SingleInstance();

            builder
                .RegisterType<Blockchain<Transaction>>()
                .As<IBlockchain<Transaction>>();
            
            switch (ProofAlgorithm?.ToUpper())
            {
                default:
                case "POW":
                    builder
                        .RegisterType<ProofOfWorkMint<Transaction>>()
                        .As<IMint<Transaction>>()
                        .SingleInstance();

                    builder
                        .RegisterType<ProofOfWorkSettings>()
                        .As<IProofOfWorkSettings>()
                        .SingleInstance();
                    break;
            }

            switch (CryptoProvider?.ToUpper())
            {
                default:
                case "SHA256":
                    builder
                      .RegisterType<SHA256CryptoProvider>()
                      .As<ICryptoProvider>();
                    break;
            }

            switch (DateTimeProvider?.ToUpper())
            {
                default:
                case "SYSTEM":
                    builder
                      .RegisterType<SystemDateTimeProvider>()
                      .As<IDateTimeProvider>();
                    break;
            }
        }

        private static INodeSettings CreateNodeSettings(IComponentContext context)
        {
            var configuration = context.Resolve<IConfiguration>();
            var nodeSettings = new NodeSettings(configuration);
            return nodeSettings;
        }
    }
}
