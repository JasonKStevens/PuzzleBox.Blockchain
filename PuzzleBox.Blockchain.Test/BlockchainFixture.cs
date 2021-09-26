using Moq;
using NUnit.Framework;
using PuzzleBox.Blockchain.Abstraction;
using PuzzleBox.Blockchain.Test.Fakes;
using System;
using System.Linq;

namespace PuzzleBox.Blockchain.Test
{
    public class BlockchainFixture
    {
        private Blockchain<string> _sut;
        private Mock<IMint<string>> _mintMock;
        private FakeBlock<string> _fakeBlock0, _fakeBlock1, _fakeBlock2;
        private FakeBlock<string>[] _fakeBlocks;

        [SetUp]
        public void Setup()
        {
            _fakeBlock0 = new FakeBlock<string>(DateTime.Now, "hash-0");
            _fakeBlock1 = new FakeBlock<string>(DateTime.Now, "hash-0", "hash-1", "data-1", 1);
            _fakeBlock2 = new FakeBlock<string>(DateTime.Now, "hash-1", "hash-2", "data-2", 2);
            _fakeBlocks = new[] { _fakeBlock0, _fakeBlock1, _fakeBlock2 };

            SetUpMintMock();

            _mintMock
                .Setup(x => x.IsValid(It.IsAny<IBlock<string>>()))
                .Returns(true);
        }

        private void SetUpMintMock()
        {
            _mintMock = new Mock<IMint<string>>(MockBehavior.Strict);

            _mintMock
                .Setup(x => x.Genesis())
                .Returns(_fakeBlock0);

            _mintMock
                .SetupSequence(x => x.Mine(It.IsAny<IBlock<string>>(), It.IsAny<string>()))
                .Returns(_fakeBlock1)
                .Returns(_fakeBlock2);
        }

        [Test]
        public void Should_create_genesis_block_as_first_block()
        {
            // Act
            _sut = new Blockchain<string>(_mintMock.Object);

            // Assert
            Assert.That(_sut.Chain.Count, Is.EqualTo(1));

            var lastBlock = _sut.Chain.First();
            Assert.That(lastBlock == _fakeBlock0, Is.True);
        }

        [Test]
        public void Should_create_mined_block_as_subsequent_block()
        {
            // Arrange
            _sut = new Blockchain<string>(_mintMock.Object);

            // Act
            _sut.AddBlock("subsequent-data");

            // Assert
            Assert.That(_sut.Chain.Count, Is.EqualTo(2));

            var lastBlock = _sut.Chain.Last();
            Assert.That(lastBlock == _fakeBlock1, Is.True);
            Assert.That(lastBlock.LastHash == _fakeBlock0.Hash, Is.True);
        }

        [Test]
        public void Should_be_valid_chain_with_only_genesis_block()
        {
            // Arrange
            _sut = new Blockchain<string>(_mintMock.Object);

            // Act
            var isValid = _sut.IsValid();

            // Assert
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void Should_be_valid_chain_with_valid_blocks()
        {
            // Arrange
            _sut = new Blockchain<string>(_mintMock.Object);
            _sut.AddBlock("subsequent-data");

            // Act
            var isValid = _sut.IsValid();

            // Assert
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void Should_be_invalid_chain_with_invalid_genesis_hash()
        {
            // Arrange
            _mintMock.Reset();
            SetUpMintMock();
            _mintMock
                .Setup(x => x.IsValid(It.IsAny<IBlock<string>>()))
                .Returns(false);

            _sut = new Blockchain<string>(_mintMock.Object);

            // Act
            var isValid = _sut.IsValid();

            // Assert
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void Should_be_invalid_chain_with_invalid_hash()
        {
            // Arrange
            _mintMock.Reset();
            SetUpMintMock();
            _mintMock
                .SetupSequence(x => x.IsValid(It.IsAny<IBlock<string>>()))
                .Returns(true)
                .Returns(false);

            _sut = new Blockchain<string>(_mintMock.Object);
            _sut.AddBlock("subsequent-data");

            // Act
            var isValid = _sut.IsValid();

            // Assert
            Assert.That(isValid, Is.False);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Should_be_invalid_chain_with_invalid_link(int invalidIndex)
        {
            // Arrange
            _sut = new Blockchain<string>(_mintMock.Object);
            _sut.AddBlock("data-1");
            _sut.AddBlock("data-2");
            _fakeBlocks[invalidIndex].LastHash = "invalid";

            // Act
            var isValid = _sut.IsValid();

            // Assert
            Assert.That(isValid, Is.False);
        }
    }
}
