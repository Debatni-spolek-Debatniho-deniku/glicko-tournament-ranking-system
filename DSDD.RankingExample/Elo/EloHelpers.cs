using DSDD.RankingExample.Model;

namespace DSDD.RankingExample.Elo;

public class EloHelpers
{   
    /// <summary>
    /// Probability of <see cref="rating1"/> beating <see cref="rating2"/>.
    /// <br/>
    /// <br/>
    /// The more closer is E to 1, the more likely is <see cref="rating1"/> to win.
    /// The more closer is E to 0, the more likely is <see cref="rating1"/> to win 
    /// </summary>
    public static double CalculateE(double rating1, double rating2)
    {
        return 1.0 / (1.0 + Math.Pow(10, (rating2 - rating1) / 400));
    }
}