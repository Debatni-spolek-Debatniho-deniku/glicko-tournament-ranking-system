using DSDD.RankingExample.Model;

namespace DSDD.RankingExample.Glicko;

public class GlickoPlayer: IPlayer
{
    public string Name { get; }

    public double Rating { get; set; } = GlickoConsts.InitialRating;

    /// <summary>
    /// Uncertainty of the rating. The lower the more accurate it gets.
    /// </summary>
    public double RatingDeviation { get; set; } = GlickoConsts.InitialRatingDeviation;

    public GlickoPlayer(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return $"Player {Name}: Rating={Rating:F1}, RatingDeviation={RatingDeviation:F1}";
    }
}