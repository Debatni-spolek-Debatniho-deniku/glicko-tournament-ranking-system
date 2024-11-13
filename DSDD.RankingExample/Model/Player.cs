using DSDD.RankingExample.Glicko;

namespace DSDD.RankingExample.Model;

public class Player
{
    public string Name { get; }

    public double Rating { get; set; } = GlickoConsts.InitialRating;

    /// <summary>
    /// Uncertainty of the rating. The lower the more accurate it gets.
    /// </summary>
    public double RatingDeviation { get; set; } = GlickoConsts.InitialRatingDeviation;

    public Player(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return $"Player {Name}: Rating={Rating:F1}, RatingDeviation={RatingDeviation:F1}";
    }
}