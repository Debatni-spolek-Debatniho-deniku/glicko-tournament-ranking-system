using DSDD.RankingExample.Model;

namespace DSDD.RankingExample.Elo;

public class EloPlayerRatingUpdater : IPlayerRatingUpdater
{
    public void UpdateRatings(IReadOnlyList<ITeam> winningOrder)
        => UpdateRatings(winningOrder
            .Select(t => t is EloTeam gt ? gt : throw new ArgumentException($"Team {t.Name} is not EloTeam!"))
            .ToArray());

    private static void UpdateRatings(IReadOnlyList<EloTeam> teamsInWinningOrder)
    {
        IReadOnlyList<EloTeam> teams = teamsInWinningOrder;

        Dictionary<EloPlayer, double> newRatings = teams
            .SelectMany<EloTeam, EloPlayer>(t => t)
            .ToDictionary(p => p, p => p.Rating);


        for (int i = 0; i < teams.Count; i++)
        {
            EloTeam currentTeam = teams[i];

            // Team first in the list will have score 1.
            // Team last in the list will have score 0.
            double score = Helpers.DeriveScoresFromWinningIndex(i);

            IReadOnlyList<EloTeam> opponents = teams.Where(t => t != currentTeam).ToArray();

            double eSum = 0;

            // Simulate as if each team played against each other team.
            foreach (EloTeam oponnent in opponents)
            {
                double expectedScore = EloHelpers.CalculateE(currentTeam.Rating, oponnent.Rating);
                // TODO: stejné zamyšlení jako u GlickoPayerRatingUpdater.
                // Teďka hráč fakticky dostává 3 různé přírůstky ratingu. Nedává smysl je spíše vyprůměrovat?
                // Zejména jestli chceme přidávat speakerpointy, bude potřeba alespoň skóre převádět do škály 0 až 1.
                eSum += EloConsts.K * (score - expectedScore);
            }

            // Update each player in current team as if they played against all other teams.
            foreach (EloPlayer player in currentTeam)
            {
                double updatedRating = newRatings[player] + eSum;
                newRatings[player] = updatedRating;
            }
        }

        // Promote new ratings back to all players.
        foreach (var kvp in newRatings)
        {
            kvp.Key.Rating = kvp.Value;
        }
    }
}
