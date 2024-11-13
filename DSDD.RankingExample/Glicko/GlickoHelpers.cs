using DSDD.RankingExample.Model;

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
        => 1 / Math.Sqrt(1 + 3 * Math.Pow(ratingDevaition, 2) / Math.PI / Math.PI);

    /// <summary>
    /// Probability of <see cref="rating1"/> beating <see cref="rating2"/>.
    /// <br/>
    /// <br/>
    /// The more closer is E to 1, the more likely is <see cref="rating1"/> to win.
    /// The more closer is E to 0, the more likely is <see cref="rating1"/> to win 
    /// </summary>
    public static double CalculateE(double rating1, double rating2, double ratingDeviationOfRating2)
        => 1 / (1 + Math.Exp(-CalculateG(ratingDeviationOfRating2) * (rating1 - rating2) / 400));

    public static void UpdateRatings(IReadOnlyList<Team> teamsInWinningOrder)
    {
        IReadOnlyList<Team> teams = teamsInWinningOrder;

        for (int i = 0; i < teams.Count; i++)
        {
            Team currentTeam = teams[i];

            // Will range from 0 to 3.
            // Expected score will range from 0 to 3 as well (see Sum bellow -> sums number between 0 and 1 three times).
            // It is important that E and S (score) are of the same scale.
            double score = i;

            IReadOnlyList<Team> opponents = teams.Where(t => t != currentTeam).ToArray();

            // Overall uncertaitity factor in oponents' ratings.
            double oponnentsG = opponents.Sum(opponent => CalculateG(opponent.RatingDeviation));

            double expectedScore = opponents.Sum(oponnent =>
                CalculateE(currentTeam.Rating, oponnent.Rating, oponnent.RatingDeviation));

            foreach (Player player in currentTeam)
                UpdatePlayer(player, score, expectedScore, oponnentsG);
        }
    }

    /// <summary></summary>
    /// <param name="player"></param>
    /// <param name="actualScore">Score between 3 and 0. <see cref="ScoreExtensions.GetGlickoScore"/></param>
    /// <param name="expectedScore">Score between between 3 and 0. <see cref="CalculateE"/></param>
    /// <param name="oponentsG">Rating deviation factor (uncertainty of ratings) of all oponnets.</param>
    /// <remarks>
    /// If <see cref="expectedScore"/> is 3, player's team was expected to win.
    /// If <see cref="expectedScore"/> is 2, player's team was expected to be second.
    /// Player's rating is adjusted based on how much actual score and expected score deviates accounting for uncertainty.
    /// </remarks>
    public static void UpdatePlayer(Player player, double actualScore, double expectedScore, double oponentsG)
    {
        // Certainty of expected outcome. The lower the more reliable the outcome is.
        double v = 1 / (oponentsG * expectedScore * (1 - expectedScore));
        // The higher Δ is, the more player outperformed expectation.
        double delta = v * oponentsG * (actualScore - expectedScore);
        
        player.RatingDeviation = Math.Sqrt((1 / (1 / (Math.Pow(player.RatingDeviation, 2)) + 1 / v)));
        player.Rating += player.RatingDeviation * (actualScore - expectedScore);
    }
}