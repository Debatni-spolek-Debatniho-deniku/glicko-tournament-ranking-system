using DSDD.RankingExample.Model;
using System.Numerics;
using System.Text.RegularExpressions;

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

    public static void UpdateRatings(IReadOnlyList<Team> teamsInWinningOrder)
    {
        IReadOnlyList<Team> teams = teamsInWinningOrder;

        // New values are temporarily stored here as values from Player object are used in the calculation.
        Dictionary<Player, (double NewRating, double NewRatingDeviation)> newRatings = teams
            .SelectMany(t => t)
            .ToDictionary(p => p, p => (p.Rating, p.RatingDeviation));

        for (int i = 0; i < teams.Count; i++)
        {
            Team currentTeam = teams[i];

            // Team first in the list will have score 1.
            // Team last in the list will have score 0.
            double score = deriveScoresFromRank(i + 1);

            IReadOnlyList<Team> opponents = teams.Where(t => t != currentTeam).ToArray();

            double gSum = 0;
            double eSum = 0;

            foreach (Team opponent in opponents)
            {
                double g = CalculateG(opponent.RatingDeviation);
                double e = CalculateE(currentTeam.Rating, opponent.Rating, opponent.RatingDeviation);

                gSum += Math.Pow(g, 2) * e * (1 - e);
                eSum += g * (score - e);
            }
            
            foreach (Player player in currentTeam)
            {
                double d2 = 1.0 / (Math.Pow(player.RatingDeviation, 2) + (Math.Pow(GlickoConsts.Q, 2) * gSum));

                var newTuple = newRatings[player];

                newTuple.NewRating += GlickoConsts.Q / (1 / Math.Pow(player.RatingDeviation, 2) + Math.Pow(GlickoConsts.Q, 2) * gSum) * eSum;
                newTuple.NewRatingDeviation = 1.0 / Math.Sqrt((1.0 / Math.Pow(player.RatingDeviation, 2)) + (Math.Pow(GlickoConsts.Q, 2) * gSum));

                newRatings[player] = newTuple;
            }
        }

        // Promote new ratings back to all players.
        foreach (var kvp in newRatings)
        {
            kvp.Key.Rating = kvp.Value.NewRating;
            kvp.Key.RatingDeviation = kvp.Value.NewRatingDeviation;
        }
        
        double deriveScoresFromRank(int playerRank)
        {
            // Normalized scores: 1 for first, 0 for last, equally distributed in between
            double maxScore = 1.0;
            double minScore = 0.0;
            double step = (maxScore - minScore) / 3; // 4 players: 3 intervals
            return new double[] { maxScore, maxScore - step, maxScore - 2 * step, minScore }[playerRank - 1];
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
        player.Rating += (GlickoConsts.Q /
            (1 / Math.Pow(player.RatingDeviation, 2) + Math.Pow(GlickoConsts.Q, 2) * oponentsG))
            
            
            * oponentsG * (actualScore / - expectedScore);


        player.RatingDeviation = 1 / Math.Sqrt(1 / (Math.Pow(player.RatingDeviation, 2) + Math.Pow(GlickoConsts.Q, 2) * oponentsG));
    }
}