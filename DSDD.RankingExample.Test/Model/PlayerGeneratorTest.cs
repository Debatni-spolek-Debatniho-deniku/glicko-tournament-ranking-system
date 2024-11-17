using DSDD.RankingExample.Model;
using Moq;

namespace DSDD.RankingExample.Test.Model;

public class PlayerGeneratorTest
{
    private readonly Mock<IPlayerFactory> _playerFactoryMock = new();

    [SetUp]
    public void Setup()
    {
        _playerFactoryMock.Reset();
        _playerFactoryMock
            .Setup(_ => _.Create(It.IsAny<string>()))
            .Returns((string name) =>
            {
                Mock<IPlayer> playerMock = new();
                playerMock.SetupGet(_ => _.Name).Returns(name);
                return playerMock.Object;
            });
    }

    [Test]
    public void Generate()
    {
        // Arrange
        var expectedNames = new[] { "A", "B", "C", "D", "E" };

        // Act
        var actualNames = PlayersGenerator.Generate(5, _playerFactoryMock.Object).Select(p => p.Name).ToArray();

        // Assert
        Assert.That(actualNames, Is.EquivalentTo(expectedNames));
    }

    [Test]
    public void Generate_MoreThanAlphabet()
    {
        // Arrange
        var expectedLastNames = new[] { "AA", "AB", "AC", "AD", "AE" };

        // Act
        var actualLastNames = PlayersGenerator.Generate(31, _playerFactoryMock.Object).Select(p => p.Name).Skip(26).ToArray();

        // Assert
        Assert.That(actualLastNames, Is.EquivalentTo(expectedLastNames));
    }
}
