using Moq;
using NUnit.Framework;
using PuzzleBox.Blockchain.Abstraction.Providers;
using System;

namespace PuzzleBox.Blockchain.Test
{
  public class BlockFixture
  {
    private Mint<string> _sut;
    private Mock<ICryptoProvider> _cryptoProviderMock;
    private Mock<IDateTimeProvider> _dateTimeProvider;
    private DateTime _time1, _time2;

    [SetUp]
    public void Setup()
    {
      _cryptoProviderMock = new Mock<ICryptoProvider>(MockBehavior.Strict);
      _cryptoProviderMock
        .Setup(x => x.GetHash(It.IsAny<string>()))
        .Returns("some-hash");

      _time1 = new DateTime(2020, 1, 2, 3, 4, 5);
      _time2 = new DateTime(2020, 1, 2, 3, 4, 6);

      _dateTimeProvider = new Mock<IDateTimeProvider>(MockBehavior.Strict);
      _dateTimeProvider.SetupGet(x => x.Now).Returns(_time1);

      _sut = new Mint<string>(_cryptoProviderMock.Object, _dateTimeProvider.Object);
    }

    [Test]
    public void Should_create_genesis_block_with_expected_values()
    {
      // Act
      var genesisBlock = _sut.Genesis();

      // Assert
      Assert.That(genesisBlock.Timestamp, Is.EqualTo(_time1));
      Assert.That(genesisBlock.LastHash, Is.Null);
      Assert.That(genesisBlock.Hash, Is.EqualTo("some-hash"));
      Assert.That(genesisBlock.Data, Is.Null);
    }

    [Test]
    public void Should_create_new_block_with_expected_values()
    {
      var genesisBlock = _sut.Genesis();
      _dateTimeProvider.Reset();
      _dateTimeProvider.SetupGet(x => x.Now).Returns(_time2);

      // Act
      var block = _sut.Mine(genesisBlock, "some-data");

      // Assert
      Assert.That(block.Timestamp, Is.EqualTo(_time2));
      Assert.That(block.LastHash, Is.EqualTo(genesisBlock.Hash));
      Assert.That(block.Hash, Is.EqualTo("some-hash"));
      Assert.That(block.Data, Is.EqualTo("some-data"));

      _cryptoProviderMock.Verify(x => x.GetHash(It.IsAny<string>()), Times.Exactly(2));
    }
  }
}
