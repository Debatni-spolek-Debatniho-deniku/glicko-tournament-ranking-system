namespace DSDD.RankingExample.Glicko;

public static class GlickoHelpers
{
    /// <summary>
    /// Rating deviation factor
    /// <br/>
    /// <br/>
    /// The more uncertain (higher <see cref="ratingDevaition"/>, higher uncertainty) the rating is, the less impact it has.
    /// </summary>
    public static double CalculateG(double ratingDevaition)
        => 1 / Math.Sqrt(1 + 3 * Math.Pow(GlickoConsts.Q, 2) * Math.Pow(ratingDevaition, 2) / Math.Pow(Math.PI, 2));

    /// <summary>
    /// Probability of <see cref="rating1"/> beating <see cref="rating2"/>.
    /// <br/>
    /// <br/>
    /// The more closer is E to 1, the more likely is <see cref="rating1"/> to win.
    /// The more closer is E to 0, the more likely is <see cref="rating1"/> to win 
    /// </summary>
    public static double CalculateE(double rating1, double rating2, double ratingDeviationOfRating2)
        => 1.0 / (1.0 + Math.Pow(10, -ratingDeviationOfRating2 * (rating1 - rating2) / 400));
}