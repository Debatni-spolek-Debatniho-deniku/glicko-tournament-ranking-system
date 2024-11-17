using DSDD.RankingExample.Model;

namespace DSDD.RankingExample.Test;

public class PlayerGeneratorTest
{
    [Test]
    public void Generate()
    {
        // Arrange
        var expectedNames = new[] { "A", "B", "C", "D", "E" };

        // Act
        var actualNames = PlayersGenerator.Generate(5).Select(p => p.Name).ToArray();

        // Assert
        Assert.That(actualNames, Is.EquivalentTo(expectedNames));
    }

    [Test]
    public void Generate_MoreThanAlphabet()
    {
        // Arrange
        var expectedLastNames = new[] { "AA", "AB", "AC", "AD", "AE" };

        // Act
        var actualLastNames = PlayersGenerator.Generate(31).Select(p => p.Name).Skip(26).ToArray();

        // Assert
        Assert.That(actualLastNames, Is.EquivalentTo(expectedLastNames));
    }
}
