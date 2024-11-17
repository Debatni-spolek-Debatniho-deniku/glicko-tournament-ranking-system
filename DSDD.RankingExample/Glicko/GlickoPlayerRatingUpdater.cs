using DSDD.RankingExample.Model;

namespace DSDD.RankingExample.Glicko;

public class GlickoPlayerRatingUpdater: IPlayerRatingUpdater
{
    public void UpdateRatings(IReadOnlyList<ITeam> winningOrder)
        => UpdateRatings(winningOrder
            .Select(t => t is GlickoTeam gt ? gt : throw new ArgumentException($"Team {t.Name} is not GlickoTeam!"))
            .ToArray());

    private static void UpdateRatings(IReadOnlyList<GlickoTeam> teamsInWinningOrder)
    {
        IReadOnlyList<GlickoTeam> teams = teamsInWinningOrder;

        // New values are temporarily stored here as values from Player object are used in the calculation.
        Dictionary<GlickoPlayer, (double NewRating, double NewRatingDeviation)> newRatings = teams
            .SelectMany<GlickoTeam, GlickoPlayer>(t => t)
            .ToDictionary(p => p, p => (p.Rating, p.RatingDeviation));

        for (int i = 0; i < teams.Count; i++)
        {
            GlickoTeam currentTeam = teams[i];

            // Team first in the list will have score 1.
            // Team last in the list will have score 0.
            double score = Helpers.DeriveScoresFromWinningIndex(i);

            IReadOnlyList<GlickoTeam> opponents = teams.Where(t => t != currentTeam).ToArray();

            double gSum = 0;
            double eSum = 0;

            // Simulate as if each team played against each other team.
            foreach (GlickoTeam opponent in opponents)
            {
                double g = GlickoHelpers.CalculateG(opponent.RatingDeviation);
                double e = GlickoHelpers.CalculateE(currentTeam.Rating, opponent.Rating, opponent.RatingDeviation);

                gSum += Math.Pow(g, 2) * e * (1 - e);
                eSum += g * (score - e);
            }

            // Update each player in current team as if they played against all other teams.
            foreach (GlickoPlayer player in currentTeam)
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
    }
}