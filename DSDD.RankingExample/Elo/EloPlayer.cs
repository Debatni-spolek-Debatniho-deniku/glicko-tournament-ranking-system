using DSDD.RankingExample.Model;

namespace DSDD.RankingExample.Elo;

public class EloPlayer
{
    public string Name { get; }

    public double Rating { get; set; } = EloConsts.InitialRating;

    public EloPlayer(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return $"Player {Name}: Rating={Rating:F1}";
    }
}